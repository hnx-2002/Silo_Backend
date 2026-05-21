<script setup>
import { NSpace, NSwitch, NCard } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';

const act1 = false;
const act2 = true;
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="translate.t('需要表示开关状态/两种状态之间的切换时')"
        ></TitleDescription>
        <NText style="color: red">确定规定是这么小的么？13px X 22px</NText>
        <NCard :title="translate.t('示例')">
            <n-space item-style="display: flex;">
                <n-switch v-model:value="act1" />
                <n-switch v-model:value="act1" disabled />

                <n-switch v-model:value="act2" />
                <n-switch v-model:value="act2" disabled />
            </n-space>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
