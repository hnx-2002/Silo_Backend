<script setup>
import { NSpace, NBreadcrumb, NBreadcrumbItem } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
const description =
    '反映当前页面在网站结构中的位置，在少于三个层级是无需展示，' +
    '此时的全局导航能直接呈现位置，用户可通过面包屑返回上级页面';
</script>
<template>
    <NSpace vertical>
        <TitleDescription :Title="'使用场景'" :Description="description"></TitleDescription>
        <n-breadcrumb separator=">">
            <n-breadcrumb-item>一级页面</n-breadcrumb-item>
            <n-breadcrumb-item>二级页面</n-breadcrumb-item>
            <n-breadcrumb-item>当前页面</n-breadcrumb-item>
        </n-breadcrumb>
    </NSpace>
</template>
<style scoped></style>
