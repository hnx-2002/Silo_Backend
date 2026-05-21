<!--
 * @Descripttion:
 * @version:
 * @Author: 孟庆尧
 * @Date: 2024-05-08 14:04:10
 * @LastEditors: 孟庆尧
 * @LastEditTime: 2024-05-08 14:18:15
-->
<script setup></script>

<template>
    <div class="title">
        <slot>Title</slot>
        <div>
            <slot name="affix"></slot>
        </div>
        <div class="last">
            <slot name="last"></slot>
        </div>
    </div>
</template>
<style lang="scss" scoped>
    .title {
        font-weight: bold;
        position: relative;
        border-left: 3px solid #3083fd;
        margin: 10px 0;
        padding-left: 8px;
        font-size: 15px;
        display: flex;
        height: 15px;
        line-height: 15px;
        align-items: center;
        justify-content: flex-start;
        gap: 10px;

        .last {
            position: absolute;
            right: 0;
        }
    }
</style>
