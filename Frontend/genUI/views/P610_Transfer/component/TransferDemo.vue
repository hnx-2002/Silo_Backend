<script setup>
import { NCard, NTransfer, NSpace } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const options = [
    {
        label: 'Option1',
        value: 'Option1',
    },
    {
        label: 'Option2',
        value: 'Option2',
    },
    {
        label: 'Option3',
        value: 'Option3',
    },
    {
        label: 'Option4',
        value: 'Option4',
    },
    {
        label: 'Option5',
        value: 'Option5',
    },
    {
        label: 'Option6',
        value: 'Option6',
    },
];
</script>

<template>
    <NCard :title="translate.t('示例') + '1'">
        <!-- <NText style="color: red">需要自行实现，以响应规定</NText> -->
        <NSpace vertical>
            <NTransfer class="transDemo" :options="options" />
            <NTransfer class="transDemo" :options="options" source-filterable target-filterable />
        </NSpace>
    </NCard>
</template>
<style scoped>
.transDemo {
    width: 400px;
}
</style>
