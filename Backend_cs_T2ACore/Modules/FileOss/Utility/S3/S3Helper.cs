using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace T2ACore;

internal class S3Helper
{
    /// <summary>
    /// 计算 MD5（Base64 格式，S3 要求）
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static string CalculateMD5Base64(Stream stream)
    {
        using (var md5 = MD5.Create())
        {
            byte[] hash = md5.ComputeHash(stream);
            stream.Position = 0; // 重置流位置
            return Convert.ToBase64String(hash);
        }
    }

    /// <summary>
    /// 计算 SHA-256（Base64 格式）
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static string CalculateSHA256Base64(Stream stream)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hash = sha256.ComputeHash(stream);
            stream.Position = 0; // 重置流位置
            return Convert.ToBase64String(hash);
        }
    }

    /// <summary>
    /// 计算SHA256
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static string GetStreamSHA256(Stream stream)
    {
        SHA256 sha256 = SHA256.Create();
        var buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        stream.Position = 0;
        var sha256Hex = BitConverter.ToString(sha256.ComputeHash(buffer))
            .Replace("-", "").ToLowerInvariant();
        return sha256Hex;
    }


    /// <summary>
    /// 列出指定桶中的对象
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName">桶名</param>
    /// <param name="prefix">路径前缀，模拟文件夹</param>
    /// <param name="recursive">是否递归</param>
    /// <returns>Status=true 表示成功；Results 为 FileItem 列表</returns>
    public static async Task<(bool Status, string Msg, List<FileItem> Results)> ListObjectsAsync(
        AmazonS3Client client, string bucketName, string prefix = null, bool recursive = false)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
            return (false, "bucketName 不能为空", null);

        var results = new List<FileItem>();

        try
        {
            string continuationToken = null;
            var delimiter = recursive ? null : "/"; // 非递归时只列一层

            do
            {
                var req = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    Prefix = prefix,
                    Delimiter = delimiter,
                    ContinuationToken = continuationToken
                };

                var resp = await client.ListObjectsV2Async(req);

                // 1. 普通对象
                foreach (var obj in resp.S3Objects)
                {
                    results.Add(FileItem.CastFromS3(obj));
                }

                // 2. 文件夹（CommonPrefixes）
                if (!recursive && resp.CommonPrefixes != null)
                {
                    foreach (var cp in resp.CommonPrefixes)
                    {
                        results.Add(FileItem.CastFromS3Dir(cp));
                    }
                }

                continuationToken = resp.NextContinuationToken;
            } while (!string.IsNullOrEmpty(continuationToken));

            return (true, string.Empty, results);
        }
        catch (AmazonS3Exception ex)
        {
            return (false, $"S3 错误：{ex.ErrorCode} - {ex.Message}", null);
        }
        catch (Exception ex)
        {
            return (false, $"其他错误：{ex.Message}", null);
        }
    }

    /// <summary>
    /// 从源对象拷贝数据到目标对象
    /// </summary>
    /// <param name="client"></param>
    /// <param name="fromBucketName"></param>
    /// <param name="fromObjectName"></param>
    /// <param name="destBucketName"></param>
    /// <param name="destObjectName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<bool> CopyObject(AmazonS3Client client,
        string fromBucketName, string fromObjectName,
        string destBucketName, string destObjectName)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(fromBucketName))
            throw new ArgumentException("fromBucketName 不能为空", nameof(fromBucketName));
        if (string.IsNullOrWhiteSpace(fromObjectName))
            throw new ArgumentException("fromObjectName 不能为空", nameof(fromObjectName));
        if (string.IsNullOrWhiteSpace(destBucketName))
            throw new ArgumentException("destBucketName 不能为空", nameof(destBucketName));

        // 如果目标对象名未指定，默认使用源对象名
        if (string.IsNullOrWhiteSpace(destObjectName))
            destObjectName = fromObjectName;

        try
        {
            var copySource = $"{fromBucketName}/{fromObjectName}";

            var request = new CopyObjectRequest
            {
                SourceBucket = fromBucketName,
                SourceKey = fromObjectName,
                DestinationBucket = destBucketName,
                DestinationKey = destObjectName
            };

            var response = await client.CopyObjectAsync(request);

            // 可根据需要进一步验证 response.HttpStatusCode == HttpStatusCode.OK
            return true;
        }
        catch (Exception ex)
        {
            // 生产环境可替换为日志记录
            Console.Error.WriteLine($"CopyObject 失败：{ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 检查指定存储桶是否存在
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<bool> BucketExists(AmazonS3Client client, string bucketName)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("bucketName 不能为空", nameof(bucketName));

        try
        {
            var res = await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);
            return res;
        }
        catch (Exception ex)
        {
            // 按需记录日志
            Console.Error.WriteLine($"检查桶存在时发生错误：{ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// 带文件长度与 Content-Type 的上传实现
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="stream"></param>
    /// <param name="fileLength"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public static async Task<(bool Status, Exception Ex)> FPutObject(AmazonS3Client client,
        string bucketName, string objectName, Stream stream, long fileLength, string contentType)
    {
        if (client == null)
            return (false, new ArgumentNullException(nameof(client)));
        if (string.IsNullOrWhiteSpace(bucketName))
            return (false, new ArgumentException("bucketName 不能为空"));
        if (string.IsNullOrWhiteSpace(objectName))
            return (false, new ArgumentException("objectName 不能为空"));
        if (stream == null)
            return (false, new ArgumentNullException(nameof(stream)));
        if (fileLength <= 0)
            return (false, new ArgumentException("fileLength 必须大于 0"));

        try
        {
            // 必须先重置流位置
            if (stream.CanSeek)
                stream.Position = 0;

            // 计算两个校验和
            string md5Base64 = CalculateMD5Base64(stream);
            string sha256Base64 = CalculateSHA256Base64(stream);

            //string sha256Hex = GetStreamSHA256(stream);

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = stream,
                AutoCloseStream = false,
                // 设置 MD5 - 这是解决报错的关键
                MD5Digest = md5Base64,
                // SHA-256 作为额外校验（可选）
                ChecksumAlgorithm = ChecksumAlgorithm.SHA256,
                ChecksumSHA256 = sha256Base64,
                Headers =
                {
                    ContentLength = fileLength,
                    ContentType = string.IsNullOrWhiteSpace(contentType)
                        ? "application/octet-stream"
                        : contentType
                }
            };

            await client.PutObjectAsync(request);
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex);
        }
    }

    /// <summary>
    /// 从 S3 桶下载对象到本地文件
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<bool> FGetObject(AmazonS3Client client,
        string bucketName, string objectName, string fileName)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("bucketName 不能为空");
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("objectName 不能为空");
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("fileName 不能为空");

        try
        {
            // 确保本地目录存在
            var dir = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
            };

            // 发送请求
            using var response = await client.GetObjectAsync(request);
            // 写入本地文件
            await response.WriteResponseStreamToFileAsync(fileName, false, default);
            return true;
        }
        catch (Exception ex)
        {
            // 可按需记录日志
            Console.Error.WriteLine($"下载失败：{ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 返回对象数据的流（MemoryStream）
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<MemoryStream> GetObjectAsync(
        AmazonS3Client client, string bucketName, string objectName)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("bucketName 不能为空");
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("objectName 不能为空");

        try
        {
            if (objectName.StartsWith("/"))
            {
                objectName = objectName.Substring(1);
            }

            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectName
            };

            using var response = await client.GetObjectAsync(request);
            var ms = new MemoryStream();
            await response.ResponseStream.CopyToAsync(ms);
            ms.Position = 0;   // 重置指针，方便后续读取
            return ms;
        }
        catch (Exception ex)
        {
            // 根据需求可记录日志或包装后抛出
            Console.Error.WriteLine($"下载失败：{ex.ToString()}");
            return null;
        }
    }


    /// <summary>
    /// 生成一个带签名的 GET URL，供客户端下载私有对象
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="expiresInt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<(bool Status, string URL)> PresignedGetObject(AmazonS3Client client,
        string bucketName, string objectName, int expiresInt = 604800)   // 默认 7 天
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(bucketName) || string.IsNullOrWhiteSpace(objectName))
            return Task.FromResult((false, (string)null));

        // S3 限制最大 7 天
        const int maxExpires = 7 * 24 * 3600;
        expiresInt = Math.Max(1, Math.Min(expiresInt, maxExpires));

        try
        {
            var url = client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = objectName,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddSeconds(expiresInt)
            });

            return Task.FromResult((true, url));
        }
        catch (Exception ex)
        {
            // 按需记录日志
            return Task.FromResult((false, (string)null));
        }
    }


    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<(bool Status, string Msg)> DeleteObjectAsync(
        AmazonS3Client client, string bucketName, string objectName)
    {
        try
        {
            bool found = await BucketExists(client, bucketName);
            if (!found)
            {
                throw new Exception(string.Format("源存储桶[{0}]不存在", bucketName));
            }

            if (objectName.StartsWith("/") ||
                objectName.StartsWith("\\"))
            {
                objectName = objectName.Substring(1);
            }
            var request = new DeleteObjectsRequest
            {
                BucketName = bucketName
            };
            request.AddKey(objectName);
            var response = await client.DeleteObjectsAsync(request);

            // 处理response信息
            if (response.DeletedObjects.Count > 0)
            {
                return (true, "删除成功");
            }
            else
            {
                return (false, "删除失败，未找到对象");
            }
        }
        catch (AmazonS3Exception e)
        {
            throw new Exception(e.ToString());
        }

    }

    /// <summary>
    /// 创建UploadId
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public static async Task<string> CreateUploadId(AmazonS3Client client,
        string bucketName, string objectName)
    {
        var initiateRequest = new InitiateMultipartUploadRequest
        {
            BucketName = bucketName,
            Key = objectName
        };
        var initiateResponse = await client
            .InitiateMultipartUploadAsync(initiateRequest);
        return initiateResponse.UploadId;
    }

    /// <summary>
    /// 上传分块
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="uploadId"></param>
    /// <param name="chunkStream"></param>
    /// <param name="chunkNo"></param>
    /// <param name="chunkTotal"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<(bool Status, string Msg)> UploadChunk(AmazonS3Client client,
        string bucketName, string objectName, string uploadId,
        Stream chunkStream, int chunkNo, int chunkTotal)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("bucketName 不能为空");
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("objectName 不能为空");
        if (string.IsNullOrWhiteSpace(uploadId))
            throw new ArgumentException("uploadId 不能为空");

        try
        {
            bool found = await BucketExists(client, bucketName);
            if (!found)
            {
                throw new Exception(string.Format("源存储桶[{0}]不存在", bucketName));
            }

            // 必须先重置流位置
            if (chunkStream.CanSeek)
                chunkStream.Position = 0;

            var uploadRequest = new UploadPartRequest
            {
                BucketName = bucketName,
                Key = objectName,
                UploadId = uploadId,
                PartNumber = chunkNo,
                InputStream = chunkStream,
                PartSize = chunkStream.Length
            };
            UploadPartResponse uploadResponse = await client
                .UploadPartAsync(uploadRequest);

            return (true, objectName + "分片(" + chunkNo + "_" + chunkTotal + ")上传成功");
        }
        catch (Exception ex)
        {
            return (false, ex.ToString());
        }

    }

    /// <summary>
    /// 上传分块完成
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="uploadId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<(bool Status, string Msg)> UploadChunkEnd(
        AmazonS3Client client, string bucketName, string objectName, string uploadId)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("bucketName 不能为空");
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("objectName 不能为空");
        if (string.IsNullOrWhiteSpace(uploadId))
            throw new ArgumentException("uploadId 不能为空");

        try
        {
            var listPartsRequest = new ListPartsRequest
            {
                BucketName = bucketName,
                Key = objectName,
                UploadId = uploadId,
            };
            var listPartsResponse = await client.ListPartsAsync(listPartsRequest);

            var parts = listPartsResponse.Parts;

            var etags = new List<PartETag>();

            foreach (var part in parts)
            {
                var partETag = new PartETag(part.PartNumber.Value, part.ETag);
                etags.Add(partETag);
            }

            var completeRequest = new CompleteMultipartUploadRequest
            {
                BucketName = bucketName,
                Key = objectName,
                UploadId = uploadId,
                PartETags = etags
            };

            await client.CompleteMultipartUploadAsync(completeRequest);

            return (true, "合并成功");
        }
        catch (Exception ex)
        {
            return (false, ex.ToString());
        }


    }
}
