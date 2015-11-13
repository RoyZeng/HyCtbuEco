using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
	/// <summary>
	///		此用于处理表对象 'tb_SqlQuery' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	/// </remarks>
	
    [Table("tb_SqlQuery")]
    public class TbSqlQuery
    {
              //说明:  
     [Key] 
		public System.Int32 Id { get; set; } 
      //说明:sql的名称  
  		public System.String SSqlName { get; set; } 
      //说明:SQL语句  
  		public System.String SSqlStr { get; set; } 
      //说明:建立ID  
  		public System.Int32 ICreateID { get; set; } 
      //说明:创建修改时间  
  		public System.DateTime DCreate { get; set; } 
      //说明:排序号  
  		public System.Int32 ISort { get; set; } 
		//其中的主键列:
		//Item.ID
		//外键列:

    }
}
