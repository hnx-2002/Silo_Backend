<script setup>
import { NDivider, NSpace } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import { translate } from '@utility/i18n.js';
</script>
<template>
    <div>本页待开发，招募开发者中...</div>
    <NSpace justify="center">
        <div>
            <TitleDescription
                :Title="translate.t('中文')"
                :Description="'Chinese'"
            ></TitleDescription>
            <NDivider></NDivider>
            <!-- 字体样式表格 -->
            <NDivider></NDivider>
            <!-- 字体大小表格 -->
        </div>
        <NDivider vertical></NDivider>
        <div>
            <TitleDescription
                :Title="translate.t('英文')"
                :Description="'English'"
            ></TitleDescription>
            <NDivider></NDivider>
            <!-- 字体样式表格 -->
            <NDivider></NDivider>
            <!-- 字体大小表格 -->
        </div>
    </NSpace>
</template>
<style scoped></style>
