<script setup>
import { onMounted, ref } from 'vue';
import { NCard } from 'naive-ui';
import FrameIndi_OnlyHead from '@common/layout/FrameIndi_OnlyHead.vue';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import GetMenu from './MenuData';
import {
    getMenuHome,
    executeMenuIcon,
    executeMenuLabel,
    killMenuChildren,
    searchMenuByKey,
} from '@common/layout/indexFunc';

import { translate } from '@utility/i18n.js';

const PageFrameRef = ref(null);

let initMenuHome = getMenuHome(translate.t('首页'));

const menus = ref([]);
const menuKey = ref('Home');
const menuObj = ref(initMenuHome);

onMounted(() => {
    menus.value = GetMenu();
    killMenuChildren(menus.value);
    updateMenu(location.hash.substring(1));
    if (PageFrameRef.value) {
        PageFrameRef.value.DoMenuRefresh();
    }
});

//更新menuKey,menuObj
function updateMenu(key) {
    if (key == 'Home' || key == '') {
        menuKey.value = key;
        menuObj.value = initMenuHome;
    } else {
        let tempMenuObj = searchMenuByKey(menus.value, key);

        if (Object.keys(tempMenuObj).length > 0) {
            menuKey.value = key;
            menuObj.value = tempMenuObj;
        } else {
            menuKey.value = '404';
            menuObj.value = {};
        }
    }
}

//渲染菜单文字
function renderMenuLabel(val) {
    return executeMenuLabel(translate.t(val.label));
}

//渲染菜单图标
function renderMenuIcon(val) {
    return executeMenuIcon(val);
}
</script>

<template>
    <TitleDescription
        :Title="translate.t('上下布局适配方案')"
        :Description="
            translate.t(
                '用于上下布局的设计方案中，做法是对两边留白区域进行最小值得定义，当留白区域达到限定值之后再对中间的主内容区域进行动态缩放'
            )
        "
    ></TitleDescription>
    <NCard style="height: 450px">
        <FrameIndi_OnlyHead
            ref="PageFrameRef"
            :platformTitle="translate.t('上下布局')"
            :menus="menus"
            :menuSelectObj="menuObj"
            :renderMenuIcon="renderMenuIcon"
            :renderMenuLabel="renderMenuLabel"
        ></FrameIndi_OnlyHead>
    </NCard>
</template>
<style scoped></style>
