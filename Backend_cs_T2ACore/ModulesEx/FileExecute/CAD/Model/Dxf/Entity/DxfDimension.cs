namespace TPFun_Cad
{
    /// <summary>
    /// 标注(dxf解析)
    /// </summary>
    public class DxfDimension : DxfEntity
    {
        /// <summary>
        /// 第一参照点坐标
        /// </summary>
        public Point2D FirstRefePoint { get; set; }

        /// <summary>
        /// 第二参照点坐标
        /// </summary>
        public Point2D SecondRefePoint { get; set; }

        /// <summary>
        /// 文字参照点坐标
        /// </summary>
        public Point2D TextRefePoint { get; set; }

        /// <summary>
        /// 标注线位置（待核实：偏移可算出此值）
        /// </summary>
        public Point2D DimLinePosition { get; set; }

        /// <summary>
        /// 偏移(单位英尺=304.8倍的mm)
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// 旋转（0°向下，逆时针旋转）
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// 测量结果
        /// </summary>
        public double Measurement { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 改写(手改标注)
        /// </summary>
        public string Overwrite { get; set; }

        /// <summary>
        /// 标注文字替代
        /// </summary>
        public string UserText { get; set; }
    }
}