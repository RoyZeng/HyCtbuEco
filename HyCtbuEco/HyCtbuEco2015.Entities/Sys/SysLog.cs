﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
	/// <summary>
	///		此用于处理表对象 'tb_Sys_Log' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	/// </remarks>
	
    [Table("tb_Sys_Log")]
    public class TbSysLog
    {
              //说明:  
     [Key] 
		public System.Int32 Id { get; set; } 
      //说明:  
  		public System.Int32 ILogType { get; set; } 
      //说明:  
  		public System.String SMessage { get; set; } 
      //说明:  
  		public System.Int32 IUserID { get; set; } 
      //说明:  
  		public System.DateTime DWriteTime { get; set; } 
      //说明:  
  		public System.String SMemo { get; set; } 
		//其中的主键列:
		//Item.ID
		//外键列:

    }
}
