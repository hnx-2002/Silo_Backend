<script setup lang="ts">
import { NSpace } from 'naive-ui';

const inPara = defineProps({
  title: String,
});
</script>
<template>
  <NSpace vertical>
    <div id="guntext">{{ inPara.title }}</div>
    <slot></slot>
  </NSpace>
</template>
<style scoped>
#guntext {
  border-left: solid 2px #3083fd;
  line-height: 14px;
  font-size: 14px;
  font-weight: bold;
  padding-left: 14px;
}
</style>
