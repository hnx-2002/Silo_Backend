import request from '@utility/request';

//单个上传
export function AddFileData(data) {
    return request({
        url: '/T2ACore/UploadFile/UploadFile',
        method: 'post',
        data,
    });
}

//分块上传
export function UploadChunk(data) {
    return request({
        url: '/T2ACore/UploadFile/UploadChunk',
        method: 'post',
        data,
    });
}

//分块上传完成
export function UploadChunkEnd(data) {
    return request({
        url: '/T2ACore/UploadFile/UploadChunkEnd',
        method: 'post',
        data,
    });
}
