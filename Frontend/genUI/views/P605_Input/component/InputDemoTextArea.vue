<script setup>
import { NCard, NInput, NSpace, NText, NGrid, NGi } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const textVal =
    '天津水泥院的“工业工程智能设计”成功入选“十佳首展成果”。《天津日报》，津云，学习强国，《中国建材杂志》等主流媒体纷纷进行了相关报告和转载，引起了社会广泛关注。';
</script>

<template>
    <NCard :title="translate.t('示例') + '3-' + translate.t('文本输入')">
        <NText style="color: red">字数限制,左上，右下间距暂时无法响应规定</NText>
        <NSpace>
            <NInput class="textArea" type="textarea" maxlength="200" show-count></NInput>
            <NInput class="textArea" type="textarea" maxlength="200" show-count></NInput>
            <NInput
                class="textArea"
                type="textarea"
                maxlength="200"
                show-count
                v-model:value="textVal"
            ></NInput>
        </NSpace>
    </NCard>
</template>
<style scoped>
.textArea {
    width: 300px;
    height: 200px;
}
</style>
