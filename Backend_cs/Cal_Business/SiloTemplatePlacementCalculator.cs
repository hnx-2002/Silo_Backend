using System;
using System.Collections.Generic;
using System.Linq;

namespace PTools_PSilo;

/// <summary>
/// 筒仓模板族放置计算模块
/// </summary>
public static class SiloTemplatePlacementCalculator
{
    private const string BaseSymbolName = "结构库底板示意";
    private const double FeetPerMeter = 3.280839895013123;

    /// <summary>
    /// 根据库型模板记录和建模任务基点计算族放置结果
    /// </summary>
    /// <param name="siloType">库型</param>
    /// <param name="task">建模任务</param>
    /// <param name="templateSiloBusiness">库型模板业务类</param>
    /// <returns>族放置计算结果</returns>
    public static List<SiloTemplatePlacementResult> Calculate(
        string siloType,
        Task_base_Class task,
        ITemplate_silo_Business templateSiloBusiness)
    {
        if (string.IsNullOrWhiteSpace(siloType))
        {
            throw new InvalidOperationException("库型为空。");
        }

        if (task == null)
        {
            throw new InvalidOperationException("建模任务为空。");
        }

        List<Template_silo_Class> templates = templateSiloBusiness.SearchBy_Silo_name(siloType);
        if (templates.Count == 0)
        {
            throw new InvalidOperationException("未找到库型模板：" + siloType);
        }

        Template_silo_Class baseTemplate = templates.FirstOrDefault(x => x.Symbol_name == BaseSymbolName);
        if (baseTemplate == null)
        {
            throw new InvalidOperationException("模板中缺少基准族类型：" + BaseSymbolName);
        }

        double angleDegrees = decimal.ToDouble(task.Rotation_angle);
        double angleRadians = DegreesToRadians(angleDegrees);
        double cos = Math.Cos(angleRadians);
        double sin = Math.Sin(angleRadians);

        double targetBaseX = MetersToFeet(decimal.ToDouble(task.Task_x));
        double targetBaseY = MetersToFeet(decimal.ToDouble(task.Task_y));
        double targetBaseZ = MetersToFeet(decimal.ToDouble(task.Task_z));

        double baseX = decimal.ToDouble(baseTemplate.Template_x);
        double baseY = decimal.ToDouble(baseTemplate.Template_y);
        double baseZ = decimal.ToDouble(baseTemplate.Template_z);

        var results = new List<SiloTemplatePlacementResult>();
        foreach (Template_silo_Class template in templates)
        {
            double dx = decimal.ToDouble(template.Template_x) - baseX;
            double dy = decimal.ToDouble(template.Template_y) - baseY;
            double dz = decimal.ToDouble(template.Template_z) - baseZ;

            double rotatedX = dx * cos - dy * sin;
            double rotatedY = dx * sin + dy * cos;

            results.Add(new SiloTemplatePlacementResult
            {
                Template_silo_id = template.Id,
                Symbol_name = template.Symbol_name,
                Rfa_path = template.Rfa_path,
                Location_x = targetBaseX + rotatedX,
                Location_y = targetBaseY + rotatedY,
                Location_z = targetBaseZ + dz,
                Rotate_angle = angleRadians
            });
        }

        return results;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static double MetersToFeet(double meters)
    {
        return meters * FeetPerMeter;
    }
}
