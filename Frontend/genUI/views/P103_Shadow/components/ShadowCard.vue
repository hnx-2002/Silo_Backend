<script setup>
import { ref } from 'vue';
import { NSpace, NText } from 'naive-ui';

import { translate } from '@utility/i18n.js';

const shadowColor = 'rgba(210,215,218,0.6)';
const types = ref([
    {
        title: translate.t('阴影向下'),
        colorStr: translate.t('阴影') + 'rgba:(210,215,218,0.6)',
        directionStr: translate.t('阴影方向') + '-down：7px',
        blurStr: translate.t('模糊度') + '：22px',
        expandStr: '',
        xOffset: '0px',
        yOffset: '-7px',
        blurVal: '22px',
        expandVal: '0px',
    },
    {
        title: translate.t('阴影向上'),
        colorStr: translate.t('阴影') + 'rgba:(210,215,218,0.6)',
        directionStr: translate.t('阴影方向') + '-up：7px',
        blurStr: translate.t('模糊度') + '：22px',
        expandStr: '',
        xOffset: '0px',
        yOffset: '7px',
        blurVal: '22px',
        expandVal: '0px',
    },
    {
        title: translate.t('阴影向左'),
        colorStr: translate.t('阴影') + 'rgba:(210,215,218,0.6)',
        directionStr: translate.t('阴影方向') + '-left：5px',
        blurStr: translate.t('模糊度') + '：20px',
        expandStr: '',
        xOffset: '-5px',
        yOffset: '0px',
        blurVal: '20px',
        expandVal: '0px',
    },
    {
        title: translate.t('阴影向右'),
        colorStr: translate.t('阴影') + 'rgba:(210,215,218,0.6)',
        directionStr: translate.t('阴影方向') + '-right：5px',
        blurStr: translate.t('模糊度') + '：20px',
        expandStr: '',
        xOffset: '5px',
        yOffset: '0px',
        blurVal: '20px',
        expandVal: '0px',
    },
    {
        title: translate.t('全局阴影(无方向指示)'),
        colorStr: translate.t('阴影') + 'rgba:(210,215,218,0.6)',
        directionStr: '',
        blurStr: translate.t('模糊度') + '：22px',
        expandStr: translate.t('扩展值') + '：8%',
        xOffset: '0px',
        yOffset: '0px',
        blurVal: '22px',
        expandVal: '8%',
    },
]);
</script>
<template>
    <NSpace justify="space-between">
        <div v-for="item in types">
            <NSpace vertical>
                <!-- border: '1px solid', -->
                <div
                    :style="{
                        width: '260px',
                        height: '200px',
                        boxShadow:
                            shadowColor +
                            ' ' +
                            item.xOffset +
                            ' ' +
                            item.yOffset +
                            ' ' +
                            item.blurVal +
                            ' ' +
                            item.expandVal +
                            ' ',
                    }"
                ></div>
                <NText>{{ item.title }}</NText>
                <NText>{{ item.colorStr }}</NText>
                <NText>{{ item.directionStr }}</NText>
                <NText>{{ item.blurStr }}</NText>
                <NText>{{ item.expandStr }}</NText>
                <NText>
                    {{
                        shadowColor +
                        ' ' +
                        item.xOffset +
                        ' ' +
                        item.yOffset +
                        ' ' +
                        item.blurVal +
                        ' ' +
                        item.expandVal +
                        ' '
                    }}
                </NText>
            </NSpace>
        </div>
    </NSpace>
</template>
<style scoped></style>
