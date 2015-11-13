using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
    /// <summary>
    ///		此用于处理表对象 'tb_DimArea' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    /// </remarks>

    [Table("tb_DimArea")]
    public class TbDimArea
    {


        /// <summary> 
        /// 
        /// <summary> 
        public System.Int32 Id { get; set; }


        /// <summary> 
        /// 
        /// <summary> 
        public System.String SAreaCode { get; set; }


        /// <summary> 
        /// 
        /// <summary> 
        public System.String SAreaName { get; set; }


        /// <summary> 
        /// 
        /// <summary> 
        public System.Int32 IParentID { get; set; }


        /// <summary> 
        /// 
        /// <summary> 
        public System.String SMemo { get; set; }


        /// <summary> 
        /// 
        /// <summary> 
        public System.Int32 ILevel { get; set; }


        /// <summary> 
        /// 邮编
        /// <summary> 
        public System.String SPost { get; set; }



        /// <summary>
        /// LibID  关联DimLib表
        /// </summary>
        public System.Int32 ILibID { get; set; }
        //其中的主键列:
        /// <summary> 
        ///Item.Id
        /// <summary> 
        //外键列:

    }
}
