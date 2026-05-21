<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import CascaderDemo from './component/CascaderDemo.vue';
import CascaderDemoForbiden from './component/CascaderDemoForbiden.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('级联选择')"
            :Description="'需要从一组相关联的数据集合进行选择，若从一个较大的数据集合中进行选择时，用多级分类进行分隔，方便选择'"
        ></TitleDescription>
        <NText style="color: red">规定无标注</NText>
        <CascaderDemo></CascaderDemo>
        <CascaderDemoForbiden></CascaderDemoForbiden>
    </NSpace>
</template>
<style scoped></style>
