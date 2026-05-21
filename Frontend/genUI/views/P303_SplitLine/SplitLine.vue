<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import SplitLineCard from './component/SplitLineCard.vue';

import { translate } from '@utility/i18n.js';

const solid = {
    title: '实线',
    lineType: '1px solid #e5e6eb',
    isDashed: false,
};
const dashed = {
    title: '虚线',
    lineType: '1px dashed #e5e6eb    间隙1px,每个点宽度1px',
    isDashed: true,
};
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="
                translate.t(
                    '对行内文字/链接进行分隔，例如表格的操纵列；对不同章节的文本段落进行分割'
                )
            "
        ></TitleDescription>
        <NCard :title="translate.t(实线)">
            <SplitLineCard :obj="solid"></SplitLineCard>
            <NText>实例见DataTable</NText>
        </NCard>
        <NCard :title="translate.t(虚线)">
            <SplitLineCard :obj="dashed"></SplitLineCard>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
