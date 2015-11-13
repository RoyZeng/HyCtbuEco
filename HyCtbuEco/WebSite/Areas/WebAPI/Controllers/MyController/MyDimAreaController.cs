
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using System.Net;
using System.Web;
using System.Net.Http;
using HyCtbuEco.Entities;
using HyCtbuEco.Dim;
using TMS.Common;


namespace WebSite.Areas.WebApi.Controllers
{
    public class MyDimAreaController : DimAreaController
    {
        /// <summary>
        /// GET api/DimArea/Get
        ///返回所有的表格数据，json或xml格式
        /// </summary>
        public JsonResult GetBySelLibId(String id = "1")
        {
            IList<TbDimArea> items = DimAreaDAO.GetByPageDataBase(0, int.MaxValue, "ILevel ASC,IParentID ASC,Id ASC", "ILibID='" + id + "'", out totalCount);//按ILevel升级，再按ParentID排序

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

                myNode.name = items[i].SAreaName;
                myNode.open = false;

                myNode.id = items[i].Id;
                myNode.pId = (int)items[i].IParentID;
                myNode.tip = items[i].SAreaName;
                //if (items[i].ILevel != null && items[i].ILevel < 2) myNode.open = true;//自动扩展1,2级
                zNodes.Add(myNode);

            }



            return Json(zNodes, JsonRequestBehavior.AllowGet);

        }

    }
}
