/**
 * 根据inVal获取选项
 *
 * @export
 * @param {*} options 选项
 * @param {*} inVal 要匹配的值
 * @return {*}
 */
export function findOption(options, inVal) {
    if (Array.isArray(options)) {
        for (const opt of options) {
            if (opt.value === inVal) {
                return opt;
            }
        }
    } else {
        console.error('options不可遍历', options, inVal);
    }
    return null;
}

/**
 * 根据inVal获取选项值
 *
 * @export
 * @param {*} options 选项
 * @param {*} inVal 要匹配的值
 * @return {*}
 */
export function findLabel(options, inVal) {
    if (Array.isArray(options)) {
        for (const opt of options) {
            if (opt.value === inVal) {
                return opt.label;
            }
        }
    } else {
        console.error('options不可遍历', options, inVal);
    }
    return inVal;
}
