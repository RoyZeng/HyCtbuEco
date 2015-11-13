using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;
using System.Web;
using System.Net.Http;
using HyCtbuEco.Dim;
using HyCtbuEco.Entities;


namespace WebSite.Areas.WebApi.Controllers
{
    public class DimLibController : BaseController
    {

        #region gen code


        protected DimLibDAO DimLibDAO = new DimLibDAO();
        protected int totalCount = 0;

        /// <summary>
        /// GET api/DimLib/Get
        ///返回所有的表格数据，json或xml格式
        /// </summary>
        public JsonResult GetAll()
        {
            IList<TbDimLib> DimLibList = DimLibDAO.GetAll();
            return Json(DimLibList, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// GET api/DimLib/Get/id
        ///返回指定id的数据
        /// </summary>
        /// <param name="id">id,主键</param>
        public JsonResult Get(int id)
        {
            TbDimLib item = DimLibDAO.GetByID(id);


            return Json(item, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 分页查询得到数据信息列表
        /// </summary>
        /// <param name="firstResult">查询的页数，0开始</param>
        /// <param name="pagesize">每一页的记录条数</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public JsonResult GetByPage(int firstResult, int pagesize,
            string orderBy, string condition)
        {
            int totalCount = 0;  //总的数据记录条数
            condition = HttpUtility.UrlDecode(condition);
            IList<TbDimLib> userList = DimLibDAO.GetByPageDataBase(firstResult, pagesize, orderBy, condition, out totalCount);
            return Json(new
            {
                DataCount = totalCount,
                Data = userList
            }, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// POST api/DimLib/Post
        ///新增item数据，
        /// </summary>
        /// <param name="item">item,新增数据</param>
        public int Post(TbDimLib item)
        {

            DimLibDAO.Insert(item);



            return item.Id;//新增数据成功



        }


        /// <summary>
        /// PUT api/DimLib/Put/5,更新数据
        /// </summary>
        public int Put(int id, TbDimLib item)
        {


            if (id != item.Id)
            {
                //数据无效
                return -1;
            }

            try
            {
                DimLibDAO.Update(item);
            }
            catch (Exception ex)
            {
                //更新数据失败
                return -2;
            }

            //更新成功
            return 1;


        }



        /// <summary>
        /// DELETE api/DimLib/Delete/5
        /// </summary>
        public int Delete(int id)
        {

            try
            {
                DimLibDAO.Del(id);
            }
            catch (Exception ex)
            {
                //删除数据时失败
                return -2;
            }
            //删除成功
            return 1;


        }




        /// <summary>
        /// DELETE api/DimLib/Deleteids="1,2,3,4"
        /// </summary>
        public int DeleteIds(string ids)
        {
            try
            {

                String[] tableId = ids.Split(new char[] { ',' });
                foreach (var item in tableId)
                {
                    String tableName = DimLibDAO.GetByID(Int32.Parse(item)).STableName;
                    DimLibDAO.Del(item);
                    //删除表
                    DimLibDAO.DeleteTable(tableName);

                }
            }
            catch (Exception ex)
            {
                //批量删除是出错
                return -2;
            }
            //批量删除成功
            return 1;
        }




        #endregion gen code

        #region self define code



        /// <summary>
        /// 验证是否存在同名库
        /// </summary>
        /// <param name="libName"></param>
        /// <returns></returns>
        public JsonResult ValidateLibName(string libName)
        {
           IList<TbDimLib> DimlibList = DimLibDAO.findBySLibName(libName);
           if (DimlibList.Count > 0)
           {
               return Json(1, JsonRequestBehavior.AllowGet);
           }
           else {
               return Json(0, JsonRequestBehavior.AllowGet);
           }
        
        }



        /// <summary>
        /// 验证表名是否唯一
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public JsonResult ValidateTableName(string tableName) {

            IList<TbDimLib> DimLibList = DimLibDAO.findBySTableName(tableName);
            if (DimLibList.Count > 0)
            {

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(0, JsonRequestBehavior.AllowGet);
            }


        }


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Insert(TbDimLib item)
        {

            DimLibDAO.Insert(item);

            string tableName = item.STableName;
            //创建表
            DimLibDAO.CreateTable(tableName);
            return item.Id;//新增数据成功

        }



        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Update(int id, TbDimLib item)
        {


            if (id != item.Id)
            {
                //数据无效
                return -1;
            }

            try
            {
                //获取更新前的表名

                string tableName = DimLibDAO.GetByID(id).STableName;
                //如果两次的表名不相同
                if (tableName != item.STableName) {

                    DimLibDAO.UpdateTableName(tableName, item.STableName);

                }

                DimLibDAO.Update(item);
            }
            catch (Exception ex)
            {
                //更新数据失败
                return -2;
            }

            //更新成功
            return 1;


        }





        #endregion self define code
    }

}