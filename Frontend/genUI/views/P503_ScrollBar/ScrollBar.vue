<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="translate.t('所要展示的内容，超出了当前可视页面范围，出现滚动条')"
        ></TitleDescription>
        <NCard :title="translate.t('示例')">
            <NText style="color: red">颜色值没给全，圆角缺少数据</NText>
            <div style="height: 200px; width: 300px; overflow: auto">
                <div style="height: 500px; width: 300px"></div>
            </div>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
