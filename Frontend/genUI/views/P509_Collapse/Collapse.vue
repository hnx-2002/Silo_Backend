<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import CollapseDemo1 from './component/CollapseDemo1.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="
                translate.t(
                    '折叠面板通过对信息的分组和收纳，指引用户递进式的获取信息，使界面保持整洁的同事增加空间的有效利用率'
                )
            "
        ></TitleDescription>
        <NText style="color: red">规定中的Icon无法直接响应</NText>
        <CollapseDemo1></CollapseDemo1>
    </NSpace>
</template>
<style scoped></style>
