<script setup>
import { onMounted, ref } from 'vue';
import { NInput, NButton, NForm, NFormItem } from 'naive-ui';
import UserIcon from '../../components/icon/login_images/Icon_user.vue';
import PasswdIcon from '../../components/icon/login_images/Icon_passwd.vue';
import PasswdShow from '../../components/icon/login_images/Icon_passwd_show.vue';
import PasswdHide from '../../components/icon/login_images/Icon_passwd_hide.vue';
import Icon_Blank from '../../components/icon/Icon_Blank.vue';
import { checkLogin } from './login';
import cookie from '@utility/CookieTool.js';
import { translate } from '@utility/i18n.js';

const inPara = defineProps({
    welcomeTitle: String,
    logoPath: String,
});

const form = ref({
    account: '',
    password: '',
    vcode: '',
});

const welcomeText = ref('');

onMounted(() => {
    welcomeText.value = inPara.welcomeTitle ? inPara.welcomeTitle : import.meta.env.VITE_NAMESPACE;
});

function handleLogin() {
    checkLogin(form.value.account, form.value.password).then((res) => {
        if (res) {
            if (!res.status) {
                window.$message.warning(res.msg);
            } else {
                window.$message.warning(translate.t('登录成功'));
                cookie.setCookie('tp_token', res.token, 2);

                let jump = '/' + import.meta.env.VITE_NAMESPACE + '/index.html';

                if (res.isAdmin) {
                    let jumpTest = window.confirm('进入管理页?');
                    if (jumpTest) {
                        jump = '/' + import.meta.env.VITE_NAMESPACE + '/admin.html';
                    }
                }

                setTimeout(() => {
                    window.location = jump;
                }, 300);
            }
        }
    });
}

//键盘事件登录
function toLogin(e) {
    if (e.code == 'Enter') {
        handleLogin();
    }
}
</script>
<template>
    <div id="login">
        <div id="loginbg"></div>
        <div id="loginform">
            <img id="logo" :src="inPara.logoPath" />
            <slot name="prefix"></slot>
            <div class="welcome">
                <p class="text chi">
                    {{ welcomeText }}
                </p>
                <p class="text eng">WELCOME</p>
            </div>

            <NForm
                style="width: 60%"
                ref="formRef"
                :model="form"
                label-placement="left"
                :label-width="60"
                size="medium"
            >
                <NFormItem :label="' '" path="account">
                    <template #label>
                        <UserIcon></UserIcon>
                    </template>
                    <NInput
                        :placeholder="translate.t('请输入') + translate.t('账号')"
                        v-model:value="form.account"
                        @keydown="toLogin"
                    >
                        <template #suffix>
                            <Icon_Blank></Icon_Blank>
                        </template>
                    </NInput>
                </NFormItem>

                <NFormItem :label="' '" path="password">
                    <template #label>
                        <PasswdIcon></PasswdIcon>
                    </template>
                    <NInput
                        type="password"
                        show-password-on="click"
                        :placeholder="translate.t('请输入') + translate.t('密码')"
                        v-model:value="form.password"
                        @keydown="toLogin"
                    >
                        <template #password-visible-icon>
                            <PasswdShow></PasswdShow>
                        </template>
                        <template #password-invisible-icon>
                            <PasswdHide></PasswdHide>
                        </template>
                    </NInput>
                </NFormItem>

                <NFormItem :label="' '">
                    <NButton style="width: 100%" type="primary" @click="handleLogin">
                        {{ translate.t('登录') }}
                    </NButton>
                </NFormItem>
            </NForm>

            <slot name="affix"></slot>
        </div>
    </div>
</template>
<style lang="scss">
html,
body,
#login {
    width: 100%;
    height: 100%;
    margin: 0;
    display: flex;
    font-family: v-sans, system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif,
        'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol';
}

#logo {
    position: fixed;
    right: 4.7625vw;
    top: 4.375vh;
}
#loginbg {
    height: 100%;
    width: 61.1vw;
    background-image: url('/src/assets/images/loginbg.webp');
    background-repeat: no-repeat;
    background-size: cover;
    background-position: center;
}
#loginform {
    height: 100%;
    flex: 1;
    min-width: 500px;
    overflow-x: hidden;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    .welcome {
        width: 29.375vw;
        position: relative;
        height: 90px;
        margin-bottom: 50px;
        min-width: 350px;
        .text {
            margin: 0;
            position: absolute;
            line-height: 1;
            &.eng {
                top: 0;
                font-size: 90px;
                font-weight: bold;
                color: rgb(242, 243, 245);
            }
            &.chi {
                top: 50%;
                z-index: 9;
                transform: translateY(-50%);
                color: rgb(51, 51, 51);
                font-size: 42px;
                font-weight: bold;
            }
        }
    }
}
@media only screen and (max-width: 1285px) {
    #logo {
        right: 69px;
    }
}
</style>
