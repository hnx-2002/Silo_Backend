<script setup>
import { NSpace, NButton, NText } from 'naive-ui';

import { translate } from '@utility/i18n.js'; 

const inPara = defineProps({
    ButtonObj: Object,
});
</script>
<template>
    <NSpace vertical>
        <NButton type="tertiary" :disabled="inPara.ButtonObj.disabled">
            {{ translate.t('按钮') }}
        </NButton>
        <NText style="color: deepskyblue">{{ inPara.ButtonObj.ButtonType }}</NText>
        <NText>bg-color: {{ inPara.ButtonObj.BGColor }}</NText>
        <NText>radius: {{ inPara.ButtonObj.Radius }}</NText>
        <NText>outline: {{ inPara.ButtonObj.OutLine }}</NText>
        <NText>font-color: {{ inPara.ButtonObj.FontColor }}</NText>
        <NText>font-size: {{ inPara.ButtonObj.FontSize }}</NText>
        <NText>{{ inPara.ButtonObj.Note }}</NText>
        <div style="width: 200px"></div>
    </NSpace>
</template>
<style scoped></style>
