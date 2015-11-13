using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyCtbuEco.Server
{
    public class CommonUser   //定义一个公共用户类，用于模糊查询
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string SName { get; set; }
        public string SNo { get; set; }
    }
   
}
