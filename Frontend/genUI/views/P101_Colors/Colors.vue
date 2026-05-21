<script setup>
import { NDivider, NSpace } from 'naive-ui';
import ColorCard from './components/colorCard.vue';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import { translate } from '@utility/i18n.js';
</script>
<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('标准色')"
            :Description="'Brand Color'"
        ></TitleDescription>
        <NSpace justify="start">
            <ColorCard :ColorStr="'#3083fd'" :TitleName="translate.t('常规')"></ColorCard>
            <ColorCard :ColorStr="'#4590fd'" :TitleName="translate.t('悬浮')"></ColorCard>
            <ColorCard :ColorStr="'#2b78ea'" :TitleName="translate.t('点击')"></ColorCard>
            <ColorCard :ColorStr="'#6aa1ff'" :TitleName="translate.t('特殊场景')"></ColorCard>
            <ColorCard :ColorStr="'#94bfff'" :TitleName="translate.t('一般禁用')"></ColorCard>
            <ColorCard :ColorStr="'#bedaff'" :TitleName="translate.t('文字禁用')"></ColorCard>
            <ColorCard :ColorStr="'#e8f3ff'" :TitleName="translate.t('浅色/白底悬浮')"></ColorCard>
        </NSpace>
        <NSpace justify="start">
            <ColorCard :ColorStr="'#3083fd1A'" :TitleName="translate.t('常规')"></ColorCard>
            <ColorCard :ColorStr="'#4590fd14'" :TitleName="translate.t('悬浮')"></ColorCard>
            <ColorCard :ColorStr="'#2b78ea1A'" :TitleName="translate.t('点击')"></ColorCard>
            <ColorCard :ColorStr="'#6aa1ff1A'" :TitleName="translate.t('特殊场景')"></ColorCard>
            <ColorCard :ColorStr="'#94bfff1A'" :TitleName="translate.t('一般禁用')"></ColorCard>
            <ColorCard :ColorStr="'#bedaff1A'" :TitleName="translate.t('文字禁用')"></ColorCard>
            <ColorCard
                :ColorStr="'#e8f3ff1A'"
                :TitleName="translate.t('浅色/白底悬浮')"
            ></ColorCard>
        </NSpace>
        <NDivider></NDivider>
        <TitleDescription
            :Title="translate.t('标准色')"
            :Description="'Neutral Color'"
        ></TitleDescription>
        <NSpace justify="start">
            <ColorCard :ColorStr="'#1d2129'" :TitleName="translate.t('强调/正文标题')"></ColorCard>
            <ColorCard
                :ColorStr="'#333333'"
                :TitleName="translate.t('次强调/正文标题')"
            ></ColorCard>
            <ColorCard :ColorStr="'#6e767c'" :TitleName="translate.t('内容/段落信息')"></ColorCard>
            <ColorCard :ColorStr="'#86909c'" :TitleName="translate.t('次要信息')"></ColorCard>
            <ColorCard :ColorStr="'#b6bac2'" :TitleName="translate.t('弱提示信息')"></ColorCard>
            <ColorCard :ColorStr="'#ffffff'" :TitleName="translate.t('纯白文字')"></ColorCard>
            <ColorCard :ColorStr="'#86909c'" :TitleName="translate.t('重/按钮描边')"></ColorCard>
            <ColorCard :ColorStr="'#d2d7da'" :TitleName="translate.t('深/悬浮')"></ColorCard>
            <ColorCard :ColorStr="'#e5e6eb'" :TitleName="translate.t('一般')"></ColorCard>
            <ColorCard :ColorStr="'#f2f3f5'" :TitleName="translate.t('浅')"></ColorCard>
        </NSpace>
        <NDivider></NDivider>
        <TitleDescription
            :Title="translate.t('辅助色')"
            :Description="'Functional Color'"
        ></TitleDescription>
        <NSpace justify="start">
            <ColorCard :ColorStr="'#00b42a'" :TitleName="translate.t('常规')"></ColorCard>
            <ColorCard :ColorStr="'#23c343'" :TitleName="translate.t('悬浮')"></ColorCard>
            <ColorCard :ColorStr="'#009a29'" :TitleName="translate.t('点击')"></ColorCard>
            <ColorCard :ColorStr="'#7be188'" :TitleName="translate.t('禁用')"></ColorCard>
            <ColorCard :ColorStr="'#aff0b5'" :TitleName="translate.t('特殊场景')"></ColorCard>
            <ColorCard :ColorStr="'#e8faf1'" :TitleName="translate.t('浅色背景')"></ColorCard>
        </NSpace>
        <NSpace justify="start">
            <ColorCard :ColorStr="'#ff7d00'" :TitleName="translate.t('常规')"></ColorCard>
            <ColorCard :ColorStr="'#ff9a2e'" :TitleName="translate.t('悬浮')"></ColorCard>
            <ColorCard :ColorStr="'#d25f00'" :TitleName="translate.t('点击')"></ColorCard>
            <ColorCard :ColorStr="'#ffcf8b'" :TitleName="translate.t('禁用')"></ColorCard>
            <ColorCard :ColorStr="'#ffe4ba'" :TitleName="translate.t('特殊场景')"></ColorCard>
            <ColorCard :ColorStr="'#fff1dd'" :TitleName="translate.t('浅色背景')"></ColorCard>
        </NSpace>
        <NSpace justify="start">
            <ColorCard :ColorStr="'#f53f3f'" :TitleName="translate.t('常规')"></ColorCard>
            <ColorCard :ColorStr="'#f76560'" :TitleName="translate.t('悬浮')"></ColorCard>
            <ColorCard :ColorStr="'#cb2634'" :TitleName="translate.t('点击')"></ColorCard>
            <ColorCard :ColorStr="'#fbaca3'" :TitleName="translate.t('禁用')"></ColorCard>
            <ColorCard :ColorStr="'#fdcdc5'" :TitleName="translate.t('特殊场景')"></ColorCard>
            <ColorCard :ColorStr="'#ffece8'" :TitleName="translate.t('浅色背景')"></ColorCard>
        </NSpace>
    </NSpace>
</template>
<style scoped></style>
