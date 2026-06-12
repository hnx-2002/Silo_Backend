namespace TPFun_Cad
{
    /// <summary>
    /// 椭圆(dxf解析)
    /// </summary>
    public class DxfEllipse : DxfEntity
    {
        /// <summary>
        /// 椭圆心坐标
        /// </summary>
        public Point2D EllipseCenter { get; set; }

        /// <summary>
        /// 主轴长
        /// </summary>
        public double MajorAxisLength { get; set; }

        /// <summary>
        /// 镜像轴长
        /// </summary>
        public double MinorAxisLength { get; set; }

        /// <summary>
        /// 旋转（0°向右，逆时针旋转）
        /// </summary>
        public double Rotation { get; set; }
    }
}