using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using TmsDapper;

using HyCtbuEco.Entities;

namespace HyCtbuEco.Admin
{
	/// <summary>
	///		此用于处理表对象 'tb_Sys_UserRole' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	///注意:此代码可能和同名的视图生成一个完整对象
	/// </remarks>
	

	
	public partial class SysUserRoleBaseDAO
	{
		
	#region 表生成代码
		protected int TotalCount = -1;
		
		
		/// <summary>
       
		
		/// <summary>
        /// 取所有的对象,例如IList<???>
        /// </summary>
        /// <returns></returns>
		[System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, true)]
        public  IList<TbSysUserRole> GetAll()
        {
             IList<TbSysUserRole> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbSysUserRole>("select * from tb_Sys_UserRole").ToList<TbSysUserRole>();


            }
            return tmplist;
        }

		
        /// <summary>
        /// 通过ID查询对象
        /// </summary>
        /// <param name="id">对象的ID</param>
        /// <returns></returns>
        public TbSysUserRole GetByID(int id)
        {
            TbSysUserRole curItem = null;
            using (var connection = DataBase.GetOpenConnection())
            {

                curItem = connection.Get<TbSysUserRole>(id);


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
        public IList<TbSysUserRole> GetByPage(string whereClause, int start, int limit, string sort, string dir, out int count)
        {

            IList<TbSysUserRole> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_Sys_UserRole", whereClause, start, limit, sort, dir);
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbSysUserRole>(Sqlstring).ToList<TbSysUserRole>();

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
        public IList<TbSysUserRole> GetByPage(string whereClause, int start, int limit)
        {

            IList<TbSysUserRole> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_Sys_UserRole", whereClause, start, limit, "", "");
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbSysUserRole>(Sqlstring).ToList<TbSysUserRole>();

               
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

        public IList<TbSysUserRole> GetByPageDataBase(int start, int limit, string sort, string whereClause, out int count)
        {
          

            return GetByPage(whereClause,start,limit,sort,"",out count);
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
                string sqlStr = "select count(1) from tb_Sys_UserRole";
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
        public bool Update(TbSysUserRole item)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {

                res = connection.Update<TbSysUserRole>(item);

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
            TbSysUserRole item = new TbSysUserRole();
            using (var connection = DataBase.GetOpenConnection())
            {
                item = GetByID(id);
                res = connection.Delete<TbSysUserRole>(item);
            }
            return res;
        }

		/// <summary>
        /// 通过对象删除记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Del( TbSysUserRole item)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                res = connection.Delete< TbSysUserRole>(item);
            }
            return res;
        }
		
		/// <summary>
        /// 通过对象删除记录,集合对象
        /// </summary>
        /// <param name="items">需删除的IList对象</param>
        /// <returns></returns>
        public bool Del(IList<TbSysUserRole > items)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
               
                foreach (var item in items)
                {
                    connection.Delete<TbSysUserRole >(item);
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
        public bool  Del(string ids)
        {

           IList<TbSysUserRole> tmpList=GetByPage("id in ("+ids+")",0,int.MaxValue);
			return  Del(tmpList);
            
            

        }
		
		
		
		
        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="item">需要插入的对象</param>
        /// <returns></returns>
        public int Insert(TbSysUserRole item)
        {
            int id = 0;
            using (var connection = DataBase.GetOpenConnection())
            {
                id = (int)connection.Insert<TbSysUserRole>(item);
            }
            return id;
        }


        /// <summary>
        /// 插入一个IList对象
        /// </summary>
        /// <param name="items">需要插入的对象集合</param>
        /// <returns></returns>
        public bool Insert(IList<TbSysUserRole> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                foreach (var item in items)
                {
                    connection.Insert<TbSysUserRole>(item);
                }
                res = true;
            }
            return res;
        }

		
		


#endregion 表生成代码

#region 兼容代码



		

		 public  IList<TbSysUserRole> findByProperty(String propertyName, Object value)
        {
            IList<TbSysUserRole> tmpList;
            try
            {

                string whereC = "";
				 string FirC=propertyName.Substring(0,1);
                if (FirC != "I" || FirC != "F")
                {
                
                    value="'"+value.ToString()+"'";
                }
				whereC = propertyName + "= " + value.ToString();
                

              
                tmpList = GetByPage(whereC,0,int.MaxValue);
                return tmpList;
            }
            catch (Exception re)
            {
                throw re;
            }
        }//end for findByProperty
		
		
		
		  /*
       *get the Object through IRoleId
       */
	public IList<TbSysUserRole> findByIRoleId(Object _IRoleId)
        {
            return findByProperty("IRoleId",_IRoleId);
         }
		
		
		  /*
       *get the Object through IUserId
       */
	public IList<TbSysUserRole> findByIUserId(Object _IUserId)
        {
            return findByProperty("IUserId",_IUserId);
         }
		
		
		  /*
       *get the Object through SMemo
       */
	public IList<TbSysUserRole> findBySMemo(Object _SMemo)
        {
            return findByProperty("SMemo",_SMemo);
         }
		
		
#endregion 兼容代码
#region 自定义代码


		
#endregion 自定义代码

	}
}