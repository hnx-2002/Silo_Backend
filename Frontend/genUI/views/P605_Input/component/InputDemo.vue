<script setup>
import { NCard, NInput, NSpace, NText, NGrid, NGi } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '1-' + translate.t('标准输入框')">
        <NSpace vertical>
            <NGrid x-gap="8" :cols="6">
                <NGi><NInput></NInput></NGi>
                <NGi><NText>默认状态</NText></NGi>
                <NGi><NText style="color: red">描边色值：#d2d7da</NText></NGi>
                <NGi><NText style="color: red">填充色值：#ffffff</NText></NGi>
                <NGi><NText style="color: red">文字色值：#b6bac2</NText></NGi>
                <NGi><NText style="color: red"></NText></NGi>
            </NGrid>
            <NGrid x-gap="8" :cols="6">
                <NGi><NInput></NInput></NGi>
                <NGi><NText>鼠标经过状态</NText></NGi>
                <NGi><NText style="color: red">描边色值：#3083fd</NText></NGi>
                <NGi><NText style="color: red">填充色值：#ffffff</NText></NGi>
                <NGi><NText style="color: red">文字色值：#b6bac2</NText></NGi>
                <NGi><NText style="color: red"></NText></NGi>
            </NGrid>
            <NGrid x-gap="8" :cols="6">
                <NGi><NInput></NInput></NGi>
                <NGi><NText>点击编写状态</NText></NGi>
                <NGi><NText style="color: red">描边色值：#3083fd</NText></NGi>
                <NGi><NText style="color: red">填充色值：#ffffff</NText></NGi>
                <NGi><NText style="color: red">文字色值：#333333</NText></NGi>
                <NGi><NText style="color: red">阴影色值：#3083fd 模糊度：4px 方向：无</NText></NGi>
            </NGrid>
            <NGrid x-gap="8" :cols="6">
                <NGi><NInput></NInput></NGi>
                <NGi><NText>输入完成状态</NText></NGi>
                <NGi><NText style="color: red">描边色值：#d2d7da</NText></NGi>
                <NGi><NText style="color: red">填充色值：#ffffff</NText></NGi>
                <NGi><NText style="color: red">文字色值：#333333</NText></NGi>
                <NGi><NText style="color: red"></NText></NGi>
            </NGrid>
            <NGrid x-gap="8" :cols="6">
                <NGi><NInput status="error"></NInput></NGi>
                <NGi><NText>输入错误状态</NText></NGi>
                <NGi><NText style="color: red">描边色值：#f53f3e</NText></NGi>
                <NGi><NText style="color: red">填充色值：#ffffff</NText></NGi>
                <NGi><NText style="color: red">文字色值：#333333</NText></NGi>
                <NGi><NText style="color: red"></NText></NGi>
            </NGrid>
        </NSpace>
    </NCard>
</template>
<style scoped></style>
