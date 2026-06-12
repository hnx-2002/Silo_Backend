import CryptoJS from 'crypto-js';

/**
 * 计算Base64字符串的SHA256哈希值
 * @description 使用CryptoJS库计算给定Base64字符串的SHA256哈希值，并返回十六进制格式的哈希字符串
 * @param {string} base64String - 要计算哈希值的Base64字符串
 * @returns {string} 返回十六进制格式的SHA256哈希字符串
 * @example
 * const hash = calculateHash('SGVsbG8sIFdvcmxkIQ==');
 * // 返回: 'a0c9d73d99b40b6c1a13f96f0cc3bf2307f922a45b4b90b3a8d3a7a7e9e1f0f0'
 */
export function calculateHash(base64String: string): string {
  //console.log(base64String);
  const wordArray = CryptoJS.enc.Base64.parse(base64String);
  const hash = CryptoJS.SHA256(wordArray);
  return hash.toString(CryptoJS.enc.Hex);
}

/**
 * 将文件转换为Base64字符串数组
 * @description 读取文件内容并将其分割成指定大小的Base64字符串块
 * @param {File} file - 要转换的文件对象
 * @param {number} chunkSize - 每个块的大小（字节）
 * @returns {Promise<string[]>} 返回包含文件Base64块的Promise
 * @resolves {string[]} 当文件读取完成时，返回Base64字符串数组
 * @rejects {Error} 当文件读取失败时，返回错误信息
 * @example
 * const chunks = await fileToBase64(file, 25000000);
 * // 返回: ['base64_chunk_1', 'base64_chunk_2', ...]
 */
export function fileToBase64(file: File, chunkSize: number): Promise<string[]> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    let offset = 0;
    const base64Chunks: string[] = [];

    reader.onload = function (event) {
      if (!event.target?.result) {
        reject(new Error('File reading failed'));
        return;
      }
      const binaryString = new Uint8Array(event.target.result as ArrayBuffer).reduce(
        (data, byte) => data + String.fromCharCode(byte),
        '',
      );
      base64Chunks.push(btoa(binaryString));
      offset += chunkSize;
      if (offset < file.size) {
        readNextChunk();
      } else {
        console.log('切片完成,共' + base64Chunks.length + '个文件片段');
        resolve(base64Chunks);
      }
    };

    reader.onerror = reject;

    function readNextChunk() {
      const slice = file.slice(offset, offset + chunkSize);
      reader.readAsArrayBuffer(slice);
    }

    readNextChunk();
  });
}

/**
 * 将字符串分割成指定大小的块
 * @description 将长字符串分割成多个指定大小的子字符串块
 * @param {string} str - 要分割的原始字符串
 * @param {number} chunkSize - 每个块的大小（字符数）
 * @returns {string[]} 返回分割后的字符串数组
 * @example
 * const chunks = splitStringIntoChunks('HelloWorld', 3);
 * // 返回: ['Hel', 'loW', 'orl', 'd']
 */
export function splitStringIntoChunks(str: string, chunkSize: number): string[] {
  const chunks: string[] = [];
  for (let i = 0; i < str.length; i += chunkSize) {
    chunks.push(str.slice(i, i + chunkSize));
  }
  return chunks;
}
