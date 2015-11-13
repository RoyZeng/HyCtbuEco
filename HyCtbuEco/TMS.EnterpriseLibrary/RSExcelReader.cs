using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace TMS.EnterpriseLibrary
{


    public class RSExcelReader
    {

        private OleDbConnection getOleDbConnection(string path)
        {
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'");
            return con;
        }
        //返回Excel的第一张表
        public DataTable getTableByExcelpath(string path)
        {
            OleDbConnection con = this.getOleDbConnection(path);
            DataSet ds = new DataSet();
            con.Open();
            DataTable ExcelTable = con.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            if (ExcelTable.Rows.Count > 0)
            {
                //取第一张表进行返回
                string FirstTableName = ExcelTable.Rows[0]["TABLE_NAME"].ToString();
                OleDbDataAdapter adapter = new OleDbDataAdapter("select * from [" + FirstTableName + "] ", con);
                adapter.Fill(ds);
            }
            con.Close();
            return ds.Tables[0];
        }
        //返回指定Excel的行数
        public int getRowCountleByExcelpath(string path)
        {
            OleDbConnection con = this.getOleDbConnection(path);
            DataSet ds = new DataSet();
            con.Open();
            DataTable ExcelTable = con.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            if (ExcelTable.Rows.Count > 0)
            {
                //取第一张表进行返回
                string FirstTableName = ExcelTable.Rows[0]["TABLE_NAME"].ToString();
                OleDbDataAdapter adapter = new OleDbDataAdapter("select * from [" + FirstTableName + "] ", con);
                adapter.Fill(ds, "FirstTable");
            }
            con.Close();
            return ds.Tables[0].Rows.Count;
        }
        //返回指定Excel的列数
        public int getColumCountleByExcelpath(string path)
        {
            OleDbConnection con = this.getOleDbConnection(path);
            DataSet ds = new DataSet();
            con.Open();
            DataTable ExcelTable = con.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            if (ExcelTable.Rows.Count > 0)
            {
                //取第一张表进行返回
                string FirstTableName = ExcelTable.Rows[0]["TABLE_NAME"].ToString();
                OleDbDataAdapter adapter = new OleDbDataAdapter("select * from [" + FirstTableName + "] ", con);
                adapter.Fill(ds, "FirstTable");
            }
            con.Close();
            return ds.Tables[0].Columns.Count;
        }
        //返回按指定列distict后的数据表
        public DataTable getTableByExcelpath(string path, int colum)
        {
            OleDbConnection con = this.getOleDbConnection(path);
            DataSet ds = new DataSet();
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            con.Open();
            DataTable ExcelTable = con.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            if (ExcelTable.Rows.Count > 0)
            {
                //取第一张表进行返回
                string FirstTableName = ExcelTable.Rows[0]["TABLE_NAME"].ToString();
                adapter = new OleDbDataAdapter("select * from [" + FirstTableName + "]", con);
                adapter.Fill(ds, "FirstTable");
            }
            string columName = ds.Tables[0].Columns[colum].ColumnName;
            string[] parameter = new string[1];
            parameter[0] = columName;
            DataTable SecondTable = ds.Tables[0].DefaultView.ToTable(true, parameter);
            con.Close();
            return SecondTable;
        }

        //返回按指定列数组的数据表
        public DataTable getTableByExcelpath(string path, int[] colums)
        {
            OleDbConnection con = this.getOleDbConnection(path);
            DataSet ds = new DataSet();
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            con.Open();
            DataTable ExcelTable = con.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            if (ExcelTable.Rows.Count > 0)
            {
                //取第一张表进行返回
                string FirstTableName = ExcelTable.Rows[0]["TABLE_NAME"].ToString();
                adapter = new OleDbDataAdapter("select * from [" + FirstTableName + "]", con);
                adapter.Fill(ds, "FirstTable");
            }
            //string columName = ds.Tables[0].Columns[colum].ColumnName;
            //string[] parameter = new string[1];
            //parameter[0] = columName;
            string[] parameter = new string[colums.Length];
            for (int i = 0; i < colums.Length; i++)
            {
                parameter[i] = ds.Tables[0].Columns[colums[i]].ColumnName;
            }
            DataTable SecondTable = ds.Tables[0].DefaultView.ToTable(true, parameter);
            con.Close();
            return SecondTable;
        }


        //根据路径和数据table插入excel文件中
        public void insertTableByPath(string ExcelPath, DataTable dtData)
        {
            try
            {
                System.Web.UI.WebControls.DataGrid dgExport = null;
                //   当前对话     
                //System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                //   IO用于导出并返回excel文件     
                System.IO.StringWriter strWriter = null;
                System.Web.UI.HtmlTextWriter htmlWriter = null;

                if (dtData != null)
                {
                    if (dtData.Rows.Count < 65530)
                    {
                        //   设置编码和附件格式     
                        //curContext.Response.ContentType = "application/vnd.ms-excel";
                        //curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                        //curContext.Response.Charset = "GBK";
                        DateTime currentNow = DateTime.Now;

                        //   导出excel文件     
                        strWriter = new System.IO.StringWriter();
                        htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

                        //   为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid     
                        dgExport = new System.Web.UI.WebControls.DataGrid();
                        dgExport.DataSource = dtData.DefaultView;
                        dgExport.AllowPaging = false;
                        dgExport.DataBind();

                        //string fileName = Name + currentNow.ToString("yyyy-MM-dd") + ".xls";
                        //string filePath = curContext.Server.MapPath(".") + fileName;
                        System.IO.StreamWriter sw = System.IO.File.CreateText(ExcelPath);



                        dgExport.RenderControl(htmlWriter);
                        sw.Write(strWriter.ToString());

                        sw.Close();
                        //   返回客户端     
                        //this.DownFile(curContext.Response, fileName, ExcelPath);

                        //curContext.Response.End();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}