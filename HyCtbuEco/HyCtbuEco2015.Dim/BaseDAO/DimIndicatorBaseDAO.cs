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
	///		此用于处理表对象 'tb_DimIndicator' 
	/// </summary>
	/// <remarks>
	/// 	此结构由代码由TMS使用代码生成器书写生成，请不要直接修改.
	///注意:此代码可能和同名的视图生成一个完整对象
	/// </remarks>
	

	
	public partial class DimIndicatorBaseDAO
	{
		
	#region 表生成代码
		protected int TotalCount = -1;
		
		
		/// <summary>
       
		
		/// <summary>
        /// 取所有的对象,例如IList<???>
        /// </summary>
        /// <returns></returns>
		[System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, true)]
        public  IList<TbDimIndicator> GetAll()
        {
             IList<TbDimIndicator> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimIndicator>("select * from tb_DimIndicator").ToList<TbDimIndicator>();


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
        public  IList<TbDimIndicator> GetByWhere(string where,string orderby="",int topNum=-1)
        {
             IList<TbDimIndicator> tmplist;
            using (var connection = DataBase.GetOpenConnection())
            {
                string strSQL="";
                if(topNum<=0){strSQL="select * from tb_DimIndicator";}
                else{strSQL="select top "+topNum+" * from tb_DimIndicator";};
                
				 if (!string.IsNullOrEmpty(where))
                {
                    strSQL += " where " + where;
                }
				
                if(!string.IsNullOrEmpty(orderby)){
                    strSQL+=" order by "+orderby;
                    }

                tmplist = connection.Query<TbDimIndicator>(strSQL).ToList<TbDimIndicator>();


            }
            return tmplist;
        }
        
        

		
        /// <summary>
        /// 通过ID查询对象
        /// </summary>
        /// <param name="id">对象的ID</param>
        /// <returns></returns>
        public TbDimIndicator GetByID(int id)
        {
            TbDimIndicator curItem = null;
            using (var connection = DataBase.GetOpenConnection())
            {

                curItem = connection.Get<TbDimIndicator>(id);


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
        public IList<TbDimIndicator> GetByPage(string whereClause, int start, int limit, string sort, string dir, out int count)
        {

            IList<TbDimIndicator> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_DimIndicator", whereClause, start, limit, sort, dir);
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimIndicator>(Sqlstring).ToList<TbDimIndicator>();

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
        public IList<TbDimIndicator> GetByPage(string whereClause, int start, int limit)
        {

            IList<TbDimIndicator> tmplist;
            string Sqlstring = SQLHelper.GenPageSQL("tb_DimIndicator", whereClause, start, limit, "", "");
            using (var connection = DataBase.GetOpenConnection())
            {

                tmplist = connection.Query<TbDimIndicator>(Sqlstring).ToList<TbDimIndicator>();

               
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

        public IList<TbDimIndicator> GetByPageDataBase(int start, int limit, string sort, string whereClause, out int count)
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
                string sqlStr = "select count(1) from tb_DimIndicator";
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
        public bool Update(TbDimIndicator item)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {

                res = connection.Update<TbDimIndicator>(item);

            }
            return res;
        }

		/// <summary>
        /// 通过对象更新记录
        /// </summary>
        /// <param name="item">需要更新的对象</param>
        /// <returns></returns>
        public bool Update(IList<TbDimIndicator> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
				foreach(var item in items){
                res = connection.Update<TbDimIndicator>(item);
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
            TbDimIndicator item = new TbDimIndicator();
            using (var connection = DataBase.GetOpenConnection())
            {
                item = GetByID(id);
                res = connection.Delete<TbDimIndicator>(item);
            }
            return res;
        }

		/// <summary>
        /// 通过对象删除记录
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Del( TbDimIndicator item)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                res = connection.Delete< TbDimIndicator>(item);
            }
            return res;
        }
		
		/// <summary>
        /// 通过对象删除记录,集合对象
        /// </summary>
        /// <param name="items">需删除的IList对象</param>
        /// <returns></returns>
        public bool Del(IList<TbDimIndicator > items)
        {

            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
               
                foreach (var item in items)
                {
                    connection.Delete<TbDimIndicator >(item);
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

           IList<TbDimIndicator> tmpList=GetByPage("id in ("+ids+")",0,int.MaxValue);
			return  Del(tmpList);
            
            

        }
		
		
		
		
        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="item">需要插入的对象</param>
        /// <returns></returns>
        public int Insert(TbDimIndicator item)
        {
            int id = 0;
            using (var connection = DataBase.GetOpenConnection())
            {
                id = (int)connection.Insert<TbDimIndicator>(item);
            }
            return id;
        }


        /// <summary>
        /// 插入一个IList对象
        /// </summary>
        /// <param name="items">需要插入的对象集合</param>
        /// <returns></returns>
        public bool Insert(IList<TbDimIndicator> items)
        {
            bool res = false;
            using (var connection = DataBase.GetOpenConnection())
            {
                foreach (var item in items)
                {
                    connection.Insert<TbDimIndicator>(item);
                }
                res = true;
            }
            return res;
        }

		
		


#endregion 表生成代码

#region 兼容代码



		

		 public  IList<TbDimIndicator> findByProperty(String propertyName, Object value)
        {
            IList<TbDimIndicator> tmpList;
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
       *get the Object through ICateID
       */
	public IList<TbDimIndicator> findByICateID(Object _ICateID)
        {
            return findByProperty("ICateID",_ICateID);
         }
		
		
		  /*
       *get the Object through SIndName
       */
	public IList<TbDimIndicator> findBySIndName(Object _SIndName)
        {
            return findByProperty("SIndName",_SIndName);
         }
		
		
		  /*
       *get the Object through SIndIntro
       */
	public IList<TbDimIndicator> findBySIndIntro(Object _SIndIntro)
        {
            return findByProperty("SIndIntro",_SIndIntro);
         }
		
		
		  /*
       *get the Object through SIndCode
       */
	public IList<TbDimIndicator> findBySIndCode(Object _SIndCode)
        {
            return findByProperty("SIndCode",_SIndCode);
         }
		
		
		  /*
       *get the Object through ILibID
       */
	public IList<TbDimIndicator> findByILibID(Object _ILibID)
        {
            return findByProperty("ILibID",_ILibID);
         }
		
		
#endregion 兼容代码
#region 自定义代码


		
#endregion 自定义代码

	}
}