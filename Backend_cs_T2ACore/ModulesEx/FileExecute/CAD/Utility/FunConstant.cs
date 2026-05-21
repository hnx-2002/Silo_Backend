using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TPFun_Cad
{
    /// <summary>
    /// 常量
    /// </summary>
    public static class FunConstant
    {
        /// <summary>
        /// 窗口类型：模型+布局的 字段集合(不能修改顺序，因为使用[i])(还用于忽略的块名)
        /// </summary>
        public static List<string> CadWindowType { get; set; } = new List<string>() { "*Model", "*Paper" };

        /// <summary>
        /// 用于判断两点重合的公差
        /// </summary>
        public static double EqualPoint { get; } = Math.Pow(10, -3);

        /// <summary>
        /// 用于判断两数值相同的公差
        /// </summary>
        public static double EqualNum { get; } = Math.Pow(10, -4);
    }
}