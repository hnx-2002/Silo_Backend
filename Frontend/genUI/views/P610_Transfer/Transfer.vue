<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import TransferDemo from './component/TransferDemo.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription :Title="translate.t('使用场景')" :Description="''"></TitleDescription>
        <TransferDemo></TransferDemo>
    </NSpace>
</template>
<style scoped></style>
