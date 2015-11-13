using Aspose.Cells;
using HyCtbuEco.Admin;
using HyCtbuEco.Dim;
using HyCtbuEco.Entities;
using HyCtbuEco.Models;
using HyCtbuEco.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TMS.EnterpriseLibrary;

namespace WebSite.Areas.WebApi.Controllers
{
    [UserAuthorizeSys]
    public class CommonDataImportController : Controller
    {
        private int IsDeleMainData; //是否清除原主表数据
        private int IsAddUnExistData;//是否追加附表中不存的数据
        private string uploadPath = "~/upload/import/";
        private string errorPath = "~/upload/error/";
        private string CurrentFileName = "";





        private IList<ImportTableFieldStruct> CreateFormat(String savePaht)
        {
            IList<ImportTableFieldStruct> list = new List<ImportTableFieldStruct>();

            Workbook workbook = new Workbook();
            workbook.Open(savePaht);
            Cells cells = workbook.Worksheets[1].Cells;

            for (int i = 1; i < cells.MaxDataRow + 1; i++)
            {
                for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                {
                    string s = cells[i, j].StringValue.Trim();

                }
            }
            return list;
        }


        /// <summary>
        /// 上传Excel文件
        /// </summary>
        /// <param name="sender"></param>
        [HttpPost]
        public JsonResult btnExcelUploadClick(string qqfile)
        {
            const string FilePath = "/Upload/UploadExcel/";  //文件的保存路径

            string currentUserIDString = SysUserInfo.GetUserID().ToString();
            //switch (Session["UserType"].ToString())
            //{
            //    case "0":  //当前登录用户的类型为学生
            //        //currentUserIDString = frontCurrentStu.GetNo();
            //        currentUserIDString = "1";
            //        break;
            //    case "1": //当前登录用户的类型为教师
            //        currentUserIDString ="2";
            //        break;
            //    case "2"://当前登录用户的类型为管理员
            //        //Test11.14将GetUserNO改为GetSysUserName()
            //        currentUserIDString = FrontUserInfo.GetSysUserName();
            //        break;
            //    default:
            //        break;
            //}
            string fileName = DateTime.Now.ToString("ddHHmmssff");//名称
            CurrentFileName = fileName;
            string fileType = qqfile.Substring(qqfile.LastIndexOf("."));//类型
            string webFile = FilePath + currentUserIDString + "/UploadFile/" + fileName + fileType;//服务器端路径          

            string uploadPath = "";
            uploadPath = Server.MapPath(FilePath + currentUserIDString + "/UploadFile/");//物理存储路径
            Session["uploadPath"] = webFile;
            //若不存在当前目录则创建
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            //流存储
            using (var inputStream = Request.InputStream)
            {
                using (var flieStream = new FileStream(Server.MapPath(webFile), FileMode.Create))
                {
                    inputStream.CopyTo(flieStream);
                }
            }
            return Json(qqfile, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据提交的数据进行导入
        /// </summary>//返回两个数据, 一个表示状态 出错地方,,仅仅在成功时表现插入的条数
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///   protected void btnImportDataClick(object sender, DirectEventArgs e)
        public JsonResult btnImportDataClick(String tableName, String cityCode, String attrArr)
        {
            Database db = DBHelper.CreateDataBase("PlatformData");
            //接受并保存excel
            string path = Server.MapPath("/uploadExcel");

            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count <= 0)
            {
                return Json("没有接受到文件", JsonRequestBehavior.AllowGet);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            String name = files[0].FileName;
            string savePaht = Server.MapPath("/uploadExcel/" + name);
            files[0].SaveAs(savePaht);


            String StrErrorOrNot = "";//表示错误原因或者正确导入时返回的值的相关信息
            //(1)取导入表结构

            //(2)读到数据,准备循环处理;
            string targTableName = tableName;//目标表名
            string excelPath = savePaht;
            DataTable dt = null, dtSec = null;
            if (!System.IO.File.Exists(excelPath))
            {
                return null;
            }
            try
            {
                dt = new RSExcelReader().getTableByExcelpath(excelPath);  //读取数据到表中，第一行为列的说明名,第二行为列名

            }
            catch (Exception e3)
            {
                StrErrorOrNot = "出错了，读取Excel错误:" + e3.Message.ToString();
                return Json(StrErrorOrNot, JsonRequestBehavior.AllowGet);
            }

            //(4)判断此数据是否合格;
            int I_TeaDetailsCount = dt.Rows.Count;

            if (I_TeaDetailsCount < 2)
            {
                StrErrorOrNot = "出错了，读取Excel格式错误，行数太少!";
                return Json(StrErrorOrNot, JsonRequestBehavior.AllowGet);
            }

            int dtCount = dt.Rows.Count;//总行数
            int dtColCount = dt.Columns.Count;//总列数

            DbCommand cmd;

            StringBuilder insertSQL = new StringBuilder(), fieldSQL = new StringBuilder(), valueSQL = new StringBuilder();
            //取已有的指标和指标单位
            List<String> indCodeList = new List<string>();
            List<String> unitList = new List<string>();
            string insertSecSQL = "";
            List<string> filedName = new List<string>();
            for (int i = 1; i < dtColCount; i++)
            {
                String unit = dt.Rows[0][i] == null ? "null" : dt.Rows[0][i].ToString();
                unitList.Add(unit);
                indCodeList.Add(dt.Rows[1][i].ToString());
            }

            for (int i = 2; i < dtCount; i++)
            {


                for (int j = 1; j < dtColCount; j++)
                {

                    String value = dt.Rows[i][j].ToString();
                    if (value.Equals("-"))
                    {
                        continue;
                    }
                    insertSQL.Append("INSERT INTO [HYCtbuEco].[dbo].[" + tableName + "]( ");
                    insertSQL.Append("[STimeCode],[IAreaID],[SIndCode],[FValue],[SUnit],[SMemo],[ITimeID],[IIndID]");
                    insertSQL.Append(" ) VALUES ");

                    valueSQL.Append("(");

                    String timeCode = dt.Rows[i][0].ToString();
                    valueSQL.Append("'" + timeCode + "',");
                    valueSQL.Append("'" + cityCode + "',");
                    String indCode = indCodeList[j - 1];
                    valueSQL.Append("'" + indCode + "',");

                    valueSQL.Append(value + ",");
                    String unit = unitList[j - 1].Equals("null") ? "null" : "'" + unitList[j - 1] + "'";
                    valueSQL.Append(unit + ",");
                    valueSQL.Append("null,");//对应SMemo

                    IEnumerable<TbDimTime> timelist = MyCache.GetTimeId().Where(a => a.STimeCode == timeCode);
                    TbDimTime time = timelist.ToList().Count > 0 ? timelist.First() : null;
                    if (time == null)
                    {
                        valueSQL.Append("null,");//对应ITimeID
                    }
                    else
                    {
                        valueSQL.Append("'" + time.Id + "',");
                    }


                    IEnumerable<TbDimIndicator> indicatorlist = MyCache.GetIndId().Where(a => a.SIndCode == indCode);
                    TbDimIndicator indicator = indicatorlist.ToList().Count > 0 ? indicatorlist.First() : null;
                    if (indicator == null)
                    {
                        valueSQL.Append("null");//对应IIndID
                    }
                    else
                    {
                        valueSQL.Append("'" + indicator.Id + "'");
                    }
                    valueSQL.Append(");");

                    insertSQL.Append(valueSQL);
                    insertSecSQL += insertSQL;

                    insertSQL.Clear();
                    valueSQL.Clear();

                }

            }

            try
            {
                cmd = db.GetSqlStringCommand(insertSecSQL);
                db.ExecuteNonQuery(cmd);
            }
            catch (Exception e)
            {

                return Json("数据库错误，上传失败", JsonRequestBehavior.AllowGet);
            }
            return Json("上传成功", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据字段类型，返回value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string getValueStringByType(string value, string type)
        {
            type = type.ToLower();
            switch (type)
            {

                case "int"://直接返回value

                    break;
                case "numeric":
                    break;
                case "float":
                    break;

                default:
                    value = "'" + value + "'";

                    break;
            }
            return value;
        }






        /// <summary>
        /// 生成字段和列，并绑定数据源
        /// </summary>
        /// <param name="sourceFileName">文件名</param>
        /// <param name="targTableName">目标表名选中的表</param>
        /// <param name="_gp"></param>
        /// <param name="_store"></param>
        public JsonResult BindData()
        {
            string StrError = null;//返回错误原因
            //清除旧数据与记录集
            //(1)读取目标表的所有列数据Session["uploadPath"]
            string uploadPath = Session["uploadPath"].ToString();
            string targTableName = Session["TargetTable"].ToString();//获取目标表
            string myFiledsSql = @"SELECT syscolumns.name,systypes.name type,syscolumns.isnullable,
        syscolumns.length 
        FROM syscolumns, systypes 
        WHERE syscolumns.xusertype = systypes.xusertype 
        AND syscolumns.id = object_id('" + targTableName + "')";
            IList<ImportTableFieldStruct> structList = new List<ImportTableFieldStruct>();
            IList<ColumeStructs> colsList = new List<ColumeStructs>();

            DataTable targetTableStru = null;//目标表表结构数据

            //示例:
            //name type isnullable length
            // ID	int	0	4
            //S_SubName	nvarchar	1	40
            //I_ParentID	int	1	4
            //S_Memo	nvarchar	1	40
            //I_Level	int	1	4
            //I_Sort	int	1	4
            try
            {
                Database db = DBHelper.CreateDataBase("PlatformData");


                DbCommand cmd = db.GetSqlStringCommand(myFiledsSql);

                using (DataSet myDs = db.ExecuteDataSet(cmd))
                {
                    targetTableStru = myDs.Tables[0];

                    for (int i2 = 0; i2 < targetTableStru.Rows.Count; i2++)
                    {
                        ColumeStructs colsItem = new ColumeStructs();
                        colsItem.ColName = targetTableStru.Rows[i2][0].ToString();
                        colsItem.ColType = targetTableStru.Rows[i2][1].ToString();
                        colsItem.ColNullable = targetTableStru.Rows[i2][2].ToString();
                        colsItem.ColLength = targetTableStru.Rows[i2][3].ToString();

                        colsList.Add(colsItem);//DataTable属性
                    }

                }
            }
            catch (Exception e2)
            {
                StrError = "出错了!" + e2.Message.ToString();
                return Json(new { structList, StrError }, JsonRequestBehavior.AllowGet);
            }




            //(3)读入源数据的Excel文件;
            string excelPath = Server.MapPath(uploadPath);
            DataTable dt = null;
            if (!System.IO.File.Exists(excelPath))
            {
                return null;
            }
            try
            {
                dt = new RSExcelReader().getTableByExcelpath(excelPath);  //读取数据到表中，第一行为列的说明名,第二行为列名

            }
            catch (Exception e3)
            {
                StrError = "出错了，读取Excel错误:" + e3.Message.ToString();
                return Json(new { structList, StrError }, JsonRequestBehavior.AllowGet);
            }



            //(4)判断此数据是否合格;

            DataTable dt_Error = dt.Clone();   //添加一个一样结构的table
            dt_Error.Columns.Add("异常原因");
            int I_TeaDetailsCount = dt.Rows.Count;
            if (I_TeaDetailsCount < 1)
            {

                StrError = "出错了，读取Excel格式错误，行数太少!";
                return Json(new { structList, StrError }, JsonRequestBehavior.AllowGet);
            }
            //(5)读取有效行,并写入临时类






            //只处理第二列

            DataRow sourcRow = dt.Rows[0];
            int colCount = dt.Columns.Count;
            for (int j = 0; j < colCount; j++)
            {
                ImportTableFieldStruct strutItem = new ImportTableFieldStruct();
                //SourceFieldMem表字段说明
                strutItem.SourceFieldMem = dt.Columns[j].ColumnName;//源表说明
                string tmpField = sourcRow[j].ToString();//源表字段名
                string[] myFields = tmpField.Split(';');//源表字段可以指定目标字段，以;号分隔

                //查询与源表字段名相同的名称作为匹配字段，如果找不到，返回为空
                strutItem.SoureFieldName = myFields[0];//
                int taregIndex = CheckTareget(strutItem.SoureFieldName, targetTableStru);//taregIndex为对应的字段的所在行targetTableStru从数据库里面获取的字段
                if (taregIndex > -1)
                {
                    //找到了同名列，需要处理

                    strutItem.TargetFieldName = targetTableStru.Rows[taregIndex][0].ToString();
                    strutItem.TargetFieldType = targetTableStru.Rows[taregIndex][1].ToString();
                    strutItem.TargetFieldLength = targetTableStru.Rows[taregIndex][3].ToString();
                    strutItem.TargetFieldNullable = targetTableStru.Rows[taregIndex][2].ToString();
                }

                if (myFields.Length == 6)
                {
                    //有第三方列
                    strutItem.TargetFieldName = myFields[1]; //返回的对应目标字段
                    strutItem.SecTableName = myFields[2];
                    strutItem.SecFieldName = myFields[3];
                    strutItem.SecFieldType = myFields[4].ToLower();//字段类型，小写
                    strutItem.SecFieldLength = myFields[5];
                }
                strutItem.GenID = (j + 1);


                structList.Add(strutItem);

            }
            Session["ImportTbStructs"] = structList;//临时表
            return Json(new { structList, StrError }, JsonRequestBehavior.AllowGet);
        }






        /// <summary>
        /// 检查sourceName与targettable中的列中是否一致，如果不一致，则返回-1，否则返回一致当的index
        /// </summary>
        /// <param name="SourceName"></param>
        /// <param name="targetName"></param>
        /// <returns></returns>
        private int CheckTareget(string SourceName, DataTable targetName)
        {
            int returnValue = -1;
            SourceName = SourceName.Trim();//去掉多余的空格
            int tmpCount = targetName.Rows.Count;
            for (int i = 0; i < tmpCount; i++)
            {
                string myName = targetName.Rows[i][0].ToString().Trim();
                if (myName.ToUpper() == SourceName.ToUpper())
                {
                    returnValue = i;
                    break;
                }
            }

            return returnValue;
        }
        /// <summary>
        /// 判断是否清除主表原有数据
        /// </summary>
        /// <param name="IsChecked"></param>
        /// <returns></returns>
        public void ValidateIsClearMainTable(int IsChecked)
        {
            Session["IsClearMainTable"] = IsChecked;
        }
        /// <summary>
        /// 判断是否需要追加附表中不存在的数据
        /// </summary>
        /// <param name="IsChecked"></param>
        public void ValidateIsAddUnExistData(int IsChecked)
        {

            Session["IsAddUnExistData"] = IsChecked;
        }
        /// <summary>
        /// 获取目标表名
        /// </summary>
        /// <param name="TargetTable">目标表名</param>
        public void GetCurrentChosenTargetTable(string TargetTable)
        {

            Session["TargetTable"] = TargetTable;
        }
        #region 导出Excel操作

        /// <returns></returns>
        public string Printer(DataTable Error)
        {

            const string FilePath = "/Upload/CommonUploadErrorFile/";  //文件的保存路径//Server.MapPath(FilePath);//物理存储路径
            string ErrorFile = "导入出错列表" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            string PathNew = Server.MapPath(FilePath);
            if (!Directory.Exists(PathNew))
            {
                Directory.CreateDirectory(PathNew);
            }

            ExcelHelper.CreateExcel(Error, PathNew, ErrorFile);
            return FilePath + ErrorFile;
        }
        #endregion

        //根据IndCate的Id查找指标
        public JsonResult GetIndicator(int cateId)
        {
            DimIndicatorDAO ddDao = new DimIndicatorDAO();
            var result = ddDao.GetByPage("ICateID=" + cateId, 0, int.MaxValue);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
