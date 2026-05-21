import {
    NButton,
    NSpace,
    NIcon,
    NTooltip,
    NDivider,
    NCheckbox,
    NDropdown,
    NTag,
    NFlex,
} from 'naive-ui';
import { h } from 'vue';

//列表单行按钮
export function h_NButton(btnName, type, func) {
    return h(
        NButton,
        {
            type: type,
            onClick: func,
        },
        {
            default: () => btnName,
        }
    );
}

//列表单行按钮
export function make_h_NButton(btnName, func) {
    return h(
        NButton,
        {
            size: 'small',
            type: 'primary',
            onClick: func,
        },
        {
            default: () => btnName,
        }
    );
}

//列表文字按钮 lhy added 20220623
export function make_text_NButton(btnName, func, isDisabled = false) {
    return h(
        NButton,
        {
            text: true,
            disabled: isDisabled,
            size: 'small',
            type: 'primary',
            onClick: func,
        },
        {
            default: () => btnName,
        }
    );
}

//下载用a标签
export function make_download_Abutton(btnName, fileUrl, isDisabled = false) {
    return h(
        NButton,
        {
            size: 'small',
            type: 'primary',
            download: true,
            tag: 'a',
            text: true,
            disabled: isDisabled,
            href: fileUrl,
        },
        {
            default: () => btnName,
        }
    );
}

//NSpace
export function make_h_NSpace(inComponents, justify, vertical) {
    return h(NSpace, { justify, vertical }, inComponents);
}

//NFlex
export function make_h_NFlex(inComponents, justify, align) {
    return h(NFlex, { justify, align }, inComponents);
}

//NDivider 竖分割线
export function make_h_NDivider() {
    return h(NDivider, { vertical: true }, null);
}

//span
export function make_h_span(text) {
    return h('span', text);
}

//NCheckbox 不可编辑的复选框 lhy added 20220623
export function make_h_checkbox(checked) {
    return h(
        NCheckbox,
        {
            checked: checked,
            disabled: true,
        },
        {
            default: () => '',
        }
    );
}

//通用div
export function make_divWithStyle(inClass, inStyle, inValue, content) {
    return h(
        'div',
        {
            class: inClass,
            style: inStyle,
            value: inValue,
        },
        content
    );
}

//菜单图标
export function make_icon(icon) {
    return h(NIcon, null, { default: () => h(icon) });
}

//ToolTip
export function make_toolTip(triggerText, toolTipText) {
    return h(
        NTooltip,
        {
            placement: 'right',
            trigger: 'hover',
        },
        {
            trigger: () => h('span', {}, triggerText),
            default: () => h('span', {}, toolTipText),
        }
    );
}

//DropDown
export function make_dropDown(opt, text, func) {
    return h(
        NDropdown,
        {
            placement: 'right',
            trigger: 'hover',
            showArrow: true,
            size: 'medium',
            options: opt,
            onSelect: func,
        },
        {
            // trigger: () => h('span', {}, triggerText),
            default: () => h('span', {}, text),
        }
    );
}

//动态标签
export function h_tag(txt, tagType = 'info') {
    return h(
        NTag,
        { type: tagType },
        {
            default: () => {
                return txt;
            },
        }
    );
}

//动态标签,自定义颜色
export function h_colorTag(txt, txtColor, backColor, borderColor) {
    return h(
        NTag,
        {
            color: {
                color: backColor,
                textColor: txtColor,
                borderColor: borderColor,
            },
        },
        {
            default: () => {
                return txt;
            },
        }
    );
}

//生成图片
export function h_base64Img(mime, base64str, imgDescription) {
    //<img src="data:[MIME类型];base64,[Base64编码字符串]" alt="图片描述">
    return h('img', { src: 'data:' + mime + ';base64,' + base64str, alt: imgDescription });
}
