using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
	/// <summary>
	///		此用于处理表对象 'tb_DimIndCate' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	/// </remarks>
	
    [Table("tb_DimIndCate")]
    public class TbDimIndCate
    {
        		
		
		/// <summary> 
		/// 
		/// <summary> 
     [Key] 
		public System.Int32 Id { get; set; } 
		
		
		/// <summary> 
		/// 指标类型名
		/// <summary> 
public System.String SCateName { get; set; } 
		
		
		/// <summary> 
		/// 所属库Id
		/// <summary> 
public System.Int32 ILibID { get; set; } 
		
		
		/// <summary> 
		/// 父结点
		/// <summary> 
public System.Int32 IParentID { get; set; } 
		
		
		/// <summary> 
		/// 级别
		/// <summary> 
public System.Int32 ILevel { get; set; } 
		
		
		/// <summary> 
		/// 是否叶子结点
		/// <summary> 
public System.Int32 ILeaf { get; set; } 
		
		
		/// <summary> 
		/// 介绍
		/// <summary> 
public System.String SCateIntro { get; set; } 
		
		
		/// <summary> 
		/// 从根到当前结点的全路径，例如：根>类别1>类别11>类别111
		/// <summary> 
public System.String SCateAllName { get; set; } 
		
		
		/// <summary> 
		/// 备注
		/// <summary> 
public System.String SMemo { get; set; } 
		
		
		/// <summary> 
		/// 指标类型代码
		/// <summary> 
public System.String SCateCode { get; set; } 
		//其中的主键列:
		/// <summary> 
		///Item.Id
		/// <summary> 
		//外键列:

    }
}
