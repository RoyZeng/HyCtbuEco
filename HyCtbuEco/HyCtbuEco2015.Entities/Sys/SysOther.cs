using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
	/// <summary>
	///		此用于处理表对象 'tb_Sys_Other' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	/// </remarks>
	
    [Table("tb_Sys_Other")]
    public class TbSysOther
    {
              //说明:  
  		public System.Int32 Id { get; set; } 
      //说明:  
  		public System.String SOtherName { get; set; } 
      //说明:  
  		public System.String SMemo { get; set; } 
      //说明:  
  		public System.Int32 IDM { get; set; } 
      //说明:类型  
  		public System.Int32 IType { get; set; }

        /// <summary>
        /// 可能需要的日期
        /// </summary>
        public DateTime? DTime { get; set; }

    }
}
