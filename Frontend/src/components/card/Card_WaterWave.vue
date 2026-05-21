<script setup>
//xiexin added 20220428
import { NP, NSpace, NButton } from 'naive-ui';

const inPara = defineProps({
    data: String,
    name: String,
    method: Function,
});
</script>
<template>
    <div id="container" style="text-align: center">
        <div class="circleLabel"></div>
        <div class="circleLabel"></div>
        <div class="circleLabel"></div>
        <div class="circleLabel"></div>
        <NSpace vertical>
            <div style="height: 8px"></div>
            <NButton
                style="z-index: 20; color: white"
                type="info"
                strong
                quaternary
                round
                @click="inPara.method"
            >
                {{ data }}
            </NButton>
            <div></div>
            <NP style="color: white">{{ name }}</NP>
        </NSpace>
    </div>
</template>
<style scoped>
@keyframes turnAround {
    100% {
        transform: rotate(360deg);
    }
}

.circleLabel {
    position: fixed;
    border-radius: 50%;
    background: rgba(0, 0, 0, 0.01);
    z-index: 10;
}

.circleLabel:nth-of-type(1) {
    width: 60px;
    height: 55px;
    animation: turnAround 3s infinite linear;
    box-shadow: 0 0 1px 0 #869fe4, inset 0 0 10px 0 #869fe4;
}

.circleLabel:nth-of-type(2) {
    width: 55px;
    height: 60px;
    animation: turnAround 5s infinite linear;
    box-shadow: 0 0 1px 0 #3397f2, inset 0 0 10px 0 #3397f2;
}

.circleLabel:nth-of-type(3) {
    width: 65px;
    height: 60px;
    animation: turnAround 2s infinite linear;
    box-shadow: 0 0 1px 0 #3eaadc, inset 0 0 10px 0 #3eaadc;
}

.circleLabel:nth-of-type(4) {
    width: 60px;
    height: 65px;
    animation: turnAround 7s infinite linear;
    box-shadow: 0 0 1px 0 #09f, inset 0 0 10px 0 #09f;
}

#container {
    padding-top: 10px;
    perspective: 9999;
    -webkit-perspective: 9999;
    width: 90px;
    height: 110px;
    display: flex;
    justify-content: center;
}
</style>
