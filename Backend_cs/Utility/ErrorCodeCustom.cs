namespace PTools_PSilo;
using T2ACore;

/// <summary>
/// 自定义错误码
/// </summary>

[ErrorCodePrefix(1)]
public class ErrorCodeCustom : ErrorCodeBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public ErrorCodeCustom(int code, string message) : base(code, message) { }

    // 这种方式的优势就是便于修改与添加
    // 不便的地方就是，会产生许多并不是很必要一直存在在内存中的 static 数据

    ///// <summary>
    ///// 内部错误
    ///// </summary>
    //public static ErrorCodeGeneric InternalError = new(0, "系统内部错误");

    ///// <summary>
    ///// 登录过期
    ///// </summary>
    //public static ErrorCodeGeneric AuthExpired = new(1, "用户登录过期，请尝试重新登陆");

    ///// <summary>
    ///// 无权限
    ///// </summary>
    //public static ErrorCodeGeneric UnAuthorized = new(2, "当前用户禁止进行操作");

    ////////////////////////////////////////////////////////////
    // 仿照上边，请开发负责人制定项目级的错误编码和提示，显示在下边
    ////////////////////////////////////////////////////////////

    /// <summary>
    /// 举例
    /// </summary>
    public static ErrorCodeGeneric ModuleError = new(4, "xxx模块错误");
}
