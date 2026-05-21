/**
 * 重载写入localStorage的事件
 *
 */
export function overrideLocalStorageEvent() {
    var originSetItem = window.localStorage.setItem;
    window.localStorage.setItem = function (key, newValue) {
        if (key == 'lan') {
            let lanChangeEvent = new Event('lanChange');
            lanChangeEvent.newValue = newValue;
            window.dispatchEvent(lanChangeEvent);
            originSetItem.apply(this, [key, newValue]);
        } else if (key == 'theme') {
            let themeChangeEvent = new Event('themeChange');
            themeChangeEvent.newValue = newValue;
            window.dispatchEvent(themeChangeEvent);
            originSetItem.apply(this, [key, newValue]);
        } else {
            originSetItem.apply(this, [key, newValue]);
        }
    };
}
