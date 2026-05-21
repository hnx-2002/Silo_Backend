<script setup>
import { NCard, NInput, NSpace, NText, NGrid, NGi, NInputNumber } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '2'">
        <NText style="color: red">需要自行实现，以响应规定</NText>
        <NSpace>
            <NInputNumber button-placement="both"></NInputNumber>
            <NInputNumber button-placement="both"></NInputNumber>
            <NInputNumber button-placement="both"></NInputNumber>
            <NInputNumber button-placement="both" :disabled="true"></NInputNumber>
        </NSpace>
    </NCard>
</template>
<style scoped></style>
