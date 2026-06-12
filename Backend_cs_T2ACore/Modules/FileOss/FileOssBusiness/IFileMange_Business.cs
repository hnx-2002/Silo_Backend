using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
///
/// </summary>
public interface IFileMange_Business
{
    /// <summary>
    /// 复制目录下文件
    /// </summary>
    /// <param name="path">目录全名 prefix</param>
    /// <param name="origin">原桶名</param>
    /// <param name="dest">新桶名</param>
    /// <returns></returns>
    Task CopyBatch(string path, string origin, string dest);

    /// <summary>
    ///
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    Task<byte[]> GetObjectStream(string filePath);

    /// <summary>
    ///
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    Task<(MemoryStream MStream, string Msg)> GetObjectStreamOss(string filePath, string bucketName);


    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
    Task<(bool Status, string Msg)> DeleteObject(
        string bucketName, string objectName);

    /// <summary>
    /// 清空桶
    /// </summary>
    /// <returns></returns>
    Task<(bool Status, string Msg)> ClearBucket();

    /// <summary>
    /// 上传分片
    /// </summary> 
    /// <param name="objectName"></param>
    /// <param name="uploadId"></param>
    /// <param name="chunkData"></param>
    /// <param name="chunkNo"></param>
    /// <param name="chunkTotal"></param>
    /// <returns></returns>
    Task<(bool Status, string Msg)> UploadChunk(string objectName,
        string uploadId, byte[] chunkData, int chunkNo, int chunkTotal);

    /// <summary>
    /// 上传分片结束
    /// </summary> 
    /// <param name="objectName"></param>
    /// <param name="uploadId"></param>
    /// <param name="account"></param>
    /// <param name="username"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    Task<Res_UploadFile> UploadChunkEnd(
        string objectName, string uploadId,
        string account, string username, string tenant);


    /// <summary>
    /// 列出存储桶的对象
    /// </summary>
    /// <param name="bucketName">桶名称</param>
    /// <param name="prefix">前置符</param>
    /// <param name="recursive">是否递归</param>
    /// <returns></returns>
    (bool Status, string Msg, List<FileItem> Results) ListObjects(string bucketName, string prefix = null, bool recursive = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="bucketName"></param>
    /// <param name="expiredTime"></param>
    /// <returns></returns>
    (bool Status, string Msg, List<string> FilePaths) PresignedDirectoryAsyncWithExpiredTime(string dirPath, string bucketName, DateTime expiredTime);

    /// <summary>
    ///
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    Task<string> PresignedGetObjectAsync(string filePath, string bucketName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="bucketName"></param>
    /// <param name="expiredTime"></param>
    /// <returns></returns>
    Task<string> PresignedGetObjectAsyncWithExpiredTime(string filePath, string bucketName, DateTime expiredTime);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="File">IFormFile</param>
    /// <param name="fileId">文件Id</param>
    /// <param name="bucketName">桶名称</param>
    /// <returns></returns>
    Task<Res_UploadFile> PushOss(IFormFile File, Guid fileId, string bucketName);

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">文件</param> 
    /// <param name="bucketName">桶名称</param>
    /// <param name="fileId">文件Id</param>
    /// <param name="fileName">文件名</param>
    /// <param name="contentType">文件类型</param>
    /// <param name="isZip">是否为压缩包</param>
    /// <returns></returns>  
    Task<Res_UploadFile> PushOssFromBytes(byte[] file, string bucketName, Guid fileId, string fileName, string contentType, bool isZip = false);

    /// <summary>
    /// 上传单个文件
    /// </summary>
    /// <param name="uploadFile">文件</param>
    /// <param name="account">账号</param>
    /// <param name="userName">姓名</param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    Task<Res_UploadFile> UploadFile(IFormFile uploadFile, string account, string userName, string tenant);

    /// <summary>
    /// 上传单个文件
    /// </summary>
    /// <param name="fileBytes"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <param name="account"></param>
    /// <param name="userName"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    Task<Res_UploadFile> UploadFileByBytes(byte[] fileBytes, string fileName, string contentType, string account, string userName, string tenant);

    /// <summary>
    /// 上传多个文件
    /// </summary>
    /// <param name="files">文件集合</param>
    /// <param name="account">账号</param>
    /// <param name="userName">姓名</param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    Task<Res_UploadFiles> UploadFiles(IFormFileCollection files, string account, string userName, string tenant);

    /// <summary>
    /// 上传文件，并做一层解压
    /// </summary>
    /// <param name="uploadFile">上传的文件</param>
    /// <param name="account">账户</param>
    /// <param name="userName">姓名</param>
    /// <param name="tenant">租户</param>
    /// <returns></returns>
    Task<Res_UploadFile> UploadZipFile(IFormFile uploadFile, string account, string userName, string tenant);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileBytes"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <param name="account"></param>
    /// <param name="userName"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    Task<Res_UploadFile> UploadZipFileByBytes(byte[] fileBytes, string fileName,
        string contentType, string account, string userName, string tenant);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    Task<string> CreateUploadId(string objectName);
}