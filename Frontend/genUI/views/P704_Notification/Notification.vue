<script setup>
import { NSpace, NCard, NButton } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { h_NButton, make_h_NSpace } from '@utility/NaiveHComponent';
import { translate } from '@utility/i18n.js';

const showContent = {
    title: '此处是标题',
    content: '这里是内容展示区域，这里是内容展示区域',
    duration: 999000,
    keepAliveOnHover: true,
};

const showContentWithButtons = {
    title: '此处是标题',
    content: '这里是内容展示区域，这里是内容展示区域',
    duration: 999000,
    keepAliveOnHover: true,
    action: () => {
        let opts = [];
        opts.push(h_NButton(translate.t('取消'), 'info', () => editRow(row)));
        opts.push(h_NButton(translate.t('确定'), 'primary', () => editRow(row)));

        return make_h_NSpace(() => opts, 'end');
    },
};

function show(type) {
    switch (type) {
        case 'Common':
            window.$notification.create(showContent);
            break;
        case 'Error':
            window.$notification.error(showContent);
            break;
        case 'Success':
            window.$notification.success(showContent);
            break;
        case 'Warning':
            window.$notification.warning(showContent);
            break;
        case 'Info':
            window.$notification.info(showContent);
            break;
        case 'WithButtons':
            window.$notification.create(showContentWithButtons);
            break;
        default:
            break;
    }
}
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="'带有交互的通知，给出用户下一步的行动点，系统主动推送'"
        ></TitleDescription>

        <NCard :title="translate.t('通知提醒')">
            <NSpace>
                <NButton type="primary" @click="show('Common')">常规</NButton>
                <NButton type="primary" @click="show('Error')">错误</NButton>
                <NButton type="primary" @click="show('Success')">成功</NButton>
                <NButton type="primary" @click="show('Warning')">警告</NButton>
                <NButton type="primary" @click="show('Info')">信息</NButton>
                <NButton type="primary" @click="show('WithButtons')">常规带按钮</NButton>
            </NSpace>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
