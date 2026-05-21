<script setup>
import Hls from 'hls.js';

defineExpose({ previewM3U8 });
const emit = defineEmits([]);
const inPara = defineProps({});

function previewM3U8(inPath) {
    setTimeout(() => {
        var video = document.getElementById('previewVideo');
        if (Hls.isSupported()) {
            var config = {
                maxBufferLength: 30, // 增加缓冲区长度
                maxMaxBufferLength: 600, // 最大缓冲区长度
                enableWorker: true, // 启用Web Worker
                lowLatencyMode: true, // 低延迟模式
            };

            var hls = new Hls(config);
            hls.loadSource(inPath);
            hls.attachMedia(video);
            hls.on(Hls.Events.MANIFEST_PARSED, () => {
                video.play();
            });
            hls.on(Hls.Events.ERROR, (event, data) => {
                console.error('HLS.js error:', data);
            });
            hls.on(Hls.Events.FRAG_LOADED, (event, data) => {
                console.log('Fragment loaded:', data);
            });

            hls.on(Hls.Events.BUFFER_APPENDED, (event, data) => {
                console.log('Buffer appended:', data);
            });
        } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
            video.src = inPath;
            video.addEventListener('canplay', () => {
                video.play();
            });
        }
    }, 1);
}
</script>
<template>
    <video id="previewVideo" controls style="width: 100%; height: 100%"></video>
</template>
<style scoped></style>
