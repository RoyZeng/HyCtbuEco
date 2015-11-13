using HyCtbuEco.Dim;
using HyCtbuEco.Entities;
using HyCtbuEco.Models;
using HyCtbuEco.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Common;
using TMS.EnterpriseLibrary;

namespace WebSite.Areas.WebApi.Controllers
{
    public class SysAllDataController : BaseController
    {



        private DimIndicatorDAO _DimIndicatorDao = new DimIndicatorDAO();


        private DimLibDAO _DimLibDao = new DimLibDAO();


        private DimTimeDAO _DimTimeDao = new DimTimeDAO();


        private DimAreaDAO _DimAreaDao = new DimAreaDAO();



        /// <summary>
        /// 获取指点分类指标Id下的所有指标
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        public JsonResult GetIndicatorByCateId(int cateId)
        {


            IList<TbDimIndicator> IndicatorList = _DimIndicatorDao.findByICateID(cateId);
            return Json(IndicatorList, JsonRequestBehavior.AllowGet);



        }




        /// <summary>
        /// 条件查询，获取对应的数据
        /// </summary>
        /// <param name="libId">库ID</param>
        /// <param name="indiactorArray">指标参数ID数组</param>
        /// <param name="areaArray">地区参数ID数组</param>
        /// <param name="timeArray">时间参数ID数组</param>
        /// <returns></returns>
        public JsonResult QueryData(int libId, int[] indiactorArray, int[] areaArray, int[] timeArray)
        {

            SysUserInfo.SetIndiactorArray(indiactorArray);
            SysUserInfo.SetAreaArray(areaArray);
            SysUserInfo.SetTimeArray(timeArray);

            //返回值数据集合
            List<List<FacHGYDKExt>> resultList = new List<List<FacHGYDKExt>>();

            //1、根据库ID获取对应的表
            String tblaeName = _DimLibDao.GetByID(libId).STableName;
            FacHGYDKBaseDAO _FacHGYDKDao = new FacHGYDKBaseDAO(tblaeName);






            //根据表名确定对应的数据库名

            string whereClause = "";

            //进行分指标筛选


            //2.1  以时间参数作为纵轴进行查询

            for (int i = 0; i < timeArray.Length; i++)
            {
                List<FacHGYDKExt> itemList = new List<FacHGYDKExt>();

                //2.1.1  以指标参数作为第一横轴

                for (int j = 0; j < indiactorArray.Length; j++)
                {


                    //2.1.1.1  以地区参数作为第二横轴
                    for (int k = 0; k < areaArray.Length; k++)
                    {
                        FacHGYDKExt tempItem = new FacHGYDKExt();

                        whereClause = "ITimeID=" + timeArray[i] + "and  IIndID=" + indiactorArray[j] + " and IAreaID=" + areaArray[k];
                        IList<TbFacHGYDK> tempList = _FacHGYDKDao.GetByPage(whereClause, 0, int.MaxValue);
                        //说明存在
                        if (tempList.Count > 0)
                        {
                            ClassValueCopier.Copy(tempItem, tempList[0]);
                            tempItem.STimeName = getTimeName(tempItem.ITimeID);  //获取时间参数名
                            tempItem.SIndName = getIndicatorName(tempItem.IIndID);  //获取指标参数名
                            tempItem.SAreaName = getAreaName(tempItem.IAreaID); //获取地区参数名
                            itemList.Add(tempItem);
                        }
                        //如果不存在，添加空对象
                        else
                        {
                            tempItem.STimeName = getTimeName(timeArray[i]);  //获取时间参数名
                            tempItem.SIndName = getIndicatorName(indiactorArray[j]);  //获取指标参数名
                            tempItem.SAreaName = getAreaName(areaArray[k]); //获取地区参数名
                            itemList.Add(tempItem);
                        }
                    }


                }

                resultList.Add(itemList);

            }



            //3.1处理纵轴时间问题

            List<String> timeNameList = new List<string>();


            //时间参数名
            string DimTimeName = "";

            for (int i = 0; i < timeArray.Length; i++)
            {


                //获取时间参数名
                DimTimeName = _DimTimeDao.GetByID(timeArray[i]).SName;
                timeNameList.Add(DimTimeName);
            }



            //4.1  处理第一横轴指标参数名

            List<String> IndicatorNameList = new List<string>();

            //指标参数名

            string indicatorName = "";

            for (int i = 0; i < indiactorArray.Length; i++)
            {

                //对指标参数名做特殊处理:如果存在单位，SIndName=SIndName(SUnit);
                //如果不存在单位则不加

                TbDimIndicator tempDimInd = _DimIndicatorDao.GetByID(indiactorArray[i]);

                string unitName = tempDimInd.SUnit;
                if (!string.IsNullOrEmpty(unitName))
                {
                    indicatorName = tempDimInd.SIndName + "(" + unitName + ")";  //获取指标参数名
                }

                else
                {
                    indicatorName = tempDimInd.SIndName;
                }

                IndicatorNameList.Add(indicatorName);


            }


            //5.1  处理第二横轴地区参数名

            List<String> AreaNameList = new List<string>();

            //地区参数名

            string areaName = "";

            for (int i = 0; i < areaArray.Length; i++)
            {
                areaName = _DimAreaDao.GetByID(areaArray[i]).SAreaName;

                AreaNameList.Add(areaName);

            }



            return Json(new
            {
                timeNameList = timeNameList,//时间参数名
                IndicatorNameList = IndicatorNameList,  //指标参数名
                AreaNameList = AreaNameList,  //地区参数名
                dataList = resultList   //结果
            }, JsonRequestBehavior.AllowGet);


        }






