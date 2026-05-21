<script setup>
import { ref, computed } from 'vue';
import { NCard, NText, NSpace, NTag } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '2'">
        <NSpace vertical>
            <!-- :color="{ color: '#BBB', textColor: '#555', borderColor: '#555' }" -->
            <NSpace>
                <n-tag type="info">
                    {{ translate.t('标签') }}
                </n-tag>
                <NText>填充色值：#eaf2ff</NText>
                <NText>文字色值：#3083fd</NText>
                <NText style="color: red">
                    这里填充颜色貌似规定标错了，标签高24px与上一个例子不符，宽度40px没意义，要看文字
                </NText>
            </NSpace>

            <NSpace>
                <n-tag type="success">
                    {{ translate.t('标签') }}
                </n-tag>
                <NText>填充色值：#e9faf2</NText>
                <NText>文字色值：#00b52a</NText>
                <NText style="color: red">这里填充颜色貌似规定标错了</NText>
            </NSpace>

            <NSpace>
                <n-tag type="error">
                    {{ translate.t('标签') }}
                </n-tag>
                <NText>填充色值：#ffece8</NText>
                <NText>文字色值：#f53f3e</NText>
                <NText style="color: red">这里填充颜色貌似规定标错了</NText>
            </NSpace>

            <NSpace>
                <n-tag type="warning">
                    {{ translate.t('标签') }}
                </n-tag>
                <NText>填充色值：#fff1dd</NText>
                <NText>文字色值：#ff7d01</NText>
                <NText style="color: red">这里填充颜色貌似规定标错了</NText>
            </NSpace>

            <NSpace>
                <n-tag :disabled="true">
                    {{ translate.t('暂停') }}
                </n-tag>
                <NText>填充色值：#fff1dd</NText>
                <NText>文字色值：#ff7d01</NText>
                <NText style="color: red">这里填充颜色貌似规定标错了，颜色跟上边橙色一样</NText>
            </NSpace>
        </NSpace>
    </NCard>
</template>
<style scoped></style>
