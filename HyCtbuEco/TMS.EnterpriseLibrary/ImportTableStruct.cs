using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TMS.EnterpriseLibrary
{

    [Serializable]  
    /// <summary>
    /// 导入表字段结构类
    /// </summary>
    public class ImportTableFieldStruct
    {
        /// <summary>
        /// Auto Gen ID
        /// </summary>
        public int GenID { get; set; }

        /// <summary>
        /// 源表字段说明
        /// </summary>
        public string SourceFieldMem { get; set; }

        /// <summary>
        /// 源表字段名
        /// </summary>
        public string SoureFieldName { get; set; }

        /// <summary>
        /// 目的表字段名
        /// </summary>
        public string TargetFieldName { get; set; }

        /// <summary>
        /// 目的表字段类型
        /// </summary>
        public string TargetFieldType { get; set; }

        /// <summary>
        /// 目的表字段可空
        /// </summary>
        public string TargetFieldNullable { get; set; }

        /// <summary>
        /// 目的表字段长度
        /// </summary>
        public string TargetFieldLength { get; set; }

        /// <summary>
        /// 第二目的表名
        /// </summary>
        public string SecTableName { get; set; }


        /// <summary>
        /// 第二目的表字段名
        /// </summary>
        public string SecFieldName { get; set; }

        /// <summary>
        /// 第二目的表字段类型
        /// </summary>
        public string SecFieldType { get; set; }

        /// <summary>
        /// 第二目的表字段长度
        /// </summary>
        public string SecFieldLength{ get; set; }


    }


    [Serializable]  
    /// <summary>
    /// 列结构
    /// </summary>
    public class ColumeStructs
    {
        public string ColName { get; set; }
        public string ColType { get; set; }
        public string ColLength { get; set; }
        public string ColMem { get; set; }
        public string ColNullable { get; set; }
    }
}