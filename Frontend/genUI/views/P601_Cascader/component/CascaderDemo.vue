<script setup>
import { NCard, NCascader } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const options = [
    {
        label: '一级选项1',
        value: '1_1',
        children: [
            {
                label: '二级选项1_1',
                value: '1_1_1',
            },
            {
                label: '二级选项1_2',
                value: '1_1_2',
            },
            {
                label: '二级选项1_3',
                value: '1_1_3',
            },
        ],
    },
    {
        label: '一级选项2',
        value: '1_2',
    },
    {
        label: '一级选项3',
        value: '1_3',
    },
];
</script>

<template>
    <NCard :title="translate.t('示例') + '1'">
        <n-cascader :options="options" :check-strategy="'all'" :show-path="true" />
    </NCard>
</template>
<style scoped></style>
