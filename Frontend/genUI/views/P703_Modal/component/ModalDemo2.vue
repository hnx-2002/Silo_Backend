<script setup>
import { NSpace } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical :align="'center'" :wrap-item="false">
        <div id="iconArea"></div>
        <div id="blankArea"></div>
        <div id="contentArea">此处为内容区域</div>
    </NSpace>
</template>
<style scoped>
#iconArea {
    width: 44px;
    height: 44px;
    background-color: #cccccc;
    text-align: center;
}
#blankArea {
    height: 22px;
}
#contentArea {
    width: 300px;
    height: 100px;
    background-color: darksalmon;
    text-align: center;
}
</style>
