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
using TMS.Common;


namespace WebSite.Areas.WebApi.Controllers
{
  public class DimTimeController : BaseController
   {
       
        #region gen code
		
		
		protected DimTimeDAO DimTimeDAO = new DimTimeDAO();
		protected int totalCount = 0;
		
		 /// <summary>
		 /// GET api/DimTime/Get
		 ///返回所有的表格数据，json或xml格式
		 /// </summary>
        public JsonResult GetAll()
        {
			IList<TbDimTime> DimTimeList =  DimTimeDAO.GetAll();  
			return Json(DimTimeList, JsonRequestBehavior.AllowGet);
           
        }

		/// <summary>
        /// GET api/DimTime/Get/id
		///返回指定id的数据
		/// </summary>
		/// <param name="id">id,主键</param>
        public JsonResult Get(int id)
        {
            TbDimTime item = DimTimeDAO.GetByID(id);
           

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
            IList<TbDimTime> userList= DimTimeDAO.GetByPageDataBase(firstResult, pagesize, orderBy, condition, out totalCount);
            return Json(new
            {
                DataCount = totalCount,
                Data = userList
            },JsonRequestBehavior.AllowGet);
        }

		
		
		
		/// <summary>
        /// POST api/DimTime/Post
		///新增item数据，
		/// </summary>
		/// <param name="item">item,新增数据</param>
        public int  Post(TbDimTime item)
        {
			 
                DimTimeDAO.Insert(item);

                
                
                return item.Id;//新增数据成功
           


        }

		
		/// <summary>
        /// PUT api/DimTime/Put/5,更新数据
		/// </summary>
        public int  Put(int id, TbDimTime item)
        {
			

             if (id != item.Id)
            {
                //数据无效
                return -1;
            }

            try
            {
                DimTimeDAO.Update(item);
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
        /// DELETE api/DimTime/Delete/5
        /// </summary>
        public int Delete(int id)
        {
			//tree
            IList<TbDimTime> chidrens = DimTimeDAO.findByIParentID(id);
            if (chidrens.Count > 0)
            {
                return -1;// "为安全，无法直接删除有子结点的树！！";
            }
            


            try
            {
               DimTimeDAO.Del(id);
            }
            catch (Exception ex)
            {
                //删除数据时失败
                return -2;
            }

            return 1;

        }

		
		
		
		/// <summary>
        /// DELETE api/DimTime/Delete/?ids="1,2,3,4"
		/// </summary>
        public int  DeleteIds(string  ids)
        {
			try	{
				DimTimeDAO.Del(ids);
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
            
          
            IList<TbDimTime> items = DimTimeDAO.GetByPageDataBase(0, int.MaxValue, "ILevel ASC,IParentID ASC,Id ASC", "1=1", out totalCount);//按ILevel升级，再按ParentID排序

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
               
                myNode.name = items[i].SName;
				myNode.open = false;

                myNode.id = items[i].Id;
				myNode.pId = (int)items[i].IParentID;
                myNode.tip = items[i].SName;
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
           

             IList<TbDimTime> items = DimTimeDAO.GetByPageDataBase(0, int.MaxValue, "ILevel ASC,IParentID ASC,Id ASC", "1=1", out totalCount);//按ILevel升级，再按ParentID排序

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
               
                myNode.name = items[i].SName;
				myNode.open = false;

                myNode.id = items[i].Id;
				myNode.pId = (int)items[i].IParentID;
                myNode.tip = items[i].SName;
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
        /// DELETE api/DimTime/TreeDel?id=5
		/// </summary>
        public int  TreeDel(int id)
        {
			

            

            try
            {
               DimTimeDAO.DelTree(id);
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

            //api/DimTime/AddTreeNode/0

			
			TbDimTime tmpNode = new TbDimTime();
            tmpNode.IParentID = id;//此处的id为parentid
            tmpNode.SName = "新增结点";
            if (id != 0)
            {
                //非一级节点,先读取父结点
                TbDimTime parentNode = DimTimeDAO.GetByID(id);
                if (parentNode != null)
                {
                    tmpNode.ILevel = parentNode.ILevel + 1;
                }
                else
                {
                    return -1;
					//Request.CreateResponse(HttpStatusCode.BadRequest, "出错了，找不到新建 DimTime 的 父结点:"+id+"!!");
                }
            }
            else
            {
                //一级节点
                tmpNode.ILevel = 1;
            }
           

            DimTimeDAO.Insert(tmpNode);
			
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
            IList<TbDimTime> chidrens = DimTimeDAO.findByIParentID(id);
            if (chidrens.Count > 0)
            {
                return -1;// "为安全，无法直接删除有子结点的树！！";
            }
            


            try
            {
                TbDimTime parent = DimTimeDAO.GetByID(id);
                parentId = parent.IParentID;
               DimTimeDAO.Del(id);
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



      /// <summary>
      /// 获取当前库的所有时间指标
      /// </summary>
      /// <param name="libId"></param>
      /// <returns></returns>
        public JsonResult DimLibTimeTree(int libId) {

            IList<TbDimTime> items = DimTimeDAO.GetByPageDataBase(0, int.MaxValue, "ILevel ASC,IParentID ASC,Id ASC", "ILibID=" + libId, out totalCount);//按ILevel升级，再按ParentID排序

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

                myNode.name = items[i].SName;
                myNode.open = false;

                myNode.id = items[i].Id;
                myNode.pId = (int)items[i].IParentID;
                myNode.tip = items[i].SName;
                if (items[i].ILevel != null && items[i].ILevel < 2) myNode.open = true;//自动扩展1,2级
                zNodes.Add(myNode);

            }



            return Json(zNodes, JsonRequestBehavior.AllowGet);
        
        
        }

		
		#endregion self define code
   }

}