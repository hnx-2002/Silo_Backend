import type { Composer } from 'vue-i18n';
import type { LanDict } from '../../types/LanDict.ts';

/**
 * 获取task_result的layout_type选项
 *
 * @export
 * @param {Composer<LanDict, {}, {}, keyof LanDict>} translate
 * @return {*}
 */
export function get_task_result_layout_type_options(translate: Composer<LanDict, {}, {}, keyof LanDict>) {
  return [
    { label: translate.t('放置'), value: '放置' },
    { label: translate.t('旋转'), value: '旋转' },
    { label: translate.t('镜像'), value: '镜像' },
  ];
}

