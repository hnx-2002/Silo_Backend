<script setup>
import { NCard, NCollapse, NCollapseItem } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '2'">
        <n-card :title="translate.t('卡片标题')">
            <template #cover>
                <div style="height: 200px; width: 300px; background-color: #ff7200">图片示意</div>
            </template>
        </n-card>
    </NCard>
</template>
<style scoped></style>
