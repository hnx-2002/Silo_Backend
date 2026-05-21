<script setup>
import { NSpace, NCard, NProgress, NButton } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';
import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription
            :Title="translate.t('使用场景')"
            :Description="'在操作需要较长时间才能完成时，为用户显示该操作的当前进度和状态'"
        ></TitleDescription>
        <NCard :title="translate.t('示例')">
            <NSpace vertical>
                <NSpace>
                    <n-progress
                        style="width: 200px"
                        type="line"
                        :percentage="0"
                        :show-indicator="false"
                    />
                    <span>0%</span>
                </NSpace>
                <NSpace>
                    <n-progress
                        style="width: 200px"
                        type="line"
                        :percentage="70"
                        :show-indicator="false"
                    />
                    <span>进行中</span>
                </NSpace>
                <NSpace>
                    <n-progress
                        style="width: 200px"
                        type="line"
                        :percentage="100"
                        :show-indicator="false"
                    />
                    <span>完成</span>
                </NSpace>
                <NSpace>
                    <n-progress
                        style="width: 200px"
                        type="line"
                        :percentage="100"
                        :show-indicator="false"
                        :status="'error'"
                    />
                    <span>失败</span>
                </NSpace>

                <NSpace>
                    <n-progress
                        style="width: 200px"
                        type="line"
                        :percentage="100"
                        :show-indicator="false"
                        :color="'#d2d7da'"
                    />
                    <span>暂停</span>
                </NSpace>
                <n-progress type="circle" :percentage="25" :offset-degree="180" />
            </NSpace>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
