<script setup lang="ts">
import { onMounted, ref, useTemplateRef } from 'vue';
import { NDrawer, NDrawerContent } from 'naive-ui';
import FilePreview from './FilePreview.vue';

const show = ref(false);
const fileName = ref('');
const FilePreviewRef = useTemplateRef('FilePreviewRef'); //预览控件

const inPara = defineProps({});
defineExpose({ openPreviewerDrawer });

onMounted(() => {});

function openPreviewerDrawer(filePath: string) {
  fileName.value = getFileName(filePath) || '';
  show.value = true;
  setTimeout(() => {
    FilePreviewRef.value?.preview(filePath);
  }, 1);
}

/**
 * 提取纯文件名（含扩展名）
 * @param {string} filePath 任意文件地址，如
 *   http://xxxx/xxxx/aaa.pdf?token=123
 *   C:\Users\xxx\bbb.PNG
 *   /usr/local/ccc.JPEG#hash
 * @returns {string} 文件名；若无法解析返回空字符串
 */
function getFileName(filePath = '') {
  // 1. 去掉查询参数或 hash
  const clean = filePath.split(/[?#]/)[0];
  // 2. 取最后一个路径分隔符后的部分
  const name = clean?.slice(Math.max(clean.lastIndexOf('/'), clean.lastIndexOf('\\')) + 1);
  return name;
}
</script>
<template>
  <n-drawer v-model:show="show" :width="'100%'" :placement="'right'" :block-scroll="false">
    <n-drawer-content :title="fileName" closable>
      <FilePreview ref="FilePreviewRef"></FilePreview>
    </n-drawer-content>
  </n-drawer>
</template>
<style scoped></style>
