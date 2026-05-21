<script setup>
import { NCard, NInput, NSpace, NText, NButton, NInputGroup, NTooltip } from 'naive-ui';

import Icon_search from '@common/icon/icon_search.vue';
import Icon_Exclamation from '@common/icon/Icon_Exclamation.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '2-' + translate.t('搜索输入框')">
        <NText style="color: red">此处按钮宽度需要单独设置，以响应规定</NText>
        <NSpace vertical>
            <NSpace>
                <NSpace vertical>
                    <NText style="color: red">normal</NText>
                    <NInputGroup>
                        <NInput></NInput>
                        <NButton class="groupIconButtonSearch" type="primary">搜索</NButton>
                    </NInputGroup>
                </NSpace>
                <NSpace vertical>
                    <NText style="color: red">hover</NText>
                    <NInputGroup>
                        <NInput></NInput>
                        <NButton class="groupIconButtonSearch" type="primary">搜索</NButton>
                    </NInputGroup>
                </NSpace>
                <NSpace vertical>
                    <NText style="color: red">click</NText>
                    <NInputGroup>
                        <NInput></NInput>
                        <NButton class="groupIconButtonSearch" type="primary">搜索</NButton>
                    </NInputGroup>
                </NSpace>
                <NSpace vertical>
                    <NText style="color: red">forbiden</NText>
                    <NInputGroup>
                        <NInput :disabled="true"></NInput>
                        <NButton class="groupIconButtonSearch" type="primary" :disabled="true">
                            搜索
                        </NButton>
                    </NInputGroup>
                </NSpace>
            </NSpace>

            <NSpace>
                <NInputGroup>
                    <NInput></NInput>
                    <NButton class="groupIconButton" type="primary">
                        <template #icon>
                            <Icon_search></Icon_search>
                        </template>
                    </NButton>
                </NInputGroup>

                <NInput>
                    <template #suffix>
                        <NButton class="iconButtonInInput" text type="primary">
                            <template #icon>
                                <Icon_search></Icon_search>
                            </template>
                        </NButton>
                    </template>
                </NInput>

                <NInput>
                    <template #suffix>
                        <n-tooltip placement="bottom" trigger="hover">
                            <template #trigger>
                                <NButton class="iconButtonInInput" text type="primary">
                                    <template #icon>
                                        <Icon_Exclamation></Icon_Exclamation>
                                    </template>
                                </NButton>
                            </template>
                            {{ translate.t('文字解释说明') }}
                        </n-tooltip>
                    </template>
                </NInput>
            </NSpace>
        </NSpace>
    </NCard>
</template>
<style scoped>
.groupIconButton {
    --n-padding: 0 8px !important;
    min-width: unset !important;
}
.groupIconButtonSearch {
    --n-padding: 0 15px !important;
    min-width: unset !important;
}
.iconButtonInInput {
    min-width: unset !important;
}
</style>
