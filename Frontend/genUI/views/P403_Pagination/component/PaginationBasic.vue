<script setup>
import { ref } from 'vue';
import { NPagination, NCard } from 'naive-ui';

import { translate } from '@utility/i18n.js';

const page = ref(2);
</script>

<template>
    <NCard :title="translate.t('基础用法')">
        <n-pagination v-model:page="page" :page-count="100" />
    </NCard>
</template>
<style scoped></style>
