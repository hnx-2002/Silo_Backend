<script setup>
import { onMounted, ref } from 'vue';
import { NCard } from 'naive-ui';
import FrameIndi_Classic from '@common/layout/FrameIndi_Classic.vue';
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
        :Title="translate.t('左右布局适配方案')"
        :Description="
            translate.t(
                '用于左右布局的设计方案中，常见的做法是将左边的导航栏固定，对右边的工作区域进行动态缩放'
            )
        "
    ></TitleDescription>
    <NCard style="height: 450px">
        <FrameIndi_Classic
            ref="PageFrameRef"
            :platformTitle="translate.t('左右布局')"
            :menus="menus"
            :menuSelectObj="menuObj"
            :renderMenuIcon="renderMenuIcon"
            :renderMenuLabel="renderMenuLabel"
        ></FrameIndi_Classic>
    </NCard>
</template>
<style scoped></style>
