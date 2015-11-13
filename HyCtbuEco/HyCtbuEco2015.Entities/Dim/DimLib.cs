using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
	/// <summary>
	///		此用于处理表对象 'tb_DimLib' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	/// </remarks>
	
    [Table("tb_DimLib")]
    public class TbDimLib
    {
        		
		
		/// <summary> 
		/// 
		/// <summary> 
     [Key] 
		public System.Int32 Id { get; set; } 
		
		
		/// <summary> 
		/// 库名
		/// <summary> 
public System.String SLibName { get; set; } 
		
		
		/// <summary> 
		/// 表名
		/// <summary> 
public System.String STableName { get; set; } 
		
		
		/// <summary> 
		/// 建立时间
		/// <summary> 
     public System.DateTime? DCreate { get; set; } 
		
		
		/// <summary> 
		/// 说明信息
		/// <summary> 
public System.String SIntro { get; set; } 
		
		
		/// <summary> 
		/// 备注信息
		/// <summary> 
public System.String SMemo { get; set; } 
		//其中的主键列:
		/// <summary> 
		///Item.Id
		/// <summary> 
		//外键列:

    }
}
