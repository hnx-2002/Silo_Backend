using System;
using System.Collections.Generic;

namespace PTools_PSilo;

/// <summary>
/// 筒仓建模计算业务类接口
/// </summary>
public interface ISiloModelingCalculation_Business
{
    /// <summary>
    /// 根据任务Id计算族放置结果
    /// </summary>
    /// <param name="taskId">任务Id</param>
    /// <returns>族放置计算结果</returns>
    List<SiloTemplatePlacementResult> Calculate(Guid taskId);
}
