import type { Composer } from 'vue-i18n';
import type { LanDict } from '../../types/LanDict';

/**
 * 显示删除确认对话框
 * @param translate - 国际化翻译对象
 * @param inFunc - 确认删除后执行的回调函数
 */
export function confirmDel(
  translate: Composer<LanDict, {}, {}, keyof LanDict>,
  inFunc: () => void
): void {
  window.$dialog.warning({
    title: translate.t('警告'),
    content: translate.t('确定删除') + '?',
    positiveText: translate.t('确定'),
    negativeText: translate.t('取消'),
    onPositiveClick: () => {
      inFunc();
    },
    onNegativeClick: () => {},
  });
}
