<script setup>
    import { onMounted, ref } from 'vue';
    import { NButton, NInputNumber, NFlex, NIcon, NScrollbar } from 'naive-ui';
    import Previewer_PDF from './Previewer_PDF.vue';
    import prev from './icon/prev.vue';
    import next from './icon/next.vue';
    import jump from './icon/jump.vue';
    import zoomin from './icon/zoomin.vue';
    import zoomout from './icon/zoomout.vue';
    import rotate1 from './icon/rotate1.vue';
    import rotate2 from './icon/rotate2.vue';
    import download from './icon/download.vue';
    import { frontDownload } from '@utility/frontDownload.js';
    import { translate } from '@utility/i18n.js';

    const Previewer_PDFRef = ref(); //pdf预览控件
    const pageNum = ref(1); //绑定输入框的值

    const currentFilePath = ref('');

    defineExpose({ preview, getScrollNum, setScrollNum });
    const emit = defineEmits(['loadDone']);
    const inPara = defineProps({
        useToolBar: Boolean, //是否启用工具栏，此处为否后续均不起作用
        useToolBar_Pages: Boolean, //是否启用工具栏中的页面操作部分
        useToolBar_Download: Boolean, //是否启用工具栏中的下载按钮
    });

    onMounted(() => { });

    function preview(filePath, fileName, width, height) {
        currentFilePath.value = filePath;
        if (!!filePath) {
            Previewer_PDFRef.value.previewPDF(filePath, fileName, width, height);
        } else {
            window.$message.info(translate.t('文件错误'));
        }
    }

    //加载完成
    function loadDone() {
        emit('loadDone');
    }

    function getScrollNum() {
        Previewer_PDFRef.value.getCurrentScrollNum();
    }

    function setScrollNum(val) {
        Previewer_PDFRef.value.setCurrentScrollNum(val);
    }

    //跳页
    function toPage(pageNum) {
        Previewer_PDFRef.value.pageTo(pageNum || pageNum.value);
    }

    //前一页
    function toPrev() {
        let currentPageNum = Previewer_PDFRef.value.getCurrentPageNum();
        toPage(currentPageNum - 1);
    }

    //后一页
    function toPost() {
        let currentPageNum = Previewer_PDFRef.value.getCurrentPageNum();
        toPage(currentPageNum + 1);
    }

    //下载
    function downloadFile() {
        frontDownload(currentFilePath.value);
        // currentFilePath.value
    }

    //改变呈现页码
    function changeCurrentPage(data) {
        pageNum.value = data;
    }

    //修改比例
    function changePageScale(val) {
        Previewer_PDFRef.value.updateCurrentPageScale(val);
    }

    //旋转页面
    function rotatePage(val) {
        Previewer_PDFRef.value.rotateCurrentPage(val);
    }
</script>
<template>
    <NFlex vertical style="width: 100%; height: 100%">
        <NFlex v-if="inPara.useToolBar" justify="space-between">
            <!-- <NButton @click="show">打印位置</NButton> -->
            <NFlex>
                <NButton type="primary" @click="toPrev">
                    <template #icon>
                        <NIcon>
                            <prev></prev>
                        </NIcon>
                    </template>
                    {{ translate.t('前一页') }}
                </NButton>
                <NButton type="primary" @click="toPost">
                    <template #icon>
                        <NIcon>
                            <next></next>
                        </NIcon>
                    </template>
                    {{ translate.t('后一页') }}
                </NButton>
                <NInputNumber v-model:value="pageNum" style="width: 100px"></NInputNumber>
                <NButton type="primary" @click="toPage(pageNum)">
                    <template #icon>
                        <NIcon>
                            <jump></jump>
                        </NIcon>
                    </template>
                    {{ translate.t('跳转') }}
                </NButton>
            </NFlex>
            <NFlex>
                <NButton type="primary" @click="changePageScale(-0.25)">
                    <template #icon>
                        <NIcon>
                            <zoomin></zoomin>
                        </NIcon>
                    </template>
                    {{ translate.t('缩小') }}
                </NButton>
                <NButton type="primary" @click="changePageScale(0.25)">
                    <template #icon>
                        <NIcon>
                            <zoomout></zoomout>
                        </NIcon>
                    </template>
                    {{ translate.t('放大') }}
                </NButton>
            </NFlex>
            <NFlex>
                <NButton type="primary" @click="rotatePage(false)">
                    <template #icon>
                        <NIcon>
                            <rotate1></rotate1>
                        </NIcon>
                    </template>
                    {{ translate.t('逆时针') }}
                </NButton>
                <NButton type="primary" @click="rotatePage(true)">
                    <template #icon>
                        <NIcon>
                            <rotate2></rotate2>
                        </NIcon>
                    </template>
                    {{ translate.t('顺时针') }}
                </NButton>
            </NFlex>
            <NFlex>
                <NButton type="primary" @click="downloadFile">
                    <template #icon>
                        <NIcon>
                            <download></download>
                        </NIcon>
                    </template>
                    {{ translate.t('下载') }}
                </NButton>
            </NFlex>
        </NFlex>

        <Previewer_PDF style="flex: 1; height: 100px"
                       ref="Previewer_PDFRef"
                       @loadDone="loadDone"
                       @changeCurrentPage="changeCurrentPage"></Previewer_PDF>
    </NFlex>
</template>
<style></style>
