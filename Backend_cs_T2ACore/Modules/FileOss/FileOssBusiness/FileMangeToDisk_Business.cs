using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using SqlSugar;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace T2ACore;

/// <summary>
/// 文件处理类
/// </summary>
public class FileMangeToDisk_Business : IFileMange_Business
{
    private readonly IFile_record_Business FileRecordBusiness;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FileMangeToDisk_Business(IFile_record_Business fileRecordBusiness)
    {
        FileRecordBusiness = fileRecordBusiness;
    }

    /// <summary>
    /// 上传单个文件
    /// </summary>
    /// <param name="uploadFile">文件</param>
    /// <param name="account">账号</param>
    /// <param name="userName">姓名</param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    public async Task<Res_UploadFile> UploadFile(IFormFile uploadFile, string account, string userName, string tenant)
    {
        if (string.IsNullOrEmpty(Config.OssConfig.BucketName))
        {
            return Res_UploadFile.Error("未指定Bucket名称");
        }

        var fileRecord = File_record_Class.Create(Config.OssConfig.BucketName,
            uploadFile, Guid.Empty, account, userName, tenant);

        try
        {
            var res = await PushOss(uploadFile,
                fileRecord.Id, Config.OssConfig.BucketName);

            var uploadRes = new Res_UploadFile();
            uploadRes.Status = res.Status;
            uploadRes.Msg = res.Msg;

            if (res.Status)
            {
                uploadRes.MD5 = fileRecord.Md5;
                FileRecordBusiness.Add(fileRecord);
                //uploadRes.FilePath = $"/{Config.OssConfig.BucketName}/{res.FilePath}";
                uploadRes.FilePath = $"/{res.FilePath}";
                return uploadRes;
            }
            else
            {
                return uploadRes;
            }
        }
        catch (Exception ex)
        {
            var fileLog = new FileLog();
            fileLog.FileName = uploadFile.FileName;
            fileLog.FileType = uploadFile.ContentType;
            fileLog.FilePath = "";
            fileLog.BucketName = Config.OssConfig.BucketName;

            Logger.Error(fileLog.Error(ex));

            return null;
        }
    }

