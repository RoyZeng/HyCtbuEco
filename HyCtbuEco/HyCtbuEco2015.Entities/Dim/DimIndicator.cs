using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
    /// <summary>
    ///		此用于处理表对象 'tb_DimIndicator' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    /// </remarks>

    [Table("tb_DimIndicator")]
    public class TbDimIndicator
    {


        /// <summary> 
        /// Id
        /// <summary> 
        [Key]
        public System.Int32 Id { get; set; }


        /// <summary> 
        /// 指标类型ID
        /// <summary> 
        public System.Int32 ICateID { get; set; }


        /// <summary> 
        /// 指标名
        /// <summary> 
        public System.String SIndName { get; set; }


        /// <summary> 
        /// 指标说明
        /// <summary> 
        public System.String SIndIntro { get; set; }


        /// <summary> 
        /// 指标代码
        /// <summary> 
        public System.String SIndCode { get; set; }


        /// <summary> 
        /// 库ID,冗余，可能需要
        /// <summary> 
        public System.Int32 ILibID { get; set; }


        /// <summary>
        /// 分类名，冗余
        /// </summary>
        public System.String SCateName { get; set; }

        /// <summary>
        /// 库名称，冗余
        /// </summary>
        public System.String SLibName { get; set; }


        /// <summary>
        /// 单位
        /// </summary>
        public System.String SUnit { get; set; }

        //其中的主键列:
        /// <summary> 
        ///Item.Id
        /// <summary> 
        //外键列:

    }
}
