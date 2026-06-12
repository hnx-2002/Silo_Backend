<script setup lang="ts">
import { onMounted, ref, useTemplateRef } from 'vue';
import { NImage } from 'naive-ui';
import Previewer_PDFWithToolBar from './PDFJS/Previewer_PDFWithToolBar.vue';
import Previewer_M3U8 from './M3U8/Previewer_M3U8.vue';

import JsPreviewDocx from '@js-preview/docx';
import '@js-preview/docx/lib/index.css';
import JsPreviewExcel from '@js-preview/excel';
import '@js-preview/excel/lib/index.css';

import { XMarkdown } from 'vue-element-plus-x';

//import { translate } from '@utility/i18n.ts';

const filePath = ref(''); //文档路径
const fileType = ref(''); //文档类型
const fileText = ref(''); //文件文本内容

const fileTypeMap: Record<string, string> = {
  pdf: 'pdf',
  png: 'pic',
  jpg: 'pic',
  docx: 'word',
  xlsx: 'excel',
  mp4: 'mp4',
  md: 'md',
};

const Previewer_M3U8Ref = useTemplateRef('Previewer_M3U8Ref'); //视频控件
const Previewer_PDFWithToolBarRef = useTemplateRef('Previewer_PDFWithToolBarRef'); //PDF预览控件

const inPara = defineProps({});

defineExpose({ preview });

onMounted(() => {});

//预览
function preview(filePreviewPath: string) {
  const lowerExt = getExt(filePreviewPath);
  filePath.value = filePreviewPath;
  fileType.value = fileTypeMap[lowerExt] || '';
  setTimeout(() => {
    if (fileType.value == 'mp4') {
      Previewer_M3U8Ref.value?.previewM3U8(filePath.value);
    } else {
      if (fileType.value == 'pdf') {
        Previewer_PDFWithToolBarRef.value?.preview(filePath.value, '', 0, 660);
      } else if (fileType.value == 'word') {
        const myDocxPreviewer = JsPreviewDocx.init(document.getElementById('docx-container')!);
        myDocxPreviewer.preview(filePath.value);
      } else if (fileType.value == 'excel') {
        const myXlsxPreviewer = JsPreviewExcel.init(document.getElementById('xlsx-container')!);
        myXlsxPreviewer.preview(filePath.value);
      } else if (fileType.value == 'md') {
        fetch(filePath.value).then((res) => {
          res.text().then((content) => {
            fileText.value = content;
          });
        });
      }
    }
  }, 1);
}

/**
 * 提取文件扩展名（小写、不带点）
 * @param {string} filePath 任意文件地址，如
 *   http://xxxx/xxxx/aaa.pdf?token=123
 *   C:\Users\xxx\bbb.PNG
 *   /usr/local/ccc.JPEG#hash
 * @returns {string} 扩展名（小写）；无扩展名时返回空字符串
 */
function getExt(filePath = '') {
  // 1. 去掉查询参数或 hash
  const clean = filePath.split(/[?#]/)[0];
  // 2. 取最后一个 . 之后的部分
  const ext = clean?.slice((clean?.lastIndexOf('.') >>> 0) + 1) || '';
  return ext.toLowerCase();
}
</script>
<template>
  <Previewer_PDFWithToolBar
    v-if="fileType == 'pdf'"
    ref="Previewer_PDFWithToolBarRef"
    :useToolBar="true"
    :useToolBar_Pages="true"
    :useToolBar_Download="true"
  ></Previewer_PDFWithToolBar>
  <template v-if="fileType != 'pdf'">
    <NImage v-if="fileType == 'pic'" :src="filePath" :img-props="{ style: { width: '100%' } }" />
    <div id="docx-container" v-if="fileType == 'word'" style="height: 100%"></div>
    <div id="xlsx-container" v-if="fileType == 'excel'" style="height: 100%"></div>
    <Previewer_M3U8
      ref="Previewer_M3U8Ref"
      v-if="fileType == 'mp4'"
      :src="filePath"
    ></Previewer_M3U8>
    <XMarkdown :markdown="fileText || ''" allowHtml></XMarkdown>
  </template>
</template>
<style scoped></style>