    /// <summary>
    /// 上传单个文件
    /// </summary>
    /// <param name="fileBytes">文件</param>
    /// <param name="fileName">文件名</param>
    /// <param name="contentType">文件类型</param>
    /// <param name="account">账号</param>
    /// <param name="userName">姓名</param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    public async Task<Res_UploadFile> UploadFileByBytes(byte[] fileBytes,
        string fileName, string contentType, string account, string userName, string tenant)
    {
        if (string.IsNullOrEmpty(Config.OssConfig.BucketName))
        {
            return Res_UploadFile.Error("未指定Bucket名称");
        }


        var fileRecord = File_record_Class.CreateFromBytes(
                        Config.OssConfig.BucketName, fileName,
                        fileBytes, Guid.Empty, account, userName, tenant);
        try
        {
            var res = await PushOssFromBytes(fileBytes,
                Config.OssConfig.BucketName,
                fileRecord.Id, fileName, contentType);

            var uploadRes = new Res_UploadFile();
            uploadRes.Status = res.Status;
            uploadRes.Msg = res.Msg;

            if (res.Status)
            {
                uploadRes.MD5 = fileRecord.Md5;
                FileRecordBusiness.Add(fileRecord);
                //uploadRes.FilePath = $"/{Config.OssConfig.BucketName}/{res.FilePath}";
                uploadRes.FilePath = $"/{res.FilePath}";
                return uploadRes;
            }
            else
            {
                return uploadRes;
            }
        }
        catch (Exception ex)
        {
            var fileLog = new FileLog();
            fileLog.FileName = fileName;
            fileLog.FileType = contentType;
            fileLog.FilePath = "";
            fileLog.BucketName = Config.OssConfig.BucketName;

            Logger.Error(fileLog.Error(ex));

            return null;
        }
    }

    /// <summary>
    /// 上传多个文件
    /// </summary>
    /// <param name="files">文件集合</param>
    /// <param name="account">账号</param>
    /// <param name="userName">姓名</param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    public async Task<Res_UploadFiles> UploadFiles(IFormFileCollection files, string account, string userName, string tenant)
    {
        if (string.IsNullOrEmpty(Config.OssConfig.BucketName))
        {
            return Res_UploadFiles.Error("未指定Bucket名称");
        }

        List<IFormFile> uploadFiles = files.ToList();

        List<File_record_Class> records = new();
        List<File_record_Class> failRecords = new();
        List<Res_UploadFile> uploadDetails = new();
        List<Res_UploadFile> failedDetails = new();

        foreach (var uploadFile in uploadFiles)
        {
            var fileRecord = File_record_Class.Create(Config.OssConfig.BucketName,
                uploadFile, Guid.Empty, account, userName, tenant);

            var uploadRes = await PushOss(uploadFile, fileRecord.Id, Config.OssConfig.BucketName);

            if (uploadRes.Status)
            {
                uploadRes.MD5 = fileRecord.Md5;

                uploadRes.FilePath = $"/{Config.OssConfig.BucketName}/{uploadRes.FilePath}";
                uploadDetails.Add(uploadRes);
                records.Add(fileRecord);
            }
            else
            {
                failedDetails.Add(uploadRes);
                failRecords.Add(fileRecord);
            }
        }

        FileRecordBusiness.Add(records); //写入数据库台账

        var res = new Res_UploadFiles();
        res.UploadDetail = uploadDetails;
        res.FailedDetail = failedDetails;
        res.DiskName = Config.OssConfig.BucketName;
        res.Msg = records.Count.ToString() + "个文件成功," +
              failRecords.Count.ToString() + "个文件失败";

        return res;
    }

    /// <summary>
    /// 上传文件，并做一层解压
    /// </summary>
    /// <param name="uploadFile">上传的文件</param>
    /// <param name="account">账户</param>
    /// <param name="userName">姓名</param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    public async Task<Res_UploadFile> UploadZipFile(IFormFile uploadFile, string account, string userName, string tenant)
    {
        if (string.IsNullOrEmpty(Config.OssConfig.BucketName))
        {
            return Res_UploadFile.Error("未指定Bucket名称");
        }


        List<File_record_Class> fileRecords = new();

        var fileRecord = File_record_Class.Create(Config.OssConfig.BucketName,
            uploadFile, Guid.Empty, account, userName, tenant);

        try
        {
            var res = await PushOss(uploadFile,
                fileRecord.Id, Config.OssConfig.BucketName);

            if (res.Status)
            {
                res.MD5 = fileRecord.Md5;

                fileRecords.Add(fileRecord);

                var fileStream = uploadFile.OpenReadStream();
                var zipResults = FileHelper.UnZipFile(fileStream);

                foreach (var (subFileName, subFileContent, subFileLength) in zipResults)
                {
                    var zipRecord = File_record_Class.CreateFromBytes(
                        Config.OssConfig.BucketName, subFileName,
                        subFileContent, fileRecord.Id, account, userName, tenant);

                    var uploadRes = await PushOssFromBytes(subFileContent,
                        Config.OssConfig.BucketName, fileRecord.Id,
                        subFileName, "application/octet-stream", true);

                    //if (uploadRes.Status)
                    //{
                    //    fileRecords.Add(zipRecord);
                    //}
                }

                FileRecordBusiness.Add(fileRecords); //写入数据库台账

                return res;
            }
            else
            {
                var fileLog = new FileLog();
                fileLog.FileName = uploadFile.FileName;
                fileLog.FileType = uploadFile.ContentType;
                fileLog.FilePath = "";
                fileLog.BucketName = Config.OssConfig.BucketName;

                Logger.Error(fileLog.Error(new Exception(res.Msg)));

                return res;
            }
        }
        catch (Exception ex)
        {
            var fileLog = new FileLog();
            fileLog.FileName = uploadFile.FileName;
            fileLog.FileType = uploadFile.ContentType;
            fileLog.FilePath = "";
            fileLog.BucketName = Config.OssConfig.BucketName;

            Logger.Error(fileLog.Error(ex));

            return null;
        }
    }


    /// <summary>
    /// 上传单个文件,并做一层解压
    /// </summary>
    /// <param name="fileBytes">文件</param>
    /// <param name="fileName">文件名</param>
    /// <param name="contentType">文件类型</param>
    /// <param name="account">账号</param>
    /// <param name="userName">姓名</param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    public async Task<Res_UploadFile> UploadZipFileByBytes(byte[] fileBytes,
        string fileName, string contentType, string account, string userName, string tenant)
    {
        if (string.IsNullOrEmpty(Config.OssConfig.BucketName))
        {
            return Res_UploadFile.Error("未指定Bucket名称");
        }

        List<File_record_Class> fileRecords = new();
        var fileRecord = File_record_Class.CreateFromBytes(
                        Config.OssConfig.BucketName, fileName,
                        fileBytes, Guid.Empty, account, userName, tenant);

        try
        {
            var res = await PushOssFromBytes(fileBytes,
                Config.OssConfig.BucketName,
                fileRecord.Id, fileName, contentType);

            if (res.Status)
            {
                res.MD5 = fileRecord.Md5;
                fileRecords.Add(fileRecord);

                var zipResults = FileHelper.UnZipFile(new MemoryStream(fileBytes));

                foreach (var (subFileName, subFileContent, subFileLength) in zipResults)
                {
                    var zipRecord = File_record_Class.CreateFromBytes(
                        Config.OssConfig.BucketName, subFileName,
                        subFileContent, fileRecord.Id, account, userName, tenant);

                    var uploadRes = await PushOssFromBytes(subFileContent,
                        Config.OssConfig.BucketName, fileRecord.Id,
                        subFileName, "application/octet-stream", true);
                    if (uploadRes.Status)
                    {
                        fileRecords.Add(zipRecord);
                    }
                    else
                    {
                        Console.WriteLine(subFileName + "  " + uploadRes.Msg);
                    }

                }

                FileRecordBusiness.Add(fileRecords); //写入数据库台账

                return res;
            }
            else
            {
                var fileLog = new FileLog();
                fileLog.FileName = fileName;
                fileLog.FileType = contentType;
                fileLog.FilePath = "";
                fileLog.BucketName = Config.OssConfig.BucketName;

                Logger.Error(fileLog.Error(new Exception(res.Msg)));

                return res;
            }
        }
        catch (Exception ex)
        {
            var fileLog = new FileLog();
            fileLog.FileName = fileName;
            fileLog.FileType = contentType;
            fileLog.FilePath = "";
            fileLog.BucketName = Config.OssConfig.BucketName;

            Logger.Error(fileLog.Error(ex));

            return null;
        }




    }

    //////////////////////////////////////////////////////////////////////////
    // 以下为原有服务代码
    //////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 列出存储桶的对象
    /// </summary>
    /// <param name="bucketName">桶名称</param>
    /// <param name="prefix">前置符</param>
    /// <param name="recursive">是否递归</param>
    /// <returns></returns>
    public (bool Status, string Msg, List<FileItem> Results) ListObjects(
        string bucketName, string prefix = null, bool recursive = false)
    {
        List<FileItem> files = new List<FileItem>();

        var flagJump = false;
        var error = "未找到任何文件";
        var (flag, filePaths) = FileHelper.ListObjects(bucketName, recursive);

        if (!flag)
        {
            return (false, "列出错误", null);
        }

        var exeFilePaths = filePaths;

        if (prefix != null)
        {
            exeFilePaths = new List<string>();
            foreach (var filePath in filePaths)
            {
                if (filePath.Contains(prefix))
                {
                    exeFilePaths.Add(filePath);
                }
            }
        }

        var baseDir = Path.Combine(Common.CurrentDllPath, Config.OssConfig.BucketName);

        foreach (var filePath in exeFilePaths)
        {
            var fileItem = new FileItem();

            if (File.Exists(filePath))
            {
                FileInfo info = new FileInfo(filePath);
                var ms = new MemoryStream(File.ReadAllBytes(filePath));

                fileItem.Key = info.FullName.Replace(baseDir, "").Replace("\\", "/");
                fileItem.LastModified = info.LastWriteTimeUtc.ToString(); //TODO
                fileItem.ETag = FileHelper.GetFileMD5(ms);
                fileItem.Size = (ulong)ms.Length;
                fileItem.IsDir = false;
                fileItem.VersionId = fileItem.ETag;
                fileItem.IsLatest = true;
                fileItem.LastModifiedDateTime = info.LastWriteTimeUtc;

                flagJump = flagJump && true;
                files.Add(fileItem);
            }
            else if (Directory.Exists(filePath))
            {
                DirectoryInfo info = new DirectoryInfo(filePath);
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(info.FullName));
                fileItem.Key = info.FullName.Replace(baseDir, "").Replace("\\", "/");
                fileItem.LastModified = info.LastWriteTimeUtc.ToString(); //TODO
                fileItem.ETag = FileHelper.GetFileMD5(ms);
                fileItem.Size = 0;
                fileItem.IsDir = true;
                fileItem.VersionId = fileItem.ETag;
                fileItem.IsLatest = true;
                fileItem.LastModifiedDateTime = info.LastWriteTimeUtc;

                flagJump = flagJump && true;
                files.Add(fileItem);
            }
            else
            {
                flagJump = flagJump && false;
                error += "路径" + filePath + "异常\r\n";
            }
        }

        if (files.Count > 0)
        {
            return (true, "OK", files);
        }

        return (flagJump, error, files);
    }

    /// <summary>
    /// 复制目录下文件
    /// </summary>
    /// <param name="path">目录全名 prefix</param>
    /// <param name="origin">原桶名</param>
    /// <param name="dest">新桶名</param>
    /// <returns></returns>
    public async Task CopyBatch(string path, string origin, string dest)
    {
        try
        {
            var (status, msg, results) = ListObjects(origin, path);

            if (!status)
            {
                return;
            }

            foreach (var file in results)
            {
                FileHelper.CopyObject(true, origin, file.Key, dest, file.Key);
            }
        }
        catch (Exception)
        {
            //RedisLog.Error(ex.ToString());
            throw;
        }
    }

    #region Upload

    /// <summary>
    /// 将文件通过流的方式上传到落盘
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileId"></param>
    /// <param name="bucketName"></param>
    public async Task<Res_UploadFile> PushOss(
        IFormFile file, Guid fileId, string bucketName)
    {
        //检测存储桶是否存在  没有则返回当前桶不存在
        var exists = FileHelper.BucketExists(bucketName);
        if (!exists)
        {
            return Res_UploadFile.Error("磁盘不存在，请检查");
        }

        var filePath = fileId.ToString().ToUpper() + "/" + file.FileName;

        var fileLog = new FileLog()
        {
            FileName = file.FileName,
            FilePath = filePath,
            FileType = file.ContentType,
            BucketName = bucketName
        };

        //是否上传成功
        var (status, ex) = FileHelper.FPutObject(bucketName, filePath, file);
        if (!status)
        {
            var error = fileLog.Error(ex);
            Logger.Error(error);

            return Res_UploadFile.Error("上传失败" + ex.ToString());
        }
        else
        {
            var log = fileLog.Create();
            Logger.Info(log);

            return new Res_UploadFile()
            {
                Status = true,
                Msg = "上传成功",
                FilePath = filePath,
            };
        }
    }

    /// <summary>
    /// 将文件通过流的方式上传到落盘
    /// </summary>
    /// <param name="file"></param>
    /// <param name="bucketName"></param>
    /// <param name="fileId"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <param name="isZip"></param>
    public async Task<Res_UploadFile> PushOssFromBytes(byte[] file,
        string bucketName, Guid fileId, string fileName, string contentType,
        bool isZip = false)
    {
        //检测存储桶是否存在  没有则返回当前桶不存在
        var exists = FileHelper.BucketExists(bucketName);
        if (!exists)
        {
            return Res_UploadFile.Error("磁盘不存在，请检查");
        }

        var fileIdStr = fileId.ToString().ToUpper();
        var filePath = fileIdStr + "/" + fileName;
        var fileDir = Path.Combine(Common.CurrentDllPath, bucketName, fileIdStr);
        Directory.CreateDirectory(fileDir);

        if (isZip)
        {
            var fileZipDir = Path.Combine(fileDir, "UnZip");
            Directory.CreateDirectory(fileZipDir);

            filePath = fileIdStr + "/UnZip/" + fileName;
        }

        var fileLog = new FileLog()
        {
            FileName = fileName,
            FilePath = filePath,
            FileType = contentType,
            BucketName = bucketName
        };

        if (file.Length == 0)
        {
            return new Res_UploadFile()
            {
                Status = false,
                Msg = "文件流上传错误,文件为0,疑似为文件夹",
                FilePath = filePath,
            };

            //Logger.Error("文件流上传错误,文件为0:" + filePath);
        }

        //是否上传成功
        var (status, ex) = FileHelper.FPutObject(bucketName, filePath, file);
        if (!status)
        {
            var error = fileLog.Error(ex);
            Logger.Error("文件流上传错误:" + filePath);
            Logger.Error("文件流上传错误:" + error);

            return Res_UploadFile.Error("上传失败" + ex.ToString());
        }
        else
        {
            var log = fileLog.Create();
            Logger.Info(log);

            return new Res_UploadFile()
            {
                Status = true,
                Msg = "上传成功",
                FilePath = filePath,
            };
        }
    }

    #endregion Upload

    /// <summary>
    /// 获取对象流,这个写的怎么这么蜜汁操作。。。
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public async Task<byte[]> GetObjectStream(string filePath)
    {
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        try
        {
            Stream ss = Stream.Null;
            FileHelper.FGetObject(Config.OssConfig.BucketName, filePath, filePath);
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffur = new byte[fs.Length];
            fs.Read(buffur, 0, (int)fs.Length);
            fs.Close();
            File.Delete(filePath);
            return buffur;
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// 获取文件流
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    public async Task<(MemoryStream, string)> GetObjectStreamOss(
        string filePath, string bucketName)
    {
        var fileName = Path.GetFileName(filePath);
        var ext = Path.GetExtension(filePath);

        try
        {
            var exists = FileHelper.BucketExists(bucketName);
            if (!exists)
            {
                return (null, "桶名称不存在，请检查");
            }

            filePath = filePath.Replace($"/{bucketName}", "");//如果fullpath 包含桶名，则替换掉

            var ss = FileHelper.GetObject(bucketName, filePath);

            var fileLog = new FileLog()
            {
                FileName = fileName,
                FilePath = filePath,
                FileType = ext,
                BucketName = bucketName
            };

            var log = fileLog.Create();
            Logger.Info(log);

            return (ss, "ok");
        }
        catch (Exception ex)
        {
            var fileLog = new FileLog();
            fileLog.FileName = fileName;
            fileLog.FileType = ext;
            fileLog.FilePath = filePath;
            fileLog.BucketName = Config.OssConfig.BucketName;
            Logger.Error(fileLog.Error(ex));

            return (null, "获取失败");
        }
    }

    /// <summary>
    /// 生成临时url，半小时有效
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="bucketName"></param> 
    /// <returns></returns>
    public async Task<string> PresignedGetObjectAsync(string filePath, string bucketName)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 生成临时url,指定过期时间
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="bucketName"></param> 
    /// <param name="expiredTime"></param> 
    /// <returns></returns>
    public async Task<string> PresignedGetObjectAsyncWithExpiredTime(string filePath, string bucketName, DateTime expiredTime)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 为某路径下所有文件生成临时路径,指定过期时间
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="bucketName"></param> 
    /// <param name="expiredTime"></param> 
    /// <returns></returns>
    public (bool Status, string Msg, List<string> FilePaths) PresignedDirectoryAsyncWithExpiredTime(
        string dirPath, string bucketName, DateTime expiredTime)
    {
        throw new NotImplementedException();
    }

    


    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public async Task<(bool Status, string Msg)> DeleteObject(
        string bucketName, string objectName)
    {
        return await FileHelper.DeleteObjectAsync(bucketName, objectName);
    }

    /// <summary>
    /// 清空桶
    /// </summary>
    /// <returns></returns>
    public async Task<(bool Status, string Msg)> ClearBucket()
    {
        var allRecords = FileRecordBusiness.GetAll();

        var failCount = 0;

        foreach (var record in allRecords)
        {
            var (status, msg) = await FileHelper.DeleteObjectAsync(
                record.Bucket_name, record.File_path);

            if (!status)
            {
                failCount++;
            }
        }

        return (failCount == 0, failCount == 0
            ? "成功" : failCount + "个失败");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<string> IFileMange_Business.CreateUploadId(string objectName)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary> 
    /// <param name="objectName"></param>
    /// <param name="uploadId"></param>
    /// <param name="chunkData"></param>
    /// <param name="chunkNo"></param>
    /// <param name="chunkTotal"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<(bool Status, string Msg)> IFileMange_Business.UploadChunk(string objectName,
        string uploadId, byte[] chunkData, int chunkNo, int chunkTotal)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary> 
    /// <param name="objectName"></param>
    /// <param name="uploadId"></param>
    /// <param name="account"></param>
    /// <param name="username"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<Res_UploadFile> IFileMange_Business.UploadChunkEnd(
        string objectName, string uploadId, string account, string username, string tenant)
    {
        throw new NotImplementedException();
    }


}
