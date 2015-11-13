using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using TmsDapper;

using HyCtbuEco.Entities;
using System.Data;

namespace HyCtbuEco.Dim
{
    /// <summary>
    ///		此用于处理表对象 'tb_DimLib' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    ///注意:此代码可能和同名的视图生成一个完整对象
    /// </remarks>



    public partial class DimLibBaseDAO
    {

        #region 表生成代码
        protected int TotalCount = -1;


        /// <summary>


        /// <summary>
        /// 取所有的对象,例如IList<???>
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, true)]
        public IList<TbDimLib> GetAll()
        {
            IList<TbDimLib> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimLib>("select * from tb_DimLib").ToList<TbDimLib>();


            }
            return tmplist;
        }



        /// <summary>
        /// 取Where条件对象,可选择排序和取top,返回List<???>
        /// </summary>
        /// <param name="where">必填</param>
        /// <param name="orderby">排序字段，选填</param>
        /// <param name="topNum">取头部，选填</param>
        /// <returns></returns>
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, true)]
        public IList<TbDimLib> GetByWhere(string where, string orderby = "", int topNum = -1)
        {
            IList<TbDimLib> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {
                string strSQL = "";
                if (topNum <= 0) { strSQL = "select * from tb_DimLib"; }
                else { strSQL = "select top " + topNum + " * from tb_DimLib"; };

                if (!string.IsNullOrEmpty(where))
                {
                    strSQL += " where " + where;
                }

                if (!string.IsNullOrEmpty(orderby))
                {
                    strSQL += " order by " + orderby;
                }

                tmplist = connection.Query<TbDimLib>(strSQL).ToList<TbDimLib>();


            }
            return tmplist;
        }




        /// <summary>
        /// 通过ID查询对象
        /// </summary>
        /// <param name="id">对象的ID</param>
        /// <returns></returns>
        public TbDimLib GetByID(int id)
        {
            TbDimLib curItem = null;
            using (var connection = DataBase.GetOpenConnection())
            {

                curItem = connection.Get<TbDimLib>(id);


            }

            return curItem;

        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereClause">查询条件</param>
        /// <param name="start">起始位置</param>
        /// <param name="limit">最大条数</param>
        /// <param name="sort">排序列</param>
        /// <param name="dir">排序方向</param>
        /// <param name="count">取得记录数</param>
        /// <returns></returns>
        public IList<TbDimLib> GetByPage(string whereClause, int start, int limit, string sort, string dir, out int count)
        {

            IList<TbDimLib> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_DimLib", whereClause, start, limit, sort, dir);
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimLib>(Sqlstring).ToList<TbDimLib>();

                count = GetCount(whereClause);
            }

            return tmplist;
        }


        /// <summary>
        /// 分页查询２，无排序
        /// </summary>
        /// <param name="whereClause">查询条件</param>
        /// <param name="start">起始位置</param>
        /// <param name="limit">最大条数</param>
        /// <param name="sort">排序列</param>
        /// <param name="dir">排序方向</param>
        /// <param name="count">取得记录数</param>
        /// <returns></returns>
        public IList<TbDimLib> GetByPage(string whereClause, int start, int limit)
        {

            IList<TbDimLib> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_DimLib", whereClause, start, limit, "", "");
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimLib>(Sqlstring).ToList<TbDimLib>();


            }

            return tmplist;
        }

        /// <summary>
        ///兼容查询
        /// </summary>
        /// <param name="whereClause">查询条件</param>
        /// <param name="start">起始位置</param>
        /// <param name="limit">最大条数</param>
        /// <param name="sort">排序列</param>
        /// <param name="dir">排序方向("DESC","ASC")</param>
        /// <param name="count">取得记录数</param>
        /// <returns>IList<???></returns>

        public IList<TbDimLib> GetByPageDataBase(int start, int limit, string sort, string whereClause, out int count)
        {


            return GetByPage(whereClause, start, limit, sort, "", out count);
        }



        /// <summary>
        /// 获取分页查询的总记录条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetCount(string where = "")
        {
            int count = 0;
            using (var connection = DataBase.GetOpenConnection())
            {
                string sqlStr = "select count(1) from tb_DimLib";
                if (!string.IsNullOrEmpty(where))
                {
                    sqlStr += " where " + where;
                }
                List<int> tmpList = connection.Query<int>(sqlStr).ToList<int>();
                count = tmpList[0];


            }

            return count;


        }



        /// <summary>
        /// 通过对象更新记录
        /// </summary>
        /// <param name="item">需要更新的对象</param>
        /// <returns></returns>
        public bool Update(TbDimLib item)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {

                res = connection.Update<TbDimLib>(item);

            }
            return res;
        }

        /// <summary>
        /// 通过对象更新记录
        /// </summary>
        /// <param name="item">需要更新的对象</param>
        /// <returns></returns>
        public bool Update(IList<TbDimLib> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                foreach (var item in items)
                {
                    res = connection.Update<TbDimLib>(item);
                }

            }
            return res;
        }






        /// <summary>
        /// 通过ID删除记录
        /// </summary>
        /// <param name="id">需要删除的对象的ID</param>
        /// <returns></returns>
        public bool Del(int id)
        {
            bool res = false;
            TbDimLib item = new TbDimLib();
            using (var connection = DataBase.GetOpenConnection())
            {
                item = GetByID(id);
                res = connection.Delete<TbDimLib>(item);
            }
            return res;
        }

        /// <summary>
        /// 通过对象删除记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Del(TbDimLib item)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                res = connection.Delete<TbDimLib>(item);
            }
            return res;
        }

        /// <summary>
        /// 通过对象删除记录,集合对象
        /// </summary>
        /// <param name="items">需删除的IList对象</param>
        /// <returns></returns>
        public bool Del(IList<TbDimLib> items)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {

                foreach (var item in items)
                {
                    connection.Delete<TbDimLib>(item);
                }
                res = true;
            }
            return res;

        }

        /// <summary>
        /// 通过对象删除记录,集合对象
        /// </summary>
        /// <param name="items">需删除的IList对象</param>
        /// <returns></returns>
        public bool Del(string ids)
        {

            IList<TbDimLib> tmpList = GetByPage("id in (" + ids + ")", 0, int.MaxValue);
            return Del(tmpList);



        }




        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="item">需要插入的对象</param>
        /// <returns></returns>
        public int Insert(TbDimLib item)
        {
            int id = 0;
            using (var connection = DataBase.GetOpenConnection())
            {
                id = (int)connection.Insert<TbDimLib>(item);
            }
            return id;
        }


        /// <summary>
        /// 插入一个IList对象
        /// </summary>
        /// <param name="items">需要插入的对象集合</param>
        /// <returns></returns>
        public bool Insert(IList<TbDimLib> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                foreach (var item in items)
                {
                    connection.Insert<TbDimLib>(item);
                }
                res = true;
            }
            return res;
        }





        #endregion 表生成代码

        #region 兼容代码





        public IList<TbDimLib> findByProperty(String propertyName, Object value)
        {
            IList<TbDimLib> tmpList;
            try
            {

                string whereC = "";
                string FirC = propertyName.Substring(0, 1);
                if (FirC != "I" || FirC != "F")
                {

                    value = "'" + value.ToString() + "'";
                }
                whereC = propertyName + "= " + value.ToString();



                tmpList = GetByPage(whereC, 0, int.MaxValue);
                return tmpList;
            }
            catch (Exception re)
            {
                throw re;
            }
        }//end for findByProperty



        /*
     *get the Object through SLibName
     */
        public IList<TbDimLib> findBySLibName(Object _SLibName)
        {
            return findByProperty("SLibName", _SLibName);
        }


        /*
     *get the Object through STableName
     */
        public IList<TbDimLib> findBySTableName(Object _STableName)
        {
            return findByProperty("STableName", _STableName);
        }


        /*
     *get the Object through DCreate
     */
        public IList<TbDimLib> findByDCreate(Object _DCreate)
        {
            return findByProperty("DCreate", _DCreate);
        }


        /*
     *get the Object through SIntro
     */
        public IList<TbDimLib> findBySIntro(Object _SIntro)
        {
            return findByProperty("SIntro", _SIntro);
        }


        /*
     *get the Object through SMemo
     */
        public IList<TbDimLib> findBySMemo(Object _SMemo)
        {
            return findByProperty("SMemo", _SMemo);
        }


        #endregion 兼容代码



        #region 自定义代码


        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int CreateTable(string  tableName)
        {
            int res = -1;
            using (var connection = DataBase.GetOpenConnection())
            {
                var parameter = new { tableName = tableName, };
                //调用存储过程，执行动态创建表
                res = connection.Execute("dbo.procCreateTable", parameter, commandType: CommandType.StoredProcedure);

            }
            return res;
        }


        /// <summary>
        /// 更新表名
        /// </summary>
        /// <param name="oldtableName">旧的表名</param>
        /// <param name="newtableName">新的表名</param>
        /// <returns></returns>
        public int UpdateTableName(string oldtableName,  string newtableName)
        {
            int res = -1;
            using (var connection = DataBase.GetOpenConnection())
            {
                var parameter = new { @objname = oldtableName, @newname = newtableName };
                //调用存储过程，执行更新表名
                res = connection.Execute("sp_rename", parameter, commandType: CommandType.StoredProcedure);

            }
            return res;
        }




        /// <summary>
        /// 删除表
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, true)]
        public int DeleteTable(String tableName)
        {
            using (var connection = DataBase.GetOpenConnection())
            {

                int i = connection.Execute("drop table " + tableName);
                return i;

            }
        }




        #endregion 自定义代码

    }
}