        /// <summary>
        /// 条件查询，获取对应的数据
        /// </summary>
        /// <param name="libId">库ID</param>
        /// <param name="indiactorArray">指标参数ID数组</param>
        /// <param name="areaArray">地区参数ID数组</param>
        /// <param name="timeArray">时间参数ID数组</param>
        /// <returns></returns>
        public JsonResult QueryDataFormat(int libId, int[] indiactorArray, int[] areaArray, int[] timeArray)
        {



            SysUserInfo.SetIndiactorArray(indiactorArray);
            SysUserInfo.SetAreaArray(areaArray);
            SysUserInfo.SetTimeArray(timeArray);

            //返回值数据集合
            List<List<FacHGYDKExt>> resultList = new List<List<FacHGYDKExt>>();

            //1、根据库ID获取对应的表
            String tblaeName = _DimLibDao.GetByID(libId).STableName;
            FacHGYDKBaseDAO _FacHGYDKDao = new FacHGYDKBaseDAO(tblaeName);






            //根据表名确定对应的数据库名

            string whereClause = "";

            //进行分指标筛选


            //2.1  以指标参数作为纵轴进行查询

            for (int i = 0; i < indiactorArray.Length; i++)
            {


                //2.1.1  以地区参数作为第一横轴

                for (int j = 0; j < areaArray.Length; j++)
                {
                    List<FacHGYDKExt> itemList = new List<FacHGYDKExt>();

                    //2.1.1.1  以时间参数作为第二横轴
                    for (int k = 0; k < timeArray.Length; k++)
                    {
                        FacHGYDKExt tempItem = new FacHGYDKExt();

                        whereClause = "ITimeID=" + timeArray[k] + "and  IIndID=" + indiactorArray[i] + " and IAreaID=" + areaArray[j];
                        IList<TbFacHGYDK> tempList = _FacHGYDKDao.GetByPage(whereClause, 0, int.MaxValue);
                        //说明存在
                        if (tempList.Count > 0)
                        {
                            ClassValueCopier.Copy(tempItem, tempList[0]);
                            tempItem.STimeName = getTimeName(tempItem.ITimeID);  //获取时间参数名
                            
                            //对指标参数名做特殊处理:如果存在单位，SIndName=SIndName(SUnit);
                            //如果不存在单位则不加

                            TbDimIndicator tempDimInd = _DimIndicatorDao.GetByID(tempItem.IIndID);

                            string unitName = tempDimInd.SUnit;
                            if (!string.IsNullOrEmpty(unitName))
                            {
                                tempItem.SIndName = tempDimInd.SIndName + "(" + unitName + ")";  //获取指标参数名
                            }
                            else
                            {
                                tempItem.SIndName = tempDimInd.SIndName;
                            }
                            tempItem.SAreaName = getAreaName(tempItem.IAreaID); //获取地区参数名
                            itemList.Add(tempItem);
                        }
                        //如果不存在
                        else
                        {

                            tempItem.STimeName = getTimeName(timeArray[k]);  //获取时间参数名
                            //对指标参数名做特殊处理:如果存在单位，SIndName=SIndName(SUnit);
                            //如果不存在单位则不加

                            TbDimIndicator tempDimInd = _DimIndicatorDao.GetByID(indiactorArray[i]);

                            string unitName = tempDimInd.SUnit;
                            if (!string.IsNullOrEmpty(unitName))
                            {
                                tempItem.SIndName = tempDimInd.SIndName + "(" + unitName + ")";  //获取指标参数名
                            }
                            else
                            {
                                tempItem.SIndName = tempDimInd.SIndName;
                            }
                            tempItem.SAreaName = getAreaName(areaArray[j]); //获取地区参数名
                            itemList.Add(tempItem);
                        }
                    }

                    resultList.Add(itemList);
                }





            }



            //3.1处理纵轴时间问题

            List<String> timeNameList = new List<string>();


            //时间参数名
            string DimTimeName = "";

            for (int i = 0; i < timeArray.Length; i++)
            {


                //获取时间参数名
                DimTimeName = _DimTimeDao.GetByID(timeArray[i]).SName;
                timeNameList.Add(DimTimeName);
            }







            return Json(new
            {
                timeNameList = timeNameList,//时间参数名
                dataList = resultList   //结果
            }, JsonRequestBehavior.AllowGet);


        }



