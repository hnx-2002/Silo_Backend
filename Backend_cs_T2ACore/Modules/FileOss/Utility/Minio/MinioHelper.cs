using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Encryption;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
/// minio方法
/// </summary>
public class MinioHelper
{
    /// <summary>
    /// 检查存储桶是否存在
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <returns></returns>
    public static async Task<bool> BucketExists(IMinioClient minio, string bucketName)
    {
        bool flag;
        try
        {
            var temp = new BucketExistsArgs().WithBucket(bucketName);
            flag = await minio.BucketExistsAsync(temp);
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        return flag;
    }

    /// <summary>
    /// 列出存储桶里的对象
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="prefix">对象的前缀</param>
    /// <param name="recursive">true代表递归查找，false代表类似文件夹查找，以'/'分隔，不查子文件夹</param>
    public static (bool Status, IObservable<Item> Observer) ListObjects(
        IMinioClient minio, string bucketName, string prefix = null, bool recursive = true)
    {
        bool flag = false;
        IObservable<Item> observable;
        try
        {
            var found = minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (found.Result)
            {
                observable = minio.ListObjectsAsync(new ListObjectsArgs()
                    .WithBucket(bucketName)
                    .WithPrefix(prefix)
                    .WithRecursive(recursive));
                flag = true;
            }
            else
            {
                throw new Exception(string.Format("存储桶[{0}]不存在", bucketName));
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        return (flag, observable);
    }

    /// <summary>
    /// 从桶下载文件到本地
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="fileName">要下载到的本地路径</param>
    /// <param name="sse"></param>
    /// <returns></returns>
    public static async Task<bool> FGetObject(
        IMinioClient minio, string bucketName, string objectName, string fileName, IServerSideEncryption sse = null)
    {
        bool flag;
        try
        {
            bool found = await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (found)
            {
                //if (File.Exists(fileName))
                //{
                //    File.Delete(fileName);
                //}
                await minio.GetObjectAsync(new GetObjectArgs()
                                              .WithBucket(bucketName)
                                              .WithObject(objectName)
                                              .WithFile(fileName)
                                              .WithServerSideEncryption(sse)).ConfigureAwait(false);
                flag = true;
            }
            else
            {
                throw new Exception(string.Format("存储桶[{0}]不存在", bucketName));
            }
        }
        catch (MinioException e)
        {
            throw new Exception(e.ToString());
        }
        return flag;
    }

    /// <summary>
    /// 上传文件至存储桶
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="file">web端上传的附件</param>
    /// <returns></returns>
    public static async Task<(bool Status, Exception Ex)> FPutObject(
        IMinioClient minio, string bucketName, string objectName, IFormFile file)
    {
        Stream stream = file.OpenReadStream();
        //StreamReader reader = new StreamReader(stream);
        //string text = reader.ReadToEnd();
        return await FPutObject_Exe(minio, bucketName, objectName,
            stream, file.Length, file.ContentType);
    }

    /// <summary>
    /// 上传文件至存储桶
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="file">web端上传的附件</param>
    /// <param name="contentType">文件类型</param>
    /// <returns></returns>
    public static async Task<(bool Status, Exception Ex)> FPutObject(
        IMinioClient minio, string bucketName, string objectName,
        byte[] file, string contentType)
    {
        return await FPutObject_Exe(minio, bucketName, objectName,
            new MemoryStream(file), file.Length, contentType);
    }

    /// <summary>
    /// 上传文件至存储桶
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="stream">文件流</param>
    /// <param name="fileLength">文件长度</param>
    /// <param name="contentType">文件类型</param>
    /// <returns></returns>
    private static async Task<(bool Status, Exception Ex)> FPutObject_Exe(
        IMinioClient minio, string bucketName, string objectName,
        Stream stream, long fileLength, string contentType)
    {
        try
        {
            var md5 = MD5.Create();
            var bytes = md5.ComputeHash(stream);
            stream.Position = 0;
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            sb.ToString();
            var total_minio_file_count = (int)Math.Ceiling((double)(fileLength / 5242880));

            //minio.SetTraceOn(new MinioRequestLog()
            //{
            //    //日志记录
            //    BackLogFunc = fileNo =>
            //    {
            //        decimal process = Math.Round((decimal)fileNo * 100 / total_minio_file_count, 2);
            //        string videoProcess = $"后台上传进度：{process}%"; 
            //        //RedisLog.Trace(videoProcess);
            //    }
            //});
            await minio.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(fileLength)
                    .WithContentType(contentType));
            return (true, null);
        }
        catch (MinioException ex)
        {
            return (false, ex);
        }
    }

    /// <summary>
    /// 上传本地文件至存储桶
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="fileName">本地路径</param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public static async Task<bool> FPutObject(
        IMinioClient minio, string bucketName, string objectName, string fileName, string contentType)
    {
        bool flag;
        try
        {
            await minio.PutObjectAsync(
                new PutObjectArgs().WithBucket(bucketName)
                                   .WithObject(objectName)
                                   .WithFileName(fileName)
                                   .WithContentType(contentType));
            flag = true;
        }
        catch (MinioException e)
        {
            throw new Exception(e.ToString());
        }
        return flag;
    }

    #region Presigned操作

    /// <summary>
    /// 生成一个给HTTP GET请求用的presigned URL。
    /// 浏览器/移动端的客户端可以用这个URL进行下载，即使其所在的存储桶是私有的。
    /// 这个presigned URL可以设置一个失效时间，默认值是7天。
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="expiresInt">失效时间（以秒为单位），默认是7天，不得大于七天</param>
    /// <returns></returns>
    public static async Task<(bool Status, string URL)> PresignedGetObject(
        IMinioClient minio, string bucketName, string objectName, int expiresInt = 1000)
    {
        bool flag;
        string Ret;
        try
        {
            bool found = await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (found)
            {
                var reqParams = new Dictionary<string, string> {
                    { "response-content-type", "application/json" }
                };
                string presignedUrl = await minio.PresignedGetObjectAsync(
                    new PresignedGetObjectArgs().WithBucket(bucketName)
                                                .WithObject(objectName)
                                                .WithExpiry(expiresInt)
                                                .WithHeaders(reqParams));
                Ret = presignedUrl;
                flag = true;
            }
            else
            {
                throw new Exception(string.Format("存储桶[{0}]不存在", bucketName));
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        return (flag, Ret);
    }

    #endregion Presigned操作

    /// <summary>返回对象数据的流
    /// 返回对象数据的流
    /// </summary>
    /// <param name="minio">连接实例</param>
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <returns></returns>
    public static async Task<MemoryStream> GetObjectAsync(IMinioClient minio, string bucketName, string objectName)
    {
        MemoryStream _outStream = new();
        try
        {
            await minio.StatObjectAsync(
                new StatObjectArgs().WithBucket(bucketName)
                                    .WithObject(objectName));

            await minio.GetObjectAsync(
                new GetObjectArgs().WithBucket(bucketName)
                                   .WithObject(objectName)
                                   .WithCallbackStream((stream) =>
                                   {
                                       stream.CopyTo(_outStream);
                                       _outStream.Position = 0;
                                   }
            ));
        }
        catch (MinioException e)
        {
            throw new Exception(e.ToString());
        }
        return _outStream;
    }

    /// <summary>
    /// 从objectName指定的对象中将数据拷贝到destObjectName指定的对象
    /// </summary>
    /// <param name="minio"></param>
    /// <param name="fromBucketName">源存储桶名称</param>
    /// <param name="fromObjectName">源存储桶中的源对象名称</param>
    /// <param name="destBucketName">目标存储桶名称</param>
    /// <param name="destObjectName">要创建的目标对象名称,如果为空，默认为源对象名称</param>
    /// <param name="copyConditions">拷贝操作的一些条件Map</param>
    /// <param name="sseSrc"></param>
    /// <param name="sseDest"></param>
    /// <returns></returns>
    public static async Task<bool> CopyObject(
        IMinioClient minio, string fromBucketName, string fromObjectName, string destBucketName,
        string destObjectName, CopyConditions copyConditions = null,
        IServerSideEncryption sseSrc = null, IServerSideEncryption sseDest = null)
    {
        bool flag;
        try
        {
            bool found = await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(fromBucketName));
            if (!found)
            {
                throw new Exception(string.Format("源存储桶[{0}]不存在", fromBucketName));
            }
            bool foundtwo = await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(destBucketName));
            if (!foundtwo)
            {
                throw new Exception(string.Format("目标存储桶[{0}]不存在", destBucketName));
            }
            await minio.CopyObjectAsync(
                new CopyObjectArgs().WithBucket(destBucketName)
                                    .WithObject(destObjectName)
                                    .WithCopyObjectSource(
                                        new CopySourceObjectArgs().WithBucket(fromBucketName)
                                                                  .WithObject(fromObjectName)
                                                                  .WithCopyConditions(copyConditions)));

            flag = true;
        }
        catch (MinioException e)
        {
            throw new Exception(e.ToString());
        }
        return flag;
    }


    /// <summary>
    /// 解压zip格式的文件Stream
    /// </summary>
    /// <param name="fileStream"></param>
    /// <returns>返回文件名+流</returns>
    public static List<(string FileName, byte[] FileContent, int FileLength)> UnZipFile(Stream fileStream)
    {
        List<(string, byte[], int)> result = new();
        try
        {
            //var temp = Encoding.Convert(Encoding.Default, Encoding.UTF8, zipFileBytes);
            //Stream fileStream = new MemoryStream(zipFileBytes);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using ZipInputStream s = new(fileStream);
            ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string fileName = theEntry.Name;
                if (fileName != string.Empty)
                {
                    byte[] buffer = new byte[theEntry.Size];
                    s.Read(buffer, 0, buffer.Length);
                    result.Add((fileName, buffer, buffer.Length));
                }
            }
        }
        catch (Exception ex)
        {
            string error = ex.ToString();
            throw;
        }

        return result;
    }

    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="minio"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<(bool Status, string Msg)> DeleteObjectAsync(
        IMinioClient minio, string bucketName, string objectName)
    {
        try
        {
            bool found = await minio.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName));
            if (!found)
            {
                throw new Exception(string.Format("源存储桶[{0}]不存在", bucketName));
            }
            await minio.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName));
            return (true, "删除成功");
        }
        catch (MinioException e)
        {
            throw new Exception(e.ToString());
        }

    }
}