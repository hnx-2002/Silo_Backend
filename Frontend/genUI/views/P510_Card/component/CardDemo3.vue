<script setup>
import { NCard, NCollapse, NCollapseItem } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '2'">
        <n-card :title="translate.t('卡片标题')">
            <div style="height: 200px; width: 100%; background-color: #ff7200">内容区域</div>
        </n-card>
    </NCard>
</template>
<style scoped></style>