        /// <summary>
        /// 根据时间参数ID获取名称
        /// </summary>
        /// <param name="ITimeID"></param>
        /// <returns></returns>
        public string getTimeName(int ITimeID)
        {

            return _DimTimeDao.GetByID(ITimeID).SName;
        }




        /// <summary>
        /// 根据指标参数ID获取指标参数名
        /// </summary>
        /// <param name="IIndID"></param>
        /// <returns></returns>
        public string getIndicatorName(int IIndID)
        {

            return _DimIndicatorDao.GetByID(IIndID).SIndName;
        }



        /// <summary>
        /// 根据地区参数ID获取地区名
        /// </summary>
        /// <param name="IAreaID"></param>
        /// <returns></returns>
        public string getAreaName(int IAreaID)
        {

            return _DimAreaDao.GetByID(IAreaID).SAreaName;
        }




        /// <summary>
        /// 导出到Excel表
        /// </summary>
        /// <param name="libId">库ID</param>
        /// <param name="indiactorArray">指标参数ID数组</param>
        /// <param name="areaArray">地区参数ID数组</param>
        /// <param name="timeArray">时间参数ID数组</param>
        /// <returns></returns>
        [HttpPost]
        public void PostDataToExcel(int libId)
        {


            int[] indiactorArray = SysUserInfo.GetIndiactorArray();

            int[] areaArray = SysUserInfo.GetAreaArray();
            int[] timeArray = SysUserInfo.GetTimeArray();

            //返回值数据集合
            List<List<FacHGYDKExt>> resultList = new List<List<FacHGYDKExt>>();

            //1、根据库ID获取对应的表
            TbDimLib dimLib = _DimLibDao.GetByID(libId);
            string tblaeName = dimLib.STableName;

            //获取库名称

            string libName = dimLib.SLibName;

            FacHGYDKBaseDAO _FacHGYDKDao = new FacHGYDKBaseDAO(tblaeName);






            //根据表名确定对应的数据库名

            string whereClause = "";

            //进行分指标筛选


            //2.1  以时间参数作为纵轴进行查询

            for (int i = 0; i < timeArray.Length; i++)
            {
                List<FacHGYDKExt> itemList = new List<FacHGYDKExt>();

                //2.1.1  以指标参数作为第一横轴

                for (int j = 0; j < indiactorArray.Length; j++)
                {


                    //2.1.1.1  以地区参数作为第二横轴
                    for (int k = 0; k < areaArray.Length; k++)
                    {
                        FacHGYDKExt tempItem = new FacHGYDKExt();

                        whereClause = "ITimeID=" + timeArray[i] + "and  IIndID=" + indiactorArray[j] + " and IAreaID=" + areaArray[k];
                        IList<TbFacHGYDK> tempList = _FacHGYDKDao.GetByPage(whereClause, 0, int.MaxValue);
                        //说明存在
                        if (tempList.Count > 0)
                        {
                            ClassValueCopier.Copy(tempItem, tempList[0]);
                            tempItem.STimeName = getTimeName(tempItem.ITimeID);  //获取时间参数名
                            tempItem.SIndName = getIndicatorName(tempItem.IIndID);  //获取指标参数名
                            tempItem.SAreaName = getAreaName(tempItem.IAreaID); //获取地区参数名
                            itemList.Add(tempItem);
                        }
                    }


                }

                resultList.Add(itemList);

            }



            //3.1处理纵轴时间问题

            List<String> timeNameList = new List<string>();


            //时间参数名
            string DimTimeName = "";

            for (int i = 0; i < timeArray.Length; i++)
            {


                //获取时间参数名
                DimTimeName = _DimTimeDao.GetByID(timeArray[i]).SName;
                timeNameList.Add(DimTimeName);
            }



            //4.1  处理第一横轴指标参数名

            List<String> IndicatorNameList = new List<string>();

            //指标参数名

            string indicatorName = "";

            for (int i = 0; i < indiactorArray.Length; i++)
            {

                //获取指标参数名
                indicatorName = _DimIndicatorDao.GetByID(indiactorArray[i]).SIndName;

                IndicatorNameList.Add(indicatorName);


            }


            //5.1  处理第二横轴地区参数名

            List<String> AreaNameList = new List<string>();

            //地区参数名

            string areaName = "";

            for (int i = 0; i < areaArray.Length; i++)
            {
                areaName = _DimAreaDao.GetByID(areaArray[i]).SAreaName;

                AreaNameList.Add(areaName);

            }



            //创建自定义DataTable,并设置表头信息


            DataTable dt = new DataTable();

            DataColumn dc = null;

            //赋值给 dc，是便于对每一个datacolumn的操作
            dc = dt.Columns.Add("时间/(指标、地区)", Type.GetType("System.String"));

            dc.AllowDBNull = true;//

            for (int i = 0; i < AreaNameList.Count; i++)
            {
                for (int j = 0; j < IndicatorNameList.Count; j++)
                {

                    dc = dt.Columns.Add(AreaNameList[i] + "(" + IndicatorNameList[j] + ")", Type.GetType("System.String"));

                }

            }



            //由于DataTable不支持复合表头，因此更改样式为"地区参数名(指标名)"

            //添加表格体内容,填充数据
            for (int j = 0; j < resultList.Count; j++)
            {
                int count = 1;
                DataRow newRow;
                newRow = dt.NewRow();
                newRow[0] = timeNameList[j]; //时间参数名
                for (int k = 0; k < resultList[j].Count; k++)
                {
                    newRow[count] = resultList[j][k].FValue;
                    count++;
                }
                dt.Rows.Add(newRow);
            }




            //导出到excel

            ExcelHelper.ExportDataTableToExcel(dt, libName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls", libName);


        }






        /// <summary>
        /// 查询需要在作图页面上显示的数据
        /// </summary>
        /// <param name="libId">库Id</param>
        /// <param name="indiactorArray">已选择查看的指标</param>
        /// <param name="areaId">已选地区的Id</param>
        /// <returns></returns>
        public JsonResult SearchData(int libId, int[] indiactorArray, int areaId)
        {


            //返回值数据集合
            List<List<FacHGYDKExt>> resultList = new List<List<FacHGYDKExt>>();

            //1、根据库ID获取对应的表
            String tblaeName = _DimLibDao.GetByID(libId).STableName;
            FacHGYDKBaseDAO _FacHGYDKDao = new FacHGYDKBaseDAO(tblaeName);






            //根据表名确定对应的数据库名

            string whereClause = "";


            //根据前台需要，选择了多少个指标，就有多少条折线,因此根据指标来进行查询

            int[] timeArray = SysUserInfo.GetTimeArray();


            List<string> timeNameList = new List<string>();

            for (int i = 0; i < timeArray.Length; i++)
            {
                timeNameList.Add(_DimTimeDao.GetByID(timeArray[i]).SName);
            }


            for (int i = 0; i < indiactorArray.Length; i++)
            {
                List<FacHGYDKExt> itemList = new List<FacHGYDKExt>();

                for (int k = 0; k < timeArray.Length; k++)
                {

                    whereClause = "ITimeID=" + timeArray[k] + "and  IIndID=" + indiactorArray[i] + " and IAreaID=" + areaId;
                    IList<TbFacHGYDK> tempList = _FacHGYDKDao.GetByPage(whereClause, 0, int.MaxValue);
                    //说明存在
                    if (tempList.Count > 0)
                    {
                        FacHGYDKExt temp = new FacHGYDKExt();
                        ClassValueCopier.Copy(temp, tempList[0]);
                        //对指标参数名做特殊处理:如果存在单位，SIndName=SIndName(SUnit);
                        //如果不存在单位则不加

                        TbDimIndicator tempDimInd = _DimIndicatorDao.GetByID(temp.IIndID);

                        string unitName = tempDimInd.SUnit;
                        if (!string.IsNullOrEmpty(unitName))
                        {
                            temp.SIndName = tempDimInd.SIndName + "(" + unitName + ")";  //获取指标参数名
                        }
                        else
                        {
                            temp.SIndName = tempDimInd.SIndName;
                        }
                        itemList.Add(temp);
                    }
                    else 
                    {
                        FacHGYDKExt temp = new FacHGYDKExt();
                        temp.FValue = 0;
                        temp.SAreaName = _DimAreaDao.GetByID(areaId).SAreaName;
                        //对指标参数名做特殊处理:如果存在单位，SIndName=SIndName(SUnit);
                        //如果不存在单位则不加

                        TbDimIndicator tempDimInd = _DimIndicatorDao.GetByID(indiactorArray[i]);

                        string unitName = tempDimInd.SUnit;
                        if (!string.IsNullOrEmpty(unitName))
                        {
                            temp.SIndName = tempDimInd.SIndName + "(" + unitName + ")";  //获取指标参数名
                        }
                        else
                        {
                            temp.SIndName = tempDimInd.SIndName;
                        }
                        temp.STimeName = _DimTimeDao.GetByID(timeArray[k]).SName;
                        itemList.Add(temp);
                    }

                }
                resultList.Add(itemList);

            }


            return Json(new
            {
                timeNameList = timeNameList,
                dataList = resultList
            }, JsonRequestBehavior.AllowGet);


        }




        /// <summary>
        /// 清空GuestUserInfo类中保存的session
        /// </summary>
        public void ClearSession()
        {

            SysUserInfo.ClearSession();
        }



        /// <summary>
        /// 保存参数到session中
        /// </summary>
        /// <param name="indiactorArray">指标参数数组</param>
        /// <param name="areaArray">地区参数数组</param>
        /// <param name="timeArray">时间参数数组</param>
        /// <returns></returns>
        public JsonResult SaveParamToSession(int[] indiactorArray, int[] areaArray, int[] timeArray)
        {

            SysUserInfo.SetAreaArray(areaArray);
            SysUserInfo.SetIndiactorArray(indiactorArray);
            SysUserInfo.SetTimeArray(timeArray);

            return Json(1, JsonRequestBehavior.AllowGet);
        }



    }
}
