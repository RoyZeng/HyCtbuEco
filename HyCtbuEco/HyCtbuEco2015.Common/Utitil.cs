using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/// <summary>
/// Utitil 的摘要说明
/// </summary>

namespace TMS.Common
{
    public static class Utitil
    {
        private static int rep = 0;

        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="FirstStr">字符串1,格式如"1,2,3,4"</param>
        /// <param name="SecondStr">要找的父串,格式如"2,3,5,6"</param>
        /// <returns></returns>
        public static bool IsFirstListInSecond(string FirstStr, string SecondStr)
        {
            bool rs = false;
            string[] firstStrs = FirstStr.Split(',');
            string[] secondStrs = SecondStr.Split(',');
            List<string> tmpList = new List<string>();
            int sc = secondStrs.Length;
            int fc = firstStrs.Length;
            for (int si = 0; si < sc; si++)
            {
                string item = secondStrs[si];//去掉所有的空串
                if (item != string.Empty)
                    tmpList.Add(item);

            }
            sc = tmpList.Count;//重新取tmplist中的计数
            //以下为循环查找
            foreach (string item2 in firstStrs)
            {
                foreach (string item3 in tmpList)
                {
                    if (item2 == item3)
                    {
                        rs = true;
                        return rs;
                    }
                }

            }

            return rs;

        }
        /// <summary>
        /// 使用MD5加密算法对口令进行加密
        /// </summary>
        /// <param name="password">需要加密的口令</param>
        /// <returns>返回已经加密的字符串，长度32位</returns>
        public static string MD5(string password)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToString();
        }
        public static string getIP()
        {
            //return "127.0.0.1";
            string ip = "";
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;

        }
        /// <summary>
        /// 取当前用户的IP,32位数
        /// </summary>
        /// <returns></returns>
        public static long getIPLong()
        {

            IPAddress myIP = IPAddress.Parse(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return Convert.ToInt64(myIP.Address);
        }

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);
            foreach (byte c in bytestr)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 合并字符串数组,例如:"1,2,3,"和"2,3,5,"生成"1,2,3,5,"
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string UnionStr(string s1, string s2)
        {
            string[] strs1, strs2;
            strs1 = s1.Split(',');
            strs2 = s2.Split(',');

            System.Collections.ArrayList sc = new System.Collections.ArrayList();
            sc.AddRange(strs1);
            foreach (string s in strs2)
            {
                if (s != string.Empty && !sc.Contains(s))
                {
                    sc.Add(s);
                }
            }

            string lastStr = ",";
            foreach (string s in sc)
            {
                lastStr += s + ",";
            }
            return lastStr;
        }
        /// <summary>
        /// 返回一个三位随机数
        /// </summary>
        /// <returns></returns>
        public static string GetRnd()
        {


            int tmpR = new Random(DateTime.Now.Millisecond).Next(1, 100);
            return tmpR.ToString();


        }

        /// <summary>
        /// oDateTime 对象是DateTime对象
        /// </summary>
        /// <param name="oDateTime"></param>
        /// <returns></returns>
        public static string FormatDateTime(object oDateTime)
        {
            if (oDateTime != null)
            {
                return oDateTime.ToString();
            }
            return "";
        }
        /// <summary>
        /// oDateTime 对象是DateTime对象,FormatString是格式字符串
        /// </summary>
        /// <param name="oDateTime"></param>
        /// <param name="FormatString"></param>
        /// <returns></returns>
        public static string FormatDateTime(object oDateTime, string FormatString)
        {
            if (oDateTime != null)
                return ((DateTime)oDateTime).ToString(FormatString);
            return "";
        }
        /// <summary>
        /// 格式化数值为二位的字符串，不足位的补0
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string Format2String(int i)
        {

            if (i < 10)
            {
                return "0" + i.ToString();
            }
            else
                return i.ToString();

        }
        /// <summary>
        /// 只返回路径，不包含文件名,注意,true表示web,falase表示物理
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetOnlyDir(string filePath, bool isWeb = true)
        {
            string dirName = "";
            if (isWeb)
            {//web路径
                dirName = filePath.Substring(0, filePath.LastIndexOf("/"));
            }
            else
            {
                //物理路径
                dirName = filePath.Substring(0, filePath.LastIndexOf("\\"));
            }
            return dirName;
        }
        /// <summary>
        /// 进行文件复制.例:
        /// CopyToTarget("~\aa\bb.doc", "~\bb")返回路径~\bb\bb.doc
        /// </summary>
        /// <param name="SourcePath">源路径(包含文件名)</param>
        /// <param name="TargetPath">目的目录路径</param>
        /// <returns></returns>
        public static string CopyToTarget(string SourcePath, string TargetPath)
        {
            if (!File.Exists(SourcePath))
            {
                return "源文件不存在";
            }
            if (!Directory.Exists(TargetPath))
            {
                Directory.CreateDirectory(TargetPath);
            }
            string fileName = SourcePath.Substring(SourcePath.LastIndexOf("\\"));
            TargetPath = TargetPath + fileName;


            File.Copy(SourcePath, TargetPath);

            File.Delete(SourcePath);
            return TargetPath;
        }


