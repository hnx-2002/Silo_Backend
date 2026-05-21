<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import TagDemo1 from './component/TagDemo1.vue';
import TagDemo2 from './component/TagDemo2.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="translate.t('用于分类，标记事物的属性和维度')"
        ></TitleDescription>

        <TagDemo1></TagDemo1>
        <TagDemo2></TagDemo2>
    </NSpace>
</template>
<style scoped></style>
