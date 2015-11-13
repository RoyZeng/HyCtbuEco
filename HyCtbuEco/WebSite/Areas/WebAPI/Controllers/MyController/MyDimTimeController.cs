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
    public class MyDimTimeController : DimTimeController
    {
        /// <summary>
        /// return all tree
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBySelLibId(String id = "1")
        {


            IList<TbDimTime> items = DimTimeDAO.GetByPageDataBase(0, int.MaxValue, "ILevel ASC,IParentID ASC,Id ASC", "ILibID='" + id + "'", out totalCount);//按ILevel升级，再按ParentID排序

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
                //if (items[i].ILevel != null && items[i].ILevel < 2) myNode.open = true;//自动扩展1,2级
                zNodes.Add(myNode);

            }



            return Json(zNodes, JsonRequestBehavior.AllowGet);

        }

    }
}
