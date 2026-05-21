<script setup>
import { NSpace, NCard, NButton, NText } from 'naive-ui';

import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import PrimaryButtonCard from './components/PrimaryButtonCard.vue';
import SecondaryButtonCard from './components/SecondaryButtonCard.vue';
import SecondaryButtonCard2 from './components/SecondaryButtonCard2.vue';
import IconButtonCard from './components/IconButtonCard.vue';
import TextButtonCard from './components/TextButtonCard.vue';
import IconTextButtonCard from './components/IconTextButtonCard.vue';

import { translate } from '@utility/i18n.js';

//主要按钮
const mainButtons = [
    {
        ButtonType: 'Normal',
        BGColor: '#3083fd',
        Radius: '3px',
        FontColor: '#FFFFFF',
        FontSize: '12px',
    },
    {
        ButtonType: 'Hover',
        BGColor: '#4690fd',
        Radius: '3px',
        FontColor: '#FFFFFF',
        FontSize: '12px',
        Note: '悬停起效',
    },
    {
        ButtonType: 'Active',
        BGColor: '#2b78ea',
        Radius: '3px',
        FontColor: '#FFFFFF',
        FontSize: '12px',
        Note: '点击之后起效',
    },
    {
        ButtonType: 'Forbid',
        BGColor: '#94c0ff',
        Radius: '3px',
        FontColor: '#FFFFFF',
        FontSize: '12px',
        disabled: true,
    },
];

//次要按钮1
const secondButtons1 = [
    {
        ButtonType: 'Normal',
        BGColor: '#ffffff',
        Radius: '3px',
        OutLine: '1px #3083fd',
        FontColor: '#3083fd',
        FontSize: '12px',
    },
    {
        ButtonType: 'Hover',
        BGColor: '#ffffff',
        Radius: '3px',
        OutLine: '1px #4690fd',
        FontColor: '#4690fd',
        FontSize: '12px',
        Note: '悬停起效',
    },
    {
        ButtonType: 'Active',
        BGColor: '#eaf2ff',
        Radius: '3px',
        OutLine: '1px #2b78ea',
        FontColor: '#2b78ea',
        FontSize: '12px',
        Note: '点击之后起效',
    },
    {
        ButtonType: 'Forbid',
        BGColor: '#ffffff',
        Radius: '3px',
        OutLine: '1px #94c0ff',
        FontColor: '#94c0ff',
        FontSize: '12px',
        disabled: true,
    },
];

//次要按钮2
const secondButtons2 = [
    {
        ButtonType: 'Normal',
        BGColor: '#ffffff',
        Radius: '3px',
        OutLine: '1px #d2d7da',
        FontColor: '#333333',
        FontSize: '12px',
    },
    {
        ButtonType: 'Hover',
        BGColor: '#ffffff',
        Radius: '3px',
        OutLine: '1px #3083fd',
        FontColor: '#3083fd',
        FontSize: '12px',
        Note: '悬停起效',
    },
    {
        ButtonType: 'Active',
        BGColor: '#ffffff',
        Radius: '3px',
        OutLine: '1px #2b78ea',
        FontColor: '#2b78ea',
        FontSize: '12px',
        Note: '点击之后起效',
    },
    {
        ButtonType: 'Forbid',
        BGColor: '#ffffff',
        Radius: '3px',
        OutLine: '1px #d2d7da',
        FontColor: '#b6bac2',
        FontSize: '12px',
        disabled: true,
    },
];
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('按钮样式')"
            :Description="translate.t('主按钮 / 次按钮 / 文字按钮 / 图标按钮 / 文字图标按钮组合')"
        ></TitleDescription>
        <NCard :title="translate.t('主要按钮')">
            <NText>通过 按钮 type='primary' 实现，已修改主题编辑器参数</NText>
            <NSpace>
                <PrimaryButtonCard
                    :ButtonObj="item"
                    v-for="item in mainButtons"
                ></PrimaryButtonCard>
            </NSpace>
        </NCard>
        <NCard :title="translate.t('示例')">
            <NSpace>
                <NButton type="primary">{{ translate.t('文字') }}</NButton>
                <NButton type="primary">{{ translate.t('文字多') }}</NButton>
                <NButton type="primary">{{ translate.t('文字多多') }}</NButton>
                <NButton type="primary">{{ translate.t('文字多多多') }}</NButton>
            </NSpace>
            <NText style="color: red">
                按钮最小宽度为60px,2个字符以上保持左右padding值16px不变，按钮宽度随字符个数做自适应，且按钮高度固定不变。
            </NText>
        </NCard>
        <NCard :title="translate.t('次要按钮1')">
            <NText>通过 按钮 type='info' 实现，已修改主题编辑器参数</NText>
            <NSpace>
                <SecondaryButtonCard
                    :ButtonObj="item"
                    v-for="item in secondButtons1"
                ></SecondaryButtonCard>
            </NSpace>
        </NCard>
        <NCard :title="translate.t('次要按钮2')">
            <NText style="color: red">暂时无法响应规定</NText>
            <NSpace>
                <SecondaryButtonCard2
                    :ButtonObj="item"
                    v-for="item in secondButtons2"
                ></SecondaryButtonCard2>
            </NSpace>
        </NCard>
        <NCard :title="translate.t('图标按钮')">
            <NText style="color: red">规定标注不全,没有指定颜色,暂按主按钮设置参数</NText>
            <NSpace>
                <IconButtonCard :ButtonObj="item" v-for="item in mainButtons"></IconButtonCard>
            </NSpace>
        </NCard>
        <NCard :title="translate.t('文字按钮')">
            <NText style="color: red">规定标注不全,没有指定颜色,暂按主按钮设置参数</NText>
            <NSpace>
                <TextButtonCard :ButtonObj="item" v-for="item in mainButtons"></TextButtonCard>
            </NSpace>
        </NCard>
        <NCard :title="translate.t('图标文字按钮')">
            <NText style="color: red">规定标注不全,没有指定颜色,暂按主按钮设置参数</NText>
            <NSpace>
                <IconTextButtonCard
                    :ButtonObj="item"
                    v-for="item in mainButtons"
                ></IconTextButtonCard>
            </NSpace>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
