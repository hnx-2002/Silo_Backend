<script setup>
import { NSpace } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import PaginationBasic from './component/PaginationBasic.vue';
import PaginationForbidden from './component/PaginationForbidden.vue';
import PaginationJumper from './component/PaginationJumper.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="translate.t('当数据过多时可以切换页码浏览数据')"
        ></TitleDescription>
        <PaginationBasic></PaginationBasic>
        <PaginationJumper></PaginationJumper>
        <PaginationForbidden></PaginationForbidden>
    </NSpace>
</template>
<style scoped></style>
