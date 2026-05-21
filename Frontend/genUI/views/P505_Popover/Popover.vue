<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="
                translate.t(
                    '不想破坏原界面用户使用节奏与信息密度的情况下，可以利用气泡卡片信息承载量大的特点来进行一些页面的解耦，达到屏幕空间复用的目的。'
                )
            "
        ></TitleDescription>

        <NText style="color: red">需要单独实现</NText>
    </NSpace>
</template>
<style scoped></style>
