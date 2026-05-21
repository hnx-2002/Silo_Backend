using System.Collections.Generic;

namespace T2ACore;

/// <summary>
/// 资源处理器
/// </summary>
public interface IAssetExecuter
{
    /// <summary>
    /// 将不同角色维度的资源结果进行整合，实现接口方法中进行处理
    /// </summary>
    /// <param name="groups"></param>
    /// <returns></returns>
    public List<Res_Asset> GetAssetsResult(List<Res_AssetGroup> groups);

}
