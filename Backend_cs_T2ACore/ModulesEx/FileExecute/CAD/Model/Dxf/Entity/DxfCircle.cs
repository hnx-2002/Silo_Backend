namespace TPFun_Cad
{
    /// <summary>
    /// 圆(dxf解析)
    /// </summary>
    public class DxfCircle : DxfEntity
    {
        /// <summary>
        /// 圆心坐标
        /// </summary>
        public Point2D CircleCenter { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }
    }
}