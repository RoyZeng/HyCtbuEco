
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyCtbuEco.Entities
{
    /// <summary>
    /// 扩展用户信息以支持SPartName
    /// </summary>
    public class UserAndUserRole : TbSysUser
    {
        public String SPartName { get; set; }
    }
}