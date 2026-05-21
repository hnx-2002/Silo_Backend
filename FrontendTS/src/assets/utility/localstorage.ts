/**
 * LocalStorage 工具模块
 * 提供了重写 localStorage.setItem 方法以支持自定义事件的功能
 * 当特定的键（如 'lan' 或 'theme'）被修改时，会触发相应的事件通知
 */

/**
 * 自定义事件类型，包含 newValue 属性
 * 用于在 localStorage 值变更时传递新的值
 */
interface CustomEvent extends Event {
  newValue: string;
}

/**
 * 重载写入 localStorage 的事件
 *
 * 该函数重写了原生的 localStorage.setItem 方法，使其在设置特定键值时触发自定义事件
 * 主要用于监听语言（lan）和主题（theme）的变更，实现跨组件的状态同步
 *
 * 功能说明：
 * - 当设置 'lan' 键时，触发 'lanChange' 事件
 * - 当设置 'theme' 键时，触发 'themeChange' 事件
 * - 其他键的设置行为保持不变
 *
 * 使用示例：
 * ```typescript
 * // 初始化重载
 * overrideLocalStorageEvent();
 *
 * // 监听语言变更事件
 * window.addEventListener('lanChange', (e) => {
 *     console.log('语言已变更:', e.newValue);
 * });
 *
 * // 监听主题变更事件
 * window.addEventListener('themeChange', (e) => {
 *     console.log('主题已变更:', e.newValue);
 * });
 * ```
 */
export function overrideLocalStorageEvent() {
  var originSetItem = window.localStorage.setItem;
  window.localStorage.setItem = function (key: string, newValue: string) {
    if (key == 'lan') {
      let lanChangeEvent = new Event('lanChange') as CustomEvent;
      lanChangeEvent.newValue = newValue;
      window.dispatchEvent(lanChangeEvent);
      originSetItem.apply(this, [key, newValue]);
    } else if (key == 'theme') {
      let themeChangeEvent = new Event('themeChange') as CustomEvent;
      themeChangeEvent.newValue = newValue;
      window.dispatchEvent(themeChangeEvent);
      originSetItem.apply(this, [key, newValue]);
    } else {
      originSetItem.apply(this, [key, newValue]);
    }
  };
}
