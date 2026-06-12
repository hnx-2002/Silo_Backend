using System;
using System.Collections.Generic;
using TPGeometryPro;

namespace PTools_PSilo;

/// <summary>
/// 筒仓族放置计算入口。
/// </summary>
public static class SiloCalculator
{
    private const string LayoutTypePlace = "放置";
    private const string LayoutTypeRotate = "旋转";
    private const string LayoutTypeMirror = "镜像";

    /// <summary>
    /// 根据建模任务、库型和该库型下的全部族资源计算放置结果。
    /// </summary>
    /// <param name="task">建模任务。</param>
    /// <param name="dictSilo">任务选择的库型。</param>
    /// <param name="rfas">该库型下的全部族资源。</param>
    /// <returns>筒仓放置计算结果。</returns>
    public static SiloResult Calculate(Task_base_Class task, Dict_silo_Class dictSilo, List<Rfa_resource_Class> rfas)
    {
        SiloResult result = Task_base_Class.Cast(task, dictSilo);
        result.Placements = new List<SiloPlacement>(rfas.Count);

        double angleRadians = decimal.ToDouble(task.Rotation_angle).ToRadian();
        double cos = Math.Cos(angleRadians);
        double sin = Math.Sin(angleRadians);
        XYZ taskBasePoint = XYZ.New(
            decimal.ToDouble(task.Task_x).ToBritish(),
            decimal.ToDouble(task.Task_y).ToBritish(),
            decimal.ToDouble(task.Task_z).ToBritish());

        for (int i = 0; i < rfas.Count; i++)
        {
            result.Placements.Add(CalculatePlacement(taskBasePoint, rfas[i], i + 1, angleRadians, cos, sin));
        }

        return result;
    }

    /// <summary>
    /// 计算单个族资源的放置操作。
    /// </summary>
    /// <param name="taskBasePoint">任务基点。</param>
    /// <param name="rfa">族资源。</param>
    /// <param name="sort">排序号。</param>
    /// <param name="angleRadians">旋转弧度。</param>
    /// <param name="cos">旋转角余弦值。</param>
    /// <param name="sin">旋转角正弦值。</param>
    /// <returns>单个族的放置操作。</returns>
    private static SiloPlacement CalculatePlacement(
        XYZ taskBasePoint,
        Rfa_resource_Class rfa,
        int sort,
        double angleRadians,
        double cos,
        double sin)
    {
        SiloPlacement placement = Rfa_resource_Class.Cast(rfa);
        placement.Sort = sort;
        placement.Layout_type = GetLayoutType(angleRadians);

        double templateX = decimal.ToDouble(rfa.Template_x);
        double templateY = decimal.ToDouble(rfa.Template_y);
        double templateZ = decimal.ToDouble(rfa.Template_z);

        double rotatedX = templateX * cos - templateY * sin;
        double rotatedY = templateX * sin + templateY * cos;

        placement.Location = XYZ.New(
            taskBasePoint.X + rotatedX,
            taskBasePoint.Y + rotatedY,
            taskBasePoint.Z + templateZ);
        placement.Normal = XYZ.BasisZ;
        placement.Rotate_angle = (decimal)angleRadians;

        return placement;
    }

    /// <summary>
    /// 根据旋转角度确定布置类型。
    /// </summary>
    /// <param name="angleRadians">旋转弧度。</param>
    /// <returns>布置类型。</returns>
    private static string GetLayoutType(double angleRadians)
    {
        double angleDegrees = angleRadians.ToDegree();
        if (angleDegrees == 180)
        {
            return LayoutTypeMirror;
        }

        if (angleDegrees != 0)
        {
            return LayoutTypeRotate;
        }

        return LayoutTypePlace;
    }
}
