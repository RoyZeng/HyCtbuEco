using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using HyCtbuEco.Entities;
using HyCtbuEco.Dim;
public class MyCache
{
    private static ICacheManager cache = CacheFactory.GetCacheManager();


    /// <summary>
    /// 获取时间Id tb_dimTime
    /// </summary>
    /// <returns></returns>
    public static List<TbDimTime> GetTimeId()
    {

        DimTimeDAO timeDao = new DimTimeDAO();
        List<TbDimTime> timelist;

        if (cache.GetData("GetTimeId") != null)
        {
            timelist = (List<TbDimTime>)cache.GetData("GetTimeId");
        }
        else
        {
            timelist = timeDao.GetAll().ToList();
            cache.Add("GetTimeId", timelist);
        }
        if (timelist.Count == 0)
        {
            timelist = timeDao.GetAll().ToList();
            cache.Add("GetTimeId", timelist);
        }

        return timelist;
    }

    /// <summary>
    /// 获取时间IIndID dbo.tb_DimIndicator
    /// </summary>
    /// <returns></returns>
    public static List<TbDimIndicator> GetIndId()
    {
        DimIndicatorDAO indicatorDao = new DimIndicatorDAO();
        List<TbDimIndicator> indicatorlist;
        if (cache.GetData("GetIndId") != null)
        {
            indicatorlist = (List<TbDimIndicator>)cache.GetData("GetIndId");
        }
        else
        {
            indicatorlist = indicatorDao.GetAll().ToList();
            cache.Add("GetIndId", indicatorlist);
        }
        if (indicatorlist.Count == 0)
        {
            indicatorlist = indicatorDao.GetAll().ToList();
            cache.Add("GetIndId", indicatorlist);
        }
        return indicatorlist;
    }
}
