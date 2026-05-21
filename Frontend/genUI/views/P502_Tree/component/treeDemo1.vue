<script setup>
import { ref, computed } from 'vue';
import { NCard, NText, NTree } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const data = ref([
    {
        label: computed(() => translate.t('一级内容1')),
        key: 'L1_1',
        children: [
            {
                label: computed(() => translate.t('二级内容1')),
                key: 'L2_1',
            },
            {
                label: computed(() => translate.t('二级内容2')),
                key: 'L2_2',
                children: [
                    {
                        label: computed(() => translate.t('三级内容1')),
                        key: 'L3_1',
                    },
                ],
            },
        ],
    },
    {
        label: computed(() => translate.t('一级内容2')),
        key: 'L1_2',
    },
]);
</script>

<template>
    <NCard style="width: 300px" :title="translate.t('基本用法')">
        <n-tree block-line :data="data" expand-on-click selectable />
    </NCard>
</template>
<style scoped></style>
