<script setup>
import { NSpace, NCheckbox, NCheckboxGroup, NCard } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="translate.t('自带数据域管理，包含数据录入，校验以及对应样式')"
        ></TitleDescription>
        <NCard :title="translate.t('示例')">
            <n-checkbox-group>
                <n-space item-style="display: flex;">
                    <n-checkbox value="val1" label="选项一" />
                    <n-checkbox value="val2" label="选项二" />
                    <n-checkbox value="val3" label="选项三" />
                    <n-checkbox value="val4" label="选项四" :disabled="true" />
                </n-space>
            </n-checkbox-group>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
