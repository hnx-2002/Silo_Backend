<script setup>
import { NSpace, NText, NH2 } from 'naive-ui';
const inPara = defineProps({
    Title: String,
    Description: String,
});
</script>
<template>
    <NSpace vertical>
        <NH2 style="color: darkturquoise">{{ inPara.Title }}</NH2>
        <NText>{{ inPara.Description }}</NText>
    </NSpace>
</template>
<style scoped></style>
