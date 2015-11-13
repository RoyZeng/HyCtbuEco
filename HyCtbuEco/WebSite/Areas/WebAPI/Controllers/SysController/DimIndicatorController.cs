
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
    public class DimIndicatorController : BaseController
    {

        #region gen code


        protected DimIndicatorDAO DimIndicatorDAO = new DimIndicatorDAO();
        protected int totalCount = 0;

        /// <summary>
        /// GET api/DimIndicator/Get
        ///返回所有的表格数据，json或xml格式
        /// </summary>
        public JsonResult GetAll()
        {
            IList<TbDimIndicator> DimIndicatorList = DimIndicatorDAO.GetAll();
            return Json(DimIndicatorList, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// GET api/DimIndicator/Get/id
        ///返回指定id的数据
        /// </summary>
        /// <param name="id">id,主键</param>
        public JsonResult Get(int id)
        {
            TbDimIndicator item = DimIndicatorDAO.GetByID(id);


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
            IList<TbDimIndicator> userList = DimIndicatorDAO.GetByPageDataBase(firstResult, pagesize, orderBy, condition, out totalCount);
            return Json(new
            {
                DataCount = totalCount,
                Data = userList
            }, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// POST api/DimIndicator/Post
        ///新增item数据，
        /// </summary>
        /// <param name="item">item,新增数据</param>
        public int Post(TbDimIndicator item)
        {

            DimIndicatorDAO.Insert(item);



            return item.Id;//新增数据成功



        }


        /// <summary>
        /// PUT api/DimIndicator/Put/5,更新数据
        /// </summary>
        public int Put(int id, TbDimIndicator item)
        {


            if (id != item.Id)
            {
                //数据无效
                return -1;
            }

            try
            {
                DimIndicatorDAO.Update(item);
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
        /// DELETE api/DimIndicator/Delete/5
        /// </summary>
        public int Delete(int id)
        {

            try
            {
                DimIndicatorDAO.Del(id);
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
        /// DELETE api/DimIndicator/Delete/?ids="1,2,3,4"
        /// </summary>
        public int DeleteIds(string ids)
        {
            try
            {
                DimIndicatorDAO.Del(ids);
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


        protected DimIndCateDAO _DimIndCateDao = new DimIndCateDAO();


        protected DimLibDAO _DimLibDao = new DimLibDAO();



        /// <summary>
        /// 验证指标名是否唯一
        /// </summary>
        /// <param name="SIndName">指标名称</param>
        /// <param name="ICateId">指标分类Id</param>
        /// <param name="LibId">库Id</param>
        /// <returns></returns>
        public JsonResult ValidateIndName(string SIndName, int ICateId, int LibId)
        {
            int flag = -1;
            string whereClause = "SIndName='" + SIndName + "' and ICateID=" + ICateId + "and ILibID=" + LibId;
            IList<TbDimIndicator> itemList = DimIndicatorDAO.GetByPage(whereClause, 0, int.MaxValue);
            if (itemList.Count > 0)
            {
                flag = 1;

            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }





        /// <summary>
        /// 
        ///新增数据
        /// </summary>
        /// <param name="item">item,新增数据</param>
        public int NewPost(TbDimIndicator item)
        {

            //获取指标类型名称
            string cateName = _DimIndCateDao.GetByID(item.ICateID).SCateName;

            //获取库名
            string libName = _DimLibDao.GetByID(item.ILibID).SLibName;

            item.SCateName = cateName;
            item.SLibName = libName;


            DimIndicatorDAO.Insert(item);



            return item.Id;//新增数据成功



        }





        #endregion self define code
    }

}