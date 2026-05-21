<script setup>
//xiexin added 20220428
import { NFlex } from 'naive-ui';
import blue from './imgs/blue.png';
import cyan from './imgs/cyan.png';
import green from './imgs/green.png';
const inPara = defineProps({
    data: String,
    name: String,
    method: Function,
    forbidden: Boolean,
    color: String,
});

function dataColor() {
    switch (inPara.color) {
        case 'blue':
            return '#41b7fc';
        case 'cyan':
            return '#02e5f3';
        case 'green':
            return '#64de6e';
        default:
            return '#41b7fc';
    }
}
function databg() {
    switch (inPara.color) {
        case 'blue':
            return blue;
        case 'cyan':
            return cyan;
        case 'green':
            return green;
        default:
            return blue;
    }
}
</script>
<template>
    <div class="container" style="text-align: center">
        <NFlex vertical :size="3">
            <button
                :disabled="typeof inPara.method != 'function' || inPara.forbidden"
                class="water-btn"
                :style="{
                    color: dataColor(),
                    background: `url(${databg()})`,
                }"
                @click="inPara.method"
            >
                {{ data }}
            </button>
            <div style="color: #fff; transform: translateY(-5px); font-size: 13px">{{ name }}</div>
        </NFlex>
    </div>
</template>
<style scoped>
.water-btn {
    background: url('./imgs/blue.png');
    border: none;
    color: #fff;
    height: 85px;
    width: 85px;
    background-size: cover;
    color: #43aff0;
    font-weight: bold;
    font-size: 18px;
    cursor: pointer;
}
.container {
    height: 100%;
    display: flex;
    justify-content: center;
}
</style>
