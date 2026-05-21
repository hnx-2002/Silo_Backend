/**
 * 文件大小格式化函数
 * 将字节数转换为人类可读的格式（如：1.5K、2.3M、1.2G等）
 *
 * @param num - 文件大小（字节数）
 * @param digits - 保留的小数位数
 * @returns 格式化后的文件大小字符串
 *
 * @example
 * FileLenghtFormatter(1500, 2) // "1.5K"
 * FileLenghtFormatter(2500000, 2) // "2.5M"
 * FileLenghtFormatter(1500, 0) // "2K"
 */
export function FileLenghtFormatter(num: number, digits: number): string {
  const si = [
    { value: 1, symbol: '' },
    { value: 1e3, symbol: 'K' },
    { value: 1e6, symbol: 'M' },
    { value: 1e9, symbol: 'G' },
    { value: 1e12, symbol: 'T' },
    { value: 1e15, symbol: 'P' },
    { value: 1e18, symbol: 'E' },
  ];
  const rx = /\.0+$|(\.[0-9]*[1-9])0+$/;
  let i = 0; // ← 兜底初始值，保证 si[i] 永远存在
  for (i = si.length - 1; i >= 0; i--) {
    if (num >= si[i]!.value) {
      break;
    }
  }
  return (num / si[i]!.value).toFixed(digits).replace(rx, '$1') + si[i]!.symbol;
}
