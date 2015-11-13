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
    [UserAuthorizeSys]
    public class EconomicDataController : Controller
    {


        private DimLibDAO _DimLibDao = new DimLibDAO();



        private DimIndicatorDAO _DimIndicatorDao = new DimIndicatorDAO();


        private DimAreaDAO _DimAreaDao = new DimAreaDAO();

        //
        // GET: /EconomicData/

        public ActionResult AllData()
        {
            IList<TbDimLib> allLib = _DimLibDao.GetAll();
            ViewBag.AllDimLib = allLib;
            return View();
        }



        /// <summary>
        /// 指标分类管理
        /// </summary>
        /// <returns></returns>
        public ActionResult IndicatorCate()
        {

            //获取所有的库对象并传递到页面
            ViewBag.DimLibList = _DimLibDao.GetAll();
            return View();
        
        }



        /// <summary>
        /// 查看指定库Id以及指定分类Id下具体的指标
        /// </summary>
        /// <param name="cateId">分类Id</param>
        /// <param name="libId">库Id</param>
        /// <returns></returns>
        public ActionResult Indicator(int cateId, int libId)
        {


            //将分类Id带回视图
            ViewBag.cateId = cateId;
            //将库的Id带回视图
            ViewBag.libId = libId;
            return View();
        }




        /// <summary>
        /// 地区参数管理
        /// </summary>
        /// <returns></returns>
        public ActionResult DimArea() {

            ViewBag.lib = new DimLibDAO().GetAll();
            return View();
        }




        /// <summary>
        /// 时间参数管理
        /// </summary>
        /// <returns></returns>
        public ActionResult DimTime()
        {
            ViewBag.lib = new DimLibDAO().GetAll();
            return View();
        }


        /// <summary>
        /// 库管理
        /// </summary>
        /// <returns></returns>
        public ActionResult DimLib()
        {
            return View();
        }




        /// <summary>
        /// 作图
        /// </summary>
        /// <returns></returns>
        public ActionResult MakeGraphics(int libID)
        {

            TbDimLib  item= _DimLibDao.GetByID(libID);

            //返回库名称
            ViewBag.LibName =item.SLibName;

            //返回库ID

            ViewBag.LibId = item.Id;

            //从session中获取已选的指标参数，地区参数，时间参数


            int [] IndicatorIds = SysUserInfo.GetIndiactorArray(); //是一个数组


            List<TbDimIndicator> DimIndicatorList = new List<TbDimIndicator>();

            //获取指标对象并返回页面
            for (int i = 0; i < IndicatorIds.Length; i++)
            {
                DimIndicatorList.Add(_DimIndicatorDao.GetByID(IndicatorIds[i]));

            }


            ViewBag.IndicatorList = DimIndicatorList;  //传递指标对象集合到页面


            int []AreasIds = SysUserInfo.GetAreaArray();//是一个包含已选地区参数Id的数组


            List<TbDimArea> DimAreaList = new List<TbDimArea>();


            for (int i = 0; i < AreasIds.Length; i++)
            {
                DimAreaList.Add( _DimAreaDao.GetByID(AreasIds[i]));
            }

            ViewBag.AreaList = DimAreaList;

            return View();
        }



    }
}
