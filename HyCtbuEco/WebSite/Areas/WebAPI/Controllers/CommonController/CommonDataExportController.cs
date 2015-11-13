using HyCtbuEco.Admin;
using HyCtbuEco.Entities;
using HyCtbuEco.Models;
using HyCtbuEco.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.EnterpriseLibrary;

namespace WebSite.Areas.WebApi.Controllers
{
     [UserAuthorizeSys]
    public class CommonDataExportController : Controller
    {


        private SqlQueryDAO SqlDao = new SqlQueryDAO();    //定义SqlQueryDAO的对象






        #region Search函数，查找数据库中存在的sql语句，管理SQL语句使用
        /// <summary>
        /// 查询所有的SQL语句
        /// </summary>
        /// <param name="firstResult">起始值</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="orderBy">排序列</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>

        public Object Search(int firstResult, int pagesize, string orderBy, string condition)
        {
            int totalCount = 0;  //总的数据记录条数I_NoticesType
            condition = HttpUtility.UrlDecode(condition);
            if (condition.Length > 3) //说明不是默认查找,而是带有参数的查找
            {
                string substringfront = condition.Substring(0, 4); //获取condition的前四个字符串
                int substringfrontlength = substringfront.Length;//获取子字串的长度
                int conLength = condition.Length; //获取字符串的长度
                string substringbehind = condition.Substring(5, conLength - substringfrontlength - 1);//获取condition的其余条件
                if (substringfront == "(Id") //判断是否是按id查询
                {
                    substringfront = "(Id"; //修正id
                    condition = substringfront + substringbehind; //重新组合查询条件
                }

            }
            List<NewSqlQuery> resultList = new List<NewSqlQuery>();
            IList<TbSqlQuery> QueryList = new SqlQueryDAO().GetByPageDataBase(firstResult, pagesize, orderBy, condition, out totalCount);
            if (QueryList.Count > 0)
            {
                foreach (var item in QueryList)
                {
                    NewSqlQuery tempNewSqlQuery = new NewSqlQuery();
                    int id = int.Parse(item.ICreateID.ToString());
                    TbSysUser tempUser = new SysUserDAO().GetByID(id);
                    tempNewSqlQuery.Id = item.Id;
                    tempNewSqlQuery.SSqlName = item.SSqlName;
                    tempNewSqlQuery.SSqlStr = item.SSqlStr;
                    tempNewSqlQuery.ICreateID = item.ICreateID;
                    tempNewSqlQuery.SUserName = tempUser.SUserName;
                    tempNewSqlQuery.DCreate = item.DCreate;
                    tempNewSqlQuery.ISort = item.ISort;
                    resultList.Add(tempNewSqlQuery);
                }
            }
            return this.Json(new
            {
                DataCount = totalCount,
                Data = resultList
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion





        #region 获取所有可用的SQL语句
        /// <summary>
        /// 获取所有的SQL语句
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSQLName()
        {

            List<TbSqlQuery> SQLList = new List<TbSqlQuery>();
            // TbSqlQuery tempQuery = new TbSqlQuery(); //定义一个表对象
            try
            {
                Database db = DBHelper.CreateDataBase("PlatformData");

                DbCommand cmd = db.GetSqlStringCommand("select ID, SSqlName  from tb_SqlQuery");

                using (DataSet myDs = db.ExecuteDataSet(cmd))
                {
                    DataTable tmpDT = myDs.Tables[0];
                    for (int i = 0; i < tmpDT.Rows.Count; i++)
                    {
                        TbSqlQuery tempQuery = new TbSqlQuery(); //定义一个表对象
                        string tempID = tmpDT.Rows[i][0].ToString();  //获取SQL语句的id
                        int id = int.Parse(tempID);
                        string tempSQLName = tmpDT.Rows[i][1].ToString(); //获取SQL语句的名字
                        tempQuery.Id = id;
                        tempQuery.SSqlName = tempSQLName;
                        SQLList.Add(tempQuery);

                    }


                }
                return Json(SQLList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e2)
            {

                return null;

            }
        }
        #endregion





        #region 获取所选SQL语句的查询结果
        /// <summary>
        /// 获取所选SQL语句的查询结果
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetSelectedData(int id)
        {
            TbSqlQuery tempQuery = new SqlQueryDAO().GetByID(id); //查找该id下的数据

            List<object> resultList = new List<object>(); //定义一个object类型的集合
            if (tempQuery != null)
            {
                string mySQL = tempQuery.SSqlStr; //获取SQL语句
                Database db = DBHelper.CreateDataBase("PlatformData");//打开数据库
                DbCommand cmd = db.GetSqlStringCommand(mySQL);
                List<string> headerlist = new List<string>();  //定义表头对象集
                using (DataSet myDs = db.ExecuteDataSet(cmd))
                {
                    DataTable dt = myDs.Tables[0];
                    string tempheader;
                    for (int i = 0; i < dt.Columns.Count; i++)  //取表头
                    {
                        tempheader = dt.Columns[i].ToString();
                        headerlist.Add(tempheader);
                    }
                    int RowsCount = dt.Rows.Count; //获取表的总行数
                    int ColCount = dt.Columns.Count;//获取表的总列数
                    for (int i = 0; i < RowsCount; i++)//读取每一行的数据
                    {
                        List<string> tempList = new List<string>();//定义一个string类型的集合，保存每一行的数据
                        for (int j = 0; j < ColCount; j++) //读取每一列的数据
                        {
                            string temp = dt.Rows[i][j].ToString();
                            tempList.Add(temp);  //把每一列的数据加到tempList中
                        }
                        resultList.Add(tempList); //把每一行的数据加到resultList中
                    }

                }
                return Json(new { dataContent = resultList, dataHeader = headerlist }, JsonRequestBehavior.AllowGet);

            }
            else  //未找到
            {
                return null;
            }




        }
        #endregion




        #region 把所选的SQL语句获得结果导出到Excel表
        /// <summary>
        /// 把所选的SQL语句获得结果导出到Excel表
        /// </summary>
        /// <param name="firstResult"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderBy"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost]
        public void PostDataToExcel(int id)
        {
            try
            {
                TbSqlQuery tempQuery = new SqlQueryDAO().GetByID(id);
                if (tempQuery != null)
                {
                    string mySql = tempQuery.SSqlStr; //获取SQL语句


                    Database db = DBHelper.CreateDataBase("PlatformData");

                    DbCommand cmd = db.GetSqlStringCommand(mySql);
                    using (DataSet myDs = db.ExecuteDataSet(cmd))
                    {
                        //excel
                        ExcelHelper.ExportDataSetToExcel(myDs, tempQuery.SSqlName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls", tempQuery.SSqlName);

                    }
                }
            }
            catch (Exception e2)
            {

            }






        }
        #endregion

    }
}
