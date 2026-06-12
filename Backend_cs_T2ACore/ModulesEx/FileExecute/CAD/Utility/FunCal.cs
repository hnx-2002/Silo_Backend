using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace TPFun_Cad
{
    /// <summary>
    /// 通用计算方法
    /// </summary>
    public static class FunCal
    {
        /// <summary>
        /// 判断两数值（double double）是否相同
        /// </summary>
        /// <param name="inNum1"></param>
        /// <param name="inNum2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsEqual(double inNum1, double inNum2, double tolerance)
        {
            if (Math.Abs(inNum1 - inNum2) < tolerance)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 判断两点（Point2D   Point2D）坐标是否相同
        /// </summary>
        /// <param name="inPoint1"></param>
        /// <param name="inPoint2"></param>
        /// <returns></returns>
        public static bool IsEqualPoint(Point2D inPoint1, Point2D inPoint2)
        {
            if (Math.Abs(inPoint1.X - inPoint2.X) < FunConstant.EqualPoint &&
                Math.Abs(inPoint1.Y - inPoint2.Y) < FunConstant.EqualPoint)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="degree"></param>
        public static double TransRadian(double degree)
        {
            return Math.PI * degree / 180;
        }

        /// <summary>
        /// 分离前后缀(string)和标注值(double)
        /// </summary>
        /// <param name="targetStr">目标字符串，形式：文字+数字+文字</param>
        /// <param name="strPrefix">前缀</param>
        /// <param name="num">数值</param>
        /// <param name="strSuffix">后缀</param>
        public static bool SeparateStrToNumAndAffix
            (string targetStr, out string strPrefix, out double num, out string strSuffix)
        {
            strPrefix = "";
            strSuffix = "";
            num = -1;
            try
            {
                //入参形式："\\A1;ø 500"
                //          "\\A1;{\\fSimSum|bo|io|c134|p2;ø 179.3438}"
                //          "\\A1;ø 235.1467mm"
                //          "\\A1;{\\fArial|b0|i0|c238|p34;±}ø 3794{\\fSimSun|b0|i0|c134|p2;（净空）}"
                //个人理解{}里边套的是字体，因为手改标注的时候字体变了
                //先去掉\\A1; → {\\fArial|b0|i0|c238|p34;±}ø 3794{\\fSimSun|b0|i0|c134|p2;（净空）}
                string subStr = targetStr.Substring(targetStr.IndexOf(";") + 1);
                //处理字体产生的{} → ±ø 3794（净空）
                if (subStr.Contains("{"))
                {
                    subStr = SetBracePart(subStr);
                }
                //去掉：一般符号(除.{}()（）*~外):ø 179.3438  ø 235.1467mm
                string normalSymbolFilter = "[`!@#$^&-=|':;,\\[\\]<>/?" +
                                            "！@#￥……&—|‘’“”：；，。【】《》、？]";
                subStr = subStr.Replace(normalSymbolFilter, "");
                //置换：%%C→Ø  %%D→° %%P→±
                subStr = subStr.Replace("%%C", "Ø").Replace("%%D", "°").Replace("%%P", "±");
                //取出字符串中的数值
                string numFilter = "([0-9]*[.]?[0-9]*)";
                string nonNumFilter = @"[^0-9.]";
                string strNum = Regex.Replace(subStr, nonNumFilter, "");
                bool isDouble = double.TryParse(strNum, out double doubleNum);
                if (isDouble)
                {
                    num = doubleNum;
                }
                else
                {
                    return false;
                }
                //取出字符串中的所有文字
                MatchCollection mcNum = Regex.Matches(subStr, numFilter);
                //去掉长度==0的无效数据
                List<Match> listValidNum = new List<Match>();
                foreach (Match match in mcNum)
                {
                    if (match.Length != 0)
                    {
                        listValidNum.Add(match);
                    }
                }
                if (listValidNum.Count != 1)
                {
                    return false;
                }
                else
                {
                    string strFilterEntity = listValidNum.First().ToString();
                    strPrefix = subStr.Substring(0, subStr.LastIndexOf(strFilterEntity));
                    strSuffix = subStr.Substring(subStr.LastIndexOf(strFilterEntity) + strFilterEntity.Count());
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 处理字体产生的{}
        /// </summary>
        /// <param name="targetStr"></param>
        /// <returns></returns>
        private static string SetBracePart(string targetStr)
        {
            string resStr = "";
            try
            {
                //{}中保留;后边的内容
                //第一个{}的序号
                int leftBrace = targetStr.IndexOf("{");
                int rightBrace = targetStr.IndexOf("}");
                string firstBracePart = targetStr.Substring(leftBrace + 1, rightBrace - leftBrace - 1);
                string firstBraceValidPart = firstBracePart.Substring(firstBracePart.LastIndexOf(";") + 1);
                resStr = targetStr.Replace("{" + firstBracePart + "}", firstBraceValidPart);
                if (resStr.Contains("{"))
                {
                    resStr = SetBracePart(resStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("处理字体产生的{}出错：" + ex.ToString());
                //Logger.Error("处理字体产生的{}出错：", ex.ToString()); 
            }
            return resStr;
        }

        /// <summary>
        /// 获取double的精度(返回0.001而非3位小数)
        /// </summary>
        /// <param name="inValue"></param>
        public static double GetPrecision(double inValue)
        {
            string strValue = inValue.ToString();
            if (strValue.Contains("."))
            {
                int MaxLength = strValue.Length;
                int Index = strValue.IndexOf(".");
                int fractionalPartLength = MaxLength - 1 - Index;
                return Math.Pow(0.1, fractionalPartLength);
            }
            else
            {
                //整数个位1
                return Math.Pow(0.1, 0);
            }
        }
    }
}