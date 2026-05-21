//处理返回值
export function executeRes(res, translate, cb) {
    if (res) {
        if (res.status) {
            window.$message.info(translate.t('操作成功'));
            cb();
        } else {
            window.$message.warning(translate.t(res.message));
        }
    } else {
        window.$message.error(translate.t('操作失败'));
    }
}
