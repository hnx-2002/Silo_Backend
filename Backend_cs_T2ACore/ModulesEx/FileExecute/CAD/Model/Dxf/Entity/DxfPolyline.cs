using System.Collections.Generic;

namespace TPFun_Cad
{
    /// <summary>
    /// 多段线(dxf解析)
    /// </summary>
    public class DxfPolyline : DxfEntity
    {
        /// <summary>
        /// 类型：多边形"polygon" / 多段线"polyline"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 端点、拐点
        /// </summary>
        public List<Point2D> Vertexes { get; set; } = new List<Point2D>();
    }
}