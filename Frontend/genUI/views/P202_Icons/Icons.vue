<script setup>
import { NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';

const description =
    '请遵循设计系统完成图标的绘制；图标以识别性和可读性为优先级，因此线性图标，尽可能简洁，直达其意';
</script>

<template>
    <TitleDescription
        :Title="translate.t('设计说明')"
        :Description="translate.t(description)"
    ></TitleDescription>
    <NText style="color: red">图标按项目要求</NText>
</template>
<style scoped></style>
