import { AddFileData, UploadChunk, UploadChunkEnd } from '@request/File_Request.js';
import { fileToBase64, calculateHash } from './uploadChunk.js';
import { makeTPGuid } from '@utility/makeGuid';

//上传执行
export function exeUpload(fileData, ok, error, bigFileMsg1, bigFileMsg2) {
    if (fileData.file_length > 25000000) {
        bigFileMsg1();
        fileToBase64(fileData.content, 25000000).then((chunks) => {
            //let fileHash = calculateHash(fileAllText);

            if (chunks && chunks.length > 0) {
                bigFileMsg2(chunks.length);
                //let chunks = splitStringIntoChunks(fileAllText, 25000000);
                let fileHash = '';
                let taskId = makeTPGuid();
                let promiseTasks = [];

                for (let i = 0; i < chunks.length; i++) {
                    const element = chunks[i];
                    fileHash += calculateHash(element);
                    let taskChunk = new Promise((resolve, reject) => {
                        UploadChunk({ taskId: taskId, Sort: i, fileChunkContent: element }).then(
                            (res) => {
                                resolve(res);
                            }
                        );
                    });
                    promiseTasks.push(taskChunk);
                }

                Promise.all(promiseTasks).then(() => {
                    UploadChunkEnd({
                        taskId: taskId,
                        total: chunks.length,
                        fileName: fileData.file_name,
                        hashCode: fileHash,
                    })
                        .then((fileRes) => {
                            fileData.file_fullpath = fileRes.filePath;
                            ok(fileRes.msg);
                        })
                        .catch((e) => {
                            console.log('异常信息', e);
                            error(`接口异常,文件上传失败`);
                        });
                });
            } else {
                error(`文件过大，请联系管理员`);
            }
        });
        // .error(() => {
        //     error(`文件过大，请联系管理员`);
        // });
    } else {
        let upLoadData = new FormData();
        upLoadData.append('files', fileData.content);

        AddFileData(upLoadData)
            .then((fileRes) => {
                fileData.file_fullpath = fileRes.filePath;
                ok(fileRes.msg);
            })
            .catch((e) => {
                console.log('异常信息', e);
                error(`接口异常,文件上传失败`);
            });
    }
}
