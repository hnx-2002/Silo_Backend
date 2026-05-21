/**
 * 获取task_base的status选项
 *
 * @export
 * @param {*} translate
 * @return {*}
 */
export function get_task_base_status_options(translate) {
    return [
        { label: translate.t('新建'), value: 10 },
        { label: translate.t('计算中'), value: 11 },
        { label: translate.t('计算成功'), value: 12 },
        { label: translate.t('计算失败'), value: 13 },
        { label: translate.t('错误'), value: 20 },
    ];
}

