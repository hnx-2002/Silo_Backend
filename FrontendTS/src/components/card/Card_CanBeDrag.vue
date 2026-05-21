<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { NCard, NSwitch, NDivider, NIcon, NSpace } from 'naive-ui';
import { RoundedCornerSharp as Icon_Drag } from '@vicons/material';
import { translate } from '@utility/i18n.ts';

const inPara = defineProps({
  show: Boolean, //是否显示
  title: String, //标题
  inWidth: String, //宽度
  useExtent: Boolean, //是否使用拖拽放大窗口
});

const emit = defineEmits(['modalClose', 'dragging', 'extenting']);

interface WindowPosition {
  width: string;
  position: string;
  left: string;
  top: string;
  height?: string;
}

const ww = ref<WindowPosition>({} as WindowPosition);

const dragSwitch = ref(false); //开关
const dragStart = ref(false); //header拖动
const extentIconIsDown = ref(false); //右下角图标是否被按下
const originX = ref(0); //拖动操作时的原点x
const originY = ref(0); //拖动操作时的原点y

onMounted(() => {
  ww.value = {
    width: inPara.inWidth || 'default',
    position: 'fixed',
    left: '100px',
    top: '100px',
  };
});

//关闭
function modalClose() {
  emit('modalClose');
}

//开关
function handleSwitch(val: boolean) {
  dragSwitch.value = val;
}

//头部开始拖拽
function cardMouseDown(e: MouseEvent) {
  if (dragSwitch.value) {
    //事件点-窗口原点
    originX.value = e.clientX - Number(ww.value.left.substring(0, ww.value.left.length - 2));
    originY.value = e.clientY - Number(ww.value.top.substring(0, ww.value.top.length - 2));
    dragStart.value = true;
  }
}

//右下角扩大的按钮被点下
function extendIconMouseDown(e: MouseEvent) {
  if (dragSwitch.value) {
    //事件点-窗口原点
    originX.value = Number(ww.value.left.substring(0, ww.value.left.length - 2));
    originY.value = Number(ww.value.top.substring(0, ww.value.top.length - 2));
    extentIconIsDown.value = true;
  }
}

//拖拽中
function dragCard(e: MouseEvent) {
  if (dragSwitch.value) {
    if (e.screenX !== 0 || e.screenY !== 0) {
      if (dragStart.value && !extentIconIsDown.value) {
        //不是右下角
        ww.value.left = e.clientX - originX.value + 'px';
        ww.value.top = e.clientY - originY.value + 'px';
        emit('dragging', {
          x: ww.value.left,
          y: ww.value.top,
        });
      } else if (!dragStart.value && extentIconIsDown.value) {
        //是右下角
        ww.value.width = e.clientX - originX.value + 20 + 'px';
        ww.value.height = e.clientY - originY.value + 20 + 'px';
        emit('extenting', {
          x: ww.value.width,
          y: ww.value.height,
        });
      }
    }
  }
}
//拖拽结束
function cardMouseUp() {
  dragStart.value = false;
  extentIconIsDown.value = false;
}
</script>
<template>
  <NCard
    v-if="inPara.show"
    :style="ww"
    :content-style="{ width: '100%', height: '100%' }"
    closable
    @close="modalClose"
    @mousemove="dragCard"
    @mouseup="cardMouseUp"
  >
    <template #header>
      <div style="width: 100%; height: 100%" @mousedown="cardMouseDown">
        {{ inPara.title }}
      </div>
    </template>
    <template #header-extra>
      <NSwitch @update:value="handleSwitch">
        <template #checked>
          {{ translate.t('关闭拖拽') }}
        </template>
        <template #unchecked>
          {{ translate.t('开启拖拽') }}
        </template>
      </NSwitch>
      <NDivider vertical />
    </template>
    <template #default>
      <slot></slot>
    </template>
    <template #footer>
      <NSpace v-if="inPara.useExtent" justify="end">
        <NIcon size="20">
          <Icon_Drag style="transform: rotate(90deg)" @mousedown="extendIconMouseDown" />
        </NIcon>
      </NSpace>
    </template>
  </NCard>
</template>
<style scoped></style>
