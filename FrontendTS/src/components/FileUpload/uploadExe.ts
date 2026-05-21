import { AddFileData, UploadChunk, UploadChunkEnd } from '@request/File_Request.ts';
import { fileToBase64, calculateHash } from './uploadChunk.ts';
import { makeTPGuid } from '@utility/makeGuid.ts';
import type { FileData, Res_UploadFile } from './FileUpload.d.ts';

//上传执行
export function exeUpload(
  fileData: FileData,
  ok: (msg: string) => void,
  error: (msg: string) => void,
  bigFileMsg1: () => void,
  bigFileMsg2: (count: number) => void,
) {
  if (fileData.file_length && fileData.file_length! > 25000000) {
    bigFileMsg1();
    fileToBase64(fileData.content!, 25000000)
      .then((chunks: string[]) => {
        //let fileHash = calculateHash(fileAllText);

        if (chunks && chunks.length > 0) {
          bigFileMsg2(chunks.length);
          //let chunks = splitStringIntoChunks(fileAllText, 25000000);
          let fileHash = '';
          let taskId = makeTPGuid();
          let promiseTasks: Promise<boolean>[] = [];

          for (let i = 0; i < chunks.length; i++) {
            const element = chunks[i] || '';
            fileHash += calculateHash(element);
            let taskChunk = new Promise<boolean>((resolve, reject) => {
              UploadChunk({ taskId: taskId, Sort: i, fileChunkContent: element })
                .then((res) => {
                  resolve(res.status);
                })
                .catch(reject);
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
              .then((fileRes: Res_UploadFile) => {
                fileData.file_fullpath = fileRes.filePath;
                ok(fileRes.msg);
              })
              .catch((e: Error) => {
                console.log('异常信息', e);
                error(`接口异常,文件上传失败`);
              });
          });
        } else {
          error(`文件过大，请联系管理员`);
        }
      })
      .catch((e: Error) => {
        console.log('异常信息', e);
        error(`文件处理失败`);
      });
  } else {
    let upLoadData = new FormData();
    upLoadData.append('files', fileData.content!);

    AddFileData(upLoadData)
      .then((fileRes: Res_UploadFile) => {
        fileData.file_fullpath = fileRes.filePath;
        ok(fileRes.msg);
      })
      .catch((e: Error) => {
        console.log('异常信息', e);
        error(`接口异常,文件上传失败`);
      });
  }
}
