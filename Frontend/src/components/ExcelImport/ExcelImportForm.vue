<script setup>
import { onMounted, ref, useTemplateRef } from 'vue';
import { NSpace, NButton, NInput, NForm, NModal, NGrid, NFormItemGi } from 'naive-ui';
import moment from 'moment';
import FileUpload from '@common/FileUpload/FileUpload.vue';
import { translate } from '@utility/i18n.js';

const sheetData = ref({}); //表单数据
const dialogShow = ref(false); //表单显隐
const formRef = useTemplateRef('formRef'); //表单控件

//表单规则
const sheetRules = {
    filePath: [
        {
            required: true,
            message: translate.t('导入文件') + translate.t('不能为空'),
            trigger: ['input', 'blur'],
        },
    ],
};

defineExpose({ open });
const emit = defineEmits(['sheetDone']);
const inPara = defineProps({});

onMounted(() => {});

//表单开启
function open() {
    dialogShow.value = true;
}

//提交
function btnSubmit() {
    formRef.value
        ?.validate((errors) => {
            if (!errors) {
                emit('sheetDone', sheetData.value.filePath);
                dialogShow.value = false;
            }
        })
        .catch((err) => {
            window.$message.warning(err[0][0].message);
        });
}

//取消
function btnCancel() {
    sheetData.value = {};
    dialogShow.value = false;
}
//filePath上传返回
function filePath_uploadDone(data) {
    sheetData.value.filePath = data.file_fullpath;
    setTimeout(() => {
        btnSubmit();
    }, 0);
}
</script>

<template>
    <NModal
        :title="translate.t('导入')"
        v-model:show="dialogShow"
        preset="card"
        :style="{ width: '60%' }"
        :segmented="{ footer: 'soft' }"
        :on-after-leave="btnCancel"
    >
        <NForm
            ref="formRef"
            :rules="sheetRules"
            :model="sheetData"
            label-placement="left"
            :label-width="120"
            require-mark-placement="right-hanging"
            size="medium"
        >
            <NGrid :cols="24" :x-gap="24">
                <NFormItemGi :span="24" :label="translate.t('上传导入文件')" path="filePath">
                    <NSpace style="width: 100%" vertical>
                        <NInput
                            disabled
                            :placeholder="translate.t('请返回') + ' ' + translate.t('导入文件')"
                            v-model:value="sheetData.filePath"
                        />
                        <FileUpload @uploadDone="filePath_uploadDone"></FileUpload>
                    </NSpace>
                </NFormItemGi>
            </NGrid>
        </NForm>
        <template #footer>
            <NSpace justify="center">
                <NButton type="primary" @click="btnSubmit">{{ translate.t('提交') }}</NButton>
                <NButton type="primary" @click="btnCancel">{{ translate.t('取消') }}</NButton>
            </NSpace>
        </template>
    </NModal>
</template>
<style scoped></style>
