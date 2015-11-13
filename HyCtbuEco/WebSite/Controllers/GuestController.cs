using HyCtbuEco.Dim;
using HyCtbuEco.Entities;
using HyCtbuEco.Models;
using HyCtbuEco.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HyCtbuEco2015.Controllers
{

    [UserAuthorizeGuest]
    public class GuestController : Controller
    {



        private DimLibDAO _DimLibDao = new DimLibDAO();

        private DimIndicatorDAO _DimIndicatorDao = new DimIndicatorDAO();

        private DimAreaDAO _DimAreaDao = new DimAreaDAO();

        //
        // GET: /Guest/

        public ActionResult Index()
        {
            return View();
        }




        /// <summary>
        /// Home
        /// </summary>
        /// <returns></returns>
        public ActionResult Home(int libId=1) 
        {
            ViewBag.LibId = libId;
            return View();
        }



        /// <summary>
        /// 菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult Menu() {
            IList<TbDimLib> LibList = _DimLibDao.GetAll();
            ViewBag.LibList = LibList;
            return View();
        
        }



        /// <summary>
        /// 客户端的作图
        /// </summary>
        /// <returns></returns>
        public ActionResult GuestMakeGraphics(int libID)
        {
            TbDimLib item = _DimLibDao.GetByID(libID);

            //返回库名称
            ViewBag.LibName = item.SLibName;

            //返回库ID

            ViewBag.LibId = item.Id;

            //从session中获取已选的指标参数，地区参数，时间参数


            int[] IndicatorIds = GuestUserInfo.GetIndiactorArray(); //是一个数组


            List<TbDimIndicator> DimIndicatorList = new List<TbDimIndicator>();

            //获取指标对象并返回页面
            for (int i = 0; i < IndicatorIds.Length; i++)
            {
                DimIndicatorList.Add(_DimIndicatorDao.GetByID(IndicatorIds[i]));

            }


            ViewBag.IndicatorList = DimIndicatorList;  //传递指标对象集合到页面


            int[] AreasIds = GuestUserInfo.GetAreaArray();//是一个包含已选地区参数Id的数组


            List<TbDimArea> DimAreaList = new List<TbDimArea>();


            for (int i = 0; i < AreasIds.Length; i++)
            {
                DimAreaList.Add(_DimAreaDao.GetByID(AreasIds[i]));
            }

            ViewBag.AreaList = DimAreaList;
            return View();
        
        }

    }
}
