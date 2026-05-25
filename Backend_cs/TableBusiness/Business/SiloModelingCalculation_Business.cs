using System;
using System.Collections.Generic;

namespace PTools_PSilo;

/// <summary>
/// 筒仓建模计算业务类
/// </summary>
public class SiloModelingCalculation_Business : ISiloModelingCalculation_Business
{
    private readonly ITask_base_Business taskBaseBusiness;
    private readonly IDict_silo_Business dictSiloBusiness;
    private readonly ITemplate_silo_Business templateSiloBusiness;

    /// <summary>
    /// 构造函数
    /// </summary>
    public SiloModelingCalculation_Business(
        ITask_base_Business taskBaseBusiness,
        IDict_silo_Business dictSiloBusiness,
        ITemplate_silo_Business templateSiloBusiness)
    {
        this.taskBaseBusiness = taskBaseBusiness;
        this.dictSiloBusiness = dictSiloBusiness;
        this.templateSiloBusiness = templateSiloBusiness;
    }

    /// <summary>
    /// 根据任务Id计算族放置结果
    /// </summary>
    /// <param name="taskId">任务Id</param>
    /// <returns>族放置计算结果</returns>
    public List<SiloTemplatePlacementResult> Calculate(Guid taskId)
    {
        Task_base_Class task = taskBaseBusiness.GetOne(taskId);
        if (task == null)
        {
            throw new InvalidOperationException("未找到建模任务：" + taskId);
        }

        Guid dictSiloId = Guid.Parse(task.Silo_type);
        Dict_silo_Class dictSilo = dictSiloBusiness.GetOne(dictSiloId);
        if (dictSilo == null)
        {
            throw new InvalidOperationException("未找到库型字典：" + task.Silo_type);
        }

        return SiloTemplatePlacementCalculator.Calculate(dictSilo.Silo_type, task, templateSiloBusiness);
    }
}
