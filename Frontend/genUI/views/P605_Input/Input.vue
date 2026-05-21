<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import InputDemo from './component/InputDemo.vue';
import InputDemo2 from './component/InputDemo2.vue';
import InputDemoTextArea from './component/InputDemoTextArea.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="'需要用户输入表单域内容时，提供组合输入框，带搜索的输入框'"
        ></TitleDescription>
        <InputDemo></InputDemo>
        <InputDemo2></InputDemo2>
        <InputDemoTextArea></InputDemoTextArea>
    </NSpace>
</template>
<style scoped></style>
