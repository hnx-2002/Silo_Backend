<script setup>
import { NSpace, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import TreeDemo1 from './component/treeDemo1.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('Tree树形控件')"
            :Description="translate.t('页面内多层次的结构展示')"
        ></TitleDescription>
        <TreeDemo1></TreeDemo1>
    </NSpace>
</template>
<style scoped></style>
