<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import SelectorDemo from './component/SelectorDemo.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription :Title="translate.t('下拉选择')" :Description="''"></TitleDescription>
        <SelectorDemo></SelectorDemo>
    </NSpace>
</template>
<style scoped></style>
