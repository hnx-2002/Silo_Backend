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
  NImage,
} from 'naive-ui';
import { h } from 'vue';
import type { VNode, Component } from 'vue';
import type { DropdownOption } from 'naive-ui';

export function h_NButton(
  btnName: string,
  type: 'default' | 'tertiary' | 'primary' | 'info' | 'success' | 'warning' | 'error',
  func: () => void
): VNode {
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

export function make_h_NButton(btnName: string, func: () => void): VNode {
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

export function make_text_NButton(
  btnName: string,
  func: () => void,
  isDisabled: boolean = false
): VNode {
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

export function make_download_Abutton(
  btnName: string,
  fileUrl: string,
  isDisabled: boolean = false
): VNode {
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

export function make_h_NSpace(
  inComponents: VNode[],
  justify: 'start' | 'end' | 'center' | 'space-around' | 'space-between' | 'space-evenly',
  vertical: boolean
): VNode {
  return h(NSpace, { justify, vertical }, {
    default: () => inComponents
  });
}

export function make_h_NFlex(inComponents: VNode[], justify: string, align: string): VNode {
  return h(NFlex, { justify, align }, {
    default: () => inComponents
  });
}

export function make_h_NDivider(): VNode {
  return h(NDivider, { vertical: true });
}

export function make_h_span(text: string): VNode {
  return h('span', text);
}

export function make_h_checkbox(checked: boolean): VNode {
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

export function make_divWithStyle(
  inClass: string,
  inStyle: object,
  inValue: string,
  content: VNode[]
): VNode {
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

export function make_icon(icon: Component): VNode {
  return h(NIcon, null, { default: () => h(icon) });
}

export function make_toolTip(triggerText: string, toolTipText: string): VNode {
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

export function make_dropDown(
  opt: DropdownOption[],
  text: string,
  func: (key: string) => void
): VNode {
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

export function h_tag(
  txt: string,
  tagType: 'default' | 'primary' | 'success' | 'info' | 'warning' | 'error' = 'info'
): VNode {
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

export function h_colorTag(
  txt: string,
  txtColor: string,
  backColor: string,
  borderColor: string
): VNode {
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

export function h_base64Img(
  mime: string,
  base64str: string,
  imgDescription: string,
  width: number
): VNode {
  return h(NImage, {
    src: 'data:' + mime + ';base64,' + base64str,
    alt: imgDescription,
    width: width,
  });
}
