namespace TPFun_Cad
{
    /// <summary>
    /// block块(dxf解析)
    /// </summary>
    public class DxfBlock : DxfBase
    {
        /// <summary>
        /// 块名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Point2D Position { get; set; }

        /// <summary>
        /// 块内部的实体
        /// </summary>
        public DxfLayout InnerEntities { get; set; } = new DxfLayout();

        /// <summary>
        /// 缩放比例(二维)
        /// </summary>
        public Point2D Scale { get; set; } = new Point2D(1.0, 1.0);

        /// <summary>
        /// 旋转（0°向右，逆时针旋转）
        /// </summary>
        public double Rotation { get; set; }
    }
}