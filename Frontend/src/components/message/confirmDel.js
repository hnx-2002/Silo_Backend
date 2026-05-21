export function confirmDel(translate, inFunc) {
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
