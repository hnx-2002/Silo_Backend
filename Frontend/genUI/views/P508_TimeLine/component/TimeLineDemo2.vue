<script setup>
import { ref, computed } from 'vue';
import { NCard, NTimeline, NTimelineItem } from 'naive-ui';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NCard :title="translate.t('示例') + '2'">
        <n-timeline horizontal>
            <n-timeline-item :title="translate.t('天津水泥院大事件记录一')" time="2022-06-26" />
            <n-timeline-item :title="translate.t('天津水泥院大事件记录二')" time="2022-06-26" />
            <n-timeline-item
                type="info"
                :title="translate.t('天津水泥院大事件记录三')"
                :content="'AAAAA'"
                time="2022-07-06"
            />
        </n-timeline>
    </NCard>
</template>
<style scoped></style>
