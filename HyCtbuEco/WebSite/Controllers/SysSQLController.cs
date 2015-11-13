using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HyCtbuEco.Entities;
using HyCtbuEco.Admin;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using TMS.EnterpriseLibrary;
using HyCtbuEco.Services;

namespace HyCtbuEco2015.Controllers
{
     [UserAuthorizeSys]
    public class SysSQLController : Controller
    {
        private SqlQueryDAO _sqDao = new SqlQueryDAO();

        //
        // GET: /SysSQL/

        public ActionResult SQLManager()
        {
            return View();
        }


        public ActionResult SQLQuery()
        {
            IList<TbSqlQuery> sqlList = _sqDao.GetAll();

            ViewBag.sqlList = sqlList;
            return View();
        }

        

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DataImport()
        {


            string[] tablesNameArray = new string[6]; //表名数组
            tablesNameArray[0] = "商品分类";
            //tablesNameArray[1] = "学院课程信息表";


            List<object> TableNameList = new List<object>();

            //(1)读取所有当前数据库中的表名
            try
            {
                Database db = DBHelper.CreateDataBase("PlatformData");


                DbCommand cmd = db.GetSqlStringCommand("select name from dbo.sysobjects Where XType='U'  ORDER BY Name");

                using (DataSet myDs = db.ExecuteDataSet(cmd))
                {
                    DataTable tmpDT = myDs.Tables[0];
                    for (int i = 0; i < tmpDT.Rows.Count; i++)
                    {
                        tableName temptableName = new tableName();
                        string temp = ((DataRow)tmpDT.Rows[i])[0].ToString();

                        temptableName.tableTrueName = temp;
                        temptableName.tableChineseName = temp;// tablesNameArray[i];
                        TableNameList.Add(temptableName);
                    }

                }
               
            }
            catch (Exception e2)
            {

                return null;

            }


            ViewBag.tableList = TableNameList;
            return View();
        }
    }
}
