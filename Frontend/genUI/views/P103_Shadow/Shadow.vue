<script setup>
import { NDivider, NSpace, NText } from 'naive-ui';
import ShadowCard from './components/ShadowCard.vue';

import { translate } from '@utility/i18n.js';
</script>
<template>
    <div>本页待开发，招募开发者中...</div>
    <NSpace vertical>
        <NText>{{ translate.t('阴影方向使用场景说明：') }}</NText>
        <NText>
            {{ translate.t('阴影向下：主要应用于组件内部或组件本身，是比较常规场景的用法；') }}
        </NText>
        <NText>{{ translate.t('阴影向上：主要应用于底部导航或工具栏等；') }}</NText>
        <NText>{{ translate.t('阴影向左：主要应用于右边导航栏，抽屉组件或固定表格栏；') }}</NText>
        <NText>{{ translate.t('阴影向右：主要应用于左边导航栏，抽屉组件或固定表格栏；') }}</NText>
        <NText>{{ translate.t('全局阴影：适用于卡片且无明显方向指示。') }}</NText>
        <NText style="color: red">
            {{ translate.t('扩展值 8% 不起作用，待核实') }}
        </NText>
        <NDivider></NDivider>
        <ShadowCard></ShadowCard>
    </NSpace>
</template>
<style scoped></style>
