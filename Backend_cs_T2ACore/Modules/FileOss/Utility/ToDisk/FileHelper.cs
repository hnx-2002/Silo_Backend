using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
/// 文件方法
/// </summary>
public class FileHelper
{
    /// <summary>
    /// 检查存储桶是否存在
    /// </summary> 
    /// <param name="bucketName">存储桶名称</param>
    /// <returns></returns>
    public static bool BucketExists(string bucketName)
    {
        return Path.Exists(Path.Combine(Common.CurrentDllPath, bucketName));
    }

    /// <summary>
    /// 列出存储桶里的对象
    /// </summary> 
    /// <param name="bucketName">存储桶名称</param> 
    /// <param name="recursive">true代表递归查找，false代表类似文件夹查找，以'/'分隔，不查子文件夹</param>
    public static (bool Status, List<string> FileFullPaths) ListObjects(string bucketName, bool recursive = true)
    {
        bool flag = false;
        List<string> files = new();

        if (BucketExists(bucketName))
        {
            var rootPath = Path.Combine(Common.CurrentDllPath, bucketName);
            files = Directory.GetFiles(rootPath, "*",
                recursive
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly).ToList();
            flag = true;
        }
        return (flag, files);
    }

    /// <summary>
    /// 从桶下载文件到本地
    /// </summary> 
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="fileName">本地路径</param> 
    /// <returns></returns>
    public static bool FGetObject(string bucketName, string objectName, string fileName)
    {
        bool flag = false;
        if (BucketExists(bucketName))
        {
            var filePathOnDisk = Path.Combine(Common.CurrentDllPath, bucketName, objectName);
            var fileContent = File.ReadAllBytes(filePathOnDisk);
            File.WriteAllBytes(fileName, fileContent);
            flag = true;
        }
        return flag;
    }

    /// <summary>
    /// 上传文件至存储桶
    /// </summary> 
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="file">web端上传的附件</param>
    /// <returns></returns>
    public static (bool Status, Exception Ex) FPutObject(
        string bucketName, string objectName, IFormFile file)
    {
        Stream stream = file.OpenReadStream();
        return FPutObject_Exe(bucketName,
            objectName, stream);
    }

    /// <summary>
    /// 上传文件至存储桶
    /// </summary> 
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="file">web端上传的附件</param> 
    /// <returns></returns>
    public static (bool Status, Exception Ex) FPutObject(
        string bucketName, string objectName, byte[] file)
    {
        return FPutObject_Exe(bucketName,
            objectName, new MemoryStream(file));
    }

    /// <summary>
    /// 上传文件至存储桶
    /// </summary> 
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <param name="stream">文件流</param> 
    /// <returns></returns>
    private static (bool Status, Exception Ex) FPutObject_Exe(
        string bucketName, string objectName, Stream stream)
    {
        try
        {
            if (!BucketExists(bucketName))
            {
                return (false, new Exception("No Bucket"));
            }

            var parts = objectName.Split('/');
            var currentPath = Path.Combine(Common.CurrentDllPath, bucketName);
            for (int i = 0; i < parts.Length - 1; i++)
            {
                var subPathStr = parts[i];
                currentPath = Path.Combine(currentPath, subPathStr);
                //FunConsole.ConsoleLog("创建文件夹", currentPath);
                Directory.CreateDirectory(currentPath);
            }

            var filePath = Path.Combine(Common.CurrentDllPath, bucketName, objectName);
            stream.Position = 0;
            using (FileStream fileStream = new(filePath, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex);
        }


    }

    /// <summary>返回对象数据的流
    /// 返回对象数据的流
    /// </summary> 
    /// <param name="bucketName">存储桶名称</param>
    /// <param name="objectName">存储桶里的对象名称</param>
    /// <returns></returns>
    public static MemoryStream GetObject(string bucketName, string objectName)
    {
        if (!BucketExists(bucketName))
        {
            return null;
        }

        if (objectName.StartsWith("/") ||
            objectName.StartsWith("\\"))
        {
            objectName = objectName.Substring(1);
        }

        //var filePath = Path.Combine(Common.CurrentDllPath, bucketName, objectName);

        //Console.WriteLine("读取文件流,Base地址   ：" + Common.CurrentDllPath); //  /app/
        //Console.WriteLine("读取文件流,桶名称     ：" + bucketName);            //  rfamanage
        //Console.WriteLine("读取文件流,文件地址   ：" + objectName);            //  /CE202311-0416-5032-6128-227E80283ECF/test.glb
        //Console.WriteLine("读取文件流,全文件地址 ：" + filePath);              //  /CE202311-0416-5032-6128-227E80283ECF/test.glb

        var filePath = Path.Combine(Common.CurrentDllPath, bucketName, objectName);

        var bytes = File.ReadAllBytes(filePath);
        return new MemoryStream(bytes);
    }

    /// <summary>
    /// 从objectName指定的对象中将数据拷贝到destObjectName指定的对象
    /// </summary>
    /// <param name="overWrite">目标是否重名覆盖</param>
    /// <param name="fromBucketName">源存储桶名称</param>
    /// <param name="fromObjectName">源存储桶中的源对象名称</param>
    /// <param name="destBucketName">目标存储桶名称</param>
    /// <param name="destObjectName">要创建的目标对象名称,如果为空，默认为源对象名称</param> 
    /// <returns></returns>
    public static bool CopyObject(bool overWrite,
        string fromBucketName, string fromObjectName,
        string destBucketName, string destObjectName)
    {
        bool flag = false;
        if (BucketExists(fromBucketName) &&
            BucketExists(destBucketName))
        {
            var filePath1 = Path.Combine(Common.CurrentDllPath, fromBucketName, fromObjectName);
            var filePath2 = Path.Combine(Common.CurrentDllPath, destBucketName, destObjectName);
            File.Copy(filePath1, filePath2, overWrite);
            flag = true;
        }
        return flag;

    }

    /// <summary>
    /// 字符串MD5加密
    /// </summary>
    /// <param name="inStream"></param>
    /// <returns></returns>
    public static string GetFileMD5(Stream inStream)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        var result = md5.ComputeHash(inStream);
        var strResult = BitConverter.ToString(result);
        return strResult.Replace("-", "");
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
    /// 获取文件头信息
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetContentType(string fileName)
    {
        if (fileName.Contains(".jpg"))
        {
            return "image/jpg";
        }
        else if (fileName.Contains(".jpeg"))
        {
            return "image/jpeg";
        }
        else if (fileName.Contains(".png"))
        {
            return "image/png";
        }
        else if (fileName.Contains(".gif"))
        {
            return "image/gif";
        }
        else if (fileName.Contains(".pdf"))
        {
            return "application/pdf";
        }
        else
        {
            return "application/octet-stream";
        }
    }

    
    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<(bool Status, string Msg)> DeleteObjectAsync(
        string bucketName, string objectName)
    {
        try
        {
            if (!BucketExists(bucketName))
            {
                throw new Exception(string.Format("源存储桶[{0}]不存在", bucketName));
            }

            if (objectName.StartsWith("/") ||
                objectName.StartsWith("\\"))
            {
                objectName = objectName.Substring(1);
            }

            var filePath = Path.Combine(Common.CurrentDllPath, bucketName, objectName);

            File.Delete(filePath);

            return (true, "删除成功");
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }

    }
}