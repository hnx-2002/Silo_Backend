<script setup>
import { NSpace, NCard, NButton } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';

function show(type) {
    switch (type) {
        case 'Error':
            window.$message.error('这是一条全局错误信息提示消息通用样式！');
            break;
        case 'Success':
            window.$message.success('这是一条全局成功信息提示消息通用样式！');
            break;
        case 'Warning':
            window.$message.warning('这是一条全局警告信息提示消息通用样式！');
            break;
        case 'Info':
            window.$message.info('这是一条全局常规信息提示消息通用样式！');
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
            :Description="'向用户传达成功，警告和错误等反馈信息，顶部居中显示并自动消失，是一种不打断用户操作的轻量级提示方式'"
        ></TitleDescription>
        <NCard :title="translate.t('通用样式')">
            <NSpace>
                <NButton type="primary" @click="show('Error')">错误</NButton>
                <NButton type="primary" @click="show('Success')">成功</NButton>
                <NButton type="primary" @click="show('Warning')">警告</NButton>
                <NButton type="primary" @click="show('Info')">常规</NButton>
            </NSpace>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
