<script setup>
import { NSpace, NCard, NTooltip, NButton } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="'带有交互的提示，鼠标悬浮时出现，旨在对内容的补充和解释说明'"
        ></TitleDescription>
        <NCard :title="translate.t('示例')">
            <NSpace vertical>
                <n-tooltip trigger="hover" placement="right">
                    <template #trigger>
                        <n-button text>{{ translate.t('需要解释') }}</n-button>
                    </template>
                    这里是对文字的解释说明
                </n-tooltip>
                <n-tooltip trigger="hover" style="max-width: 100px" placement="right-start">
                    <template #trigger>
                        <span>文字省略...</span>
                    </template>
                    这里是对文字的解释说明，文字多可以折行，旗袍宽度不能过大
                </n-tooltip>

                <n-tooltip trigger="hover" placement="right-start">
                    <template #trigger>
                        <span>示例2：（深色背景）</span>
                    </template>
                    这里是对文字的解释说明
                </n-tooltip>
            </NSpace>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
