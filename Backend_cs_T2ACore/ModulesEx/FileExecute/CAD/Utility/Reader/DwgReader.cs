using ACadSharp.Attributes;
using ACadSharp.Objects;
using ACadSharp;
using ACadSharp.Tables;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System;
using ACadSharp.Entities;
using CSMath;
using Newtonsoft.Json;
using System.IO;

namespace TPFun_Cad
{
    /// <summary>
    /// DWG读图
    /// </summary>
    public class DwgReader
    {
        /// <summary>
        /// 处理DWG文件
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static JArray ExecuteDWG(Stream fileStream)
        {
            CadDocument doc = ACadSharp.IO.DwgReader.Read(fileStream);

            var entities = doc.Entities;
            var result = new JArray();
            foreach (var item in entities)
            {
                if (item is Polyline2D)
                {
                    result.Add(MakePolyline(item as Polyline2D));
                }
                else if (item is Line)
                {
                    result.Add(MakeLine(item as Line));
                }
                else if (item is MText)
                {
                    result.Add(MakeMText(item as MText));
                }
                else if (item is Circle)
                {
                    result.Add(MakeCircle(item as Circle));
                }
                else if (item is Arc)
                {
                    result.Add(MakeArc(item as Arc));
                }
                else
                {
                    var noneJo = new JObject();
                    noneJo["objectName"] = item.ObjectName;
                    result.Add(noneJo);
                }
            }

            return result;
        }

        /// <summary>
        /// 构造多段线
        /// </summary>
        /// <param name="pline"></param>
        /// <returns></returns>
        private static JObject MakePolyline(Polyline2D pline)
        {
            var joRes = new JObject();

            joRes["entityName"] = pline.ObjectName;
            joRes["thickness"] = pline.Thickness;
            joRes["color"] = CreateColor(pline.Color);
            joRes["startWidth"] = pline.StartWidth;
            joRes["endWidth"] = pline.EndWidth;
            joRes["smoothSurface"] = pline.SmoothSurface.ToString();

            var points = new List<XYZ>();
            var pointCount = 0;

            foreach (Vertex item in pline.Vertices)
            {
                pointCount++;
                points.Add(item.Location);
            }
            joRes["pointsCount"] = pointCount;
            joRes["points"] = CreatePoints(points);

            return joRes;
        }

        /// <summary>
        /// 构造线
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static JObject MakeLine(Line line)
        {
            var joRes = new JObject();
            joRes["entityName"] = line.ObjectName;
            joRes["thickness"] = line.Thickness;
            joRes["color"] = CreateColor(line.Color);

            joRes["normal"] = CreatePoint(line.Normal);
            joRes["startPoint"] = CreatePoint(line.StartPoint);
            joRes["endPoint"] = CreatePoint(line.EndPoint);

            return joRes;
        }

        /// <summary>
        /// 构造文字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static JObject MakeMText(MText text)
        {
            var joRes = new JObject();
            joRes["entityName"] = text.ObjectName;
            joRes["color"] = CreateColor(text.Color);

            joRes["normal"] = CreatePoint(text.Normal);
            joRes["alignmentPoint"] = CreatePoint(text.AlignmentPoint);
            joRes["insertPoint"] = CreatePoint(text.InsertPoint);

            joRes["height"] = text.Height;
            joRes["rectangleWitdth"] = text.RectangleWidth;
            joRes["rectangleHeight"] = text.RectangleHeight;
            joRes["horizontalWidth"] = text.HorizontalWidth;
            joRes["verticalHeight"] = text.VerticalHeight;
            joRes["rotation"] = text.Rotation;
            joRes["lineSpacing"] = text.LineSpacing;
            joRes["backgroundScale"] = text.BackgroundScale;
            joRes["backgroundColor"] = CreateColor(text.BackgroundColor);

            joRes["value"] = text.Value;
            joRes["isAnnotative"] = text.IsAnnotative;
            //joRes["additionalText"] = text.AdditionalText;

            return joRes;
        }

        /// <summary>
        /// 处理圆
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        private static JObject MakeCircle(Circle circle)
        {
            var joRes = new JObject();
            joRes["entityName"] = circle.ObjectName;
            joRes["color"] = CreateColor(circle.Color);
            joRes["thickness"] = circle.Thickness;
            joRes["normal"] = CreatePoint(circle.Normal);
            joRes["center"] = CreatePoint(circle.Center);
            joRes["radius"] = circle.Radius;

            return joRes;
        }

        /// <summary>
        /// 处理弧
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        private static JObject MakeArc(Arc arc)
        {
            var joRes = MakeCircle(arc);
            joRes["startAngle"] = arc.StartAngle;
            joRes["endAngle"] = arc.EndAngle;
            return joRes;
        }

        /// <summary>
        /// 点转为JSON
        /// </summary>
        /// <param name="inPoint"></param>
        /// <returns></returns>
        private static JObject CreatePoint(XYZ inPoint)
        {
            var jo = new JObject();
            jo["X"] = inPoint.X;
            jo["Y"] = inPoint.Y;
            jo["Z"] = inPoint.Z;
            return jo;
        }

        /// <summary>
        /// 点转为JSON
        /// </summary>
        /// <param name="inPoints"></param>
        /// <returns></returns>
        private static JArray CreatePoints(List<XYZ> inPoints)
        {
            var ja = new JArray();
            foreach (var item in inPoints)
            {
                ja.Add(CreatePoint(item));
            }
            return ja;
        }

        /// <summary>
        /// 转换颜色
        /// </summary>
        /// <param name="inColor"></param>
        /// <returns></returns>
        private static string CreateColor(ACadSharp.Color inColor)
        {
            //short idx = inColor.Index;
            //var temp = ACadSharp.Color.GetIndexRGB(idx);
            int r = inColor.R;
            int g = inColor.G;
            int b = inColor.B;
            return "rgb(" + r + "," + g + "," + b + ")";
        }
    }
}