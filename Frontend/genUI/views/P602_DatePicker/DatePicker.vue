<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import DatePickerDemo from './component/DatePickerDemo.vue';
import DatePickerDemoDateTime from './component/DatePickerDemoDateTime.vue';
import DatePickerDemoRange from './component/DatePickerDemoRange.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="'当用户需要输入一个日期，可以点击标准输入框，弹出日期面板进行选择'"
        ></TitleDescription>
        <NText style="color: red">规定无标注</NText>
        <DatePickerDemo></DatePickerDemo>
        <DatePickerDemoDateTime></DatePickerDemoDateTime>
        <DatePickerDemoRange></DatePickerDemoRange>
    </NSpace>
</template>
<style scoped></style>
