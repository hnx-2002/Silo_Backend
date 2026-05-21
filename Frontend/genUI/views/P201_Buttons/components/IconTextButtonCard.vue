<script setup>
import { NSpace, NButton, NText, NIcon } from 'naive-ui';
import Icon_Search from './Icon_Search.vue';

import { translate } from '@utility/i18n.js';

const inPara = defineProps({
    ButtonObj: Object,
});
</script>
<template>
    <NSpace vertical>
        <NButton type="primary" :disabled="inPara.ButtonObj.disabled">
            <template #icon>
                <Icon_Search></Icon_Search>
            </template>
            {{ translate.t('按钮') }}
        </NButton>
        <NText style="color: deepskyblue">{{ inPara.ButtonObj.ButtonType }}</NText>
        <div style="width: 200px"></div>
    </NSpace>
</template>
<style scoped></style>
