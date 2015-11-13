using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using TmsDapper;

using HyCtbuEco.Entities;

namespace HyCtbuEco.Dim
{
    /// <summary>
    ///		此用于处理表对象 'tb_DimIndCate' 
    /// </summary>
    /// <remarks>
    /// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
    ///注意:此代码可能和同名的视图生成一个完整对象
    /// </remarks>



    public partial class DimIndCateBaseDAO
    {

        #region 表生成代码
        protected int TotalCount = -1;


        /// <summary>


        /// <summary>
        /// 取所有的对象,例如IList<???>
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, true)]
        public IList<TbDimIndCate> GetAll()
        {
            IList<TbDimIndCate> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimIndCate>("select * from tb_DimIndCate").ToList<TbDimIndCate>();


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
        public IList<TbDimIndCate> GetByWhere(string where, string orderby = "", int topNum = -1)
        {
            IList<TbDimIndCate> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {
                string strSQL = "";
                if (topNum <= 0) { strSQL = "select * from tb_DimIndCate"; }
                else { strSQL = "select top " + topNum + " * from tb_DimIndCate"; };

                if (!string.IsNullOrEmpty(where))
                {
                    strSQL += " where " + where;
                }

                if (!string.IsNullOrEmpty(orderby))
                {
                    strSQL += " order by " + orderby;
                }

                tmplist = connection.Query<TbDimIndCate>(strSQL).ToList<TbDimIndCate>();


            }
            return tmplist;
        }




        /// <summary>
        /// 通过ID查询对象
        /// </summary>
        /// <param name="id">对象的ID</param>
        /// <returns></returns>
        public TbDimIndCate GetByID(int id)
        {
            TbDimIndCate curItem = null;
            using (var connection = DataBase.GetOpenConnection())
            {

                curItem = connection.Get<TbDimIndCate>(id);


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
        public IList<TbDimIndCate> GetByPage(string whereClause, int start, int limit, string sort, string dir, out int count)
        {

            IList<TbDimIndCate> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_DimIndCate", whereClause, start, limit, sort, dir);
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimIndCate>(Sqlstring).ToList<TbDimIndCate>();

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
        public IList<TbDimIndCate> GetByPage(string whereClause, int start, int limit)
        {

            IList<TbDimIndCate> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_DimIndCate", whereClause, start, limit, "", "");
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimIndCate>(Sqlstring).ToList<TbDimIndCate>();


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

        public IList<TbDimIndCate> GetByPageDataBase(int start, int limit, string sort, string whereClause, out int count)
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
                string sqlStr = "select count(1) from tb_DimIndCate";
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
        public bool Update(TbDimIndCate item)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {

                res = connection.Update<TbDimIndCate>(item);

            }
            return res;
        }

        /// <summary>
        /// 通过对象更新记录
        /// </summary>
        /// <param name="item">需要更新的对象</param>
        /// <returns></returns>
        public bool Update(IList<TbDimIndCate> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                foreach (var item in items)
                {
                    res = connection.Update<TbDimIndCate>(item);
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
            TbDimIndCate item = new TbDimIndCate();
            using (var connection = DataBase.GetOpenConnection())
            {
                item = GetByID(id);
                res = connection.Delete<TbDimIndCate>(item);
            }
            return res;
        }

        /// <summary>
        /// 通过对象删除记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Del(TbDimIndCate item)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                res = connection.Delete<TbDimIndCate>(item);
            }
            return res;
        }

        /// <summary>
        /// 通过对象删除记录,集合对象
        /// </summary>
        /// <param name="items">需删除的IList对象</param>
        /// <returns></returns>
        public bool Del(IList<TbDimIndCate> items)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {

                foreach (var item in items)
                {
                    connection.Delete<TbDimIndCate>(item);
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

            IList<TbDimIndCate> tmpList = GetByPage("id in (" + ids + ")", 0, int.MaxValue);
            return Del(tmpList);



        }




        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="item">需要插入的对象</param>
        /// <returns></returns>
        public int Insert(TbDimIndCate item)
        {
            int id = 0;
            using (var connection = DataBase.GetOpenConnection())
            {
                id = (int)connection.Insert<TbDimIndCate>(item);
            }
            return id;
        }


        /// <summary>
        /// 插入一个IList对象
        /// </summary>
        /// <param name="items">需要插入的对象集合</param>
        /// <returns></returns>
        public bool Insert(IList<TbDimIndCate> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                foreach (var item in items)
                {
                    connection.Insert<TbDimIndCate>(item);
                }
                res = true;
            }
            return res;
        }





        //	CREATE Function [dbo].[fn_DimIndCateGetChildren]

        //	(@root as int) returns @Subs TABLE

        //	( ID int  not null,

        //			lvl int not null,

        //	unique	clustered(lvl,id)   ---用于筛选级别的索引

        //	)AS

        //	BEGIN

        //		declare @lvl as int;

        //	set @lvl=0;

        //	Insert into @Subs(id,lvl) select id,@lvl from dbo.tb_DimIndCate where id=@root; --插入根结点

        //	while @@rowcount >0   --当存在上级员工

        //	begin

        //		set @lvl=@lvl+1;  --递增级别计数器

        //		insert into @subs(id,lvl)

        //			select c.id,@lvl

        //			from @Subs as p			--p=父级

        //		join dbo.tb_DimIndCate as c			--c=子级

        //		on p.lvl=@lvl -1      --从上一级筛选父亲

        //			and c.I_ParentID=p.id;

        //	end

        //	RETURN;

        //	END





        /// <summary>
        /// 通过ID来删除对象，加强版，包括其子树,注意：需要使用对应的函数fn_XXXGetChildren
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns></returns>
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Delete, true)]
        public bool DelTree(int id)
        {

            IList<TbDimIndCate> items = GetByPage("ID in (select id from [fn_DimIndCateGetChildren](" + id.ToString() + ") ", 0, int.MaxValue);

            return Del(items);

        }


        #endregion 表生成代码

        #region 兼容代码





        public IList<TbDimIndCate> findByProperty(String propertyName, Object value)
        {
            IList<TbDimIndCate> tmpList;
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
     *get the Object through SCateName
     */
        public IList<TbDimIndCate> findBySCateName(Object _SCateName)
        {
            return findByProperty("SCateName", _SCateName);
        }


        /*
     *get the Object through ILibID
     */
        public IList<TbDimIndCate> findByILibID(Object _ILibID)
        {
            return findByProperty("ILibID", _ILibID);
        }


        /*
     *get the Object through IParentID
     */
        public IList<TbDimIndCate> findByIParentID(Object _IParentID)
        {
            return findByProperty("IParentID", _IParentID);
        }


        /*
     *get the Object through ILevel
     */
        public IList<TbDimIndCate> findByILevel(Object _ILevel)
        {
            return findByProperty("ILevel", _ILevel);
        }


        /*
     *get the Object through ILeaf
     */
        public IList<TbDimIndCate> findByILeaf(Object _ILeaf)
        {
            return findByProperty("ILeaf", _ILeaf);
        }


        /*
     *get the Object through SCateIntro
     */
        public IList<TbDimIndCate> findBySCateIntro(Object _SCateIntro)
        {
            return findByProperty("SCateIntro", _SCateIntro);
        }


        /*
     *get the Object through SCateAllName
     */
        public IList<TbDimIndCate> findBySCateAllName(Object _SCateAllName)
        {
            return findByProperty("SCateAllName", _SCateAllName);
        }


        /*
     *get the Object through SMemo
     */
        public IList<TbDimIndCate> findBySMemo(Object _SMemo)
        {
            return findByProperty("SMemo", _SMemo);
        }


        /*
     *get the Object through SCateCode
     */
        public IList<TbDimIndCate> findBySCateCode(Object _SCateCode)
        {
            return findByProperty("SCateCode", _SCateCode);
        }


        #endregion 兼容代码
        #region 自定义代码



        #endregion 自定义代码

    }
}