        /// <summary>
        /// 进行文件复制2,注意：目标必须包括全文件名。例:
        /// CopyToTarget("~\aa\bb.doc", "~\bb\cc.doc")返回路径~\bb\bb.doc
        /// </summary>
        /// <param name="SourcePath">源路径(包含文件名)</param>
        /// <param name="TargetPath">目的目录路径</param>
        /// <returns></returns>
        public static string CopyToTarget2(string SourcePath, string TargetWebPath)
        {

            if (!File.Exists(SourcePath))
            {
                return "源文件不存在";
            }
            string targetPath = TargetWebPath.Substring(0, SourcePath.LastIndexOf("\\") + 1);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            File.Copy(SourcePath, TargetWebPath);

            File.Delete(SourcePath);
            return TargetWebPath;
        }
        /// <summary>
        /// 取文件名，比如：../aabb/cc.doc,返回cc
        /// </summary>
        /// <param name="webFileName"></param>
        /// <returns></returns>
        public static string getOnlyFileName(string webFileName)
        {
            string[] webF1 = webFileName.Split('/');
            string[] webF2 = webF1[webF1.Length - 1].Split('.');//取它的文件名
            return webF2[0];

        }


        /// <summary>
        /// 通过拼音字母返回匹配的汉字
        /// </summary>
        /// <param name="s">字母</param>
        /// <param name="condition">条件字符串</param>
        /// <returns></returns>
        public static string getsql(string s, string condition)
        {
            string sql = "";
            switch (s.ToLower())
            {

                case "a":
                    sql = " (" + condition + " >= '吖') and (" + condition + " < '巴')";
                    break;
                case "b":
                    sql = " (" + condition + " >= '巴') and (" + condition + " < '擦')";
                    break;
                case "c":
                    sql = " (" + condition + " >= '擦') and (" + condition + " < '搭')";
                    break;
                case "d":
                    sql = " (" + condition + " >= '搭') and (" + condition + " < '鹅')";
                    break;
                case "e":
                    sql = " (" + condition + " >= '鹅') and (" + condition + " < '发')";
                    break;
                case "f":
                    sql = " (" + condition + " >= '发') and (" + condition + " < '旮')";
                    break;
                case "g":
                    sql = " (" + condition + " >= '旮') and (" + condition + " < '哈')";
                    break;
                case "h":
                    sql = " (" + condition + " >= '哈') and (" + condition + " < '鸡')";
                    break;
                case "j":
                    sql = " (" + condition + " >= '鸡') and (" + condition + " < ' 喀')";
                    break;
                case "k":
                    sql = " (" + condition + " >= ' 喀') and (" + condition + " < '垃')";
                    break;
                case "l":
                    sql = " (" + condition + " >= '垃') and (" + condition + " < '妈')";
                    break;
                case "m":
                    sql = " (" + condition + " >= '妈') and (" + condition + " < '嗯')";
                    break;
                case "n":
                    sql = " (" + condition + " >= '嗯') and (" + condition + " < '哦')";
                    break;
                case "o":
                    sql = " (" + condition + " >= '哦') and (" + condition + " < '趴')";
                    break;
                case "p":
                    sql = " (" + condition + " >= '趴') and (" + condition + " < '欺')";
                    break;
                case "q":
                    sql = " (" + condition + " >= '欺') and (" + condition + " < '然')";
                    break;
                case "r":
                    sql = " (" + condition + " >= '然') and (" + condition + " < '仨')";
                    break;
                case "s":
                    sql = " (" + condition + " >= '仨') and (" + condition + " < '他')";
                    break;
                case "t":
                    sql = " (" + condition + " >= '他') and (" + condition + " < '挖')";
                    break;
                case "w":
                    sql = " (" + condition + " >= '挖') and (" + condition + " < '西')";
                    break;
                case "x":
                    sql = " (" + condition + " >= '西') and (" + condition + " < '压')";
                    break;
                case "y":
                    sql = " (" + condition + " >= '压') and (" + condition + " < '杂')";
                    break;
                case "z":
                    sql = " (" + condition + " >= '杂') and (" + condition + " < '坐')";
                    break;
                default:
                    sql = "";
                    break;

            }
            return sql;
        }





