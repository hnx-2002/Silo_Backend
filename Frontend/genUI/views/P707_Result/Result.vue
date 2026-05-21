<script setup>
import { NText, NSpace } from 'naive-ui';

import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="'向用户传达任务完成结果，引导用户进行下一步操作，通过有效的反馈建立起用户对系统的信任'"
        ></TitleDescription>
        <NText>参考Modal</NText>
    </NSpace>
</template>
<style scoped></style>
