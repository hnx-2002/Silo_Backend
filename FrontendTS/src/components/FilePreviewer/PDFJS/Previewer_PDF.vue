<script setup lang="ts">
import { onMounted, ref } from 'vue';
import * as pdfjsLib from 'pdfjs-dist';
import 'pdfjs-dist/web/pdf_viewer.css';
import 'pdfjs-dist/web/pdf_viewer.mjs';
import 'pdfjs-dist/build/pdf.worker.mjs';
import { NFlex, NProgress, NScrollbar } from 'naive-ui';

const pdfContainer = ref<HTMLDivElement | null>(null);
const pdfScroller = ref<HTMLElement | null>(null);

const currentPageNum = ref(1); //当前页码

const filePath = ref(''); //文件地址
const currentScale = ref(1.5); // 当前页的缩放比例

let pages: Record<number, pdfjsLib.PDFPageProxy> = {}; // 用于存储每一页的 page 对象
let canvases: Record<number, HTMLCanvasElement> = {}; // 用于存储每一页的 canvas 对象

const loadPercent = ref(0); // 加载百分比
const pageStates = ref<Record<number, { scale: number; rotation: number }>>({}); // 用于存储每个页面的状态，包括缩放和旋转

defineExpose({
  previewPDF,
  getCurrentScrollNum,
  setCurrentScrollNum,
  pageTo,
  getCurrentPageNum,
  updateCurrentPageScale,
  rotateCurrentPage,
});
const emit = defineEmits(['loadDone', 'changeCurrentPage']);
const inPara = defineProps({});

onMounted(() => {
  //
});

function pdfScroll(e: Event) {
  let container = e.target as HTMLElement;
  let allCanvas = document.getElementsByClassName('pdf-canvas');
  const scrollTop = container.scrollTop;
  let currentPage = 1;
  let accumulatedHeight = 0;
  for (let i = 0; i < allCanvas.length; i++) {
    let child = allCanvas[i] as HTMLElement;
    accumulatedHeight += child.offsetHeight;
    if (scrollTop < accumulatedHeight) {
      break;
    }
    currentPage++;
  }
  currentPageNum.value = currentPage;
  emit('changeCurrentPage', currentPage);
}

function previewPDF(inPath: string, fileTitle: string = '', _domW?: number, _domH?: number) {
  loadPercent.value = 0;
  filePath.value = inPath;

  pages = {};
  canvases = {};
  pageStates.value = {}; // 重置页面状态

  if (!inPath) {
    window.$message.info('文件错误');
  }

  let _fileName = '';
  if (!!fileTitle) {
    _fileName = fileTitle;
  } else {
    let pieces = inPath.split('/');
    _fileName = pieces[pieces.length - 1] || '';
  }

  setTimeout(() => {
    pdfjsLib
      .getDocument(inPath)
      .promise.then((pdf) => {
        const numPages = pdf.numPages;
        let renderPromise = Promise.resolve();

        for (let i = 1; i <= numPages; i++) {
          renderPromise = renderPromise.then(() => {
            return pdf.getPage(i).then((page) => {
              let tempPercent = (i / numPages) * 0.95;
              loadPercent.value = parseFloat((tempPercent * 100).toFixed(2));
              pages[i] = page; // 存储 page 对象
              return renderPage(page, pdfContainer.value, i);
            });
          });
        }
        renderPromise.then(() => {
          loadPercent.value = 100;
          emit('loadDone');
        });
      })
      .catch((error) => {
        console.error('Error rendering PDF:', error);
      });
  }, 1);
}

function renderPage(page: pdfjsLib.PDFPageProxy, parentDom: HTMLElement | null, pageIdx: number) {
  if (!parentDom) {
    return Promise.resolve();
  }
  const canvas = createCanvas(page, pageIdx);
  parentDom.appendChild(canvas);
  const renderContext = createRenderContext(page, canvas, pageIdx);
  canvases[pageIdx] = canvas; // 存储 canvas 对象
  return page.render(renderContext).promise;
}

function createCanvas(page: pdfjsLib.PDFPageProxy, pageIdx: number): HTMLCanvasElement {
  const scale = pageStates.value[pageIdx]?.scale || currentScale.value; // 使用页面状态中的缩放比例
  const viewport = page.getViewport({ scale });
  const canvas = document.createElement('canvas');
  canvas.height = viewport.height;
  canvas.width = viewport.width;
  canvas.style.display = 'block';
  canvas.style.marginBottom = '4px';
  canvas.id = 'canvas' + pageIdx;
  canvas.className = 'pdf-canvas';
  return canvas;
}

