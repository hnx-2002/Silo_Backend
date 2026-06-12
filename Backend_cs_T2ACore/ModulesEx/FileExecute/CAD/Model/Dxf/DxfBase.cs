using System;

namespace TPFun_Cad
{
    /// <summary>
    /// dxf基类(dxf解析)
    /// </summary>
    public class DxfBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 外层嵌套块的名称
        /// </summary>
        public string Owner { get; set; } = "";

        /// <summary>
        /// 对象包围盒右上点
        /// </summary>
        public Point2D MaxPoint { get; set; }

        /// <summary>
        /// 对象包围盒左下点
        /// </summary>
        public Point2D MinPoint { get; set; }

    }
}