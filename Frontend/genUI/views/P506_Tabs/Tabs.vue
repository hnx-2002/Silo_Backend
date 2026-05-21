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
                    '提供平级的区域将大块内容进行收纳和展现，保持页面整洁，位于页内容上方，主要作用是申明页面主题，页内信息导航，页面级内容操作。'
                )
            "
        ></TitleDescription>

        <NText style="color: red">待实现</NText>
    </NSpace>
</template>
<style scoped></style>
