<script setup>
import { NSpace, NCard, NText } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import TimeLineDemo1 from './component/TimeLineDemo1.vue';
import TimeLineDemo2 from './component/TimeLineDemo2.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="
                translate.t(
                    '垂直展示的时间流信息，一般按时间倒叙记录事件，适用于包括事件，任务，日历标注以及其他相关的数据展示'
                )
            "
        ></TitleDescription>
        <TimeLineDemo1></TimeLineDemo1>
        <TimeLineDemo2></TimeLineDemo2>
    </NSpace>
</template>
<style scoped></style>
