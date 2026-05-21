<script setup>
import { NSpace, NH2, NH3, NH4, NCard } from 'naive-ui';

import { translate } from '@utility/i18n.js';
</script>

<template>
    <NSpace vertical>
        <div style="background-color: dodgerblue; width: 100%; height: 300px">
            <NSpace vertical>
                <div style="height: 10px"></div>
                <NSpace justify="center">
                    <NSpace vertical>
                        <div style="font-size: 52px; color: white">
                            {{ translate.t('平台管理系统平台视觉设计规范说明') }}
                        </div>
                        <NH3>{{ translate.t('适用于数字化研究所平台管理系统Web端产品') }}</NH3>
                    </NSpace>
                    <NCard>
                        <NSpace vertical>
                            <NH3>
                                {{ translate.t('天津水泥工业设计研究院有限公司') }}
                            </NH3>
                            <NH4>
                                {{ translate.t('数字化研究所') }}
                            </NH4>
                            <NH4>2022-12-30</NH4>
                        </NSpace>
                    </NCard>
                </NSpace>
                <div style="height: 10px"></div>
            </NSpace>
        </div>

        <NSpace vertical style="margin-left: 20px">
            <NH2>
                {{ translate.t('1 本视觉设计规范适用于数字化研究所平台管理系统Web端产品;') }}
            </NH2>
            <NH2>
                {{ translate.t('2 设计稿基于1920x1080屏幕尺寸;') }}
            </NH2>
            <NH2>
                {{
                    translate.t(
                        '3 此设计规范明确再界面布局中要遵循的准则和方法,确保界面布局和样式的统一,以达到空值产品设计质量,提高设计效率的目的;'
                    )
                }}
            </NH2>
            <NH2>
                {{
                    translate.t(
                        '4 此规范为基本标准,面向UI设计及前端开发人员,使用者应遵照本规范对产品进行设计和开发工作,规范未提及部分,应当尽可能采取相似原则,以同类模块内容为参照依据;'
                    )
                }}
            </NH2>
            <NH3>{{ translate.t('备注:') }}</NH3>
            <NH3>{{ translate.t('1 Web端在页面设计开发过程中根据实际情况可适当调节;') }}</NH3>
            <NH3>{{ translate.t('2 图片质量单位为pixt,简写px;文字质量单位为pixt,简写px;') }}</NH3>
            <NH3>
                {{
                    translate.t(
                        '3 输出空间素材名称规范: Normal/正常, Hover/选中, Active/点击, Forbid/禁用'
                    )
                }}
            </NH3>
        </NSpace>
    </NSpace>
</template>
<style scoped></style>
