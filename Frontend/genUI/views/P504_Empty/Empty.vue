<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="translate.t('当目前没有数据时，给用户提示，初始化场景时的引导创建流程。')"
        ></TitleDescription>

        <NText>注：</NText>
        <NText>1 以下场景配图文案均为示意性文案，不作为项目实际展示文案；</NText>
        <NText>
            2 场景配图文案具体内容需根据具体业务系统定义，由负责该项目的产品经理提供相应文案；
        </NText>

        <NSpace vertical :align="'center'" :wrap-item="false">
            <div style="height: 516px; width: 1162px; background-color: #ff7200">图片示意</div>
            <div style="font-size: 36px; color: rgb(51, 51, 51)">您访问的页面不存在</div>
            <div style="font-size: 24px; color: rgb(51, 51, 51)">
                <span style="color: rgb(48, 131, 253)">5秒</span>
                后跳转到首页，如没有跳转，请
                <span style="color: rgb(48, 131, 253)">点击这里</span>
            </div>
        </NSpace>
    </NSpace>
</template>
<style scoped></style>
