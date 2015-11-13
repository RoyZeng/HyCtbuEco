using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
	/// <summary>
	///		此用于处理表对象 'tb_Sys_PagePower' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	/// </remarks>
	
    [Table("tb_Sys_PagePower")]
    public class TbSysPagePower
    {
              //说明:  
     [Key] 
		public System.Int32 Id { get; set; } 
      //说明:菜单名  
  		public System.String SMenuName { get; set; } 
      //说明:菜单项url  
  		public System.String SMenuURL { get; set; } 
      //说明:  
  		public System.Int32 IParentId { get; set; } 
      //说明:  
  		public System.Int32 ILevel { get; set; } 
      //说明:  
  		public System.Int32 IPowerID { get; set; } 
      //说明:  
  		public System.Int32 IType { get; set; } 
      //说明:菜单项图标  
  		public System.String SPicUrl { get; set; } 
      //说明:  
  		public System.Int32 IMenuType { get; set; } 
      //说明:  
  		public System.Int32 ISort { get; set; } 
      //说明:  
  		public System.Int32 IIsDelete { get; set; } 
		//其中的主键列:
		//Item.ID
		//外键列:

    }
}
