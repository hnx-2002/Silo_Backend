using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPGeometryPro;

/// <summary>
/// 线的矩阵变换操作
/// </summary>
public static class CurveTransformOperation
{

    /// <summary>
    /// 移动(仅直线或弧线)
    /// </summary>
    /// <param name="ovCurves">原曲线</param>
    /// <param name="vector">向量</param>
    /// <returns></returns>
    public static List<Curve> Move(this List<Curve> ovCurves, XYZ vector)
    {
        var resCurves = new List<Curve>();
        foreach (var ovCurve in ovCurves)
        {
            var newSp = ovCurve.StartPoint.Move(vector, 1);
            var newEp = ovCurve.EndPoint.Move(vector, 1);
            if (ovCurve is Line)
            {
                var newLine = Line.Create(newSp, newEp);
                resCurves.Add(newLine);
            }
            else if (ovCurve is Arc arc)
            {
                var newMp = arc.MiddlePoint.Move(vector, 1);
                var newArc = Arc.Create(newSp, newEp, newMp);
                resCurves.Add(newArc);
            }
            else if (ovCurve is Polyline polyline)
            {
                var newPoints = polyline.Points.Move(vector, 1);
                var newPolyline = Polyline.Create(newPoints, polyline.IsClosed);
                resCurves.Add(newPolyline);
            }
            else if (ovCurve is HermiteSpline hermiteSpline)
            {
                var newPoints = hermiteSpline.FitPoints.Move(vector, 1);
                var newHermiteSpline = HermiteSpline.Create(newPoints, hermiteSpline.IsClosed);
                resCurves.Add(newHermiteSpline);
            }
        }

        return resCurves;
    }

    /// <summary>
    /// 曲线关于镜面的镜像
    /// </summary>
    /// <param name="originCurves">原来的曲线集合</param>
    /// <param name="mirrorNormal">镜面的单位法向量</param>
    /// <param name="mirrorPoint">镜面上的一点</param>
    /// <returns></returns>
    public static List<Curve> Mirror(this List<Curve> originCurves,
        XYZ mirrorNormal, XYZ mirrorPoint)
    {
        var resCurves = new List<Curve>();
        foreach (var originCurve in originCurves)
        {
            var mirrSp = originCurve.StartPoint.MirrorPoint(mirrorNormal, mirrorPoint);
            var mirrEp = originCurve.EndPoint.MirrorPoint(mirrorNormal, mirrorPoint);
            if (originCurve is Line)
            {
                var mirrLine = Line.Create(mirrSp, mirrEp);
                resCurves.Add(mirrLine);
            }
            else if (originCurve is Arc arc)
            {
                var mirrMp = arc.MiddlePoint.MirrorPoint(mirrorNormal, mirrorPoint);
                var mirrArc = Arc.Create(mirrSp, mirrEp, mirrMp);
                resCurves.Add(mirrArc);
            }
            else if (originCurve is Polyline polyline)
            {
                var newPoints = polyline.Points.MirrorPoint(mirrorNormal, mirrorPoint);
                var newPolyline = Polyline.Create(newPoints, polyline.IsClosed);
                resCurves.Add(newPolyline);
            }
            else if (originCurve is HermiteSpline hermiteSpline)
            {
                var newPoints = hermiteSpline.FitPoints.MirrorPoint(mirrorNormal, mirrorPoint);
                var newHermiteSpline = HermiteSpline.Create(newPoints, hermiteSpline.IsClosed);
                resCurves.Add(newHermiteSpline);
            }
        }
        return resCurves;
    }

}
