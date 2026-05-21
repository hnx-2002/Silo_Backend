<script setup>
import { NCard, NCollapse, NCollapseItem } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '1'">
        <n-card title="卡片" hoverable>卡片内容</n-card>
    </NCard>
</template>
<style scoped></style>
