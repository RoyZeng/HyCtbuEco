using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
    /// <summary>
    ///		此用于处理表对象 'tb_Sys_part' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    /// </remarks>

    [Table("tb_Sys_part")]
    public class TbSysPart
    {
        //说明:  
        [Key]
        public System.Int32 Id { get; set; }
        //说明:部门名称  
        public System.String SPartName { get; set; }
        //说明:部门类型  
        public System.Int32 IType { get; set; }
        //说明:  
        public System.String SScript { get; set; }
        //说明:  
        public System.Int32 IParentId { get; set; }
        //说明:  
        public System.String SMemo { get; set; }
        //说明:  
        public System.Int32 IOwer { get; set; }
        //说明:  
        public System.Int32 IIsTmp { get; set; }
        //说明:  
        public System.Int32 ILevel { get; set; }


        /// <summary>
        /// 机构代码,唯一
        /// </summary>
        public String SKey { get; set; }


        /// <summary> 
        /// 经度
        /// <summary> 
        public System.String SLong { get; set; }


        /// <summary> 
        /// 纬度
        /// <summary> 
        public System.String SLatitude { get; set; }



        /// <summary>
        /// 地址
        /// </summary>
        public string SAddr { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string SPhone { get; set; }

    }

    


}
