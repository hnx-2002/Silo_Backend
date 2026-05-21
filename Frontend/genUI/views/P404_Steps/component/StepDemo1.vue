<script setup>
import { ref } from 'vue';
import {
    NSpace,
    NCard,
    NSteps,
    NStep,
    NButtonGroup,
    NButton,
    NRadioGroup,
    NRadioButton,
} from 'naive-ui';

import { translate } from '@utility/i18n.js';

const current = ref(1);

const currentStatus = ref('process');

//向前
function prev() {
    if (current.value === 0) current.value = null;
    else if (current.value === null) current.value = 4;
    else current.value--;
}

//向后
function next() {
    if (current.value === null) current.value = 1;
    else if (current.value >= 4) current.value = null;
    else current.value++;
}
</script>

<template>
    <NCard :title="translate.t('基础用法')">
        <NSpace vertical>
            <n-steps :current="current" :status="currentStatus">
                <n-step :title="translate.t('填写信息')" />
                <n-step :title="translate.t('修改密码')" />
                <n-step :title="translate.t('提交信息')" />
                <n-step :title="translate.t('完成')" />
            </n-steps>
            <n-space>
                <n-button-group>
                    <n-button @click="prev">👈</n-button>
                    <n-button @click="next">👉</n-button>
                </n-button-group>
                <n-radio-group v-model:value="currentStatus">
                    <n-radio-button value="error">错误</n-radio-button>
                    <n-radio-button value="process">进行中</n-radio-button>
                    <n-radio-button value="wait">等待中</n-radio-button>
                    <n-radio-button value="finish">已完成</n-radio-button>
                </n-radio-group>
            </n-space>
        </NSpace>
    </NCard>
</template>
<style scoped></style>
