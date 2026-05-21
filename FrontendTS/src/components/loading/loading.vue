<script setup lang="ts">
import { ref } from 'vue';
import * as signalR from '@microsoft/signalr';

const nameSpace = import.meta.env.VITE_NAMESPACE;

const visible = ref(false);
const message = ref('');
let connection: signalR.HubConnection | null = null;

// 初始化SignalR连接
const initSignalRConnection = () => {
  connection = new signalR.HubConnectionBuilder().withUrl('/' + nameSpace + '/MessageHub').build();

  // 注册收到信息的动作
  connection.on('ReceiveSelfMessage', function (msg) {
    // window.$message.info(msg);
    // console.log(msg);
    message.value = msg;
  });

  // 启动连接
  connection
    .start()
    .then(function () {
      connection?.invoke('OnConnectedWithAccount').catch(function (err) {
        return console.error(err.toString());
      });
    })
    .catch(function (err) {
      return console.error(err.toString());
    });
};

// 销毁SignalR连接
const destroySignalRConnection = () => {
  if (connection) {
    // 取消注册事件处理器
    connection.off('ReceiveSelfMessage');

    // 停止连接
    connection.stop().catch(function (err) {
      return console.error(err.toString());
    });

    // 清空连接引用
    connection = null;
  }
};

const open = (msg: string) => {
  message.value = msg;
  visible.value = true;
  initSignalRConnection();
};

const close = () => {
  visible.value = false;
  destroySignalRConnection();
};

defineExpose({
  open,
  close,
});
</script>

<template>
  <div v-if="visible" class="loading-overlay">
    <div class="loading-img"></div>
    <p>{{ message }}</p>
  </div>
</template>

<style lang="scss" scoped>
@keyframes rotate {
  0% {
    transform: rotate(0deg);
  }

  100% {
    transform: rotate(360deg);
  }
}

.loading-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.9);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  z-index: 9999;

  p {
    margin-top: 60px;
    font-size: 26px;
    color: #3083fd;
  }

  .loading-img {
    width: 30px;
    height: 30px;
    border: 2px solid #3083fd;
    border-radius: 21px;
    border-left-color: transparent;
    animation: rotate 1000ms infinite;
  }
}
</style>
