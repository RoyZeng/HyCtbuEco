using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using TmsDapper;

using HyXiaoMian.Entities;

namespace HyXiaoMian.Server
{
	/// <summary>
	///		此用于处理表对象 'vw_Email' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	///注意:此代码可能和同名的视图生成一个完整对象
	/// </remarks>
	

	
	public partial class VwEmailBaseDAO
	{
		
	#region 表生成代码
		protected int VwTotalCount = -1;
		
		
		/// <summary>
       
		
		/// <summary>
        /// 取所有的对象,例如IList<???>
        /// </summary>
        /// <returns></returns>
		[System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, true)]
        public  IList<VwEmail> GetAll()
        {
             IList<VwEmail> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<VwEmail>("select * from vw_Email").ToList<VwEmail>();


            }
            return tmplist;
        }

		
        /// <summary>
        /// 通过ID查询对象
        /// </summary>
        /// <param name="id">对象的ID</param>
        /// <returns></returns>
        public VwEmail GetByID(int id)
        {
            VwEmail curItem = null;
            using (var connection = DataBase.GetOpenConnection())
            {

                curItem = connection.Get<VwEmail>(id);


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
        public IList<VwEmail> GetByPageVW(string whereClause, int start, int limit, string sort, string dir, out int count)
        {

            IList<VwEmail> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("vw_Email", whereClause, start, limit, sort, dir);
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<VwEmail>(Sqlstring).ToList<VwEmail>();

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
        public IList<VwEmail> GetByPageVW(string whereClause, int start, int limit)
        {

            IList<VwEmail> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("vw_Email", whereClause, start, limit, "", "");
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<VwEmail>(Sqlstring).ToList<VwEmail>();

               
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

        public IList<VwEmail> GetByPageVWDataBase(int start, int limit, string sort, string whereClause, out int count)
        {
          

            return GetByPageVW(whereClause,start,limit,sort,"",out count);
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
                string sqlStr = "select count(1) from vw_Email";
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
        public bool Update(VwEmail item)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {

                res = connection.Update<VwEmail>(item);

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
            VwEmail item = new VwEmail();
            using (var connection = DataBase.GetOpenConnection())
            {
                item = GetByID(id);
                res = connection.Delete<VwEmail>(item);
            }
            return res;
        }

		/// <summary>
        /// 通过对象删除记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Del( VwEmail item)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                res = connection.Delete< VwEmail>(item);
            }
            return res;
        }
		
		/// <summary>
        /// 通过对象删除记录,集合对象
        /// </summary>
        /// <param name="items">需删除的IList对象</param>
        /// <returns></returns>
        public bool Del(IList<VwEmail > items)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
               
                foreach (var item in items)
                {
                    connection.Delete<VwEmail >(item);
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

           IList<VwEmail> tmpList=GetByPageVW("id in ("+ids+")",0,int.MaxValue);
			return  Del(tmpList);
            
            

        }
		
		
		
		
        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="item">需要插入的对象</param>
        /// <returns></returns>
        public int Insert(VwEmail item)
        {
            int id = 0;
            using (var connection = DataBase.GetOpenConnection())
            {
                id = (int)connection.Insert<VwEmail>(item);
            }
            return id;
        }


        /// <summary>
        /// 插入一个IList对象
        /// </summary>
        /// <param name="items">需要插入的对象集合</param>
        /// <returns></returns>
        public bool Insert(IList<VwEmail> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                foreach (var item in items)
                {
                    connection.Insert<VwEmail>(item);
                }
                res = true;
            }
            return res;
        }

		
		


#endregion 表生成代码

#region 兼容代码



		

		 public  IList<VwEmail> findByProperty(String propertyName, Object value)
        {
            IList<VwEmail> tmpList;
            try
            {

                string whereC = "";
				 string FirC=propertyName.Substring(0,1);
                if (FirC != "I" || FirC != "F")
                {
                
                    value="'"+value.ToString()+"'";
                }
				whereC = propertyName + "= " + value.ToString();
                

              
                tmpList = GetByPageVW(whereC,0,int.MaxValue);
                return tmpList;
            }
            catch (Exception re)
            {
                throw re;
            }
        }//end for findByProperty
		
		
		
		  /*
       *get the Object through SSenderNo
       */
	public IList<VwEmail> findBySSenderNo(Object _SSenderNo)
        {
            return findByProperty("SSenderNo",_SSenderNo);
         }
		
		
		  /*
       *get the Object through SSenderName
       */
	public IList<VwEmail> findBySSenderName(Object _SSenderName)
        {
            return findByProperty("SSenderName",_SSenderName);
         }
		
		
		  /*
       *get the Object through SRecNameList
       */
	public IList<VwEmail> findBySRecNameList(Object _SRecNameList)
        {
            return findByProperty("SRecNameList",_SRecNameList);
         }
		
		
		  /*
       *get the Object through STitle
       */
	public IList<VwEmail> findBySTitle(Object _STitle)
        {
            return findByProperty("STitle",_STitle);
         }
		
		
		  /*
       *get the Object through DSendtime
       */
	public IList<VwEmail> findByDSendtime(Object _DSendtime)
        {
            return findByProperty("DSendtime",_DSendtime);
         }
		
		
		  /*
       *get the Object through IEmailType
       */
	public IList<VwEmail> findByIEmailType(Object _IEmailType)
        {
            return findByProperty("IEmailType",_IEmailType);
         }
		
		
		  /*
       *get the Object through IIsDestroyAffterRead
       */
	public IList<VwEmail> findByIIsDestroyAffterRead(Object _IIsDestroyAffterRead)
        {
            return findByProperty("IIsDestroyAffterRead",_IIsDestroyAffterRead);
         }
		
		
		  /*
       *get the Object through IIsCancel
       */
	public IList<VwEmail> findByIIsCancel(Object _IIsCancel)
        {
            return findByProperty("IIsCancel",_IIsCancel);
         }
		
		
		  /*
       *get the Object through SSendIp
       */
	public IList<VwEmail> findBySSendIp(Object _SSendIp)
        {
            return findByProperty("SSendIp",_SSendIp);
         }
		
		
		  /*
       *get the Object through IIsDeleteFlag
       */
	public IList<VwEmail> findByIIsDeleteFlag(Object _IIsDeleteFlag)
        {
            return findByProperty("IIsDeleteFlag",_IIsDeleteFlag);
         }
		
		
		  /*
       *get the Object through SRecNo
       */
	public IList<VwEmail> findBySRecNo(Object _SRecNo)
        {
            return findByProperty("SRecNo",_SRecNo);
         }
		
		
		  /*
       *get the Object through SRecName
       */
	public IList<VwEmail> findBySRecName(Object _SRecName)
        {
            return findByProperty("SRecName",_SRecName);
         }
		
		
#endregion 兼容代码
#region 自定义代码


		
#endregion 自定义代码

	}
}