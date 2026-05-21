import CookieTool from '@utility/CookieTool';
const API_URL = import.meta.env.VITE_API_URL;
const TIMEOUT = 90000; // 设置超时时间
window.jumpFlag = true;

const tokenKey = import.meta.env.VITE_TOKEN_KEY;
const refreshTokenKey = import.meta.env.VITE_REFRESH_TOKEN_KEY;

// 创建一个 fetch 包装函数，用于处理请求和响应
async function request(options) {
    let url = options.url;
    
    let headers = { ...options.headers };
    const token = CookieTool.getCookie(tokenKey);
    const refreshToken = CookieTool.getCookie(refreshTokenKey);
    if (token) {
        headers[tokenKey] = token;
    }
    if (refreshToken) {
        headers[refreshTokenKey] = refreshToken;
    }
     
    let newbody = null;
    if (options.data instanceof FormData) {
        newbody = options.data; // 直接使用 FormData
    } else {
        headers = { 'Content-Type': 'application/json', ...headers };
        if (typeof options.data == 'string') {
            newbody = options.data;
        } else if (typeof options.data == 'object') {
            newbody = JSON.stringify(options.data);
        }
    }

    const fullOptions = {
        method: options.method || 'GET',
        headers: headers,
        body: newbody,
        //credentials: 'include', // 如果需要处理跨域cookie，请确保服务器设置相应的CORS策略
    };

    // 创建一个带超时的 Promise
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), TIMEOUT);

    try {
        const response = await fetch(API_URL + url, {
            ...fullOptions,
            signal: controller.signal,
        });

        clearTimeout(timeoutId); // 请求成功，清除超时计时器

        if (response.headers.get('content-type') === 'application/octet-stream') {
            // 处理二进制流数据
            return response.blob();
        }

        const data = await response.json();
        if (response.ok) {
            return data.result;
        } else {
            // 处理错误响应
            throw handleErrorResponse(response, data);
        }
    } catch (error) {
        if (error.name === 'AbortError') {
            // 请求超时处理
            throw new Error('请求超时');
        } else {
            throw error;
        }
    }
}

// 错误响应处理函数
function handleErrorResponse(response, data) {
    let errorMessage;
    if (response.status === 401) {
        if (window.jumpFlag) {
            window.jumpFlag = false;
            let jumpTest = window.confirm(getConfirmText());
            if (jumpTest) {
                window.location = '/' + import.meta.env.VITE_NAMESPACE + '/login.html';
            }
        }
        errorMessage = '未授权，请登录';
    } else {
        errorMessage = data.message || '请求失败';
    }
    const error = new Error(errorMessage);
    error.status = response.status;
    error.data = data;
    return error;
}

function getConfirmText() {
    let lan = localStorage.getItem('lan') || 'zh';
    if (lan == 'zh') {
        return '鉴权失败，返回登录页?';
    } else if (lan == 'en') {
        return 'Authentication failed, redirecting to login page?';
    } else if (lan == 'ru') {
        return 'Авторизация не удалась, возвращается на страницу входа?';
    }
}

// 导出 request 函数
export default request;
