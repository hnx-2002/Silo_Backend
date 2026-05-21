<script setup>
import { NSpace, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import StepDemo1 from './component/StepDemo1.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="
                translate.t('当任务复杂或者存在先后关系时，将其分成一系列步骤，从而简化任务')
            "
        ></TitleDescription>
        <NText style="color: red">由于title的位置，naive-ui暂时无法响应规定</NText>
        <StepDemo1></StepDemo1>
    </NSpace>
</template>
<style scoped></style>
