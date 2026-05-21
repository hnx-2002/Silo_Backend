<script setup>
import { NCard, NDatePicker, NSpace } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const timestamp = 1183135260000;
</script>

<template>
    <NCard :title="translate.t('示例') + '2'">
        <NSpace>
            <div>年月</div>
            <n-date-picker class="dpdemp1" v-model:value="timestamp" type="month" />
            <div>日期时间</div>
            <n-date-picker class="dpdemp1" v-model:value="timestamp" type="datetime" />
        </NSpace>
    </NCard>
</template>
<style scoped>
.dpdemp1 {
    width: 200px;
}
</style>
