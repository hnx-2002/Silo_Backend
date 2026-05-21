<script setup>
import { NDivider, NSpace, NText } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const inPara = defineProps({
    obj: Object,
});
</script>
<template>
    <NSpace vertical>
        <NText>{{ translate.t(obj.title) }}</NText>
        <NDivider :dashed="obj.isDashed"></NDivider>
        <NText style="color: red">{{ obj.lineType }}</NText>
    </NSpace>
</template>
