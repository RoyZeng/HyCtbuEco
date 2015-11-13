
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using System.Net;
using System.Web;


using System.Net.Http;




using TMS.Common;

using System.Net.Http;
using HyCtbuEco.Services;
using HyCtbuEco.Entities;
using HyCtbuEco.Admin;
using HyCtbuEco.Models;


namespace WebSite.Areas.WebApi.Controllers
{

     [UserAuthorizeSys]
  public class SqlQueryController : BaseController
   {
       
        #region gen code
		
		
		protected SqlQueryDAO SqlQueryDAO = new SqlQueryDAO();
		protected int totalCount = 0;
		
		 /// <summary>
		 /// GET api/SqlQuery/Get
		 ///返回所有的表格数据，json或xml格式
		 /// </summary>
        public JsonResult GetAll()
        {
			IList<TbSqlQuery> SqlQueryList =  SqlQueryDAO.GetAll();  
			return Json(SqlQueryList, JsonRequestBehavior.AllowGet);
           
        }

		/// <summary>
        /// GET api/SqlQuery/Get/id
		///返回指定id的数据
		/// </summary>
		/// <param name="id">id,主键</param>
        public JsonResult Get(int id)
        {
            TbSqlQuery item = SqlQueryDAO.GetByID(id);
           

            return Json(item,JsonRequestBehavior.AllowGet);
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
            IList<TbSqlQuery> userList= SqlQueryDAO.GetByPageDataBase(firstResult, pagesize, orderBy, condition, out totalCount);
            return Json(new
            {
                DataCount = totalCount,
                Data = userList
            },JsonRequestBehavior.AllowGet);
        }

		
		
		
		

		
		/// <summary>
        /// PUT api/SqlQuery/Put/5,更新数据
		/// </summary>
        public int  Put(int id, TbSqlQuery item)
        {
			

             if (id != item.Id)
            {
                //数据无效
                return -1;
            }

            try
            {

                item.DCreate = DateTime.Now;//更新时间
                item.ICreateID = SysUserInfo.GetUserID();



                SqlQueryDAO.Update(item);
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
        /// DELETE api/SqlQuery/Delete/5
        /// </summary>
        public int Delete(int id)
        {

            try
            {
                SqlQueryDAO.Del(id);
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
        /// DELETE api/SqlQuery/Delete/?ids="1,2,3,4"
		/// </summary>
        public int  DeleteIds(string  ids)
        {
			try	{
				SqlQueryDAO.Del(ids);
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
        /// POST api/SqlQuery/Post
        ///新增item数据，
        /// </summary>
        /// <param name="item">item,新增数据</param>
        public int Post(TbSqlQuery item)
        {

            item.DCreate = DateTime.Now;
            item.ICreateID = SysUserInfo.GetUserID();


            SqlQueryDAO.Insert(item);



            return item.Id;//新增数据成功



        }

		
		

        
		#endregion self define code
   }

}