        //---------------------------下载部分的代码
        /// <summary>
        /// 导出Word,Title打印的标题,afix文件名前缀,正确返回1,否则返回错误信息
        /// </summary>
        /// <returns></returns>
        public static string ToWord(DataSet dtData, string Title, string afix)
        {

            try
            {




                System.Web.UI.WebControls.DataGrid dgExport = null;
                //   当前对话     
                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                //   IO用于导出并返回excel文件     
                System.IO.StringWriter strWriter = null;
                System.Web.UI.HtmlTextWriter htmlWriter = null;

                if (dtData != null)
                {

                    if (dtData.Tables[0].Rows.Count < 65530)
                    {
                        //   设置编码和附件格式     
                        curContext.Response.ContentType = "application/vnd.ms-word";//excel";
                        curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                        curContext.Response.Charset = "GBK";
                        DateTime currentNow = DateTime.Now;

                        // curContext.Response.AddHeader("Content-Disposition", "filename=" + "Project" + currentNow.ToString("yyyy-MM-dd") + ".xls");



                        //   导出excel文件     
                        strWriter = new System.IO.StringWriter();
                        htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

                        //   为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid     
                        dgExport = new System.Web.UI.WebControls.DataGrid();
                        dgExport.DataSource = dtData;
                        dgExport.ShowHeader = false;//***特殊情况,不显示表头



                        dgExport.AllowPaging = false;
                        dgExport.DataBind();

                        string fileName = afix + currentNow.ToString("yyyy-MM-dd-hh-mm-ss") + ".doc";
                        string filePath = curContext.Server.MapPath(".") + fileName;
                        System.IO.StreamWriter sw = System.IO.File.CreateText(filePath);



                        dgExport.RenderControl(htmlWriter);
                        sw.Write(Title);//先输出标题
                        sw.Write(strWriter.ToString());
                        sw.Close();
                        //   返回客户端     
                        DownFile(curContext.Response, fileName, filePath);
                        curContext.Response.End();
                    }
                    else
                    {
                        return "导出的Word文件行数太多，超出65530行，请缩小范围!!";

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;

            }

            return "1";
        }




        /// <summary>
        /// 导出Word,Title打印的标题,afix文件名前缀,特殊格式的Word文件,正确返回1,否则返回错误信息
        /// </summary>
        /// <returns></returns>
        public static string ToWord2(DataSet dtData, string Title, string afix)
        {

            try
            {

                System.Web.UI.WebControls.DataGrid dgExport = null;
                //   当前对话     
                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                //   IO用于导出并返回excel文件     
                //System.IO.StringWriter strWriter = null;
                // System.Web.UI.HtmlTextWriter htmlWriter = null;

                if (dtData != null)
                {

                    if (dtData.Tables[0].Rows.Count < 65530)
                    {
                        //   设置编码和附件格式     
                        curContext.Response.ContentType = "application/vnd.ms-word";//excel";
                        curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                        curContext.Response.Charset = "GBK";
                        DateTime currentNow = DateTime.Now;

                        // curContext.Response.AddHeader("Content-Disposition", "filename=" + "Project" + currentNow.ToString("yyyy-MM-dd") + ".xls");

                        DataTable dtNew = dtData.Tables[0];

                        //   导出excel文件     

                        StringBuilder strbTable = new StringBuilder();



                        for (int j = 0; j < ((dtNew.Columns.Count + 2) / 4); j++)
                        {
                            //分成四列一个表格,表格的第一列是固定不变的是,即第一列

                            strbTable.Append("<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>");

                            int rowCount = dtNew.Rows.Count - 1;


                            for (int rowI = 0; rowI <= rowCount; rowI++)
                            {

                                if (rowI != rowCount - 1)
                                    strbTable.Append("<tr height='20px'><td><b>" + dtNew.Rows[rowI][0].ToString() + "<b></td>");
                                else
                                    strbTable.Append("<tr class='RowGrid2'><td><b>" + dtNew.Rows[rowI][0].ToString() + "<b></td>");

                                for (int colJ = 1; colJ <= 4; colJ++)
                                {
                                    if ((j * 4 + colJ) < dtNew.Columns.Count) strbTable.Append("<td>" + dtNew.Rows[rowI][j * 4 + colJ].ToString() + "</td>");
                                }
                                strbTable.Append("</tr>");
                            }

                            strbTable.Append("</table><br/>");

                        }



                        string fileName = afix + currentNow.ToString("yyyy-MM-dd-hh-mm-ss") + ".doc";
                        string filePath = curContext.Server.MapPath(".") + fileName;
                        System.IO.StreamWriter sw = System.IO.File.CreateText(filePath);




                        sw.Write(Title);//先输出标题
                        sw.Write(strbTable.ToString());
                        sw.Close();
                        //   返回客户端     
                        DownFile(curContext.Response, fileName, filePath);
                        curContext.Response.End();
                    }
                    else
                    {
                        return "导出的Word文件行数太多，超出65530行，请缩小范围!!";

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;

            }

            return "1";
        }













        public static bool DownFile(System.Web.HttpResponse Response, string fileName, string fullPath)
        {
            Response.ContentType = "application/vnd.xls";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" +
            HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ";charset=UTF-8");
            System.IO.FileStream fs = System.IO.File.OpenRead(fullPath);
            try
            {

                long fLen = fs.Length;
                int size = 102400;//每100K同时下载数据 
                byte[] readData = new byte[size];//指定缓冲区的大小 
                if (size > fLen) size = Convert.ToInt32(fLen);
                long fPos = 0;
                bool isEnd = false;
                while (!isEnd)
                {
                    if ((fPos + size) < fLen)
                    {
                        size = Convert.ToInt32(fLen - fPos);
                        readData = new byte[size];
                        isEnd = false;
                    }
                    else
                    {
                        isEnd = true;
                    }
                    fs.Read(readData, 0, size);//读入一个压缩块 
                    Response.BinaryWrite(readData);
                    fPos += size;
                }
                fs.Close();

                System.IO.File.Delete(fullPath);
                return true;
            }
            catch
            {
                fs.Close();
                System.IO.File.Delete(fullPath);
                return false;
            }
            finally
            {

            }

        }



        /// <summary>
        /// 根据路径串，自动建立多级目录
        /// </summary>
        /// <param name="drPath">物理路径</param>
        /// <returns></returns>
        public static bool CreateMultiPath(string drPath)
        {
            bool isOK = false;
            string[] arrPath = drPath.Split('\\');
            string strPath = arrPath[0];
            for (int i = 1; i < arrPath.Length; i++)
            {
                strPath += "\\" + arrPath[i];
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);  //如果目录不存在 则创建该目录
                isOK = true;
            }



            return isOK;

        }






        /// <summary>
        /// 根据文件名生成随机文件外，扩展名不变,扩展方式，使用uerid
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string genRndFileName(string FileName, string UserId)
        {
            string[] fileNames = FileName.Split('.');

            string thumbnail_id = DateTime.Now.ToString("yyyyMMddHHmmssfff" + UserId);

            return thumbnail_id + "." + fileNames[fileNames.Length - 1];

        }


        //加到类的定义部分
        private static string[] _ltChinese = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        private static string[] cstr = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        private static string[] wstr = { "", "", "拾", "佰", "仟", "萬", "拾", "佰", "仟", "億", "拾", "佰", "仟" };

        /// <summary>
        /// 把十以内的阿拉伯数字转为小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToChinLitte(int value)
        {
            return _ltChinese[value];
        }


        /// <summary>
        /// 把阿拉伯数字转为中文大写，数字必须在12位整数以内的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToChinese(string str)
        {
            int len = str.Length;
            int i;
            string tmpstr, rstr;
            rstr = "";
            for (i = 1; i <= len; i++)
            {
                tmpstr = str.Substring(len - i, 1);
                rstr = string.Concat(cstr[Int32.Parse(tmpstr)] + wstr[i], rstr);
            }
            rstr = rstr.Replace("拾零", "拾");
            rstr = rstr.Replace("零拾", "零");
            rstr = rstr.Replace("零佰", "零");
            rstr = rstr.Replace("零仟", "零");
            rstr = rstr.Replace("零萬", "萬");
            for (i = 1; i <= 6; i++)
                rstr = rstr.Replace("零零", "零");
            rstr = rstr.Replace("零萬", "零");
            rstr = rstr.Replace("零億", "億");
            rstr = rstr.Replace("零零", "零");

            return rstr;
        }




        /// <summary>
        /// 根据身份证获取性别
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public static string GetSexFromIdCard(string IdCard)
        {
            string rtn; string tmp = "";
            if (IdCard.Length == 15)
            {
                tmp = IdCard.Substring(IdCard.Length - 3);
            }
            else if (IdCard.Length == 18)
            {
                tmp = IdCard.Substring(IdCard.Length - 4); tmp = tmp.Substring(0, 3);
            }
            int sx = int.Parse(tmp); int outNum;
            Math.DivRem(sx, 2, out outNum); if (outNum == 0)
            {
                rtn = "女";
            }
            else
            {
                rtn = "男";
            }
            return rtn;
        }


        /// <summary>
        /// 根据身份证获取性别,int 1为男,2为女
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public static int GetSexFromIdCardInt(string IdCard)
        {
            int rtn;
            string tmp = "";
            if (IdCard.Length == 15)
            {
                tmp = IdCard.Substring(IdCard.Length - 3);
            }
            else if (IdCard.Length == 18)
            {
                tmp = IdCard.Substring(IdCard.Length - 4); tmp = tmp.Substring(0, 3);
            }
            int sx = int.Parse(tmp); int outNum;
            Math.DivRem(sx, 2, out outNum); if (outNum == 0)
            {
                rtn = 2;
            }
            else
            {
                rtn = 1;
            }
            return rtn;
        }



        /// <summary>
        /// 清除字符串中多余的分隔符如“,,a,”换为",a"
        /// </summary>
        /// <param name="originStr">需处理的字符串</param>
        /// <param name="splitC">分隔符</param>
        /// <returns>新字符串lastStr</returns>
        public static string clearRsplit(string originStr, char splitC)
        {
            string[] str1;
            string str2 = "";
            str1 = originStr.Split(splitC);

            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != "")
                {
                    str2 = str2 + str1[i] + ",";
                }
            }
            string lastStr = "";

            lastStr = str2.Substring(0, str2.Length - 1);//清除字符串末尾的,号

            return lastStr;
        }


        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">生成的位数</param>
        /// <returns></returns>
        public static string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString() ;
            }
            return str;
        }



        /// <summary>
        /// 过滤 Sql 语句字符串中的注入脚本
        /// </summary>
        /// <param name="source">传入的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string SqlFilter(string source)
        {

            source = source.ToLower();
            //单引号替换成两个单引号
            source = source.Replace("'", "''");

            //半角封号替换为全角封号，防止多语句执行
            source = source.Replace(";", "；");

            //半角括号替换为全角括号
            source = source.Replace("(", "（");
            source = source.Replace(")", "）");

            ///////////////要用正则表达式替换，防止字母大小写得情况////////////////////

            //去除执行存储过程的命令关键字
            source = source.Replace("exec", "");
            source = source.Replace("execute", "");

            //去除系统存储过程或扩展存储过程关键字
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");

            //防止16进制注入
            source = source.Replace("0x", "0 x");

            return source;

        }




    }
}