import { translate } from '@utility/i18n.ts';
import { createNumberRequiredRule, createStringRequiredRule } from '@utility/FormValidator.ts';

export function getRules_dict_silo() {
  return {
    silo_name: [
      createStringRequiredRule(translate.t('库型名称') + translate.t('不能为空')),
    ],
  };
}
