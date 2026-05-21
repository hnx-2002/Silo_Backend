<script setup>
import { NCard, NCollapse, NCollapseItem } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '1'">
        <n-collapse>
            <n-collapse-item :title="translate.t('性别')">
                <div>男</div>
                <div>女</div>
            </n-collapse-item>
            <n-collapse-item :title="translate.t('员工')">
                <div>张三三</div>
                <div>李思思</div>
            </n-collapse-item>
        </n-collapse>
    </NCard>
</template>
<style scoped></style>
