<script setup>
import { NCard, NDatePicker, NSpace } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const timestamp = 1183135260000;
</script>

<template>
    <NCard :title="translate.t('示例') + '1'">
        <NSpace>
            <div>日期</div>
            <n-date-picker class="dpdemp1" v-model:value="timestamp" type="date" />
            <div>禁用</div>
            <n-date-picker class="dpdemp1" :disabled="true" v-model:value="timestamp" type="date" />
        </NSpace>
    </NCard>
</template>
<style scoped>
.dpdemp1 {
    width: 200px;
}
</style>
