using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
    /// <summary>
    ///		此用于处理表对象 'tb_FacHGYDK' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    /// </remarks>

    [Table("tb_FacHGYDK")]
    public class TbFacHGYDK
    {


        /// <summary> 
        /// 
        /// <summary> 
        [Key]
        public System.Int32 Id { get; set; }


        /// <summary> 
        /// 时间维代码
        /// <summary> 
        public System.String STimeCode { get; set; }


        /// <summary> 
        /// 地区ID
        /// <summary> 
        public System.Int32 IAreaID { get; set; }


        /// <summary> 
        /// 指标代码
        /// <summary> 
        public System.String SIndCode { get; set; }


        /// <summary> 
        /// 指标值
        /// <summary> 
        public System.Double FValue { get; set; }


        /// <summary> 
        /// 单位，可选
        /// <summary> 
        public System.String SUnit { get; set; }


        /// <summary> 
        /// 备注
        /// <summary> 
        public System.String SMemo { get; set; }


        /// <summary>
        /// 时间ID
        /// </summary>
        public System.Int32 ITimeID { get; set; }

        /// <summary>
        /// 指标ID
        /// </summary>
        public System.Int32 IIndID { get; set; }
        //其中的主键列:
        /// <summary> 
        ///Item.Id
        /// <summary> 
        //外键列:

    }
}
