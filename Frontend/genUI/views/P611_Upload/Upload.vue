<script setup>
import { NSpace, NCard, NText, NUpload, NUploadDragger } from 'naive-ui';
import TitleDescription from '../Common/TitleDescription/TitleDescription.vue';

import Icon_Plus from '@common/icon/Icon_Plus.vue';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <TitleDescription :Title="translate.t('使用场景')" :Description="''"></TitleDescription>
        <NCard :title="translate.t('下载示例')">
            <n-upload multiple directory-dnd :max="5">
                <n-upload-dragger>
                    <NSpace vertical>
                        <Icon_Plus></Icon_Plus>
                        <n-text style="color: rgb(51, 51, 51); font-size: 14px">
                            点击或拖拽文件到此处上传
                        </n-text>
                        <n-text style="color: rgb(134, 144, 156); font-size: 12px">
                            此处是附件上传格式，大小等具体要求
                        </n-text>
                    </NSpace>
                </n-upload-dragger>
            </n-upload>
        </NCard>
    </NSpace>
</template>
<style scoped></style>
