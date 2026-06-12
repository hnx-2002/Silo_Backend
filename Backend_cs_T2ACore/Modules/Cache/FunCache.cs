using System; 

namespace T2ACore;

/// <summary>
/// 缓存方法
/// </summary>
public static class FunCache
{
    /// <summary>
    /// 缓存双删
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cacheFunc"></param>
    /// <param name="key"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static T WithCacheCleared<T>(this ICache cacheFunc, string key, Func<T> action)
    {
        cacheFunc.Remove(key);
        var res = action();
        cacheFunc.Remove(key);
        return res;
    }
}
