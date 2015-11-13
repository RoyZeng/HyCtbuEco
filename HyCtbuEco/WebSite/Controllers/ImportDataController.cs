using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.EnterpriseLibrary;
using HyCtbuEco.Entities;
using HyCtbuEco.Dim;

namespace HyCtbuEco2015.Controllers
{
    public class ImportDataController : Controller
    {
        //
        // GET: /ImportData/

        public ActionResult Index()
        {

            List<object> TableNameList = new List<object>();

            //(1)读取所有当前数据库中的表名
            try
            {
                Database db = DBHelper.CreateDataBase("PlatformData");


                DbCommand cmd = db.GetSqlStringCommand("select * from dbo.tb_DimLib");

                using (DataSet myDs = db.ExecuteDataSet(cmd))
                {
                    DataTable tmpDT = myDs.Tables[0];
                    for (int i = 0; i < tmpDT.Rows.Count; i++)
                    {
                        tableName temptableName = new tableName();
                        string tableChineseName = ((DataRow)tmpDT.Rows[i])[1].ToString();
                        string tableTrueName = ((DataRow)tmpDT.Rows[i])[2].ToString();
                        temptableName.tableTrueName = tableTrueName;
                        temptableName.tableChineseName = tableChineseName;
                        TableNameList.Add(temptableName);
                    }

                }

            }
            catch (Exception e2)
            {

                return null;

            }

            ViewBag.city = new DimAreaDAO().GetAll();
            ViewBag.tableList = TableNameList;
            return View();
        }

    }
}
