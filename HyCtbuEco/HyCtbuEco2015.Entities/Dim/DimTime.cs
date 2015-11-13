using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
    /// <summary>
    ///		此用于处理表对象 'tb_DimTime' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    /// </remarks>

    [Table("tb_DimTime")]
    public class TbDimTime
    {


        /// <summary> 
        /// 
        /// <summary> 
        [Key]
        public System.Int32 Id { get; set; }


        /// <summary> 
        /// 名称
        /// <summary> 
        public System.String SName { get; set; }


        /// <summary> 
        /// 代码，例如：2009
        /// <summary> 
        public System.String STimeCode { get; set; }


        /// <summary> 
        /// 父结点
        /// <summary> 
        public System.Int32 IParentID { get; set; }

        /// <summary> 
        /// 
        /// <summary> 
        public System.Int32 ILevel { get; set; }


        /// <summary> 
        /// 备注
        /// <summary> 
        public System.String SMemo { get; set; }


        /// <summary>
        /// ILibID  关联DimLib表
        /// </summary>
        public System.Int32 ILibID { get; set; }
        //其中的主键列:
        /// <summary> 
        ///Item.Id
        /// <summary> 
        //外键列:

    }
}
