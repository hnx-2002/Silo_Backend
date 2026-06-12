import request from '@utility/request.ts';

const NAMESPACE = import.meta.env.VITE_NAMESPACE;

export function AddFileData(data: FormData | object) {
  return request({
    url: '/' + NAMESPACE + '/UploadFile/UploadFile',
    method: 'post',
    data,
  });
}

export function UploadChunk(data: FormData | object) {
  return request({
    url: '/' + NAMESPACE + '/UploadFile/UploadChunk',
    method: 'post',
    data,
  });
}

export function UploadChunkEnd(data: FormData | object) {
  return request({
    url: '/' + NAMESPACE + '/UploadFile/UploadChunkEnd',
    method: 'post',
    data,
  });
}
