using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using T2ACore;

namespace PTools_PSilo.Controllers;

/// <summary>
/// 筒仓建模计算控制器
/// </summary>
[ApiController]
[Route("/PTools_PSilo/[controller]/[action]")]
[ApiExplorerSettings(GroupName = "Admin")]
[TPOK]
public class SiloModelingCalculationController : ControllerBase
{
    private readonly ISiloModelingCalculation_Business siloModelingCalculationBusiness;

    /// <summary>
    /// 构造函数
    /// </summary>
    public SiloModelingCalculationController(
        ISiloModelingCalculation_Business siloModelingCalculationBusiness)
    {
        this.siloModelingCalculationBusiness = siloModelingCalculationBusiness;
    }

    /// <summary>
    /// 根据任务Id计算族放置结果
    /// </summary>
    /// <param name="taskId">任务Id</param>
    /// <returns>族放置计算结果</returns>
    [HttpGet("{taskId}")]
    public TPResponse<List<SiloTemplatePlacementResult>> Calculate([FromRoute] Guid taskId)
    {
        List<SiloTemplatePlacementResult> result = siloModelingCalculationBusiness.Calculate(taskId);
        return TPResponse.New(result);
    }
}
