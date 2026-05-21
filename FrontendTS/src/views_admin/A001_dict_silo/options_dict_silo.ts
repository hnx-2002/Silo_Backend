import type { Composer } from 'vue-i18n';
import type { LanDict } from '../../types/LanDict.ts';

/**
 * 获取dict_silo的silo_type选项
 *
 * @export
 * @param {Composer<LanDict, {}, {}, keyof LanDict>} translate
 * @return {*}
 */
export function get_dict_silo_silo_type_options(translate: Composer<LanDict, {}, {}, keyof LanDict>) {
  return [
    { label: translate.t('TP3'), value: 'TP3' },
    { label: translate.t('IBAU'), value: 'IBAU' },
    { label: translate.t('小倒锥平底库'), value: '小倒锥平底库' },
    { label: translate.t('漏斗库'), value: '漏斗库' },
  ];
}

/**
 * 获取dict_silo的silo_diameter选项
 *
 * @export
 * @param {Composer<LanDict, {}, {}, keyof LanDict>} translate
 * @return {*}
 */
export function get_dict_silo_silo_diameter_options(translate: Composer<LanDict, {}, {}, keyof LanDict>) {
  return [
    { label: translate.t('10m'), value: '10m' },
    { label: translate.t('12m'), value: '12m' },
    { label: translate.t('15m'), value: '15m' },
    { label: translate.t('18m'), value: '18m' },
    { label: translate.t('20m'), value: '20m' },
    { label: translate.t('22m'), value: '22m' },
    { label: translate.t('22.5m'), value: '22.5m' },
    { label: translate.t('25m'), value: '25m' },
  ];
}

