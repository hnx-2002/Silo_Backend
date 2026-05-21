<script setup>
import { ref, computed } from 'vue';
import { NCard, NText, NSpace, NTag } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '1'">
        <NSpace>
            <!-- :color="{ color: '#BBB', textColor: '#555', borderColor: '#555' }" -->
            <n-tag closable>
                {{ translate.t('内容') }}
            </n-tag>
            <n-tag closable>
                {{ translate.t('标签内容') }}
            </n-tag>
            <n-tag closable>
                {{ translate.t('标签内容') }}
            </n-tag>
            <n-tag closable>
                {{ translate.t('标签内容内容') }}
            </n-tag>
        </NSpace>
    </NCard>
</template>
<style scoped></style>
