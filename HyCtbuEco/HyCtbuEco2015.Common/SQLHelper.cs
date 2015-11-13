using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyXiaoMian.Common
{
   public class SQLHelper
    {
       /// <summary>
       /// GetByPage方法的sql语句构造
       /// </summary>
       /// <param name="tbName">表名</param>
       /// <param name="whereClause">查询条件</param>
       /// <param name="start">起始位置</param>
       /// <param name="limit">最大条数</param>
       /// <param name="sort">排序列</param>
       /// <param name="dir">排序方向</param>
       /// <returns></returns>
       public static string GenPageSQL(string tbName,string whereClause,int start, int limit, string sort, string dir)
       {
           if (string.IsNullOrEmpty(sort))
           {
               sort = "Id";
           }
           StringBuilder SQLstring = new StringBuilder();
           int PageLowerBound = start * limit;
           int PageUpperBound = PageLowerBound + limit;
           SQLstring.Append("with pageIndex as (select top ");
           SQLstring.Append(PageUpperBound);
           SQLstring.Append(" ROW_NUMBER() OVER (ORDER BY ");
           SQLstring.Append(sort);
           SQLstring.Append(" ");
           SQLstring.Append(dir);
           SQLstring.Append(" ) as RowIndex,* from " ); 
           SQLstring.Append(tbName);
           SQLstring.Append("   where ");
           SQLstring.Append(whereClause);
           SQLstring.Append(" ) select * from pageIndex where RowIndex >");
           SQLstring.Append(PageLowerBound);
           SQLstring.Append(" and RowIndex <= ");
           SQLstring.Append(PageUpperBound); 
           SQLstring.Append(" ORDER BY ");
           SQLstring.Append(sort);
           SQLstring.Append(" ");
           SQLstring.Append(dir);
           

           return SQLstring.ToString();
       }

       /// <summary>
       /// GetByPageDataBase方法的sql语句构造
       /// </summary>
       /// <param name="tbName">表名</param>
       /// <param name="whereClause">查询条件</param>
       /// <param name="start">起始位置</param>
       /// <param name="limit">最大条数</param>
       /// <param name="sort">排序条件</param>
       /// <returns></returns>
       public static string GenPageDataBaseSQL(string tbName, string whereClause, int start, int limit, string sort)
       {
           StringBuilder SQLstring = new StringBuilder();
           int PageLowerBound = start * limit;
           int PageUpperBound = PageLowerBound + limit;
           SQLstring.Append("with pageIndex as (select top ");
           SQLstring.Append(PageUpperBound);
           SQLstring.Append(" ROW_NUMBER() OVER (ORDER BY ");
           SQLstring.Append(sort);
           SQLstring.Append(" ) as RowIndex,* from ");
           SQLstring.Append(tbName);
           SQLstring.Append("   where ");
           SQLstring.Append(whereClause);
           SQLstring.Append(" ) select * from pageIndex where RowIndex >");
           SQLstring.Append(PageLowerBound);
           SQLstring.Append(" and RowIndex <= ");
           SQLstring.Append(PageUpperBound);
           SQLstring.Append(" ORDER BY ");
           SQLstring.Append(sort);

           return SQLstring.ToString();
       }
    }
}
