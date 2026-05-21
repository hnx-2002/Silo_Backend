<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import CardDemo1 from './component/CardDemo1.vue';
import CardDemo2 from './component/CardDemo2.vue';
import CardDemo3 from './component/CardDemo3.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="
                translate.t(
                    '卡片是一种承载信息的容器，对可承载的内容类型无过多限制，它让一类信息集中化，增强区块感的同时更易于操作'
                )
            "
        ></TitleDescription>
        <CardDemo1></CardDemo1>
        <CardDemo2></CardDemo2>
        <CardDemo3></CardDemo3>
    </NSpace>
</template>
<style scoped></style>
