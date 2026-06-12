namespace TPFun_Cad
{
    /// <summary>
    /// 图元(dxf解析)
    /// </summary>
    public class DxfEntity : DxfBase
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 图层
        /// </summary>
        public string Layer { get; set; }

        /// <summary>
        /// 线型
        /// </summary>
        public string Linetype { get; set; }
    }
}