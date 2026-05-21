<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import InputNumberDemo from './component/InputNumberDemo.vue';
import InputNumberDemo2 from './component/InputNumberDemo2.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="'当需要获取标准数值时使用'"
        ></TitleDescription>
        <InputNumberDemo></InputNumberDemo>
        <InputNumberDemo2></InputNumberDemo2>
    </NSpace>
</template>
<style scoped></style>
