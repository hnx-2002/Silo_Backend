//wrx added 20221214

/**
 * 取并集，去重
 * 取两个数组的并集并去重
 * @param arr1 第一个数组
 * @param arr2 第二个数组
 * @returns 合并后的去重数组
 */
export function arrayUnion<T>(arr1: T[], arr2: T[]) {
  return [...new Set([...arr1, ...arr2])];
}

/**
 * arr1里去重，过滤出arr2也有的元素，即取交集
 * 取两个数组的交集（去重）
 * @param arr1 第一个数组
 * @param arr2 第二个数组
 * @returns 同时存在于两个数组中的去重元素
 */
export function arrayCross<T>(arr1: T[], arr2: T[]) {
  return [...new Set(arr1)].filter((x) => arr2.includes(x));
}

/**
 * 并集中过滤出不存在在交集中的
 * 取两个数组的差集（对称差集）
 * @param arr1 第一个数组
 * @param arr2 第二个数组
 * @returns 仅存在于其中一个数组中的元素（并集减去交集）
 */
export function arrayDiff<T>(arr1: T[], arr2: T[]) {
  return arrayUnion(arr1, arr2).filter((x) => !arrayCross(arr1, arr2).includes(x));
}

/**
 * 找到仅存在于arr1中，而不存在arr2中的去重元素
 * 取仅存在于第一个数组中的元素（去重）
 * @param arr1 第一个数组
 * @param arr2 第二个数组
 * @returns 仅存在于arr1中而不存在于arr2中的去重元素
 */
export function arrayOnly<T>(arr1: T[], arr2: T[]) {
  return [...new Set(arr1.filter((x) => !arr2.includes(x)))];
}
