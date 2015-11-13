using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyCtbuEco.Entities
{
    /// <summary>
    ///		此用于处理表对象 'tb_Sys_User' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    /// </remarks>

    [Table("tb_Sys_User")]
    public class TbSysUser
    {
        //说明:  
        [Key]
        public System.Int32 Id { get; set; }
        //说明:用户名  
        public System.String SUserName { get; set; }
        //说明:密码  
        public System.String SPassword { get; set; }
        //说明:创建时间  
        public System.DateTime? DCreateDate { get; set; }
        //说明:最后登录时间  
        public System.DateTime? DLastLoginDate { get; set; }
        //说明:创建时的ip  
        public System.String SCIP { get; set; }
        //说明:最后登录ip  
        public System.String SLIP { get; set; }
        //说明:真实姓名  
        public System.String STrueName { get; set; }
        //说明:  
        public System.Int32 ISex { get; set; }
        //说明:地址  
        public System.String SAddr { get; set; }
        //说明:所属部门id  
        public System.Int32 IPart { get; set; }
        //说明:登录状态  
        public System.Int32 IState { get; set; }
        //说明:职称  
        public System.String SPro { get; set; }
        //说明:出生日期  
        public System.DateTime? DBirth { get; set; }
        //说明:国籍  
        public System.String SNational { get; set; }
        //说明:联系电话  
        public System.String SPhone { get; set; }
        //说明:传真  
        public System.String SFax { get; set; }
        //说明:身份证号  
        public System.String SIDCard { get; set; }
        //说明:头像  
        public System.String SImg { get; set; }
        //说明:  
        public System.String SSignIMG { get; set; }
        //说明:  
        public System.Int32 IScore { get; set; }
        //说明:  
        public System.Int32 IPostRecord { get; set; }
        //说明:  
        public System.Int32 ILevel { get; set; }
        //说明:  
        public System.String SIntroduce { get; set; }
        //说明:  
        public System.String Sworkplace { get; set; }
        //说明:  
        public System.String SEmail { get; set; }
        //说明:  
        public System.Int32 IShowPosition { get; set; }
        //说明:  
        public System.Int32 ISort { get; set; }
        //说明:  
        public System.String SMem { get; set; }
        //说明:拥有的角色  
        public System.String SRoleName { get; set; }
        //说明:拥有的角色id，关联role表  
        public System.String SRoleId { get; set; }

        /// <summary>
        /// 重定向主页，没有则为默认
        /// </summary>
        public String SMainURL { get; set; }


        /// <summary>
        /// 主管理ID
        /// </summary>
        public int IManagerID { get; set; }

        /// <summary>
        /// 主管姓名(冗余)
        /// </summary>
        public String SManagerName { get; set; }


        /// <summary>
        /// 第二密码
        /// </summary>
        public String SOtherPassword { get; set; }
    }
}
