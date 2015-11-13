using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;

namespace HyCtbuEco.Entities
{
    /// <summary>
    ///		此用于处理表对象 'tb_Sys_Config' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    /// </remarks>

    [Table("tb_Sys_Config")]
    public class TbSysConfig
    {


        //[NoAutoCrease]	
        /// <summary> 
        /// Id
        /// <summary> 
        public System.Int32 Id { get; set; }


        /// <summary> 
        /// 数值
        /// <summary> 
        public System.Int32 IValue { get; set; }


        /// <summary> 
        /// 名称
        /// <summary> 
        public System.String SText { get; set; }


        /// <summary> 
        /// 此字段类型
        /// <summary> 
        public System.Int32 IType { get; set; }


        /// <summary> 
        /// 备注
        /// <summary> 
        public System.String SMemo { get; set; }


        /// <summary> 
        /// 键名
        /// <summary> 
        public System.String SKey { get; set; }
        //其中的主键列:
        /// <summary> 
        ///Item.ID
        /// <summary> 
        //外键列:

    }
}
