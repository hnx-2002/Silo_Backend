interface Option {
  value: any;
  label: string;
}

/**
 * 根据给定的值从选项数组中查找对应的选项对象
 *
 * @param options - 选项数组，每个选项包含 value 和 label 属性
 * @param inVal - 要匹配的值
 * @returns 找到的选项对象，如果未找到则返回 null
 *
 * @example
 * ```typescript
 * const options = [
 *   { value: '1', label: '选项一' },
 *   { value: '2', label: '选项二' }
 * ];
 * const result = findOption(options, '1');
 * // 返回: { value: '1', label: '选项一' }
 * ```
 */
export function findOption(options: Option[], inVal: any): Option | null {
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
 * 根据给定的值从选项数组中查找对应的标签文本
 *
 * @param options - 选项数组，每个选项包含 value 和 label 属性
 * @param inVal - 要匹配的值
 * @returns 找到的标签文本，如果未找到则返回原始输入值
 *
 * @example
 * ```typescript
 * const options = [
 *   { value: '1', label: '选项一' },
 *   { value: '2', label: '选项二' }
 * ];
 * const label = findLabel(options, '1');
 * // 返回: '选项一'
 * ```
 */
export function findLabel(options: Option[], inVal: any): string | any {
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
