using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using System.Linq;

namespace TPFun_Excel
{
    /// <summary>
    /// Excel文件解析
    /// </summary>
    public static class XlsxReader
    {

        /// <summary> 
        /// 获取单元格集合,单个文件，指定工作簿 
        /// </summary> 
        /// <param name="importFile">导入的文件</param> 
        /// <param name="sheet"></param> 
        /// <returns></returns>  
        public static (bool Status, string Msg, List<Cell> Cells, string FileName) Get(
            IFormCollection importFile, string sheet)
        {
            try
            {
                var newFile = importFile.Files[0];
                var fileExt = newFile.FileName.EndsWith(".xlsx");

                if (!fileExt)
                {
                    return (false, "请上传xlsx文件", null, newFile.FileName);
                }

                var fileStream = newFile.OpenReadStream();

                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    var workbooks = package.Workbook.Worksheets;
                    ExcelWorksheet worksheet = workbooks[sheet];

                    if (worksheet == null)
                    {
                        return (false, "未找到工作簿", null, newFile.FileName);
                    }
                    if (worksheet.Dimension == null)
                    {
                        return (false, "工作薄无内容", null, newFile.FileName);
                    }

                    List<Cell> list = Cell.ExecuteCells(worksheet);

                    return (true, "OK", list, newFile.FileName);

                }

            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return (false, error, null, null);
            }
        }


        /// <summary> 
        /// 获取单元格集合 ，单个文件，所有工作簿
        /// </summary> 
        /// <param name="importFile"></param>  
        /// <returns></returns>  
        public static (bool Status, string Msg, List<Cell> Cells, string FileName) GetAll(
            IFormCollection importFile)
        {

            var newFile = importFile.Files[0];
            var fileExt = newFile.FileName.EndsWith(".xlsx");

            if (!fileExt)
            {
                return (false, "请上传xlsx文件", null, newFile.FileName);
            }

            var fileStream = newFile.OpenReadStream();
            return GetAllFromStream(fileStream, newFile.FileName);
        }


        /// <summary> 
        /// 从文件流获取单元格集合 ，单个文件，所有工作簿
        /// </summary> 
        /// <param name="fileStream"></param>  
        /// <param name="fileName"></param>  
        /// <returns></returns>  
        public static (bool Status, string Msg, List<Cell> Cells, string FileName) GetAllFromStream(
            Stream fileStream, string fileName)
        {
            try
            {
                using ExcelPackage package = new ExcelPackage(fileStream);
                var workbooks = package.Workbook.Worksheets;

                var list = new List<Cell>();
                foreach (ExcelWorksheet worksheet in workbooks)
                {
                    var tempList = Cell.ExecuteCells(worksheet);
                    list.AddRange(tempList);
                }
                return (true, "OK", list, fileName);

            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return (false, error, null, null);
            }
        }

        /// <summary>
        /// 通过文件流获取Excel文件中第一个非隐藏工作簿的所有单元格
        /// </summary>
        /// <param name="newStream"></param>
        /// <returns></returns>
        public static (bool Status, string Msg, List<Cell> Cells) GetVisibleCellsFromFirstSheet(Stream newStream)
        {
            try
            {
                using ExcelPackage excelPackage = new ExcelPackage(newStream);
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets
                    .First(x => x.Hidden == eWorkSheetHidden.Visible);
                // ExcelWorksheet excelWorksheet = worksheets[0];
                if (excelWorksheet == null)
                {
                    return (false, "未找到工作簿", null);
                }

                if (excelWorksheet.Dimension == null)
                {
                    return (false, "工作薄无内容", null);
                }

                List<Cell> item = Cell.ExecuteCells(excelWorksheet);
                return (true, "OK", item);
            }
            catch (Exception ex)
            {
                string item2 = ex.ToString();
                return (false, item2, null);
            }
        }


        /// <summary> 
        /// 获取单元格集合 ，多文件
        /// </summary> 
        /// <param name="importFile"></param>  
        /// <returns></returns>  
        public static (bool Status, string Msg, List<XlsxFile> Files) MultiFileGetAll(
            IFormCollection importFile)
        {
            try
            {
                var newFiles = importFile.Files;

                var resList = new List<XlsxFile>();
                foreach (var newFile in newFiles)
                {
                    var xlsx = new XlsxFile();
                    xlsx.FileName = newFile.FileName;
                    xlsx.Cells = new List<Cell>();

                    var fileExtTest = newFile.FileName.EndsWith(".xlsx");
                    if (!fileExtTest)
                    {
                        continue;
                    }

                    var fileStream = newFile.OpenReadStream();
                    using ExcelPackage package = new ExcelPackage(fileStream);
                    var workbooks = package.Workbook.Worksheets;

                    foreach (ExcelWorksheet worksheet in workbooks)
                    {
                        var tempList = Cell.ExecuteCells(worksheet);
                        xlsx.Cells.AddRange(tempList);
                    }

                    resList.Add(xlsx);
                }

                return (true, "OK", resList);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return (false, error, null);
            }
        }

        /// <summary> 
        /// 获取工作簿首页带格式单元格集合 ，单文件，多个工作簿
        /// </summary> 
        /// <param name="importFile"></param>  
        /// <returns></returns>  
        public static (bool Status, string Msg, CellsWithStyle Cells, string FileName) GetSheet0WithStyle(
            IFormCollection importFile)
        {
            try
            {
                var newFile = importFile.Files[0];
                var fileExt = newFile.FileName.EndsWith(".xlsx");

                if (!fileExt)
                {
                    return (false, "请上传xlsx文件", null, newFile.FileName);
                }

                var fileStream = newFile.OpenReadStream();

                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    var workbooks = package.Workbook.Worksheets;
                    ExcelWorksheet worksheet = workbooks[0];

                    if (worksheet == null)
                    {
                        return (false, "未找到工作簿", null, newFile.FileName);
                    }
                    if (worksheet.Dimension == null)
                    {
                        return (false, "工作薄无内容", null, newFile.FileName);
                    }

                    var res = new CellsWithStyle(); //结果

                    res.SheetName = worksheet.Name;

                    //处理单元格内容
                    res.Cells = Cell.ExecuteCells(worksheet);

                    //处理单元格样式
                    res.CellStyles = CellStyle.ReadCellStyle(worksheet);

                    //处理合并单元格 
                    res.Merges = RangeMerge.ReadMerges(worksheet);

                    //处理行高
                    res.RowStyles = RowStyle.ReadRowStyle(worksheet);

                    //处理列宽
                    res.ColStyles = ColStyle.ReadColStyle(worksheet);

                    //处理边框
                    res.BorderStyles = BorderStyle.ReadBorderStyle(worksheet);

                    //获取表中图片
                    res.Pictures = CellDraw.ReadPictures(worksheet);

                    //获取页眉
                    res.Header = Header.ReadHeader(worksheet);

                    //获取页脚
                    res.Footer = Footer.ReadFooter(worksheet);

                    //获取打印配置
                    res.PrintSetting = PrintSetting.ReadPrintSetting(worksheet);

                    return (true, "OK", res, newFile.FileName);
                }

            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return (false, error, null, null);
            }
        }


    }
}