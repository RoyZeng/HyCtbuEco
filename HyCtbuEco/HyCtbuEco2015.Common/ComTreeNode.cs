using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMS.Common
{

    /// <summary>
    /// 一般性的树结构，可递归
    /// </summary>
   public  class ComTreeNode
    {
        public int Id { get; set; }
        public string NodeName { get; set; }
        public string Url { get; set; }
        public string Tip { get; set; }
       
        private IList<ComTreeNode> _Childrends=new List<ComTreeNode>();

        public IList<ComTreeNode> Childrens
        {
            get { return _Childrends; }
            set { _Childrends = value; }
        }

    }
}
