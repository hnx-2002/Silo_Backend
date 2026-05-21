namespace TPFun_Cad
{
    /// <summary>
    /// 图案填充(dxf解析)
    /// </summary>
    public class DxfHatch : DxfEntity
    {
        /// <summary>
        /// 坐标(没有？)
        /// </summary>
        public Point2D Position { get; set; }

        /// <summary>
        /// 图案
        /// </summary>
        public string PatternName { get; set; }

        /// <summary>
        /// 旋转
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// 填充比例
        /// </summary>
        public double Scale { get; set; }
    }
}