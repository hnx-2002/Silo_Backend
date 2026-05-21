using System.Collections.Generic;

namespace TPFun_Cad
{
    /// <summary>
    /// Dxf解析内容
    /// </summary>
    public class DxfLayout
    {
        /// <summary>
        /// 弧集合
        /// </summary>
        public List<DxfArc> Arcs { get; set; } = new List<DxfArc>();

        /// <summary>
        /// 圆集合
        /// </summary>
        public List<DxfCircle> Circles { get; set; } = new List<DxfCircle>();

        /// <summary>
        /// 标注集合
        /// </summary>
        public List<DxfDimension> Dimensions { get; set; } = new List<DxfDimension>();

        /// <summary>
        /// 椭圆集合
        /// </summary>
        public List<DxfEllipse> Ellipses { get; set; } = new List<DxfEllipse>();

        /// <summary>
        /// 图案填充集合
        /// </summary>
        public List<DxfHatch> Hatchs { get; set; } = new List<DxfHatch>();

        /// <summary>
        /// 线集合
        /// </summary>
        public List<DxfLine> Lines { get; set; } = new List<DxfLine>();

        /// <summary>
        /// 多行文字集合
        /// </summary>
        public List<DxfMText> MTexts { get; set; } = new List<DxfMText>();

        /// <summary>
        /// 多段线集合
        /// </summary>
        public List<DxfPolyline> PLines { get; set; } = new List<DxfPolyline>();

        /// <summary>
        /// 文字集合
        /// </summary>
        public List<DxfText> Texts { get; set; } = new List<DxfText>();

        /// <summary>
        /// 块集合
        /// </summary>
        public List<DxfBlock> Blocks { get; set; } = new List<DxfBlock>();
    }
}