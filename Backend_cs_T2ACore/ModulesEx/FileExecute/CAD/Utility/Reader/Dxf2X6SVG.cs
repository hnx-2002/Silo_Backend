using netDxf;
using netDxf.Blocks;
using netDxf.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TPFun_Cad
{
    /// <summary>
    /// dxf文件转X6Svg
    /// </summary>
    public class Dxf2X6Svg
    {
        private static readonly double Scale = 0.2f;

        //private static readonly double Scale = 1;
        private static readonly double LineWeight = 0.5f;

        private static double Paper_Size_X = 0f;
        private static double Paper_Size_Y = 0f;

        private static readonly double FontSize = 5f;

        /// <summary>
        /// 处理直径转换
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double Transform_R(double x)
        {
            return x / Scale;
        }

        /// <summary>
        /// X轴坐标转换
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double Transform_X(double x)
        {
            return x / Scale;
        }

        /// <summary>
        /// Y轴坐标转换
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private static double Transform_Y(double y)
        {
            return (Paper_Size_Y - y) / Scale;
            //return y/ scale;
        }

        /// <summary>
        /// 获取图纸尺寸
        /// </summary>
        /// <param name="dxf"></param>
        private static void InitSize(DxfDocument dxf)
        {
            foreach (Block block in dxf.Blocks)
            {
                if (block.Name.Contains("*Model_Space"))
                {
                    foreach (var lt in block.Entities)
                    {
                        if (lt.CodeName == "LINE")
                        {
                            var line = (Line)lt;
                            if (line.EndPoint.X > Paper_Size_X)
                            {
                                Paper_Size_X = line.EndPoint.X;
                            }
                            if (line.EndPoint.Y > Paper_Size_Y)
                            {
                                Paper_Size_Y = line.EndPoint.Y;
                            }
                        }
                        if (lt.CodeName == "LWPOLYLINE")
                        {
                            var line = (Polyline2D)lt;
                            foreach (var li in line.Vertexes)
                            {
                                if (li.Position.X > Paper_Size_X)
                                {
                                    Paper_Size_X = li.Position.X;
                                }
                                if (li.Position.Y > Paper_Size_Y)
                                {
                                    Paper_Size_Y = li.Position.Y;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 读取dxf文件信息
        /// </summary>
        /// <param name="dxf"></param>
        /// <returns></returns>
        public static List<string> ShowDxfDocumentInformation(DxfDocument dxf)
        {
            List<string> list = new();

            list.Add("文件版本为: " + dxf.DrawingVariables.AcadVer);
            list.Add("文件COMMENTS为: " + dxf.Comments.Count + "个");

            foreach (var o in dxf.Comments)
            {
                list.Add("    分别为: " + o);
            }
            list.Add("");
            list.Add("文件时间:");
            list.Add("创建时间 (UTC): " + dxf.DrawingVariables.TduCreate + "." +
                     dxf.DrawingVariables.TduCreate.Millisecond.ToString("000"));
            list.Add("上次修改 (UTC): " + dxf.DrawingVariables.TduUpdate + "." +
                     dxf.DrawingVariables.TduUpdate.Millisecond.ToString("000"));
            list.Add("编辑时间 (UTC): " + dxf.DrawingVariables.TdinDwg);
            list.Add("");
            list.Add("应用注册了: " + dxf.ApplicationRegistries.Count + "个");

            foreach (var o in dxf.ApplicationRegistries)
            {
                list.Add("    " + o.Name + "; 参照数量为：" + dxf.ApplicationRegistries.GetReferences(o.Name).Count + "个");
            }

            list.Add("图层有: " + dxf.Layers.Count + "个");
            foreach (var o in dxf.Layers)
            {
                list.Add("    " + o.Name + "; 参照数量为: " + dxf.Layers.GetReferences(o).Count + "个");
                list.Add(DebugNotEqual(o.Linetype, dxf.Linetypes[o.Linetype.Name]));
            }
            list.Add("");

            list.Add("线型种类有: " + dxf.Linetypes.Count + "个");
            foreach (var o in dxf.Linetypes)
            {
                list.Add("    " + o.Name + "; 参照数量为: " + dxf.Linetypes.GetReferences(o.Name).Count + "个");
            }
            list.Add("");

            list.Add("文字样式有: " + dxf.TextStyles.Count + "个");
            foreach (var o in dxf.TextStyles)
            {
                list.Add("    " + o.Name + "; 参照数量为: " + dxf.TextStyles.GetReferences(o.Name).Count + "个");
            }
            list.Add("");

            list.Add("形状样式有: " + dxf.ShapeStyles.Count + "个");
            foreach (var o in dxf.ShapeStyles)
            {
                list.Add("    " + o.Name + "; 参照数量为: " + dxf.ShapeStyles.GetReferences(o.Name).Count + "个");
            }
            list.Add("");

            list.Add("标注样式: " + dxf.DimensionStyles.Count + "个");
            foreach (var o in dxf.DimensionStyles)
            {
                list.Add("    " + o.Name + "; 参照数量为: " + dxf.DimensionStyles.GetReferences(o.Name).Count + "个");
                list.Add(DebugNotEqual(o.TextStyle, dxf.TextStyles[o.TextStyle.Name]));
                list.Add(DebugNotEqual(o.DimLineLinetype, dxf.Linetypes[o.DimLineLinetype.Name]));
                list.Add(DebugNotEqual(o.ExtLine1Linetype, dxf.Linetypes[o.ExtLine1Linetype.Name]));
                list.Add(DebugNotEqual(o.ExtLine2Linetype, dxf.Linetypes[o.ExtLine2Linetype.Name]));
                if (o.DimArrow1 != null) list.Add(DebugNotEqual(o.DimArrow1, dxf.Blocks[o.DimArrow1.Name]));
                if (o.DimArrow2 != null) list.Add(DebugNotEqual(o.DimArrow2, dxf.Blocks[o.DimArrow2.Name]));
            }
            list.Add("");

            list.Add("双线样式: " + dxf.MlineStyles.Count + "个");
            foreach (var o in dxf.MlineStyles)
            {
                list.Add("    " + o.Name + "; 参照数量为: " + dxf.MlineStyles.GetReferences(o.Name).Count + "个");
                foreach (var e in o.Elements)
                {
                    list.Add(DebugNotEqual(e.Linetype, dxf.Linetypes[e.Linetype.Name]));
                }
            }
            list.Add("");

            list.Add("UCS为: " + dxf.UCSs.Count + "个");
            foreach (var o in dxf.UCSs)
            {
                list.Add("    " + o.Name);
            }
            list.Add("");

            list.Add("块有: " + dxf.Blocks.Count);
            foreach (var o in dxf.Blocks)
            {
                list.Add("    " + o.Name + "; 参照数量为: " + dxf.Blocks.GetReferences(o.Name).Count + "个");
                list.Add(DebugNotEqual(o.Layer, dxf.Layers[o.Layer.Name]));

                foreach (var e in o.Entities)
                {
                    list.Add(DebugNotEqual(e.Layer, dxf.Layers[e.Layer.Name]));
                    list.Add(DebugNotEqual(e.Linetype, dxf.Linetypes[e.Linetype.Name]));
                    list.Add(DebugNotEqual(e.Owner, dxf.Blocks[o.Name]));
                    foreach (var x in e.XData.Values)
                    {
                        list.Add(DebugNotEqual(x.ApplicationRegistry, dxf.ApplicationRegistries[x.ApplicationRegistry.Name]));
                    }

                    if (e is Text txt) list.Add(DebugNotEqual(txt.Style, dxf.TextStyles[txt.Style.Name]));

                    if (e is MText mtxt) list.Add(DebugNotEqual(mtxt.Style, dxf.TextStyles[mtxt.Style.Name]));

                    if (e is Dimension dim)
                    {
                        list.Add(DebugNotEqual(dim.Style, dxf.DimensionStyles[dim.Style.Name]));
                        list.Add(DebugNotEqual(dim.Block, dxf.Blocks[dim.Block.Name]));
                    }

                    if (e is MLine mline) list.Add(DebugNotEqual(mline.Style, dxf.MlineStyles[mline.Style.Name]));

                    if (e is netDxf.Entities.Image img) list.Add(DebugNotEqual(img.Definition, dxf.ImageDefinitions[img.Definition.Name]));

                    if (e is Insert ins)
                    {
                        list.Add(DebugNotEqual(ins.Block, dxf.Blocks[ins.Block.Name]));
                        foreach (var a in ins.Attributes)
                        {
                            list.Add(DebugNotEqual(a.Layer, dxf.Layers[a.Layer.Name]));
                            list.Add(DebugNotEqual(a.Linetype, dxf.Linetypes[a.Linetype.Name]));
                            list.Add(DebugNotEqual(a.Style, dxf.TextStyles[a.Style.Name]));
                        }
                    }
                }

                foreach (var a in o.AttributeDefinitions.Values)
                {
                    list.Add(DebugNotEqual(a.Layer, dxf.Layers[a.Layer.Name]));
                    list.Add(DebugNotEqual(a.Linetype, dxf.Linetypes[a.Linetype.Name]));
                    foreach (var x in a.XData.Values)
                    {
                        list.Add(DebugNotEqual(x.ApplicationRegistry, dxf.ApplicationRegistries[x.ApplicationRegistry.Name]));
                    }
                }
            }
            list.Add("");

            list.Add("布局有: " + dxf.Layouts.Count + "个");
            foreach (var o in dxf.Layouts)
            {
                list.Add(DebugNotEqual(o.AssociatedBlock, dxf.Blocks[o.AssociatedBlock.Name]));

                list.Add("    " + o.Name + "; 参照数量为: " + dxf.Layouts.GetReferences(o.Name).Count + "个");
                List<DxfObjectReference> entities = dxf.Layouts.GetReferences(o.Name);
                foreach (var tempE in entities)
                {
                    var e = tempE.Reference;

                    EntityObject entity = e as EntityObject;
                    if (entity != null)
                    {
                        list.Add(DebugNotEqual(entity.Layer, dxf.Layers[entity.Layer.Name]));
                        list.Add(DebugNotEqual(entity.Linetype, dxf.Linetypes[entity.Linetype.Name]));
                        list.Add(DebugNotEqual(entity.Owner, dxf.Blocks[o.AssociatedBlock.Name]));
                        foreach (var x in entity.XData.Values)
                        {
                            list.Add(DebugNotEqual(x.ApplicationRegistry, dxf.ApplicationRegistries[x.ApplicationRegistry.Name]));
                        }
                    }

                    Text txt = e as Text;
                    if (txt != null) list.Add(DebugNotEqual(txt.Style, dxf.TextStyles[txt.Style.Name]));

                    MText mtxt = e as MText;
                    if (mtxt != null) list.Add(DebugNotEqual(mtxt.Style, dxf.TextStyles[mtxt.Style.Name]));

                    Dimension dim = e as Dimension;
                    if (dim != null)
                    {
                        list.Add(DebugNotEqual(dim.Style, dxf.DimensionStyles[dim.Style.Name]));
                        list.Add(DebugNotEqual(dim.Block, dxf.Blocks[dim.Block.Name]));
                    }

                    MLine mline = e as MLine;
                    if (mline != null) list.Add(DebugNotEqual(mline.Style, dxf.MlineStyles[mline.Style.Name]));

                    netDxf.Entities.Image img = e as netDxf.Entities.Image;
                    if (img != null) list.Add(DebugNotEqual(img.Definition, dxf.ImageDefinitions[img.Definition.Name]));

                    Insert ins = e as Insert;
                    if (ins != null)
                    {
                        list.Add(DebugNotEqual(ins.Block, dxf.Blocks[ins.Block.Name]));
                        foreach (var a in ins.Attributes)
                        {
                            list.Add(DebugNotEqual(a.Layer, dxf.Layers[a.Layer.Name]));
                            list.Add(DebugNotEqual(a.Linetype, dxf.Linetypes[a.Linetype.Name]));
                            list.Add(DebugNotEqual(a.Style, dxf.TextStyles[a.Style.Name]));
                        }
                    }
                }
            }
            list.Add("");

            list.Add("图片有: " + dxf.ImageDefinitions.Count + "个");
            foreach (var o in dxf.ImageDefinitions)
            {
                list.Add("    " + o.Name + "; 文件名为: " + o.File + "; 参照数量为: " + dxf.ImageDefinitions.GetReferences(o.Name).Count + "个");
            }
            list.Add("");

            list.Add("DGN底图有: " + dxf.UnderlayDgnDefinitions.Count + "个");
            foreach (var o in dxf.UnderlayDgnDefinitions)
            {
                list.Add("    " + o.Name + "; 文件名为: " + o.File + "; 参照数量为: " + dxf.UnderlayDgnDefinitions.GetReferences(o.Name).Count + "个");
            }
            list.Add("");

            list.Add("DWF底图有: " + dxf.UnderlayDwfDefinitions.Count + "个");
            foreach (var o in dxf.UnderlayDwfDefinitions)
            {
                list.Add("    " + o.Name + "; 文件名为: " + o.File + "; 参照数量为: " + dxf.UnderlayDwfDefinitions.GetReferences(o.Name).Count + "个");
            }
            list.Add("");

            list.Add("PDF底图有: " + dxf.UnderlayPdfDefinitions.Count + "个");
            foreach (var o in dxf.UnderlayPdfDefinitions)
            {
                list.Add("    " + o.Name + "; 文件名为: " + o.File + "; 参照数量为: " + dxf.UnderlayPdfDefinitions.GetReferences(o.Name).Count + "个");
            }
            list.Add("");

            list.Add("组有: " + dxf.Groups.Count + "个");
            foreach (var o in dxf.Groups)
            {
                list.Add("    " + o.Name + "; 实体有: " + o.Entities.Count + "个");
            }
            list.Add("");

            // the entities lists contain the geometry that has a graphical representation in the drawing across all layouts,
            // to get the entities that belongs to a specific layout you can get the references through the Layouts.GetReferences(name)
            // or check the EntityObject.Owner.Record.Layout property
            list.Add("实体:");
            list.Add(EntityType.Arc + "有: " + dxf.Entities.Arcs.Count() + "个");
            //list.Add("\t{0}; count: {1}", EntityType.AttributeDefinition, dxf.AttributeDefinitions.Count());
            list.Add(EntityType.Circle + "有: " + dxf.Entities.Circles.Count() + "个");
            list.Add(EntityType.Dimension + "有: " + dxf.Entities.Dimensions.Count() + "个");
            list.Add(EntityType.Ellipse + "有: " + dxf.Entities.Ellipses.Count() + "个");
            list.Add(EntityType.Face3D + "有: " + dxf.Entities.Faces3D.Count() + "个");
            list.Add(EntityType.Hatch + "有: " + dxf.Entities.Hatches.Count() + "个");
            list.Add(EntityType.Image + "有: " + dxf.Entities.Images.Count() + "个");
            list.Add(EntityType.Insert + "有: " + dxf.Entities.Inserts.Count() + "个");
            list.Add(EntityType.Leader + "有: " + dxf.Entities.Leaders.Count() + "个");
            list.Add(EntityType.Polyline2D + "有: " + dxf.Entities.Polylines2D.Count() + "个");
            list.Add(EntityType.Line + "有: " + dxf.Entities.Lines.Count() + "个");
            list.Add(EntityType.Mesh + "有: " + dxf.Entities.Meshes.Count() + "个");
            list.Add(EntityType.MLine + "有: " + dxf.Entities.MLines.Count() + "个");
            list.Add(EntityType.MText + "有: " + dxf.Entities.MTexts.Count() + "个");
            list.Add(EntityType.Point + "有: " + dxf.Entities.Points.Count() + "个");
            list.Add(EntityType.PolyfaceMesh + "有: " + dxf.Entities.PolyfaceMeshes.Count() + "个");
            list.Add(EntityType.Polyline3D + "有: " + dxf.Entities.Polylines3D.Count() + "个");
            list.Add(EntityType.Shape + "有: " + dxf.Entities.Shapes.Count() + "个");
            list.Add(EntityType.Solid + "有: " + dxf.Entities.Solids.Count() + "个");
            list.Add(EntityType.Spline + "有: " + dxf.Entities.Splines.Count() + "个");
            list.Add(EntityType.Text + "有: " + dxf.Entities.Texts.Count() + "个");
            list.Add(EntityType.Ray + "有: " + dxf.Entities.Rays.Count() + "个");
            list.Add(EntityType.Underlay + "有: " + dxf.Entities.Underlays.Count() + "个");
            list.Add(EntityType.Viewport + "有: " + dxf.Entities.Viewports.Count() + "个");
            list.Add(EntityType.Wipeout + "有: " + dxf.Entities.Wipeouts.Count() + "个");
            list.Add(EntityType.XLine + "有: " + dxf.Entities.XLines.Count() + "个");
            list.Add("");

            list.Add("读取完成");
            return list;
        }

        private static string DebugNotEqual(object A, object B)
        {
            return "";
            if (ReferenceEquals(A, B))
            {
                return "实例参照不相同";
            }
        }

        /// <summary>
        /// 构造X6使用的Json文件
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="taskName">块名</param>
        /// <param name="fillInside">图块是否内部填充</param>
        public static (string Markup, string Anchor) MakeX6Json(Stream fileStream, string taskName, bool fillInside)
        {
            DxfDocument doc = DxfDocument.Load(fileStream);

            var resJa = new JArray();

            var entities = doc.Entities.All;

            InitSize(doc);

            Console.WriteLine("图幅:" +
                Paper_Size_X.ToString() + "," +
                Paper_Size_Y.ToString());
            //Logger.Info("图幅", Paper_Size_X.ToString(), Paper_Size_Y.ToString());

            ExecuteMarkup(taskName, fillInside, resJa, entities);

            var resAnchor = new JArray();

            foreach (var block in doc.Blocks)
            {
                //是锚点
                if (AnchorTest(block.Name, out string io, out string material))
                {
                    //ExecuteMarkup(taskName, fillInside, resJa, block.Entities);

                    //只有唯一的块, 确定是锚点
                    if (block.Entities.Count == 1 &&
                        block.Entities[0].Type == EntityType.Circle)
                    {
                        resAnchor.Add(MakeAnchor(taskName, block, doc.Entities.Inserts));
                        resJa.Add(MakeAnchorCircle(taskName, block, doc.Entities.Inserts));
                    }
                }
            }

            Console.WriteLine("输出生成的内容" + resJa.ToString());
            //Logger.Info("输出生成的内容", resJa.ToString());

            var joMarkup = new JObject();
            joMarkup["tagName"] = "g";
            joMarkup["selector"] = Guid.NewGuid().ToString();
            joMarkup["children"] = resJa;

            var newJa = new JArray();
            newJa.Add(joMarkup);

            return (newJa.ToString(), resAnchor.ToString());
        }

        /// <summary>
        /// 构造锚点
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="block"></param>
        /// <param name="inserts"></param>
        private static JObject MakeAnchor(string taskName, Block block, IEnumerable<Insert> inserts)
        {
            var color = GetAnchorColor(block.Name);

            var anchorJo = new JObject();
            anchorJo["id"] = taskName + block.Name;
            anchorJo["group"] = block.Name;

            //var attrs = new JObject();
            //var attrs_circle = new JObject();
            //attrs_circle["r"] = 2;
            //attrs_circle["magnet"] = true;
            //attrs_circle["stroke"] = color;
            //attrs_circle["fill"] = color;
            //attrs_circle["strokeWidth"] = 2;
            //attrs["circle"] = attrs_circle;

            //anchorJo["attrs"] = attrs;

            var insert = inserts.First(x => x.Block.Name == block.Name);
            //var xy = new JArray();
            //xy.Add(Math.Ceiling(Transform_X(insert.Position.X)));
            //xy.Add(Math.Ceiling(Transform_Y(insert.Position.Y)));
            var x = Math.Ceiling(Transform_X(insert.Position.X));
            var y = Math.Ceiling(Transform_Y(insert.Position.Y));

            //var positionJo = new JObject();
            //positionJo["name"] = "absolute";

            var xyJo = new JObject();
            xyJo["x"] = x;
            xyJo["y"] = y;

            // positionJo["args"] = xyJo;

            anchorJo["args"] = xyJo;

            return anchorJo;
        }

        /// <summary>
        /// 构造锚点圆
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="block"></param>
        /// <param name="inserts"></param>
        /// <returns></returns>
        private static JObject MakeAnchorCircle(string taskName, Block block, IEnumerable<Insert> inserts)
        {
            var circle = block.Entities[0] as Circle;

            var lineColor = ColorTranslator.ToHtml(circle.Color.ToColor());

            var joRes = new JObject();
            joRes["tagName"] = "circle";
            joRes["selector"] = Guid.NewGuid().ToString();
            joRes["groupSelector"] = taskName;

            var insert = inserts.First(x => x.Block.Name == block.Name);

            var cx = Math.Ceiling(Transform_X(insert.Position.X)).ToString();
            var cy = Math.Ceiling(Transform_Y(insert.Position.Y)).ToString();
            var r = Math.Ceiling(Transform_R(circle.Radius)).ToString();

            var joAttr = new JObject();

            joAttr["strokeWidth"] = CalLineWidth(circle.Thickness);
            joAttr["stroke"] = lineColor;                //应为实际颜色
            joAttr["fill"] = "rgba(255,255,255,0)";  //应为实际颜色,这里设置为全透明
            joAttr["cx"] = cx;
            joAttr["cy"] = cy;
            joAttr["r"] = r;
            //joAttr["transform"] = "rotate(180)";

            joRes["attrs"] = joAttr;

            return joRes;
        }

        /// <summary>
        /// 处理图中实体
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="fillInside"></param>
        /// <param name="resJa"></param>
        /// <param name="entities"></param>
        private static void ExecuteMarkup(string taskName, bool fillInside,
            JArray resJa, IEnumerable<EntityObject> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.Type != EntityType.Line &&
                    entity.Type != EntityType.Arc &&
                    entity.Type != EntityType.Circle &&
                    entity.Type != EntityType.MText &&
                    entity.Type != EntityType.Text &&
                    entity.Type != EntityType.Polyline2D
                    )
                {
                    Console.WriteLine("出现了未处理的图块类型：" + entity.Type.ToString());
                    //Logger.Error("出现了未处理的图块类型：", entity.Type.ToString());
                }

                if (entity.Type == EntityType.Polyline2D)
                {
                    if (entity is Polyline2D pline)
                    {
                        resJa.Add(MakePolyline(pline, taskName, fillInside));
                    }
                }
                else if (entity.Type == EntityType.Line)
                {
                    if (entity is Line line)
                    {
                        resJa.Add(MakeLine(line, taskName));
                    }
                }
                else if (entity.Type == EntityType.Text)
                {
                    if (entity is Text text)
                    {
                        resJa.Add(MakeText(text, taskName));
                    }
                }
                else if (entity.Type == EntityType.MText)
                {
                    if (entity is MText text)
                    {
                        resJa.Add(MakeMText(text, taskName));
                    }
                }
                else if (entity.Type == EntityType.Arc)
                {
                    if (entity is Arc arc)
                    {
                        resJa.Add(MakeArc(arc, taskName));
                    }
                }
                else if (entity.Type == EntityType.Circle)
                {
                    if (entity is Circle circle)
                    {
                        resJa.Add(MakeCircle(circle, taskName));
                    }
                }
            }
        }

        /// <summary>
        /// 构造多段线
        /// </summary>
        /// <param name="pline"></param>
        /// <param name="groupName"></param>
        /// <param name="fillInside"></param>
        /// <returns></returns>
        private static JObject MakePolyline(Polyline2D pline, string groupName, bool fillInside)
        {
            var joRes = new JObject();

            joRes["tagName"] = pline.IsClosed ? "polygon" : "polyline";
            joRes["selector"] = Guid.NewGuid().ToString();
            joRes["groupSelector"] = groupName;

            var lineColor = ColorTranslator.ToHtml(pline.Color.ToColor());
            var fill = pline.IsClosed && fillInside ? lineColor : "none";  //不填充内部就是空的

            joRes["fill"] = fill;

            var points = new List<string>();

            var vecList = pline.Vertexes;

            int vecI = 0; //点计数
            double widthSum = 0; //总和

            foreach (Polyline2DVertex item in vecList)
            {
                vecI++;
                widthSum += (item.StartWidth + item.EndWidth) / 2;
                var vector = item.Position;
                points.Add(Math.Ceiling(Transform_X(vector.X)).ToString() + "," +
                           Math.Ceiling(Transform_Y(vector.Y)).ToString());
            }

            var joAttr = new JObject();
            //MessageBox.Show((widthSum / vecI).ToString());
            var lineWidth = widthSum / vecI;

            joAttr["strokeWidth"] = CalLineWidth(lineWidth);
            joAttr["stroke"] = lineColor;
            joAttr["fill"] = fill;
            joAttr["points"] = string.Join(" ", points);
            //joAttr["transform"] = "rotate(180)";

            joRes["attrs"] = joAttr;

            return joRes;
        }

        /// <summary>
        /// 构造线
        /// </summary>
        /// <param name="line"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private static JObject MakeLine(Line line, string groupName)
        {
            var lineColor = ColorTranslator.ToHtml(line.Color.ToColor());

            var joRes = new JObject();
            joRes["tagName"] = "line";
            joRes["selector"] = Guid.NewGuid().ToString();
            joRes["groupSelector"] = groupName;

            var p1x = Math.Ceiling(Transform_X(line.StartPoint.X)).ToString();
            var p1y = Math.Ceiling(Transform_Y(line.StartPoint.Y)).ToString();

            var p2x = Math.Ceiling(Transform_X(line.EndPoint.X)).ToString();
            var p2y = Math.Ceiling(Transform_Y(line.EndPoint.Y)).ToString();

            var joAttr = new JObject();

            joAttr["strokeWidth"] = CalLineWidth(line.Thickness);
            joAttr["stroke"] = lineColor;                //应为实际颜色
            joAttr["x1"] = p1x;
            joAttr["y1"] = p1y;
            joAttr["x2"] = p2x;
            joAttr["y2"] = p2y;

            //joAttr["transform"] = "rotate(180)";

            joRes["attrs"] = joAttr;

            return joRes;
        }

        /// <summary>
        /// 构造文字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private static JObject MakeText(Text text, string groupName)
        {
            //public enum TextAlignment
            //{
            //    TopLeft,
            //    TopCenter,
            //    TopRight,
            //    MiddleLeft,
            //    MiddleCenter,
            //    MiddleRight,
            //    BottomLeft,
            //    BottomCenter,
            //    BottomRight,
            //    BaselineLeft,
            //    BaselineCenter,
            //    BaselineRight,
            //    Aligned,
            //    Middle,
            //    Fit
            //}

            //var (newX, newY) = CalTextBase(text.Alignment.ToString(),
            //    text.Position.X, text.Position.Y, text.Height, text.WidthFactor);

            var newX = CalTextX(text.Alignment.ToString(), text.Position.X, text.WidthFactor * text.Height);
            var newY = CalTextY(text.Alignment.ToString(), text.Position.Y, text.Height);

            return ExecuteText(text.Color, text.Value,
                Transform_X(newX), Transform_Y(newY),
                //Transform_X(text.Position.X), Transform_Y(text.Position.Y),
                //text.Position.X, text.Position.Y,
                Transform_R(text.Height), Transform_R(text.Width),
                groupName);
        }

        /// <summary>
        /// 构造文字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private static JObject MakeMText(MText text, string groupName)
        {
            //public enum MTextAttachmentPoint
            //{
            //    TopLeft = 1,
            //    TopCenter,
            //    TopRight,
            //    MiddleLeft,
            //    MiddleCenter,
            //    MiddleRight,
            //    BottomLeft,
            //    BottomCenter,
            //    BottomRight
            //}

            //var (newX, newY) = CalTextBase(text.AttachmentPoint.ToString(),
            //    text.Position.X, text.Position.Y, text.Height, text.RectangleWidth);

            var newX = CalMTextX(text.AttachmentPoint.ToString(), text.Position.X, text.Height);
            var newY = CalMTextY(text.AttachmentPoint.ToString(), text.Position.Y, text.Height);

            return ExecuteText(text.Color, text.Value,
                Transform_X(newX), Transform_Y(newY),
                Transform_R(text.Height), Transform_R(text.RectangleWidth),
                groupName);
        }

        /// <summary>
        /// 处理文字
        /// </summary>
        /// <param name="inColor"></param>
        /// <param name="content"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontLen"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private static JObject ExecuteText(AciColor inColor,
            string content, double x, double y,
            double fontSize, double fontLen, string groupName)
        {
            //text.WidthFactor = 0.7; //字宽系数 不知对应的哪里

            var color = ColorTranslator.ToHtml(inColor.ToColor());

            var joRes = new JObject();
            joRes["tagName"] = "text";
            joRes["selector"] = Guid.NewGuid().ToString();
            joRes["groupSelector"] = groupName;
            joRes["textContent"] = content;

            var joAttr = new JObject();
            joAttr["fill"] = "rgba(255,255,255,0)";  //应为实际颜色,这里设置为全透明
            joAttr["x"] = Math.Ceiling(x).ToString();
            joAttr["y"] = Math.Ceiling(y).ToString();
            joAttr["textLength"] = Math.Ceiling(fontLen).ToString();
            //joAttr["rotate"] = Math.Ceiling(text.Rotation).ToString();
            //joRes["textAnchor"] = text.Alignment.ToString();
            //joAttr["transform"] = "rotate(180)";
            joRes["attrs"] = joAttr;

            var joStyle = new JObject();
            joStyle["fontSize"] = Math.Ceiling(fontSize).ToString();
            joStyle["stroke"] = color;
            joStyle["fill"] = color;
            //joStyle["transform"] = "rotate(" + text.Rotation.ToString() + ")";
            joRes["style"] = joStyle;

            return joRes;
        }

        /// <summary>
        /// 处理弧，通过转为多段线方式处理弧
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private static JObject MakeArc(Arc arc, string groupName)
        {
            //单位弧按100长算

            var temp = (arc.EndAngle - arc.StartAngle) % 360;
            if (temp < 0)
            {
                temp = 360 + temp;
            }

            var arcLen = 2 * Math.PI * arc.Radius / 360 * temp;

            int preci = (int)Math.Max(3, Math.Ceiling(arcLen));
            return MakePolyline(arc.ToPolyline2D(preci), groupName, true);

            #region 走path arc的方法

            //var startAngle = arc.StartAngle * Math.PI / 180.0f;
            //var endAngle = arc.EndAngle * Math.PI / 180.0f;
            //var arcColor = ColorTranslator.ToHtml(arc.Color.ToColor());
            //var joRes = new JObject();
            //joRes["tagName"] = "path";
            //joRes["selector"] = Guid.NewGuid().ToString();
            //joRes["groupSelector"] = groupName;
            //var joAttr = new JObject();
            //joAttr["strokeWidth"] = "0.35";
            //joAttr["stroke"] = arcColor;                //应为实际颜色
            //joAttr["d"] = string.Format("M{0},{1} A{2},{3} {4} {5} {6} {7},{8}\" ",
            //    Transform_X(arc.Center.X) + (arc.Radius / Scale) * Math.Cos(startAngle),
            //    Transform_Y(arc.Center.Y) + (arc.Radius / Scale) * Math.Sin(startAngle),
            //    arc.Radius / Scale,
            //    arc.Radius / Scale,
            //    arc.StartAngle,
            //    arc.StartAngle > 180 ? 1 : 0,
            //    0,
            //    Transform_X(arc.Center.X) + arc.Radius / Scale * Math.Cos(endAngle),
            //    Transform_Y(arc.Center.Y) + arc.Radius / Scale * Math.Sin(endAngle)
            //    );
            //joRes["attrs"] = joAttr;
            //return joRes;

            #endregion 走path arc的方法
        }

        /// <summary>
        /// 构造圆
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private static JObject MakeCircle(Circle circle, string groupName)
        {
            var lineColor = ColorTranslator.ToHtml(circle.Color.ToColor());

            var joRes = new JObject();
            joRes["tagName"] = "circle";
            joRes["selector"] = Guid.NewGuid().ToString();
            joRes["groupSelector"] = groupName;

            var cx = Math.Ceiling(Transform_X(circle.Center.X)).ToString();
            var cy = Math.Ceiling(Transform_Y(circle.Center.Y)).ToString();
            var r = Math.Ceiling(Transform_R(circle.Radius)).ToString();

            var joAttr = new JObject();

            joAttr["strokeWidth"] = CalLineWidth(circle.Thickness);
            joAttr["stroke"] = lineColor;                //应为实际颜色
            joAttr["fill"] = "rgba(255,255,255,0)";  //应为实际颜色,这里设置为全透明
            joAttr["cx"] = cx;
            joAttr["cy"] = cy;
            joAttr["r"] = r;
            //joAttr["transform"] = "rotate(180)";

            joRes["attrs"] = joAttr;

            return joRes;
        }

        /// <summary>
        /// 计算文字的x方向偏移值
        /// </summary>
        /// <param name="AlignType"></param>
        /// <param name="ox"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static double CalTextX(string AlignType, double ox, double w)
        {
            if (AlignType == "TopLeft" ||
                AlignType == "MiddleLeft" ||
                AlignType == "BottomLeft")
            {
                return ox + w * 0.4375;
            }
            else if (AlignType == "TopCenter" ||
                     AlignType == "MiddleCenter" ||
                     AlignType == "BottomCenter")
            {
                return ox - w * 0.1;
            }
            else if (AlignType == "TopRight" ||
                     AlignType == "MiddleRight" ||
                     AlignType == "BottomRight")
            {
                return ox - w * 0.5625;
            }

            return ox;
        }

        /// <summary>
        /// 计算文字的y方向偏移值
        /// </summary>
        /// <param name="AlignType"></param>
        /// <param name="oy"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private static double CalTextY(string AlignType, double oy, double h)
        {
            if (AlignType == "TopLeft" ||
                AlignType == "TopCenter" ||
                AlignType == "TopRight")
            {
                return oy - h * 0.875;
            }
            else if (AlignType == "MiddleLeft" ||
                     AlignType == "MiddleCenter" ||
                     AlignType == "MiddleRight")
            {
                return oy - h * 0.3125;
            }
            else if (AlignType == "BottomLeft" ||
                     AlignType == "BottomCenter" ||
                     AlignType == "BottomRight")
            {
                return oy + h * 0.4375;
            }

            return oy;
        }

        /// <summary>
        /// 计算文字的x方向偏移值
        /// </summary>
        /// <param name="AlignType"></param>
        /// <param name="ox"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static double CalMTextX(string AlignType, double ox, double w)
        {
            if (AlignType == "TopLeft" ||
                AlignType == "MiddleLeft" ||
                AlignType == "BottomLeft")
            {
                return ox + w * 0.375;
            }
            else if (AlignType == "TopCenter" ||
                     AlignType == "MiddleCenter" ||
                     AlignType == "BottomCenter")
            {
                return ox - w * 0.1;
            }
            else if (AlignType == "TopRight" ||
                     AlignType == "MiddleRight" ||
                     AlignType == "BottomRight")
            {
                return ox - w * 0.5;
            }

            return ox;
        }

        /// <summary>
        /// 计算文字的y方向偏移值
        /// </summary>
        /// <param name="AlignType"></param>
        /// <param name="oy"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private static double CalMTextY(string AlignType, double oy, double h)
        {
            if (AlignType == "TopLeft" ||
                AlignType == "TopCenter" ||
                AlignType == "TopRight")
            {
                return oy - h * 0.875;
            }
            else if (AlignType == "MiddleLeft" ||
                     AlignType == "MiddleCenter" ||
                     AlignType == "MiddleRight")
            {
                return oy - h * 0.3125;
            }
            else if (AlignType == "BottomLeft" ||
                     AlignType == "BottomCenter" ||
                     AlignType == "BottomRight")
            {
                return oy + h * 0.1875;
            }

            return oy;
        }

        /// <summary>
        /// 检测块是否为锚点
        /// </summary>
        /// <param name="blockName"></param>
        /// <param name="io"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        private static bool AnchorTest(string blockName, out string io, out string material)
        {
            io = null;
            material = null;

            try
            {
                var temp = blockName.Split("_");
                if (temp.Length == 2 &&
                    (temp[0] == "inlet" ||      //进口
                     temp[0] == "outlet") &&    //出口
                    (temp[1] == "solid" ||      //物料
                     temp[1] == "gas" ||        //气体
                     temp[1] == "air" ||        //压缩空气
                     temp[1] == "oil" ||        //油
                     temp[1] == "water" ||      //水
                     temp[1] == "co2"))         //二氧化碳
                {
                    io = temp[0];
                    material = temp[1];
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("检测锚点错误:" + blockName + "," + ex.ToString());
                //Logger.Error("检测锚点错误", blockName, ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// 根据锚点名称获取颜色
        /// </summary>
        /// <param name="blockName"></param>
        /// <returns></returns>
        private static string GetAnchorColor(string blockName)
        {
            try
            {
                var temp = blockName.Split("_");
                if (temp.Length == 2 &&
                    (temp[0] == "inlet" ||      //进口
                     temp[0] == "outlet") &&    //出口
                    (temp[1] == "solid" ||      //物料
                     temp[1] == "gas" ||        //气体
                     temp[1] == "air" ||        //压缩空气
                     temp[1] == "oil" ||        //油
                     temp[1] == "water" ||      //水
                     temp[1] == "co2"))         //二氧化碳
                {
                    if (temp[1] == "solid")
                    {
                        return "#FFFFFF";  //物料，白色
                    }
                    else if (temp[1] == "gas")
                    {
                        return "#00FF00";  //气体，绿色
                    }
                    else if (temp[1] == "air")
                    {
                        return "#FFFF00";  //压缩空气，黄色
                    }
                    else if (temp[1] == "oil")
                    {
                        return "#0000FF";  //油，蓝色
                    }
                    else if (temp[1] == "water")
                    {
                        return "#00FFFF";  //水，青色
                    }
                    else if (temp[1] == "co2")
                    {
                        return "#FF0000";  //二氧化碳，红色
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("锚点颜色错误:" + blockName + "," + ex.ToString());
                //Logger.Error("锚点颜色错误", blockName, ex.ToString());

            }
            return null;
        }

        private static string CalLineWidth(double inWidth)
        {
            var defalutData = Math.Ceiling(Transform_R(0.35));
            var width = Math.Ceiling(Transform_R(inWidth));

            return width < 0.000001 ? defalutData.ToString() : width.ToString();
        }
    }
}