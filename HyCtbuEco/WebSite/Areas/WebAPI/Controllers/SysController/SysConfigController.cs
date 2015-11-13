
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
  public class SysConfigController : BaseController
   {
       
        #region gen code
		
		
		protected SysConfigDAO _SysConfigDAO = new SysConfigDAO();
		protected int totalCount = 0;
		
		 /// <summary>
		 /// GET api/SysConfig/Get
		 ///返回所有的表格数据，json或xml格式
		 /// </summary>
        public JsonResult GetAll()
        {
			IList<TbSysConfig> SysConfigList =  _SysConfigDAO.GetAll();  
			return Json(SysConfigList, JsonRequestBehavior.AllowGet);
           
        }

		/// <summary>
        /// GET api/SysConfig/Get/id
		///返回指定id的数据
		/// </summary>
		/// <param name="id">id,主键</param>
        public JsonResult Get(int id)
        {
            TbSysConfig item = _SysConfigDAO.GetByID(id);
           

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
            IList<TbSysConfig> userList= _SysConfigDAO.GetByPageDataBase(firstResult, pagesize, orderBy, condition, out totalCount);
            return Json(new
            {
                DataCount = totalCount,
                Data = userList
            },JsonRequestBehavior.AllowGet);
        }

		
		
		
		/// <summary>
        /// POST api/SysConfig/Post
		///新增item数据，
		/// </summary>
		/// <param name="item">item,新增数据</param>
        public int  Post(TbSysConfig item)
        {
			
               
                _SysConfigDAO.Insert(item);

                
                
                return item.Id;//新增数据成功
            


        }

		
		/// <summary>
        /// PUT api/SysConfig/Put/5,更新数据
		/// </summary>
        public int  Put(int id, TbSysConfig item)
        {
			

             if (id != item.Id)
            {
                //数据无效
                return -1;
            }

            try
            {
                _SysConfigDAO.Update(item);
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
        /// DELETE api/SysConfig/Delete/5
        /// </summary>
        public int Delete(int id)
        {

            try
            {
                _SysConfigDAO.Del(id);
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
        /// DELETE api/SysConfig/Delete/?ids="1,2,3,4"
		/// </summary>
        public int  DeleteIds(string  ids)
        {
			try	{
				_SysConfigDAO.Del(ids);
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
		
		
		
		#endregion self define code
   }

}