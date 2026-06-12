import { translate } from '@utility/i18n.ts';
import { createNumberRequiredRule, createStringRequiredRule } from '@utility/FormValidator.ts';

export function getRules_rfa_resource() {
  return {
    file_name: [
      createStringRequiredRule(translate.t('族文件名称') + translate.t('不能为空')),
    ],
  };
}
