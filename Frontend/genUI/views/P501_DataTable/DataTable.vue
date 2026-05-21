<script setup>
import { NSpace, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('标准列表')"
            :Description="
                translate.t(
                    '对齐方式：文本左对齐，数值右对齐(如“数量”，“金额”等字符长度有差异的数字字段，方便用户只管地对比数值)'
                )
            "
        ></TitleDescription>
    </NSpace>
</template>
<style scoped></style>
