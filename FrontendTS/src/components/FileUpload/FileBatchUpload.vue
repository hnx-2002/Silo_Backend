<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { NUpload, NSpace, NSpin, NUploadDragger, NIcon, NText, NDataTable } from 'naive-ui';
import type { UploadFileInfo, DataTableColumns } from 'naive-ui';
import type { FileData, BatchUploadRow } from './FileUpload.d.ts';
import { ArchiveOutline as ArchiveIcon } from '@vicons/ionicons5';
import { make_text_NButton } from '@utility/NaiveHComponent.ts';
import { exeUpload } from './uploadExe.ts';
import { translate } from '@utility/i18n.ts';

const fileData = ref<FileData>({} as FileData);

//显示的列
const columns = ref<DataTableColumns<BatchUploadRow>>([]);

const fileList = ref<FileData[]>([]); //批量上传的文件集合
const showSpin = ref(false);

const inPara = defineProps({
  maxLimit: Number,
});
const emit = defineEmits(['uploadChanged']);
onMounted(() => {
  resetColumns();
});

//往里放的时候会触发
function beforeUpload(data: { file: UploadFileInfo; fileList: UploadFileInfo[] }) {
  //console.log('BeforeUpload', data);
  let item = data.file;
  if (item.file?.size == 0) {
    window.$message.error(
      translate.t('文件') + item.file?.name + translate.t('为空') + ',' + translate.t('请重新选择'),
    );
    return;
  }

  let arr = item.name.split('.');

  fileData.value = {
    file_id: item.id,
    file_name: item.name, //文件名
    file_length: item.file?.size, //文件大小
    file_ext: arr[arr.length - 1], //文件扩展名
    content: item.file, //文件内容
  };

  let hasTest = false;
  for (const file of fileList.value) {
    if (file.file_id == fileData.value.file_id) {
      hasTest = true;
    }
  }

  if (!hasTest) {
    showSpin.value = true;
    exeUpload(
      fileData.value,
      (okMsg) => {
        fileList.value.push(fileData.value);
        emit('uploadChanged', fileList.value);
        window.$message.success(translate.t(okMsg));
        showSpin.value = false;
      },
      (errorMsg) => {
        window.$message.error(translate.t(errorMsg) + ':' + fileData.value.file_name);
        showSpin.value = false;
      },
      () => {
        window.$message.info(translate.t('文件较大') + ',' + translate.t('请耐心等待'));
      },
      (len) => {
        window.$message.info(
          translate.t('文件片段数') + ':' + len + ',' + translate.t('请耐心等待'),
        );
      },
    );
  }
}

//重置列
function resetColumns() {
  columns.value = [
    {
      key: 'index',
      title: translate.t('序号'),
      align: 'center',
      width: '50px',
      ellipsis: {
        tooltip: true,
      },
      render(_row, index) {
        return index + 1;
      },
    },
    {
      key: 'file_name',
      title: translate.t('文件'),
      ellipsis: {
        tooltip: true,
      },
      sorter: 'default',
    },
    {
      key: 'file_length',
      title: translate.t('大小'),
      width: '100px',
      ellipsis: {
        tooltip: true,
      },
      sorter: 'default',
    },
    {
      key: 'file_ext',
      title: translate.t('类型'),

      width: '100px',
      ellipsis: {
        tooltip: true,
      },
      sorter: 'default',
    },
    {
      key: 'operation',
      title: translate.t('操作'),
      width: 160,
      render(row) {
        return make_text_NButton(translate.t('删除'), () => delRow(row));
      },
    },
  ];
}

//删除
function delRow(row: BatchUploadRow) {
  //console.log('Remove', data);
  let newArr = [];
  for (const file of fileList.value) {
    if (file.file_id != row.file_id) {
      newArr.push(file);
    }
  }
  fileList.value = newArr;
  emit('uploadChanged', fileList.value);
}
</script>
<template>
  <NSpin :show="showSpin">
    <NSpace vertical style="width: 100%">
      <n-upload
        :show-file-list="false"
        :default-upload="false"
        :directory="false"
        :multiple="true"
        directory-dnd
        :max="inPara.maxLimit || 10"
        :on-before-upload="beforeUpload"
      >
        <n-upload-dragger>
          <div style="margin-bottom: 12px">
            <n-icon size="48" :depth="3">
              <ArchiveIcon />
            </n-icon>
          </div>
          <n-text style="font-size: 16px">
            {{ translate.t('点击或者拖动文件到该区域来上传') }}
          </n-text>
        </n-upload-dragger>
      </n-upload>
      <NDataTable
        ref="table"
        bordered
        striped
        :columns="columns"
        :data="fileList"
        :style="{ height: `300px` }"
        flex-height
      ></NDataTable>
    </NSpace>
  </NSpin>
</template>
<style scoped></style>
