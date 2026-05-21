import request from '@utility/request.ts';

export function AddFileData(data: FormData | object) {
  return request({
    url: '/T2ACore/UploadFile/UploadFile',
    method: 'post',
    data,
  });
}

export function UploadChunk(data: FormData | object) {
  return request({
    url: '/T2ACore/UploadFile/UploadChunk',
    method: 'post',
    data,
  });
}

export function UploadChunkEnd(data: FormData | object) {
  return request({
    url: '/T2ACore/UploadFile/UploadChunkEnd',
    method: 'post',
    data,
  });
}
