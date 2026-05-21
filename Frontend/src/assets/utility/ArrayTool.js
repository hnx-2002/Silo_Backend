//wrx added 20221214

//取并集，去重
export function arrayUnion(arr1, arr2) {
    return [...new Set([...arr1, ...arr2])];
}

//arr1里去重，过滤出arr2也有的元素，即取交集
export function arrayCross(arr1, arr2) {
    return [...new Set(arr1)].filter((x) => arr2.includes(x));
}

//并集中过滤出不存在在交集中的
export function arrayDiff(arr1, arr2) {
    return arrayUnion(arr1, arr2).filter((x) => !arrayCross(arr1, arr2).includes(x));
}

//找到仅存在于arr1中，而不存在arr2中的去重元素
export function arrayOnly(arr1, arr2) {
    return [...new Set(arr1.filter((x) => !arr2.includes(x)))];
}
