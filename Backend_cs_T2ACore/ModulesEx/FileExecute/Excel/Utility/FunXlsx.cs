using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Linq;

namespace TPFun_Excel
{
    /// <summary>
    /// 处理Excel数据的方法
    /// </summary>
    public static class FunXlsx
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        public static MemoryStream MakeFile(List<Cell> inData)
        {
            //构造工作簿集合
            var sheetsData = inData.GroupBy(x => x.SheetName).ToList();

            var ms = new MemoryStream();
            using (ExcelPackage package = new())
            {
                foreach (var sheet in sheetsData)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheet.Key);//创建worksheet 
                    //var RowMax = sheet.Max(x => x.Row);
                    //var ColMax = sheet.Max(x => x.Col);
                    var subDatas = sheet.ToList();

                    foreach (var item in subDatas)
                    {
                        worksheet.Cells[item.Row, item.Col].Value = item.Content;
                    }
                }
                package.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }


        /// <summary>
        /// 导出Excel,带样式
        /// </summary>
        public static MemoryStream MakeFileWithStyle(List<CellsWithStyle> inData)
        {
            //构造工作簿集合
            var sheetsData = inData.GroupBy(x => x.SheetName).ToList();

            var ms = new MemoryStream();
            using (ExcelPackage package = new())
            {
                foreach (var sheets in sheetsData)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheets.Key);//创建worksheet 

                    foreach (var sheetWithStyle in sheets)
                    {

                        if (sheetWithStyle.Cells != null)
                        {
                            //填充值
                            foreach (var item in sheetWithStyle.Cells)
                            {
                                worksheet.Cells[item.Row, item.Col].Value = item.Content;
                            }
                        }

                        if (sheetWithStyle.RowStyles != null)
                        {
                            //设置行高
                            foreach (var item in sheetWithStyle.RowStyles)
                            {
                                item.SetHeight(worksheet);
                            }
                        }

                        if (sheetWithStyle.ColStyles != null)
                        {
                            //设置列宽
                            foreach (var item in sheetWithStyle.ColStyles)
                            {
                                item.SetWidth(worksheet);
                            }
                        }

                        if (sheetWithStyle.CellStyles != null)
                        {
                            //设置单元格样式
                            foreach (var item in sheetWithStyle.CellStyles)
                            {
                                item.SetCellStyleToSheet(worksheet);
                            }
                        }

                        if (sheetWithStyle.BorderStyles != null)
                        {
                            //边框
                            foreach (var item in sheetWithStyle.BorderStyles)
                            {
                                item.SetBorderStyleToSheet(worksheet);
                            }
                        }

                        if (sheetWithStyle.Merges != null)
                        {
                            //Merge
                            foreach (var item in sheetWithStyle.Merges)
                            {
                                item.SetMergeToSheet(worksheet);
                            }
                        }
                         
                        if (sheetWithStyle.Header != null)
                        {
                            //设置页眉
                            sheetWithStyle.Header.SetHeadToSheet(worksheet);
                        }

                        if (sheetWithStyle.Footer != null)
                        {
                            //设置页脚
                            sheetWithStyle.Footer.SetFootToSheet(worksheet);
                        }

                        if (sheetWithStyle.PrintSetting != null)
                        {
                            //设置打印
                            sheetWithStyle.PrintSetting.SetPrintToSheet(worksheet);
                        }

                        if (sheetWithStyle.Pictures != null)
                        {
                            //图片
                            foreach (var picture in sheetWithStyle.Pictures)
                            {
                                picture.SetDrawToSheet(worksheet);
                            }
                        }

                    }

                }
                package.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }


    }
}
