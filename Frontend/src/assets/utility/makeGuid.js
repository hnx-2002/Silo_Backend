export function makeTPGuid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1).toUpperCase();
    }

    let now = new Date();
    let yyyy = now.getFullYear().toString();
    let MM = (now.getMonth() + 1).toString().padStart(2, '0');
    let dd = now.getDate().toString().padStart(2, '0');
    let HH = now.getHours().toString().padStart(2, '0');
    let mm = now.getMinutes().toString().padStart(2, '0');
    let ss = now.getSeconds().toString().padStart(2, '0');
    let fff = now.getMilliseconds().toString().padStart(3, '0');

    return (
        'CE' +
        yyyy +
        MM +
        '-' +
        dd +
        HH +
        '-' +
        mm +
        ss +
        '-' +
        fff +
        'A' +
        '-' +
        S4() +
        S4() +
        S4()
    );
}
