<script setup>
import { ref } from 'vue';
import { NPagination, NCard } from 'naive-ui';

import { translate } from '@utility/i18n.js';

const page = ref(2);
</script>

<template>
    <NCard :title="translate.t('改变页数') + '/' + translate.t('快速跳转')">
        <n-pagination v-model:page="page" :page-count="100" show-size-picker show-quick-jumper>
            <template #prefix="{ itemCount, startIndex }">
                {{ translate.t('共') }}
                <div style="color: #3083fd">{{ itemCount }}</div>
                {{ translate.t('条数据') }}
            </template>
            <template #goto>{{ translate.t('跳至') }}</template>
            <template #suffix>{{ translate.t('页') }}</template>
        </n-pagination>
    </NCard>
</template>
<style scoped></style>
