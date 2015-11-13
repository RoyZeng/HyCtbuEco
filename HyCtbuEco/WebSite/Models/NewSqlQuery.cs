using HyCtbuEco.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HyCtbuEco.Models
{
    /// <summary>
    /// 扩展TbSqlQuery，添加一个创建人姓名的字段
    /// </summary>
    public class NewSqlQuery : TbSqlQuery
    {
        public string SUserName { get; set; }  //创建人的姓名
    }
}
