import { translate } from '@utility/i18n.ts';
import { createNumberRequiredRule, createStringRequiredRule } from '@utility/FormValidator.ts';

export function getRules_task_base() {
  return {
    dict_silo_id: [
      createStringRequiredRule(translate.t('库型') + translate.t('不能为空')),
    ],
  };
}
