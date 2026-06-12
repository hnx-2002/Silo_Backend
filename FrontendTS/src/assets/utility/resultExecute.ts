import type { Composer } from 'vue-i18n';
import type { LanDict } from '../../types/LanDict.ts';

declare global {
  interface Window {
    $message: {
      info: (content: string) => void;
      warning: (content: string) => void;
      error: (content: string) => void;
      success: (content: string) => void;
    };
  }
}

/**
 * 执行结果处理函数
 *
 * @export
 * @param {Object} res - status?: boolean; message?: string
 * @param {Composer<LanDict, {}, {}, keyof LanDict>} translate - 本地化函数
 * @param {Function} cb - 回调函数
 */
export function executeRes(
  res: { status?: boolean; message?: string },
  translate: Composer<LanDict, {}, {}, keyof LanDict>,
  cb?: () => void
) {
  if (res) {
    if (res.status) {
      window.$message.info(translate.t('操作成功'));
      cb?.();
    } else {
      window.$message.warning(translate.t(res.message || ''));
    }
  } else {
    window.$message.error(translate.t('操作失败'));
  }
}
