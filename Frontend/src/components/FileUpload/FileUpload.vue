<script setup>
import { onMounted, ref } from 'vue';
import { NUpload, NButton, NSpace, NSpin } from 'naive-ui';
import { exeUpload } from './uploadExe.js';
import { translate } from '@utility/i18n.js';

const fileData = ref({}); //返回的文件详细信息
const showSpin = ref(false);

const inPara = defineProps({
    disabled: Boolean,
});
const emit = defineEmits(['uploadDone']);
onMounted(() => {});

//选择文件变化
function beforeUpload(data) {
    let item = data.file;
    if (item.file.size == 0) {
        window.$message.error(
            translate.t('文件') +
                data.file.file.name +
                translate.t('为空') +
                ',' +
                translate.t('请重新选择')
        );
        return;
    }

    let arr = item.name.split('.');

    fileData.value = {
        file_id: item.id,
        file_name: item.name, //文件名
        file_length: item.file.size, //文件大小
        file_ext: arr[arr.length - 1], //文件扩展名
        content: item.file, //文件内容
    };

    showSpin.value = true;
    exeUpload(
        fileData.value,
        (okMsg) => {
            emit('uploadDone', fileData.value);
            window.$message.success(translate.t(okMsg));
            showSpin.value = false;
        },
        (errorMsg) => {
            window.$message.error(translate.t(errorMsg));
            showSpin.value = false;
        },
        () => {
            window.$message.info(translate.t('文件较大') + ',' + translate.t('请耐心等待'));
        },
        (len) => {
            window.$message.info(
                translate.t('文件片段数') + ':' + len + ',' + translate.t('请耐心等待')
            );
        }
    );
}
//移除
function remove(data) {
    fileData.value = {
        file_id: '',
        file_name: '', //文件名
        file_length: 0, //文件大小
        file_ext: '', //文件扩展名
        file_fullpath: '',
    };
    emit('uploadDone', fileData.value);
}
</script>
<template>
    <NSpin :show="showSpin">
        <NSpace vertical style="width: 100%">
            <!-- <NButton @click="exeUpload">上传</NButton> -->
            <NUpload
                :directory="false"
                :show-file-list="true"
                :default-upload="false"
                :multiple="false"
                :max="1"
                :on-before-upload="beforeUpload"
                :on-remove="remove"
            >
                <NButton :disabled="inPara.disabled" type="primary">
                    {{ translate.t('选择文件') }}
                </NButton>
            </NUpload>
        </NSpace>
    </NSpin>
</template>
<style scoped></style>
