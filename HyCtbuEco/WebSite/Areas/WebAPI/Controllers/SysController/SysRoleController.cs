﻿using HyCtbuEco.Admin;
using TMS.Common;
using HyCtbuEco.Entities;
using HyCtbuEco.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace WebSite.Areas.WebApi.Controllers
{
    [UserAuthorizeSys]
   public class SysRoleController : BaseController
   {
       
        #region gen code
		
		
		protected SysRoleDAO SysRoleDAO = new SysRoleDAO();
		protected int totalCount = 0;
		
		 /// <summary>
		 /// GET api/SysRole/Get
		 ///返回所有的表格数据，json或xml格式
		 /// </summary>
        public JsonResult GetAll()
        {
			IList<TbSysRole> SysRoleList =  SysRoleDAO.GetAll();  
			return Json(SysRoleList, JsonRequestBehavior.AllowGet);
           
        }

		/// <summary>
        /// GET api/SysRole/Get/id
		///返回指定id的数据
		/// </summary>
		/// <param name="id">id,主键</param>
        public JsonResult Get(int id)
        {
            TbSysRole item = SysRoleDAO.GetByID(id);
           

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
            IList<TbSysRole> userList= SysRoleDAO.GetByPageDataBase(firstResult, pagesize, orderBy, condition, out totalCount);
            return Json(new
            {
                DataCount = totalCount,
                Data = userList
            });
        }

		
		
		
		/// <summary>
        /// POST api/SysRole/Post
		///新增item数据，
		/// </summary>
		/// <param name="item">item,新增数据</param>
        public int  Post(TbSysRole item)
        {
			 if (ModelState.IsValid)
            {
               
                SysRoleDAO.Insert(item);

                
                
                return 1;//新增数据成功
            }
           else
            {
                //数据验证失败
                return 0;
            }


        }

		
		/// <summary>
        /// PUT api/SysRole/Put/5,更新数据
		/// </summary>
        public int  Put(int id, TbSysRole item)
        {
			

             if (id != item.Id)
            {
                //数据无效
                return -1;
            }

            try
            {
                SysRoleDAO.Update(item);
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
        /// DELETE api/SysRole/Delete/5
        /// </summary>
        public int Delete(int id)
        {
			//tree
            IList<TbSysRole> chidrens = SysRoleDAO.findByIParentID(id);
            if (chidrens.Count > 0)
            {
                return -1;// "为安全，无法直接删除有子结点的树！！";
            }
            


            try
            {
               SysRoleDAO.Del(id);
            }
            catch (Exception ex)
            {
                //删除数据时失败
                return -2;
            }

            return 1;

        }

		
		
		
		/// <summary>
        /// DELETE api/SysRole/Delete/?ids="1,2,3,4"
		/// </summary>
        public int  DeleteIds(string  ids)
        {
			try	{
				SysRoleDAO.Del(ids);
			}
			 catch (Exception ex)
            {
                //批量删除是出错
                return -2;
            }
            //批量删除成功
            return 1;
        }
		
		
		
		
		
		
		/// <summary>
        /// return all tree
        /// </summary>
        /// <returns></returns>
        public  JsonResult TreeAll()
        {
            
          
            IList<TbSysRole> items = SysRoleDAO.GetByPageDataBase(0, int.MaxValue, "ILevel ASC,IParentID ASC,Id ASC", "1=1", out totalCount);//按ILevel升级，再按ParentID排序

            IList<zTreeNode> zNodes = new List<zTreeNode>();
			
		

            //写入根结点
            zTreeNode myNode = new zTreeNode();
            myNode.name = "根结点";
            myNode.open = true;

            myNode.id = 0;
            myNode.pId = -1;
            zNodes.Add(myNode);


            for (int i = 0; i < items.Count; i++)
            {
                myNode = new zTreeNode();
               
                myNode.name = items[i].SRoleName;
				myNode.open = false;

                myNode.id = items[i].Id;
				myNode.pId = (int)items[i].IParentId;
                myNode.tip = items[i].SRoleName;
                if (items[i].ILevel!= null && items[i].ILevel < 2) myNode.open = true;//自动扩展1,2级
                zNodes.Add( myNode);
                
            }
	
			
          
            return Json(zNodes,JsonRequestBehavior.AllowGet);

        }
		
     	
		
		/// <summary>
        /// return all tree,auto expend openLevel，seems，open  selecteid id with the parents
        /// </summary>
        /// <returns></returns>
        public JsonResult TreeWithSelected(int openLevel, int selectedID)
        {
           

             IList<TbSysRole> items = SysRoleDAO.GetByPageDataBase(0, int.MaxValue, "ILevel ASC,IParentID ASC,Id ASC", "1=1", out totalCount);//按ILevel升级，再按ParentID排序

            IList<zTreeNode> zNodes = new List<zTreeNode>();
			zTreeNode selectedNode=null;
			 //写入根结点
            zTreeNode myNode = new zTreeNode();
            myNode.name = "根结点";
            myNode.open = true;

            myNode.id = 0;
            myNode.pId = -1;
            zNodes.Add(myNode);
			
            for (int i = 0; i < items.Count; i++)
            {
                myNode = new zTreeNode();
               
                myNode.name = items[i].SRoleName;
				myNode.open = false;

                myNode.id = items[i].Id;
				myNode.pId = (int)items[i].IParentId;
                myNode.tip = items[i].SRoleName;
                if (items[i].ILevel!= null && items[i].ILevel < openLevel) myNode.open = true;//自动扩展openLevel级
                zNodes.Add( myNode);
                
				if(selectedID==myNode.id){
				selectedNode=myNode;//temp save node
				}
            }
	
			
            if (selectedNode != null)
            {
			
				for(int j = zNodes.Count-1; j >=0; j--)
                {
                   if(zNodes[j].id== selectedNode.pId){
						zNodes[j].open=true;
						selectedNode=zNodes[j];//point the next parent;
					}
                }
            }
            return Json(zNodes,JsonRequestBehavior.AllowGet);

        }
		
		
		
		
		/// <summary>
        /// DELETE api/SysRole/TreeDel?id=5
		/// </summary>
        public int  TreeDel(int id)
        {
			

            

            try
            {
               SysRoleDAO.DelTree(id);
            }
            catch (Exception ex)
            {
                return -1;//删除数据时失败
            }

            return 1;//删除成功

        }
		
		
		/// <summary>
       /// 新增树结点,新增后返回所有的树结点
       /// </summary>
       /// <param name="parentID">需要新增结点的父结点，可以是0(根结点)</param>
       /// <returns>返回所有树结点</returns>
       [HttpPost]
        public int AddTreeNode(int id)
        {

            //api/SysRole/AddTreeNode/0

			
			TbSysRole tmpNode = new TbSysRole();
            tmpNode.IParentId = id;//此处的id为parentid
            tmpNode.SRoleName = "新增结点";
            if (id != 0)
            {
                //非一级节点,先读取父结点
                TbSysRole parentNode = SysRoleDAO.GetByID(id);
                if (parentNode != null)
                {
                    tmpNode.ILevel = parentNode.ILevel + 1;
                }
                else
                {
                    return -1;
					//Request.CreateResponse(HttpStatusCode.BadRequest, "出错了，找不到新建 SysRole 的 父结点:"+id+"!!");
                }
            }
            else
            {
                //一级节点
                tmpNode.ILevel = 1;
            }
           

            SysRoleDAO.Insert(tmpNode);
			
			int tempId = tmpNode.Id;//新增节点的ID

            return tempId;
			
        }
		
		// <summary>
        /// 删除树节点
        /// </summary>
        public int DelTreeNode(int id)
        {
            int parentId = 0;  // parentId
			//tree
            IList<TbSysRole> chidrens = SysRoleDAO.findByIParentID(id);
            if (chidrens.Count > 0)
            {
                return -1;// "为安全，无法直接删除有子结点的树！！";
            }
            


            try
            {
                TbSysRole parent = SysRoleDAO.GetByID(id);
                parentId = parent.IParentId;
               SysRoleDAO.Del(id);
            }
            catch (Exception ex)
            {
                //删除数据时失败
                return -2;
            }

            return parentId;  //返回其父节点的id

        }

		
		
		
		#endregion gen code
		
		#region self define code
		
		
		
		#endregion self define code
   }

}