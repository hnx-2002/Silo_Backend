namespace TPFun_Cad
{
    /// <summary>
    /// 文字(dxf解析)
    /// </summary>
    public class DxfText : DxfEntity
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Point2D Position { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 旋转（0°向上，逆时针旋转）
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 偏移(单位英尺=304.8倍的mm)
        /// </summary>
        public double Offset { get; set; }
    }
}