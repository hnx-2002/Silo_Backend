<script setup>
import { NSpace, NText } from 'naive-ui';

const inPara = defineProps({
    ColorStr: String,
    TitleName: String,
});
</script>
<template>
    <NSpace vertical>
        <div :style="{ width: '100px', height: '50px', backgroundColor: inPara.ColorStr }"></div>
        <NText>{{ inPara.TitleName }}</NText>
        <NText>{{ inPara.ColorStr }}</NText>
    </NSpace>
</template>
<style scoped></style>
