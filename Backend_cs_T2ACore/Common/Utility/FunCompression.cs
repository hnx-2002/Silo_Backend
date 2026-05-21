using System;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace T2ACore
{
    /// <summary>
    /// 压缩工具类
    /// </summary>
    public static class FunCompression
    {
        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Compress(this string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] result = Compress(inputBytes);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 解压缩字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decompress(this string input)
        {
            byte[] inputBytes = Convert.FromBase64String(input);
            byte[] depressBytes = Decompress(inputBytes);
            return Encoding.UTF8.GetString(depressBytes);
        }

        /// <summary>
        /// 压缩字节数组
        /// </summary>
        /// <param name="inputBytes"></param>
        public static byte[] Compress(this byte[] inputBytes)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    zipStream.Write(inputBytes, 0, inputBytes.Length);
                    zipStream.Close(); //很重要，必须关闭，否则无法正确解压
                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        /// <param name="inputBytes"></param>
        public static byte[] Decompress(this byte[] inputBytes)
        {
            using (MemoryStream inputStream = new MemoryStream(inputBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        zipStream.CopyTo(outStream);
                        zipStream.Close();
                        return outStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CompressToBase64(byte[] data)
        {
            // 使用 MemoryStream 接收压缩后的数据
            using (var outputStream = new MemoryStream())
            {
                // 使用 GZipStream 进行压缩
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                } // 此处自动 Flush 并关闭流，确保数据写入完成

                // 将压缩后的字节数组转换为 Base64 字符串
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static byte[] DecompressFromBase64(string base64String)
        {
            byte[] compressedData = Convert.FromBase64String(base64String);
            using (var inputStream = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                gzipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="zipFileNameNoExt"></param>
        /// <returns></returns>
        public static string CompressFolder(string folderPath, string zipFileNameNoExt)
        {
            string parentPath = Path.GetDirectoryName(folderPath);
            string zipFilePath = Path.Combine(parentPath, zipFileNameNoExt + ".zip");

            // 删除已存在的ZIP文件
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }

            ZipFile.CreateFromDirectory(folderPath, zipFilePath);

            return zipFilePath;
        }


    }
}
