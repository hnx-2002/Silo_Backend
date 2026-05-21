using netDxf;
using netDxf.Collections;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TPFun_Cad
{
    /// <summary>
    /// dxf文件读取工具
    /// </summary>
    public class DxfReader
    {
        /// <summary>
        /// 解析DXF文件
        /// </summary>
        /// <param name="fileStream">Dxf文件流</param>
        public static DxfLayout AnalysisDxfDocument(Stream fileStream)
        {
            DxfDocument doc = DxfDocument.Load(fileStream);

            var entities = doc.Entities.All;
            //inserts块不使用了，暂时注掉
            //IEnumerable<Insert> inserts = doc.Entities.Inserts;
            var blocks = doc.Blocks;

            //根层实体
            DxfLayout dxfLayout = RecognizeEntities(entities);
            //所有层的块集合
            List<DxfBlock> listBlock = RecognizeBlocks(blocks);
            //块的层级关系
            GetBlocksRelation(dxfLayout, listBlock);
            //标注的特殊形式-重写(revit导出文字为带前后缀的一体化文字)
            GetDimensionOverwrite(dxfLayout);
            return dxfLayout;
        }

        /// <summary>
        /// 识别实体
        /// </summary>
        /// <param name="entities">目标实体</param>
        private static DxfLayout RecognizeEntities(IEnumerable<EntityObject> entities)
        {
            DxfLayout dxfLayout = new DxfLayout();
            var a = entities.ToList();
            foreach (var entity in entities)
            {
                //块也在entities中：块的CodeName="INSERT"并且Type=Insert
                if (entity.Type == EntityType.Insert)
                {
                    continue;
                }
                //实体
                else if (entity.Type != EntityType.Arc &&
                         entity.Type != EntityType.Circle &&
                         entity.Type != EntityType.Dimension &&
                         entity.Type != EntityType.Ellipse && //椭圆
                         entity.Type != EntityType.Hatch && //图案填充
                         entity.Type != EntityType.Line &&
                         entity.Type != EntityType.MText &&
                         entity.Type != EntityType.Polyline2D &&
                         entity.Type != EntityType.Text &&
                         entity.Type != EntityType.Polyline3D
                        )
                {
                    Console.WriteLine("出现了未处理的图块类型：" + entity.Type.ToString());
                    //Logger.Error("出现了未处理的图块类型：", entity.Type.ToString());
                }
                else
                {
                    if (entity.Type == EntityType.Arc)
                    {
                        if (entity is Arc arc)
                        {
                            dxfLayout.Arcs.Add(GetArc(arc));
                        }
                    }
                    else if (entity.Type == EntityType.Circle)
                    {
                        if (entity is Circle circle)
                        {
                            dxfLayout.Circles.Add(GetCircle(circle));
                        }
                    }
                    else if (entity.Type == EntityType.Dimension)
                    {
                        if (entity is Dimension dimension)
                        {
                            dxfLayout.Dimensions.Add(GetDimension(dimension));
                        }
                    }
                    else if (entity.Type == EntityType.Ellipse)
                    {
                        if (entity is Ellipse ellipse)
                        {
                            dxfLayout.Ellipses.Add(GetEllipse(ellipse));
                        }
                    }
                    else if (entity.Type == EntityType.Hatch)
                    {
                        if (entity is Hatch hatch)
                        {
                            dxfLayout.Hatchs.Add(GetHatch(hatch));
                        }
                    }
                    else if (entity.Type == EntityType.Line)
                    {
                        if (entity is Line line)
                        {
                            dxfLayout.Lines.Add(GetLine(line));
                        }
                    }
                    else if (entity.Type == EntityType.MText)
                    {
                        if (entity is MText mText)
                        {
                            //方法同“文字”
                            dxfLayout.MTexts.Add(GetText(mText));
                        }
                    }
                    else if (entity.Type == EntityType.Polyline2D)
                    {
                        if (entity is Polyline2D pline)
                        {
                            dxfLayout.PLines.Add(GetPolyline(pline));
                        }
                    }
                    else if (entity.Type == EntityType.Text)
                    {
                        if (entity is Text text)
                        {
                            dxfLayout.Texts.Add(GetText(text));
                        }
                    }
                    else if (entity.Type == EntityType.Polyline3D)
                    {
                        if (entity is Polyline3D pline3D)
                        {
                            dxfLayout.PLines.Add(GetPolyline3D(pline3D));
                        }
                    }
                }
            }
            return dxfLayout;
        }

        /// <summary>
        /// 识别块
        /// </summary>
        /// <param name="sysBlocks">读取的系统块集合</param>
        private static List<DxfBlock> RecognizeBlocks(BlockRecords sysBlocks)
        {
            List<DxfBlock> listBlock = new List<DxfBlock>();
            var a = sysBlocks.ToList();
            foreach (var sysBlock in sysBlocks.ToList())
            {
                var c = sysBlock.Entities;
                var d = sysBlock.Entities.ToArray();
                List<DxfObjectReference> refes = sysBlocks.GetReferences(sysBlock.Name);
                //多个块实体 对应 多个refe,分别用(refe as Insert).出来，
                //而不用 inserts.First(x => x.Block.Name == block.Name)，因为这样不同实例的块名相同，而坐标等信息不同;
                foreach (var tempRefe in refes)
                {
                    var refe = tempRefe.Reference; 
                    //只找有实体的块(滤掉：定义而无实体的块、“模型”、“布局”块)
                    if (refe is Insert)
                    {
                        DxfBlock dxfBlock = new DxfBlock();
                        dxfBlock.Name = sysBlock.Name.ToString();
                        dxfBlock.Id = Guid.NewGuid();
                        dxfBlock.Owner = refe.Owner.ToString();
                        dxfBlock.Scale = Point2D.TransPoint2D((refe as Insert).Scale);
                        dxfBlock.Rotation = (refe as Insert).Rotation;
                        dxfBlock.Position = Point2D.TransPoint2D((refe as Insert).Position);
                        //块内实体
                        dxfBlock.InnerEntities = RecognizeEntities(sysBlock.Entities);
                        listBlock.Add(dxfBlock); ;
                    }
                }
            }
            return listBlock;
        }

        /// <summary>
        /// 获取块的层级关系
        /// </summary>
        /// <param name="dxfLayout"></param>
        /// <param name="listBlock"></param>
        private static void GetBlocksRelation(DxfLayout dxfLayout, List<DxfBlock> listBlock)
        {
            Queue<DxfBlock> queBlock = new Queue<DxfBlock>();
            listBlock.ForEach(x => queBlock.Enqueue(x));
            List<DxfBlock> listOtherBlock = new List<DxfBlock>();
            //若为根层块
            while (queBlock.Count > 0)
            {
                bool isFind = false;
                var curBlock = queBlock.Dequeue();
                foreach (var type in FunConstant.CadWindowType)
                {
                    if (curBlock.Owner.StartsWith(type) ||
                        //Owner为空暂时发现为自建块(待核实)，需加入
                        curBlock.Owner.Equals(""))
                    {
                        dxfLayout.Blocks.Add(curBlock);
                        isFind = true;
                        break;
                    }
                }
                if (!isFind)
                {
                    listOtherBlock.Add(curBlock);
                }
            }
            //若为其他层的块
            foreach (var block in listOtherBlock)
            {
                foreach (var externalBlock in listBlock)
                {
                    if (externalBlock.Name.Equals(block.Owner))
                    {
                        externalBlock.InnerEntities.Blocks.Add(block);
                        //找爸爸，找到就下一个儿子
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取 标注的特殊形式-重写(revit导出文字为带前后缀的一体化文字)
        /// </summary>
        /// <param name="dxfLayout"></param>
        private static void GetDimensionOverwrite(DxfLayout dxfLayout)
        {
            try
            {
                //条件：标注.TextRefePoint = 标注块的MText.Position,测试后定位不同
                //      但有约半个字高多的偏移，故用(两坐标连线与Rotation)垂直判断
                //赋值：前者的标注文字 用 后者的Valve 覆盖
                foreach (var originDim in dxfLayout.Dimensions)
                {
                    bool isFind = false;
                    foreach (var block in dxfLayout.Blocks)
                    {
                        var listTexts = new List<DxfText>();
                        block.InnerEntities.MTexts.ForEach(x => listTexts.Add(x));
                        block.InnerEntities.Texts.ForEach(x => listTexts.Add(x));
                        foreach (var curText in listTexts)
                        {
                            //条件1.两点重合
                            bool equalPoint = FunCal.IsEqualPoint(originDim.TextRefePoint, curText.Position);
                            //条件2.两点不重合，所成直线与Rotation垂直&&距离-偏移≈½字高
                            bool vertical = GetIfVertical(originDim.TextRefePoint, curText.Position, curText.Rotation);
                            bool close = GetIfClose(originDim.TextRefePoint, curText.Position, originDim.Offset, curText.Height);
                            if (equalPoint || vertical && close)
                            {
                                isFind = true;
                                //拆之前其他参数赋值
                                originDim.Rotation = curText.Rotation;
                                //curText.Value 分离 前后缀(string) 和 标注值(double)
                                //curText.Value形式：\\A1;{\\fSimSum|bo|io|c134|p2;ø 179.3438}
                                //                   \\A1;ø 235.1467mm
                                bool isSeparated = FunCal.SeparateStrToNumAndAffix
                                    (curText.Value, out string prefix, out double num, out string suffix);
                                if (isSeparated)
                                {
                                    if (FunCal.IsEqual(originDim.Measurement, num, FunCal.GetPrecision(num)))
                                    {
                                        originDim.Prefix = prefix;
                                        originDim.Suffix = suffix;
                                        break;
                                    }
                                    //坐标相同，能解析出前后缀但是数值不同
                                    else
                                    {
                                        //TODO 暂时还没想好要打印到的位置
                                        //FunCommon.ConsoleLog("标注原值与替代值不同:", JsonConvert.SerializeObject(originDim));
                                        originDim.Overwrite = prefix + num + suffix;
                                    }
                                }
                            }
                        }
                    }
                    if (isFind)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("标注的特殊形式-重写错误：" + ex.ToString());
                //FunCommon.ConsoleLog("标注的特殊形式-重写错误：", ex.ToString());
            }
        }

        /// <summary>
        /// 判断两点所成直线向量 是否与 角度的向量 垂直
        /// </summary>
        /// <param name="inPoint1"></param>
        /// <param name="inPoint2"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static bool GetIfVertical(Point2D inPoint1, Point2D inPoint2, double angle)
        {
            bool res = false;
            //两点所成直线的向量
            Vector2 lineTwoPoint = Vector2.Subtract
                (Point2D.TransVector2(inPoint1), Point2D.TransVector2(inPoint2));
            Vector2 lineAngle = new Vector2(Math.Cos(FunCal.TransRadian(angle)),
                                            Math.Sin(FunCal.TransRadian(angle)));
            //两向量垂直→cosα= 0°
            double dotProduct = Vector2.DotProduct(lineTwoPoint, lineAngle);
            if (FunCal.IsEqual(dotProduct, 0, FunConstant.EqualNum))
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 判断两点距离-偏移量 之差 是否 ≈½字高
        /// </summary>
        /// <param name="inPoint1"></param>
        /// <param name="inPoint2"></param>
        /// <param name="offset"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static bool GetIfClose(Point2D inPoint1, Point2D inPoint2, double offset, double height)
        {
            bool res = false;
            //两点所成直线的向量
            double distance = Vector2.Distance
                (Point2D.TransVector2(inPoint1), Point2D.TransVector2(inPoint2));
            double tolerance = distance - offset * 304.8 - 0.5 * height;
            //公差放大到一倍字高
            if (FunCal.IsEqual(tolerance, 0, height))
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 读取多段线
        /// </summary>
        /// <param name="pline"></param>
        /// <returns></returns>
        private static DxfPolyline GetPolyline(Polyline2D pline)
        {
            DxfPolyline dxfPolyline = new();
            //父类通性属性
            GetParentPara(pline, dxfPolyline);
            //子类个性属性
            dxfPolyline.Type = pline.IsClosed ? "polygon" : "polyline";
            //多段线各点
            foreach (Polyline2DVertex vertex in pline.Vertexes)
            {
                Point2D position = Point2D.TransPoint2D(vertex.Position);
                dxfPolyline.Vertexes.Add(position);
            }
            //多边形再末尾点后加起始点
            if (pline.IsClosed)
            {
                Point2D firstPointPosition = Point2D.TransPoint2D(pline.Vertexes.First().Position);
                dxfPolyline.Vertexes.Add(firstPointPosition);
            }
            return dxfPolyline;
        }

        /// <summary>
        /// 父级(通用)属性
        /// </summary>
        /// <param name="inEntity"></param>
        /// <param name="dxfEntity"></param>
        private static void GetParentPara(EntityObject inEntity, DxfEntity dxfEntity)
        {
            dxfEntity.Id = Guid.NewGuid();
            dxfEntity.Owner = inEntity.Owner.ToString();
            dxfEntity.EntityType = inEntity.Type.ToString();
            dxfEntity.Layer = inEntity.Layer.Name.ToString();
            dxfEntity.Linetype = inEntity.Linetype.Name.ToString();
        }

        /// <summary>
        /// 读取线
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static DxfLine GetLine(Line line)
        {
            DxfLine dxfLine = new();
            //父类通性属性
            GetParentPara(line, dxfLine);
            //子类个性属性

            //端点
            Point2D startPosition = Point2D.TransPoint2D(line.StartPoint);
            dxfLine.Vertexes.Add(startPosition);
            Point2D endPosition = Point2D.TransPoint2D(line.EndPoint);
            dxfLine.Vertexes.Add(endPosition);
            return dxfLine;
        }

        /// <summary>
        /// 读取弧
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        private static DxfArc GetArc(Arc arc)
        {
            DxfArc dxfArc = new();
            //父类通性属性
            GetParentPara(arc, dxfArc);
            //子类个性属性
            dxfArc.CircleCenter = Point2D.TransPoint2D(arc.Center);
            dxfArc.Radius = arc.Radius;
            dxfArc.StartAngle = arc.StartAngle;
            dxfArc.EndAngle = arc.EndAngle;
            return dxfArc;
        }

        /// <summary>
        /// 读取圆
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        private static DxfCircle GetCircle(Circle circle)
        {
            DxfCircle dxfCircle = new();
            //父类通性属性
            GetParentPara(circle, dxfCircle);
            //子类个性属性
            dxfCircle.CircleCenter = Point2D.TransPoint2D(circle.Center);
            dxfCircle.Radius = circle.Radius;
            return dxfCircle;
        }

        /// <summary>
        /// 读取标注
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        private static DxfDimension GetDimension(Dimension dimension)
        {
            DxfDimension dxfDimension = new();
            //父类通性属性
            GetParentPara(dimension, dxfDimension);
            //子类个性属性
            //FirstRefePoint SecondRefePoint DimLinePosition Offset Rotation获取不到
            //2022.12.29：郭可骍
            //FirstRefePoint SecondRefePoint DimLinePosition Offset Rotation属性为线性标注和对齐标注子类属性，需对Dimension进行分类解析获取.
            //由于DxfDimension类中包含上述几种属性，暂时使用DxfDimension类去接收属性.

            //2023.1.9：郭可骍
            //增加标注属性UserText，对应CAD标注中的文字替代属性

            //线性标注
            if (dimension is LinearDimension linearDimension)
            {
                dxfDimension.FirstRefePoint = Point2D.TransPoint2D(linearDimension.FirstReferencePoint);
                dxfDimension.SecondRefePoint = Point2D.TransPoint2D(linearDimension.SecondReferencePoint);
                dxfDimension.DimLinePosition = Point2D.TransPoint2D(linearDimension.DimLinePosition);
                dxfDimension.Offset = linearDimension.Offset;
                dxfDimension.Rotation = linearDimension.Rotation;
            }
            //对齐标注
            else if (dimension is AlignedDimension alignedDimension)
            {
                dxfDimension.FirstRefePoint = Point2D.TransPoint2D(alignedDimension.FirstReferencePoint);
                dxfDimension.SecondRefePoint = Point2D.TransPoint2D(alignedDimension.SecondReferencePoint);
                dxfDimension.DimLinePosition = Point2D.TransPoint2D(alignedDimension.DimLinePosition);
                dxfDimension.Offset = alignedDimension.Offset;
            }

            dxfDimension.TextRefePoint = Point2D.TransPoint2D(dimension.TextReferencePoint);
            dxfDimension.Measurement = dimension.Measurement;
            dxfDimension.UserText = dimension.UserText;
            //前后缀
            if (!dimension.StyleOverrides.Equals(null) &&
                !dimension.StyleOverrides.Values.Equals(null) &&
                dimension.StyleOverrides.Values.Count > 0)
            {
                if (dimension.StyleOverrides.Values.First().Type.ToString() == "TextOffset")
                {
                    dxfDimension.Offset = (double)dimension.StyleOverrides.Values.First().Value;
                }
                if (dimension.StyleOverrides.Values.First().Type.ToString().Equals("DimPrefix"))
                {
                    //{DimPrefix : Ø}
                    dxfDimension.Prefix = dimension.StyleOverrides.Values.First().Value.ToString();
                }
                else if (dimension.StyleOverrides.Values.First().Type.ToString().Equals("DimSuffix"))
                {
                    dxfDimension.Suffix = dimension.StyleOverrides.Values.First().Value.ToString();
                }
            }
            return dxfDimension;
        }

        /// <summary>
        /// 读取椭圆
        /// </summary>
        /// <param name="ellipse"></param>
        /// <returns></returns>
        private static DxfEllipse GetEllipse(Ellipse ellipse)
        {
            DxfEllipse dxfEllipse = new();
            //父类通性属性
            GetParentPara(ellipse, dxfEllipse);
            //子类个性属性
            dxfEllipse.EllipseCenter = Point2D.TransPoint2D(ellipse.Center);
            dxfEllipse.MajorAxisLength = ellipse.MajorAxis;
            dxfEllipse.MinorAxisLength = ellipse.MinorAxis;
            dxfEllipse.Rotation = ellipse.Rotation;
            return dxfEllipse;
        }

        /// <summary>
        /// 读取文字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static DxfText GetText(Text text)
        {
            DxfText dxfText = new();
            //父类通性属性
            GetParentPara(text, dxfText);
            //子类个性属性
            dxfText.Position = Point2D.TransPoint2D(text.Position);
            dxfText.Height = text.Height;
            dxfText.Rotation = text.Rotation;
            dxfText.Value = text.Value;
            return dxfText;
        }

        /// <summary>
        /// 读取多行文字
        /// </summary>
        /// <param name="mtext"></param>
        /// <returns></returns>
        private static DxfMText GetText(MText mtext)
        {
            DxfMText dxfMText = new();
            //父类通性属性
            GetParentPara(mtext, dxfMText);
            //子类个性属性
            dxfMText.Position = Point2D.TransPoint2D(mtext.Position);
            dxfMText.Height = mtext.Height;
            dxfMText.Rotation = mtext.Rotation;
            dxfMText.Value = mtext.Value;
            return dxfMText;
        }

        /// <summary>
        /// 读取图像填充
        /// </summary>
        /// <param name="hatch"></param>
        /// <returns></returns>
        private static DxfHatch GetHatch(Hatch hatch /*, IEnumerable<Insert> inserts */)
        {
            DxfHatch dxfHatch = new();
            //父类通性属性
            GetParentPara(hatch, dxfHatch);
            //子类个性属性
            //用insert试试坐标×，∵没有hatch.Name
            //var insert = inserts.FirstOrDefault(x => x.Block.Name == hatch.);
            ////如果遇到 "*Model_Space"这样的块名.First找不到，需用.FirstOrDefault返回空
            //if (insert != null)
            //{
            //    dxfHatch.EntityXY = Point2D.TransPoint2D(insert.Position);
            //}
            //dxfHatch.Position = Point2D.TransPoint2D(hatch.Position);
            dxfHatch.PatternName = hatch.Pattern.Name;
            dxfHatch.Rotation = GetPlusAngle(hatch.Pattern.Angle);
            dxfHatch.Scale = hatch.Pattern.Scale;
            return dxfHatch;
        }

        /// <summary>
        /// 获取正角度
        /// </summary>
        /// <param name="inAngle"></param>
        /// <returns></returns>
        private static double GetPlusAngle(double inAngle)
        {
            double res = 0;
            if (inAngle < 0)
            {
                res = 360 + inAngle;
            }
            res = inAngle;
            return res;
        }

        /// <summary>
        /// 读取三维多段线，将三维多段线的拐点三维坐标转换为二维坐标
        /// </summary>
        /// <param name="pline3D"></param>
        /// <returns></returns>
        private static DxfPolyline GetPolyline3D(Polyline3D pline3D)
        {
            DxfPolyline dxfPolyline = new();
            //父类通性属性
            GetParentPara(pline3D, dxfPolyline);
            //子类个性属性
            dxfPolyline.Type = pline3D.IsClosed ? "polygon" : "polyline";

            foreach (Vector3 vector3 in pline3D.Vertexes)
            {
                Point2D position = Point2D.TransPoint2D(vector3);
                dxfPolyline.Vertexes.Add(position);
            }
            //多边形再末尾点后加起始点
            if (pline3D.IsClosed)
            {
                Point2D firstPointPosition = Point2D.TransPoint2D(pline3D.Vertexes.First());
                dxfPolyline.Vertexes.Add(firstPointPosition);
            }
            return dxfPolyline;
        }
    }
}