using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyCtbuEco.Entities
{
    /// <summary>
    ///		此用于处理表对象 'tb_Guests' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    /// </remarks>

    [Table("tb_Guests")]
    public class TbGuests
    {
        [Key]
        public System.Int32 Id { get; set; }
        //说明:客户用户名  
        public System.String SGuestName { get; set; }
        //说明:客户密码  
        public System.String SPassword { get; set; } 
    }
}