function createRenderContext(
  page: pdfjsLib.PDFPageProxy,
  canvas: HTMLCanvasElement,
  pageIdx: number,
) {
  const context = canvas.getContext('2d') || undefined;
  const scale = pageStates.value[pageIdx]?.scale || currentScale.value; // 使用页面状态中的缩放比例
  const rotation = pageStates.value[pageIdx]?.rotation || 0; // 使用页面状态中的旋转角度
  const viewport = page.getViewport({ scale, rotation });
  return {
    canvas: canvas,
    canvasContext: context,
    viewport: viewport,
  };
}

function getCurrentScrollNum() {
  return pdfContainer.value?.scrollTop || 0;
}

function setCurrentScrollNum(val: number) {
  if (pdfContainer.value) {
    pdfContainer.value.scrollTop = val;
  }
}

function pageTo(pageIdx: number) {
  if (!pageIdx || pageIdx < 0) {
    return;
  }
  let allCanvas = document.getElementsByClassName('pdf-canvas');
  let canvasId = 'canvas' + pageIdx;
  let totalHeight = 0;
  let pageNum = 0;
  for (let i = 0; i < allCanvas.length; i++) {
    const child = allCanvas[i] as HTMLElement;
    pageNum = i + 1;
    if (child.id == canvasId) {
      break;
    }
    totalHeight += child.offsetHeight;
  }

  if (pdfScroller.value) {
    pdfScroller.value.scrollTo({ top: totalHeight });
  }
  currentPageNum.value = pageNum;
  emit('changeCurrentPage', pageNum);
}

function getCurrentPageNum(): number {
  return currentPageNum.value;
}

function updateCurrentPageScale(changeScale: number) {
  const pageIdx = currentPageNum.value;
  const oldCanvas = canvases[pageIdx]; // 使用存储的 canvas
  const page = pages[pageIdx]; // 使用存储的 page

  if (oldCanvas && page) {
    // 初始化页面状态，如果不存在
    if (!pageStates.value[pageIdx]) {
      pageStates.value[pageIdx] = { scale: currentScale.value, rotation: 0 };
    }

    // 更新当前缩放比例
    pageStates.value[pageIdx].scale += changeScale; // 更新页面状态中的缩放比例
    const scale = pageStates.value[pageIdx].scale; // 获取更新后的缩放比例
    const rotation = pageStates.value[pageIdx].rotation; // 获取当前旋转角度

    const viewport = page.getViewport({ scale, rotation });
    const renderContext = {
      canvas: oldCanvas,
      canvasContext: oldCanvas.getContext('2d') || undefined,
      viewport: viewport,
    };
    page.render(renderContext);
    oldCanvas.height = viewport.height;
    oldCanvas.width = viewport.width;
  }
}

function rotateCurrentPage(isClockwise: boolean) {
  const pageIdx = currentPageNum.value;
  const oldCanvas = canvases[pageIdx]; // 使用存储的 canvas
  const page = pages[pageIdx]; // 使用存储的 page

  if (oldCanvas && page) {
    // 初始化页面状态，如果不存在
    if (!pageStates.value[pageIdx]) {
      pageStates.value[pageIdx] = { scale: currentScale.value, rotation: 0 };
    }

    // 更新旋转角度
    if (isClockwise) {
      pageStates.value[pageIdx].rotation = (pageStates.value[pageIdx].rotation + 90) % 360;
    } else {
      pageStates.value[pageIdx].rotation = (pageStates.value[pageIdx].rotation - 90 + 360) % 360;
    }

    const scale = pageStates.value[pageIdx].scale; // 获取当前缩放比例
    const rotation = pageStates.value[pageIdx].rotation; // 获取更新后的旋转角度
    const viewport = page.getViewport({ scale, rotation });

    const renderContext = {
      canvas: oldCanvas,
      canvasContext: oldCanvas.getContext('2d') || undefined,
      viewport: viewport,
    };
    page.render(renderContext);
    oldCanvas.height = viewport.height;
    oldCanvas.width = viewport.width;
  }
}
</script>
<template>
  <NFlex vertical>
    <NProgress
      v-if="loadPercent < 100"
      type="line"
      :percentage="loadPercent"
      indicator-placement="inside"
      processing
      :style="{
        marginTop: '6px',
      }"
    />
    <NScrollbar ref="pdfScroller" x-scrollable @scroll="pdfScroll">
      <div
        ref="pdfContainer"
        :style="{
          backgroundColor: '#CCCCCC',
          textAlign: 'center', // 水平居中
          border: '1px solid #CCCCCC', // 可选：添加边框以便于调试
          display: 'flex',
          flexDirection: 'column', // 竖直排列
          alignItems: 'center', // 居中对齐
        }"
      ></div>
    </NScrollbar>
  </NFlex>
</template>
<style scoped></style>
