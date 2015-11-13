using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMS.EnterpriseLibrary
{
    public class DBHelper
    {
        //
        // GET: /Api/DBHelper/

        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <param name="name">数据库实例名（默认name为空,调用默认数据库实例)</param>
        /// <returns>数据库对象</returns>
        public static Database CreateDataBase(string name = "")
        {
            return EnterpriseLibraryContainer.Current.GetInstance<Database>(name);
        }
    }
}
