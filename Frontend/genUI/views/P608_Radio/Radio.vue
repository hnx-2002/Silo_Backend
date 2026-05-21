<script setup>
import { NSpace, NRadio, NRadioGroup, NCard } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="
                translate.t(
                    '用于在多个备选项中选中单个状态，所有选项默认可见，方便用户在比较中选择，选项不宜过多'
                )
            "
        ></TitleDescription>
        <NCard :title="translate.t('示例')">
            <n-radio-group v-model:value="value" name="radiogroup">
                <n-space item-style="display: flex;">
                    <n-radio :key="'val1'" :value="'选项一'">选项一</n-radio>
                    <n-radio :key="'val2'" :value="'选项二'">选项二</n-radio>
                    <n-radio :key="'val3'" :value="'选项三'">选项三</n-radio>
                    <n-radio :key="'val4'" :value="'选项四'" :disabled="true">选项四</n-radio>
                </n-space>
            </n-radio-group>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
