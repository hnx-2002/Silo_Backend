<script setup>
import { ref } from 'vue';
import { NSpace, NCard, NButton, NModal } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import ModalDemo1 from './component/ModalDemo1.vue';
import ModalDemo2 from './component/ModalDemo2.vue';

import { translate } from '@utility/i18n.js';

const showCommon = ref(false);
const showNoTitle = ref(false);
const showIcon = ref(false);

function show(type) {
    switch (type) {
        case 'common':
            showCommon.value = true;
            break;
        case 'noTitle':
            showNoTitle.value = true;
            break;
        case 'icon':
            showIcon.value = true;
            break;
        default:
            break;
    }
}
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="'需要用户处理操作，又不希望跳转页面打断用户工作流程时，可以使用Modal在当前页面中打开一个浮层，承载相应的操作'"
        ></TitleDescription>
        <NCard :title="translate.t('通用样式')">
            <NSpace>
                <NButton type="primary" @click="show('common')">通用样式</NButton>
                <NButton type="primary" @click="show('noTitle')">无标题样式</NButton>
                <NButton type="primary" @click="show('icon')">图标弹框</NButton>
            </NSpace>
        </NCard>
    </NSpace>

    <n-modal v-model:show="showCommon" preset="card" style="width: 400px" title="弹窗标题">
        <ModalDemo1></ModalDemo1>
    </n-modal>

    <n-modal class="noTitleModal" v-model:show="showNoTitle" preset="card" style="width: 400px">
        <ModalDemo1></ModalDemo1>
    </n-modal>

    <n-modal class="noTitleModal" v-model:show="showIcon" preset="card" style="width: 400px">
        <ModalDemo2></ModalDemo2>
    </n-modal>
</template>
<style scoped></style>
