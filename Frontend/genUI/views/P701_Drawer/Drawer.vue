<script setup>
import { ref } from 'vue';
import { NDrawer, NDrawerContent, NButton, NSpace } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const show = ref(false);

function exeDrawShow() {
    show.value = true;
}
</script>

<template>
    <div id="drawRange">
        <NButton @click="exeDrawShow">开启抽屉</NButton>
    </div>
    <!-- to=".layoutCard" -->
    <n-drawer
        v-model:show="show"
        :width="'30%'"
        :placement="'right'"
        :block-scroll="false"
        to="#drawRange"
    >
        <n-drawer-content title="此处是标题">
            暗色背景下阴影色值特殊处理

            <template #footer>
                <NSpace justify="end">
                    <n-button type="info">取消</n-button>
                    <n-button type="primary">确定</n-button>
                </NSpace>
            </template>
        </n-drawer-content>
    </n-drawer>
</template>
<style scoped></style>
