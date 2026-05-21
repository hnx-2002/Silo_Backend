import CryptoJS from 'crypto-js';

//计算hash
export function calculateHash(base64String) {
    //console.log(base64String);
    const wordArray = CryptoJS.enc.Base64.parse(base64String);
    const hash = CryptoJS.SHA256(wordArray);
    return hash.toString(CryptoJS.enc.Hex);
}
  
//文件转为base64
export function fileToBase64(file, chunkSize) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        let offset = 0;
        const base64Chunks = [];

        reader.onload = function (event) {
            const binaryString = new Uint8Array(event.target.result).reduce(
                (data, byte) => data + String.fromCharCode(byte),
                ''
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

//将字符串分成小块
export function splitStringIntoChunks(str, chunkSize) {
    var chunks = [];
    for (var i = 0; i < str.length; i += chunkSize) {
        chunks.push(str.slice(i, i + chunkSize));
    }
    return chunks;
}
