using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyCtbuEco.Entities
{
   public class Brach
    {
        /// <summary>
        /// B00,总店
        /// </summary>
        public const string StoreT = "B00";


       /// <summary>
       /// B01,分店一
       /// </summary>
       public const string Store1="B01";

       /// <summary>
       /// B02
       /// </summary>
       public const String Store2 = "B02";
       public const String Store3 = "B03";
       public const String Store4 = "B04";
    }

   public class TbSysPartExt : TbSysPart
   {
       /// <summary>
       /// 包含经纬度的Name,Long,Latitude
       /// </summary>
       public string NameWithLL { get; set; }

   }
}
