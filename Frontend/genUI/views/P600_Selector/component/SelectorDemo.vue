<script setup>
import { NCard, NSelect } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '1'">
        <n-select
            :options="[
                {
                    label: '张一',
                    value: '张一',
                },
                {
                    label: '张二',
                    value: '张二',
                },
                {
                    label: '张三',
                    value: '张三',
                },
                {
                    label: '张四',
                    value: '张四',
                },
                {
                    label: '张五',
                    value: '张五',
                },
            ]"
        />
    </NCard>
</template>
<style scoped></style>
