using System;
using System.Collections.Generic;

namespace TPFun_Cad
{
    /// <summary>
    /// 弧(dxf解析)
    /// </summary>
    public class DxfArc : DxfCircle
    {
        /// <summary>
        /// 起始角
        /// </summary>
        public double StartAngle { get; set; }

        /// <summary>
        /// 终止角
        /// </summary>
        public double EndAngle { get; set; }
    }
}