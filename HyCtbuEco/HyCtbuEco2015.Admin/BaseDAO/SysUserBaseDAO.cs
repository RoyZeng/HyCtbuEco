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
	///		此用于处理表对象 'tb_Sys_User' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	///注意:此代码可能和同名的视图生成一个完整对象
	/// </remarks>
	

	
	public partial class SysUserBaseDAO
	{
		
	#region 表生成代码
		protected int TotalCount = -1;
		
		
		/// <summary>
       
		
		/// <summary>
        /// 取所有的对象,例如IList<???>
        /// </summary>
        /// <returns></returns>
		[System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, true)]
        public  IList<TbSysUser> GetAll()
        {
             IList<TbSysUser> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbSysUser>("select * from tb_Sys_User").ToList<TbSysUser>();


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
        public IList<TbSysUser> GetByWhere(string where, string orderby = "", int topNum = -1)
        {
            IList<TbSysUser> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {
                string strSQL = "";
                if (topNum <= 0) { strSQL = "select * from tb_Sys_User"; }
                else { strSQL = "select top " + topNum + " * from tb_Sys_User"; };

                if (!string.IsNullOrEmpty(where))
                {
                    strSQL += " where " + where;
                }

                if (!string.IsNullOrEmpty(orderby))
                {
                    strSQL += " order by " + orderby;
                }

                tmplist = connection.Query<TbSysUser>(strSQL).ToList<TbSysUser>();


            }
            return tmplist;
        }

        /// <summary>
        /// 通过ID查询对象
        /// </summary>
        /// <param name="id">对象的ID</param>
        /// <returns></returns>
        public TbSysUser GetByID(int id)
        {
            TbSysUser curItem = null;
            using (var connection = DataBase.GetOpenConnection())
            {

                curItem = connection.Get<TbSysUser>(id);


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
        public IList<TbSysUser> GetByPage(string whereClause, int start, int limit, string sort, string dir, out int count)
        {

            IList<TbSysUser> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_Sys_User", whereClause, start, limit, sort, dir);
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbSysUser>(Sqlstring).ToList<TbSysUser>();

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
        public IList<TbSysUser> GetByPage(string whereClause, int start, int limit)
        {

            IList<TbSysUser> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_Sys_User", whereClause, start, limit, "", "");
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbSysUser>(Sqlstring).ToList<TbSysUser>();

               
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

        public IList<TbSysUser> GetByPageDataBase(int start, int limit, string sort, string whereClause, out int count)
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
                string sqlStr = "select count(1) from tb_Sys_User";
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
        public bool Update(TbSysUser item)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {

                res = connection.Update<TbSysUser>(item);

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
            TbSysUser item = new TbSysUser();
            using (var connection = DataBase.GetOpenConnection())
            {
                item = GetByID(id);
                res = connection.Delete<TbSysUser>(item);
            }
            return res;
        }

		/// <summary>
        /// 通过对象删除记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Del( TbSysUser item)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                res = connection.Delete< TbSysUser>(item);
            }
            return res;
        }
		
		/// <summary>
        /// 通过对象删除记录,集合对象
        /// </summary>
        /// <param name="items">需删除的IList对象</param>
        /// <returns></returns>
        public bool Del(IList<TbSysUser > items)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
               
                foreach (var item in items)
                {
                    connection.Delete<TbSysUser >(item);
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

           IList<TbSysUser> tmpList=GetByPage("id in ("+ids+")",0,int.MaxValue);
			return  Del(tmpList);
            
            

        }
		
		
		
		
        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="item">需要插入的对象</param>
        /// <returns></returns>
        public int Insert(TbSysUser item)
        {
            int id = 0;
            using (var connection = DataBase.GetOpenConnection())
            {
                id = (int)connection.Insert<TbSysUser>(item);
            }
            return id;
        }


        /// <summary>
        /// 插入一个IList对象
        /// </summary>
        /// <param name="items">需要插入的对象集合</param>
        /// <returns></returns>
        public bool Insert(IList<TbSysUser> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                foreach (var item in items)
                {
                    connection.Insert<TbSysUser>(item);
                }
                res = true;
            }
            return res;
        }

		
		


#endregion 表生成代码

#region 兼容代码



		

		 public  IList<TbSysUser> findByProperty(String propertyName, Object value)
        {
            IList<TbSysUser> tmpList;
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
       *get the Object through SUserName
       */
	public IList<TbSysUser> findBySUserName(Object _SUserName)
        {
            return findByProperty("SUserName",_SUserName);
         }
		
		
		  /*
       *get the Object through SPassword
       */
	public IList<TbSysUser> findBySPassword(Object _SPassword)
        {
            return findByProperty("SPassword",_SPassword);
         }
		
		
		  /*
       *get the Object through DCreateDate
       */
	public IList<TbSysUser> findByDCreateDate(Object _DCreateDate)
        {
            return findByProperty("DCreateDate",_DCreateDate);
         }
		
		
		  /*
       *get the Object through DLastLoginDate
       */
	public IList<TbSysUser> findByDLastLoginDate(Object _DLastLoginDate)
        {
            return findByProperty("DLastLoginDate",_DLastLoginDate);
         }
		
		
		  /*
       *get the Object through SCIP
       */
	public IList<TbSysUser> findBySCIP(Object _SCIP)
        {
            return findByProperty("SCIP",_SCIP);
         }
		
		
		  /*
       *get the Object through SLIP
       */
	public IList<TbSysUser> findBySLIP(Object _SLIP)
        {
            return findByProperty("SLIP",_SLIP);
         }
		
		
		  /*
       *get the Object through STrueName
       */
	public IList<TbSysUser> findBySTrueName(Object _STrueName)
        {
            return findByProperty("STrueName",_STrueName);
         }
		
		
		  /*
       *get the Object through ISex
       */
	public IList<TbSysUser> findByISex(Object _ISex)
        {
            return findByProperty("ISex",_ISex);
         }
		
		
		  /*
       *get the Object through SAddr
       */
	public IList<TbSysUser> findBySAddr(Object _SAddr)
        {
            return findByProperty("SAddr",_SAddr);
         }
		
		
		  /*
       *get the Object through IPart
       */
	public IList<TbSysUser> findByIPart(Object _IPart)
        {
            return findByProperty("IPart",_IPart);
         }
		
		
		  /*
       *get the Object through IState
       */
	public IList<TbSysUser> findByIState(Object _IState)
        {
            return findByProperty("IState",_IState);
         }
		
		
		  /*
       *get the Object through SPro
       */
	public IList<TbSysUser> findBySPro(Object _SPro)
        {
            return findByProperty("SPro",_SPro);
         }
		
		
		  /*
       *get the Object through DBirth
       */
	public IList<TbSysUser> findByDBirth(Object _DBirth)
        {
            return findByProperty("DBirth",_DBirth);
         }
		
		
		  /*
       *get the Object through SNational
       */
	public IList<TbSysUser> findBySNational(Object _SNational)
        {
            return findByProperty("SNational",_SNational);
         }
		
		
		  /*
       *get the Object through SPhone
       */
	public IList<TbSysUser> findBySPhone(Object _SPhone)
        {
            return findByProperty("SPhone",_SPhone);
         }
		
		
		  /*
       *get the Object through SFax
       */
	public IList<TbSysUser> findBySFax(Object _SFax)
        {
            return findByProperty("SFax",_SFax);
         }
		
		
		  /*
       *get the Object through SIDCard
       */
	public IList<TbSysUser> findBySIDCard(Object _SIDCard)
        {
            return findByProperty("SIDCard",_SIDCard);
         }
		
		
		  /*
       *get the Object through SImg
       */
	public IList<TbSysUser> findBySImg(Object _SImg)
        {
            return findByProperty("SImg",_SImg);
         }
		
		
		  /*
       *get the Object through SSignIMG
       */
	public IList<TbSysUser> findBySSignIMG(Object _SSignIMG)
        {
            return findByProperty("SSignIMG",_SSignIMG);
         }
		
		
		  /*
       *get the Object through IScore
       */
	public IList<TbSysUser> findByIScore(Object _IScore)
        {
            return findByProperty("IScore",_IScore);
         }
		
		
		  /*
       *get the Object through IPostRecord
       */
	public IList<TbSysUser> findByIPostRecord(Object _IPostRecord)
        {
            return findByProperty("IPostRecord",_IPostRecord);
         }
		
		
		  /*
       *get the Object through ILevel
       */
	public IList<TbSysUser> findByILevel(Object _ILevel)
        {
            return findByProperty("ILevel",_ILevel);
         }
		
		
		  /*
       *get the Object through SIntroduce
       */
	public IList<TbSysUser> findBySIntroduce(Object _SIntroduce)
        {
            return findByProperty("SIntroduce",_SIntroduce);
         }
		
		
		  /*
       *get the Object through Sworkplace
       */
	public IList<TbSysUser> findBySworkplace(Object _Sworkplace)
        {
            return findByProperty("Sworkplace",_Sworkplace);
         }
		
		
		  /*
       *get the Object through SEmail
       */
	public IList<TbSysUser> findBySEmail(Object _SEmail)
        {
            return findByProperty("SEmail",_SEmail);
         }
		
		
		  /*
       *get the Object through IShowPosition
       */
	public IList<TbSysUser> findByIShowPosition(Object _IShowPosition)
        {
            return findByProperty("IShowPosition",_IShowPosition);
         }
		
		
		  /*
       *get the Object through ISort
       */
	public IList<TbSysUser> findByISort(Object _ISort)
        {
            return findByProperty("ISort",_ISort);
         }
		
		
		  /*
       *get the Object through SMem
       */
	public IList<TbSysUser> findBySMem(Object _SMem)
        {
            return findByProperty("SMem",_SMem);
         }
		
		
		  /*
       *get the Object through SRoleName
       */
	public IList<TbSysUser> findBySRoleName(Object _SRoleName)
        {
            return findByProperty("SRoleName",_SRoleName);
         }
		
		
		  /*
       *get the Object through SRoleId
       */
	public IList<TbSysUser> findBySRoleId(Object _SRoleId)
        {
            return findByProperty("SRoleId",_SRoleId);
         }
		
		
#endregion 兼容代码
#region 自定义代码


		
#endregion 自定义代码

	}
}