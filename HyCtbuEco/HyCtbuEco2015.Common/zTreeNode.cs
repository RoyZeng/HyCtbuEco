using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMS.Common
{
    public class zTreeNode
    {
        //{ id:1, pId:0, name:"展开、折叠 自定义图标不同", open:true, 
        //iconOpen:"../../../css/zTreeStyle/img/diy/1_open.png", iconClose:"../../../css/zTreeStyle/img/diy/1_close.png"},
        public int id;

        public int pId;
        public string name;
        public bool open=false;
        /// <summary>
        /// 例如： iconOpen:"../../../css/zTreeStyle/img/diy/1_open.png",
        /// </summary>
        public string iconOpen;
        /// <summary>
        /// 例如：iconClose:"../../../css/zTreeStyle/img/diy/1_close.png"
        /// </summary>
        public string iconClose;

        /// <summary>
        /// icon:"../../../css/zTreeStyle/img/diy/2.png
        /// </summary>
        public string icon;

        /// <summary>
        /// 节点是否勾选,ztree 为checked,此处需要转换名称
        /// </summary>
        public bool  nchecked=false;

        /// <summary>
        /// url 示例: url:"http://code.google.com/p/jquerytree/", target:"_blank"
        /// </summary>
        public string url;

        /// <summary>
        /// 点击打开，与url配合，示例 ： url:"http://code.google.com/p/jquerytree/", target:"_blank"
        /// </summary>
        public string target;

       

        //自定义属性
        public string tip;
        public string DiyStr1;
        public string DiyStr2;

    }
}
