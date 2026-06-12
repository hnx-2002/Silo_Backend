using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPFun_Excel
{
    /// <summary>
    /// Excel文件写入类
    /// </summary>
    public static class XlsxWriter
    {
        /// <summary> 
        /// 将cells写入xlsx文件
        /// </summary>  
        /// <param name="cells"></param> 
        /// <returns></returns>  
        public static (bool Status, string Msg, MemoryStream MS) WriteToFile(List<Cell> cells)
        {
            try
            {
                var ms = FunXlsx.MakeFile(cells);
                return (true, "OK", ms);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return (false, error, null);
            }
        }


        /// <summary> 
        /// 将cells写入xlsx文件,带样式
        /// </summary>  
        /// <param name="cells"></param> 
        /// <returns></returns>  
        public static (bool Status, string Msg, MemoryStream MS) WriteToFileWithSytle(List<CellsWithStyle> cells)
        {
            try
            {
                var ms = FunXlsx.MakeFileWithStyle(cells);
                return (true, "OK", ms);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return (false, error, null);
            }
        }

    }
}
