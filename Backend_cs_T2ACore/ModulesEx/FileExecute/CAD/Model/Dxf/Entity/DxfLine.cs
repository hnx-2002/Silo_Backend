using System.Collections.Generic;

namespace TPFun_Cad
{
    /// <summary>
    /// 线(dxf解析)
    /// </summary>
    public class DxfLine : DxfEntity
    {
        /// <summary>
        /// 起始点、末尾点
        /// </summary>
        public List<Point2D> Vertexes { get; set; } = new List<Point2D>();
    }
}