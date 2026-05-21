<script setup>
import { NSpace, NButton } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical justify="center" :align="'center'" :wrap-item="true">
        <div id="contentArea">此处为内容区域</div>
        <div id="blankArea"></div>
        <NSpace justify="center">
            <NButton type="primary">{{ translate.t('确定') }}</NButton>
            <NButton type="info">{{ translate.t('取消') }}</NButton>
        </NSpace>
    </NSpace>
</template>
<style scoped>
#contentArea {
    width: 300px;
    height: 200px;
    background-color: darksalmon;
    text-align: center;
}
#blankArea {
    height: 20px;
}
</style>
