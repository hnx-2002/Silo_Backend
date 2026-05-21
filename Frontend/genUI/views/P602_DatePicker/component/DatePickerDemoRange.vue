<script setup>
import { NCard, NDatePicker, NSpace, NText } from 'naive-ui';
import { translate } from '@utility/i18n.js';

const timestamp = 1183135260000;
</script>

<template>
    <NCard :title="translate.t('示例') + '3'">
        <NSpace vertical>
            <NText style="color: red">说明：</NText>
            <NText style="color: red">1 规定无标注</NText>
            <NText style="color: red">2 此处使用NaiveUI一个input框表示日期范围和日期时间范围</NText>
            <NSpace>
                <div>日期范围</div>
                <n-date-picker class="dpdemp2" v-model:value="timestamp" type="daterange" />
                <div>日期时间范围</div>
                <n-date-picker class="dpdemp2" v-model:value="timestamp" type="datetimerange" />
            </NSpace>
        </NSpace>
    </NCard>
</template>
<style scoped>
.dpdemp2 {
    width: 300px;
}
</style>
