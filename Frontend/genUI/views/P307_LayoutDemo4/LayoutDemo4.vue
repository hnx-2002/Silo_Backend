<script setup>
import { ref } from 'vue';
import { NSpace, NDataTable, NButton, NForm, NFormItem, NInput } from 'naive-ui';
import { translate } from '@utility/i18n.js';

//显示的列
const columns = ref([
    {
        title: 'No',
        key: 'no',
    },
    {
        title: 'Title',
        key: 'title',
    },
    {
        title: 'Length',
        key: 'length',
    },
]);
const pagination = ref({ pageSize: 20 });
const formValue = ref({
    user: {
        name: '',
        age: '',
    },
    phone: '',
});
const tableData = ref([
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
    { no: 3, title: 'Wonderwall', length: '4:18' },
    { no: 4, title: "Don't Look Back in Anger", length: '4:48' },
    { no: 12, title: 'Champagne Supernova', length: '7:27' },
]); //表格数据

function handleValidateClick() {}
</script>

<template>
    <div class="auto_fit_table">
        <n-form
            ref="formRef"
            inline
            :label-width="80"
            :model="formValue"
            label-placement="left"
            :show-feedback="false"
        >
            <n-form-item label="姓名" path="user.name">
                <n-input v-model:value="formValue.user.name" placeholder="输入姓名" />
            </n-form-item>
            <n-form-item label="年龄" path="user.age">
                <n-input v-model:value="formValue.user.age" placeholder="输入年龄" />
            </n-form-item>
            <n-form-item label="电话号码" path="phone">
                <n-input v-model:value="formValue.phone" placeholder="电话号码" />
            </n-form-item>
            <n-form-item>
                <n-button attr-type="button" @click="handleValidateClick">验证</n-button>
            </n-form-item>
        </n-form>
        <NSpace class="table_top_btn">
            <NButton type="primary">{{ translate.t('按钮1') }}</NButton>
            <NButton type="primary" ghost>{{ translate.t('按钮2') }}</NButton>
            <n-button ghost>{{ translate.t('按钮3') }}</n-button>
            <NButton type="primary" disabled>{{ translate.t('禁用按钮1') }}</NButton>
            <NButton type="primary" ghost disabled>{{ translate.t('禁用按钮2') }}</NButton>
            <n-button ghost disabled>{{ translate.t('禁用按钮3') }}</n-button>
        </NSpace>
        <div class="table-content">
            <NDataTable
                ref="table"
                bordered
                striped
                :columns="columns"
                :data="tableData"
                :style="{ height: `100%` }"
                :pagination="pagination"
                flex-height
            ></NDataTable>
        </div>
    </div>
</template>
<style scoped></style>
