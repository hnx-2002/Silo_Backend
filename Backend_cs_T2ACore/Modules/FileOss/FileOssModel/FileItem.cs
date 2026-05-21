using Amazon.S3.Model;
using NetTaste;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace T2ACore;

/// <summary>
/// 同Minio.DataModel.Item
/// </summary>
public class FileItem
{
    /// <summary>
    /// 对象的键或名称
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 对象最后修改的日期和时间，通常以 ISO 8601 格式表示。
    /// </summary>
    public string LastModified { get; set; }

    /// <summary>
    /// 一个用于表示对象内容的唯一标识符，通常用于检查对象是否已经改变
    /// </summary>
    public string ETag { get; set; }

    /// <summary>
    /// 对象的大小，以字节为单位。
    /// </summary>
    public ulong Size { get; set; }

    /// <summary>
    /// 一个布尔值，表示该项是否是一个目录。
    /// </summary>
    public bool IsDir { get; set; }

    /// <summary>
    /// 一个版本标识符，可能用于版本控制。
    /// </summary>
    public string VersionId { get; set; }

    /// <summary>
    /// 一个布尔值，表示该项是否是最新版本。
    /// </summary>
    public bool IsLatest { get; set; }

    /// <summary>
    /// 一个可空的 DateTime 对象，表示对象最后修改的日期和时间。
    /// </summary>
    public DateTime? LastModifiedDateTime { get; set; }

    /// <summary>
    /// 由Minio.DataModel.Item 转为FileItem
    /// </summary>
    /// <param name="inClass"></param>
    /// <returns></returns>
    public static FileItem Cast(Minio.DataModel.Item inClass)
    {
        var newModel = new FileItem();
        newModel.Key = inClass.Key;
        newModel.LastModified = inClass.LastModified;
        newModel.ETag = inClass.ETag;
        newModel.Size = inClass.Size;
        newModel.IsDir = inClass.IsDir;
        newModel.VersionId = inClass.VersionId;
        newModel.IsLatest = inClass.IsLatest;
        newModel.LastModifiedDateTime = inClass.LastModifiedDateTime;
        return newModel;
    }

    /// <summary>
    /// 由本地路径 转为FileItem
    /// </summary>
    /// <param name="fileFullPath"></param>
    /// <param name="baseDir"></param>
    /// <returns></returns>
    public static FileItem CastFromFilePath(string fileFullPath, string baseDir)
    {
        bool isDirectory = Directory.Exists(fileFullPath);
        FileSystemInfo info = isDirectory
            ? new DirectoryInfo(fileFullPath)
            : new FileInfo(fileFullPath);


        // 处理ETag（仅文件）
        string eTag = isDirectory ? null : CalculateFileMD5(info.FullName);

        // 处理最后修改时间
        DateTime lastModified = isDirectory
            ? ((DirectoryInfo)info).LastWriteTime
            : ((FileInfo)info).LastWriteTime;
        string lastModifiedStr = lastModified.ToString(
            "o", CultureInfo.InvariantCulture);

        //文件大小
        var size = isDirectory ? 0UL : (ulong)((FileInfo)info).Length;

        var newModel = new FileItem();
        newModel.Key = fileFullPath.Substring(baseDir.Length);
        newModel.LastModified = lastModifiedStr;
        newModel.ETag = eTag;
        newModel.Size = size;
        newModel.IsDir = isDirectory;
        newModel.VersionId = null;
        newModel.IsLatest = false;
        newModel.LastModifiedDateTime = lastModified;
        return newModel;
    }

    /// <summary>
    /// 由S3对象转为FileItem
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static FileItem CastFromS3(S3Object obj)
    {
        var newModel = new FileItem();
        newModel.Key = obj.Key;
        newModel.LastModified = obj.LastModified.ToString();
        newModel.ETag = obj.ETag.Trim('"'); // AWS 的 ETag 带双引号;
        newModel.Size = (ulong)obj.Size;
        newModel.IsDir = false;
        newModel.VersionId = null;
        newModel.IsLatest = true;               // List 接口默认最新
        newModel.LastModifiedDateTime = obj.LastModified;
        return newModel;
    }

    /// <summary>
    /// 由S3文件夹转为FileItem
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static FileItem CastFromS3Dir(string dir)
    {
        var newModel = new FileItem();
        newModel.Key = dir;
        newModel.LastModified = null;
        newModel.ETag = null;
        newModel.Size = 0;
        newModel.IsDir = true;
        newModel.VersionId = null;
        newModel.IsLatest = true; // List 接口默认最新
        newModel.LastModifiedDateTime = null;
        return newModel;
    }

    private static string CalculateFileMD5(string filePath)
    {
        using (var md5 = MD5.Create())
        using (var stream = File.OpenRead(filePath))
        {
            byte[] hashBytes = md5.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
