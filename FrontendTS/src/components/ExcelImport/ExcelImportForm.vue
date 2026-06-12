<script setup lang="ts">
import { onMounted, ref, useTemplateRef } from 'vue';
import { NSpace, NButton, NInput, NForm, NModal, NGrid, NFormItemGi } from 'naive-ui';
import FileUpload from '@common/FileUpload/FileUpload.vue';
import type { FileData } from '@common/FileUpload/FileUpload.d.ts';
import { translate } from '@utility/i18n.ts';

const excelFilePath = ref<string>(''); //文件地址
const dialogShow = ref<boolean>(false); //表单显隐
const formRef = useTemplateRef<InstanceType<typeof NForm>>('formRef'); //表单控件

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
        emit('sheetDone', excelFilePath.value);
        dialogShow.value = false;
      }
    })
    .catch((err: any) => {
      window.$message.warning(err[0][0].message);
    });
}

//取消
function btnCancel() {
  excelFilePath.value = '';
  dialogShow.value = false;
}
//filePath上传返回
function filePath_uploadDone(File: FileData) {
  excelFilePath.value = File.file_fullpath || '';
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
      label-placement="left"
      :label-width="120"
      require-mark-placement="right-hanging"
      size="medium"
    >
      <NGrid :cols="24" :x-gap="24">
        <NFormItemGi :span="24" :label="translate.t('上传导入文件')" path="">
          <NSpace style="width: 100%" vertical>
            <NInput
              disabled
              :placeholder="translate.t('请返回') + ' ' + translate.t('导入文件')"
              v-model:value="excelFilePath"
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
