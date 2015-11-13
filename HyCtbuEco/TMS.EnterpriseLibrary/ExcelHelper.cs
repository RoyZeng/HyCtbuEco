using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace TMS.EnterpriseLibrary
{
  


    public class ExcelHelper
    {

        public static DataSet ToDataSet<T>(IList<T> list)
        {
            Type elementType = typeof(T);
            var ds = new DataSet();
            var t = new DataTable();
            ds.Tables.Add(t);
            elementType.GetProperties().ToList().ForEach(propInfo => t.Columns.Add(propInfo.Name, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
            foreach (T item in list)
            {
                var row = t.NewRow();
                elementType.GetProperties().ToList().ForEach(propInfo => row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value);
                t.Rows.Add(row);
            }
            return ds;
        }


        /// <summary>
        /// 由DataSet导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>Excel工作表</returns>
        private static Stream ExportDataSetToExcel(DataSet sourceDs, string sheetName, string title, int picColIndex = -1)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            string[] sheetNames = sheetName.Split(',');

            //图像处理需要的变量
            byte[] bytes;
            HSSFCell cell;


            for (int i = 0; i < sheetNames.Length; i++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sheetNames[i]);

                // Create the drawing patriarch.  This is the top level container for all shapes. 
                //Dim patriarch As HSSFPatriarch = MySheet.CreateDrawingPatriarch()

                HSSFPatriarch patriarch = (NPOI.HSSF.UserModel.HSSFPatriarch)sheet.CreateDrawingPatriarch();//一个表单只有一个

                IRow titleRow = sheet.CreateRow(0);//标题列

                HSSFCell cellTitle = (HSSFCell)titleRow.CreateCell(0);


                IFont font = workbook.CreateFont();//标题字体
                font.FontHeightInPoints = 24;//大小
                ICellStyle style = workbook.CreateCellStyle();
                style.SetFont(font);


                cellTitle.SetCellValue(title);//写入标题
                sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, 10));
                cellTitle.CellStyle = style;
                titleRow.HeightInPoints = 30;



                IRow headerRow = sheet.CreateRow(1);
                // handling header.
                foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

                // handling value.
                int rowIndex = 2;
                int colIndex = 0;

                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        sheet.AutoSizeColumn(colIndex);


                        if (picColIndex > -1)
                        {
                            //需要生成相应的图像
                            if (picColIndex == colIndex)
                            {
                                //是这一列的话
                                try
                                {
                                    cell = (HSSFCell)dataRow.CreateCell(column.Ordinal);


                                    sheet.SetColumnWidth(picColIndex, 120 * 40);

                                    string orgPic = row[column].ToString();
                                    orgPic = orgPic.Replace("Org/", "Small/");
                                    string picURL = System.Web.HttpContext.Current.Server.MapPath(orgPic);
                                    bytes = System.IO.File.ReadAllBytes(picURL);

                                    int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);//只能是jpg图像

                                    dataRow.HeightInPoints = 120;//改此列的行高为120

                                    HSSFClientAnchor anchor = new HSSFClientAnchor(20, 20, 0, 0, colIndex, rowIndex, 0, 0);

                                    HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                                    pict.Resize(1);


                                    //,注意：单位为1/256
                                }
                                catch (Exception e5)
                                {

                                    //无图像，一般是
                                    dataRow.CreateCell(column.Ordinal).SetCellValue("");
                                }



                            }
                            else
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                            }
                        }
                        else
                        {


                            dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                        }
                        colIndex += 1;


                    }

                    rowIndex++;

                    colIndex = 0;//复位列索引
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;
            return ms;
        }

        /// <summary>
        /// 由DataSet导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fileName">指定Excel工作表名称</param>
        /// <returns>Excel工作表</returns>
        public static void ExportDataSetToExcel(DataSet sourceDs, string fileName, string sheetName, string title = "", int

picIndex = -1)
        {
            if (string.IsNullOrEmpty(title))
            {
                title = sheetName;//如果没有标题，则使用默认标题
            }
            MemoryStream ms = ExportDataSetToExcel(sourceDs, sheetName, title, picIndex) as MemoryStream;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;fileType=xls;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }



        #region Datatabl导出Excel

        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <returns>Excel工作表</returns>
        private static Stream ExportDataTableToExcel(DataTable sourceTable, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);
            // handling header.
            foreach (DataColumn column in sourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }
        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fileName">指定Excel工作表名称</param>
        /// <returns>Excel工作表</returns>
        public static void ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName)
        {
            MemoryStream ms = ExportDataTableToExcel(sourceTable, sheetName) as MemoryStream;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }

        #endregion datatable 导出excel

        #region excel 导入dataset


        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportDataSetFromExcel(Stream excelFileStream, int headerRowIndex)
        {
            DataSet ds = new DataSet();
            HSSFWorkbook workbook = new HSSFWorkbook(excelFileStream);
            for (int a = 0, b = workbook.NumberOfSheets; a < b; a++)
            {
                ISheet sheet = workbook.GetSheetAt(a);
                DataTable table = new DataTable();

                IRow headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                    {
                        // 如果遇到第一个空列，则不再继续向后读取
                        cellCount = i + 1;
                        break;
                    }

                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null || row.GetCell(0) == null || row.GetCell(0).ToString().Trim() == "")
                    {
                        // 如果遇到第一个空行，则不再继续向后读取
                        break;
                    }

                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = row.GetCell(j).ToString();
                        }
                    }

                    table.Rows.Add(dataRow);
                }
                ds.Tables.Add(table);
            }

            excelFileStream.Close();
            workbook = null;

            return ds;
        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportDataSetFromExcel(string excelFilePath, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                return ImportDataSetFromExcel(stream, headerRowIndex);
            }
        }


        /// <summary>
        /// 返回Excel的所有工作表
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        public static System.Collections.Generic.Dictionary<int, string> GetExcelSheets(string excelFilePath)
        {

            System.Collections.Generic.Dictionary<int, string> myDict = new System.Collections.Generic.Dictionary<int, string>();

            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                for (int a = 0, b = workbook.NumberOfSheets; a < b; a++)
                {
                    ISheet sheet = workbook.GetSheetAt(a);
                    myDict.Add(a, sheet.SheetName);
                }
            }

            return myDict;

        }


        /// <summary>
        /// 返回Excel的指定的工作表，此表已经被放入dataset中
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        public static DataTable GetExcelSheets(string excelFilePath, string sheetName, int startRow = 1)
        {

            DataTable dt = new DataTable();
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                for (int a = 0, b = workbook.NumberOfSheets; a < b; a++)
                {
                    ISheet sheet = workbook.GetSheetAt(a);
                    if (sheet.SheetName == sheetName)
                    {
                        dt = ImportDt(sheet, startRow, true);
                    }
                }
            }





            return dt;

        }


        /// <summary>
        /// 将制定sheet中的数据导出到datatable中
        /// </summary>
        /// <param name="sheet">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        static DataTable ImportDt(ISheet sheet, int HeaderRowIndex, bool needHeader)
        {
            DataTable table = new DataTable();
            HSSFRow headerRow;
            int cellCount;
            try
            {
                if (HeaderRowIndex < 0 || !needHeader)
                {
                    headerRow = sheet.GetRow(0) as HSSFRow;
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        DataColumn column = new DataColumn(Convert.ToString(i));
                        table.Columns.Add(column);
                    }
                }
                else
                {
                    headerRow = sheet.GetRow(HeaderRowIndex) as HSSFRow;
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        if (headerRow.GetCell(i) == null)
                        {
                            if (table.Columns.IndexOf(Convert.ToString(i)) > 0)
                            {
                                DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                                table.Columns.Add(column);
                            }
                            else
                            {
                                DataColumn column = new DataColumn(Convert.ToString(i));
                                table.Columns.Add(column);
                            }

                        }
                        else if (table.Columns.IndexOf(headerRow.GetCell(i).ToString()) > 0)
                        {
                            DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                            table.Columns.Add(column);
                        }
                        else
                        {
                            DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                            table.Columns.Add(column);
                        }
                    }
                }
                int rowCount = sheet.LastRowNum;
                for (int i = (HeaderRowIndex + 1); i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        HSSFRow row;
                        if (sheet.GetRow(i) == null)
                        {
                            row = sheet.CreateRow(i) as HSSFRow;
                        }
                        else
                        {
                            row = sheet.GetRow(i) as HSSFRow;
                        }

                        DataRow dataRow = table.NewRow();

                        for (int j = row.FirstCellNum; j <= cellCount; j++)
                        {
                            try
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    {
                                        case CellType.STRING:
                                            string str = row.GetCell(j).StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[j] = str.ToString();
                                            }
                                            else
                                            {
                                                dataRow[j] = null;
                                            }
                                            break;
                                        case CellType.NUMERIC:
                                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                            }
                                            break;
                                        case CellType.BOOLEAN:
                                            dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                            break;
                                        case CellType.ERROR:
                                            dataRow[j] = row.GetCell(j).ErrorCellValue.ToString();
                                            break;
                                        case CellType.FORMULA:
                                            switch (row.GetCell(j).CachedFormulaResultType)
                                            {
                                                case CellType.STRING:
                                                    string strFORMULA = row.GetCell(j).StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[j] = strFORMULA.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow[j] = null;
                                                    }
                                                    break;
                                                case CellType.NUMERIC:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                                    break;
                                                case CellType.BOOLEAN:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                                    break;
                                                case CellType.ERROR:
                                                    dataRow[j] = row.GetCell(j).ErrorCellValue.ToString();
                                                    break;
                                                default:
                                                    dataRow[j] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                //wl.WriteLogs(exception.ToString());
                            }
                        }
                        table.Rows.Add(dataRow);
                    }
                    catch (Exception exception)
                    {
                        //wl.WriteLogs(exception.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                //wl.WriteLogs(exception.ToString());
            }
            return table;
        }
        public static void CreateExcel(DataTable dt, string path,string FileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(dt.TableName);//创建工作表

            #region 标题
            IRow row = sheet.CreateRow(0);//在工作表中添加一行
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);//在行中添加一列
                cell.SetCellValue(dt.Columns[i].ColumnName);//设置列的内容     
            }
            #endregion

            #region 填充数据
            for (int i = 1; i <= dt.Rows.Count; i++)//遍历DataTable行
            {
                DataRow dataRow = dt.Rows[i - 1];
                row = sheet.CreateRow(i);//在工作表中添加一行

                for (int j = 0; j < dt.Columns.Count; j++)//遍历DataTable列
                {
                    ICell cell = row.CreateCell(j);//在行中添加一列
                    cell.SetCellValue(dataRow[j].ToString());//设置列的内容     
                }
            }
            #endregion

            #region 输出到Excel
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);

            using (FileStream fs = new FileStream(path + "\\" + FileName, FileMode.Create, FileAccess.Write))
            {
                byte[] bArr = ms.ToArray();
                fs.Write(bArr, 0, bArr.Length);
                fs.Flush();
            }
            #endregion

        }

        #endregion excel导入dataset
    }
}