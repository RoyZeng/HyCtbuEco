using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
	/// <summary>
	///		此用于处理表对象 'tb_Sys_Role' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	/// </remarks>
	
    [Table("tb_Sys_Role")]
    public class TbSysRole
    {
              //说明:  
     [Key] 
		public System.Int32 Id { get; set; } 
      //说明:角色名（系统管理员，系统测试员）  
  		public System.String SRoleName { get; set; } 
      //说明:角色描述  
  		public System.String SDis { get; set; } 
      //说明:  
  		public System.String SMEM { get; set; } 
      //说明:拥有的权限  
  		public System.String SPower { get; set; } 
      //说明:  
  		public System.Int32 ILevel { get; set; } 
      //说明:  
  		public System.Int32 IParentId { get; set; } 
		//其中的主键列:
		//Item.ID
		//外键列:

    }
}
