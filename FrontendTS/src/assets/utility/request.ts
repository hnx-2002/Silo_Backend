import CookieTool from '@utility/CookieTool.ts';

const API_URL = import.meta.env.VITE_API_URL;
const TIMEOUT = 90000; // 设置超时时间
window.jumpFlag = true;

const tokenKey = import.meta.env.VITE_TOKEN_KEY;
const refreshTokenKey = import.meta.env.VITE_REFRESH_TOKEN_KEY;

/**
 * 发送 HTTP 请求的包装函数
 * 自动处理 token、超时、错误响应等逻辑
 * @param options - 请求配置选项
 * @returns 返回响应数据或抛出错误
 */
async function request(options: RequestOptions) {
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
  let timeoutId: number | null = null;
  const timeoutValue = options.timeout !== undefined ? options.timeout : TIMEOUT;

  if (timeoutValue !== -1) {
    timeoutId = window.setTimeout(() => controller.abort(), timeoutValue);
  }

  try {
    const response = await fetch(API_URL + url, {
      ...fullOptions,
      signal: controller.signal,
    });

    if (timeoutId) {
      window.clearTimeout(timeoutId); // 请求成功，清除超时计时器
    }

    if (response.headers.get('content-type') === 'application/octet-stream') {
      // 处理二进制流数据
      return response.blob();
    }

    if (response.ok) {
      const data = await response.json();
      return data.result;
    } else {
      // 处理错误响应
      try {
        const data = await response.json();
        throw handleErrorResponse(response, data);
      } catch (jsonError) {
        // 如果响应不是有效的JSON，创建一个默认错误对象
        const error = new Error('请求失败') as CustomError;
        error.status = response.status;
        error.data = { result: null, message: `HTTP ${response.status}: ${response.statusText}` };
        throw error;
      }
    }
  } catch (error) {
    if (error instanceof Error && error.name === 'AbortError') {
      throw new Error('请求超时');
    } else {
      throw error;
    }
  }
}

// 错误响应处理函数
function handleErrorResponse(response: Response, data: ResponseData) {
  let errorMessage;
  if (response.status === 401) {
    if (window.jumpFlag) {
      window.jumpFlag = false;
      let jumpTest = window.confirm(getConfirmText());
      if (jumpTest) {
        window.location.href = '/' + import.meta.env.VITE_NAMESPACE + '/login.html';
      }
    }
    errorMessage = '未授权，请登录';
  } else {
    errorMessage = data.message || '请求失败';
  }
  const error = new Error(errorMessage) as CustomError;
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

/**
 * 请求配置选项
 */
interface RequestOptions {
  /** 请求地址 */
  url: string;
  /** 请求方法，默认为 GET */
  method?: string;
  /** 请求头 */
  headers?: Record<string, string>;
  /** 请求数据 */
  data?: unknown;
  /** 超时时间（毫秒），-1表示不超时，默认90000毫秒 */
  timeout?: number;
}

/**
 * 响应数据结构
 */
interface ResponseData {
  /** 响应结果数据 */
  result: unknown;
  /** 错误信息 */
  message?: string;
}

/**
 * 自定义错误类型
 */
interface CustomError extends Error {
  /** HTTP 状态码 */
  status?: number;
  /** 响应数据 */
  data?: ResponseData;
}

declare global {
  interface Window {
    jumpFlag: boolean;
  }
}

export default request;
