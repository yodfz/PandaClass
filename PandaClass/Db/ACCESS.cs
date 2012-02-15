using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace PandaClass.Db
{
    public class ACCESS
    {
        public static String ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString.ToString();

        public static OleDbConnection StaticConn = new OleDbConnection(ConnString);
        /// <summary>
        /// 返回一个Parameter
        /// </summary>
        /// <param name="Text">项</param>
        /// <param name="Value">值</param>
        /// <returns></returns>
        public static OleDbParameter MakeParameter(String Text, String Value)
        {
            OleDbParameter Par = new OleDbParameter(Text, Value);
            return Par;
        }


        /// <summary>
        /// 执行单句SQL
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <returns></returns>
        public static int ConnExecute(String Sql)
        {
            try
            {
                using (OleDbConnection Conn = new OleDbConnection(ConnString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(Sql, Conn))
                    {
                        Conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return 0;
        }

        /// <summary>
        /// 执行SQL
        /// Date:2009-10-21
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <param name="Par">参数</param>
        /// <returns></returns>
        public static int ConnExecute(String Sql, OleDbParameter[] Par)
        {
            using (OleDbConnection Conn = new OleDbConnection(ConnString))
            {
                using (OleDbCommand cmd = new OleDbCommand(Sql, Conn))
                {
                    Conn.Open();
                    for (int i = 0; i < Par.Length; i++)
                    {
                        cmd.Parameters.Add(Par[i]);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <returns></returns>
        public static object ExecuteScalar(String Sql)
        {
            using (OleDbConnection Conn = new OleDbConnection(ConnString))
            {
                using (OleDbCommand cmd = new OleDbCommand(Sql, Conn))
                {
                    Conn.Open();
                    object ob = cmd.ExecuteScalar();
                    if (ob != null)
                    {
                        return ob;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
        }


        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <returns></returns>
        public static object ExecuteScalar(String Sql, OleDbParameter[] Par)
        {
            using (OleDbConnection Conn = new OleDbConnection(ConnString))
            {
                using (OleDbCommand cmd = new OleDbCommand(Sql, Conn))
                {
                    Conn.Open();
                    for (int i = 0; i < Par.Length; i++)
                    {
                        cmd.Parameters.Add(Par[i]);
                    }



                    object ob = cmd.ExecuteScalar();
                    if (ob != null)
                    {
                        return ob;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
        }


        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(String Sql)
        {
            //using (OleDbConnection Conn = new OleDbConnection(ConnString))
            //{
            //OleDbConnection Conn = new OleDbConnection(ConnString);
            using (OleDbCommand cmd = new OleDbCommand(Sql, StaticConn))
            {
                if (StaticConn.State == System.Data.ConnectionState.Closed)
                {
                    StaticConn.Open();
                }


                OleDbDataReader Dr = cmd.ExecuteReader();

                return Dr;
            }
            //}
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(String Sql, OleDbParameter[] Par)
        {
            //using (OleDbConnection Conn = new OleDbConnection(ConnString))
            //{
            //OleDbConnection Conn = new OleDbConnection(ConnString);
            using (OleDbCommand cmd = new OleDbCommand(Sql, StaticConn))
            {
                if (StaticConn.State == System.Data.ConnectionState.Closed)
                {
                    StaticConn.Open();
                }
                for (int i = 0; i < Par.Length; i++)
                {
                    cmd.Parameters.Add(Par[i]);
                }



                OleDbDataReader Dr = cmd.ExecuteReader();

                cmd.Dispose();

                return Dr;
            }
            //}
        }

        /// <summary>
        /// 读取一张表
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <returns></returns>
        public static DataTable Read(String Sql)
        {
            using (OleDbConnection Conn = new OleDbConnection(ConnString))
            {
                using (OleDbDataAdapter Da = new OleDbDataAdapter(Sql, Conn))
                {
                    DataTable Dt = new DataTable();
                    Conn.Open();
                    Da.Fill(Dt);
                    return Dt;
                }
            }
        }







        public static string recordID(string query, int passCount)
        {
            using (OleDbConnection Conn = new OleDbConnection(ConnString))
            {
                Conn.Open();

                using (OleDbCommand cmd = new OleDbCommand(query, Conn))
                {
                    string result = string.Empty;
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (passCount < 1)
                            {
                                result += "," + dr.GetInt32(0);
                            }
                            passCount--;
                        }
                    }
                    return result.Substring(1);
                }
            }
        }
        /// <summary>
        /// 获取当前页应该显示的记录，注意：查询中必须包含名为ID的自动编号列
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">分页容量</param>
        /// <param name="showString">显示的字段</param>
        /// <param name="TableName">表名</param>
        /// <param name="whereString">查询条件，若有条件限制则必须以where 开头</param>
        /// <param name="orderString">排序规则</param>
        /// <param name="pageCount">传出参数：总页数统计</param>
        /// <param name="recordCount">传出参数：总记录统计</param>
        /// <returns>装载记录的DataTable</returns>
        public static DataTable ExecutePager(int pageIndex, int pageSize, string showString, string TableName, string whereString, string orderString, out int pageCount, out int recordCount)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (String.IsNullOrEmpty(showString)) showString = "*";
            if (String.IsNullOrEmpty(orderString)) orderString = "ID desc";
            using (OleDbConnection Conn = new OleDbConnection(ConnString))
            {
                //String myVw = String.Format (" ( {0} ) tempVw " , queryString);
                String myVw = TableName;
                OleDbCommand cmdCount = new OleDbCommand(String.Format(" select count(0) as recordCount from {0} {1}", myVw, whereString), Conn);

                Conn.Open();
                recordCount = Convert.ToInt32(cmdCount.ExecuteScalar());

                if ((recordCount % pageSize) > 0)
                    pageCount = recordCount / pageSize + 1;
                else
                    pageCount = recordCount / pageSize;
                OleDbCommand cmdRecord;
                if (pageIndex == 1)//第一页
                {
                    cmdRecord = new OleDbCommand(String.Format("select top {0} {1} from {2} {3} order by {4} ", pageSize, showString, myVw, whereString, orderString), Conn);
                }
                else if (pageIndex > pageCount)//超出总页数
                {
                    cmdRecord = new OleDbCommand(String.Format("select top {0} {1} from {2} {3} order by {4} ", pageSize, showString, myVw, "where 1=2", orderString), Conn);
                }
                else
                {
                    int pageLowerBound = pageSize * pageIndex;
                    int pageUpperBound = pageLowerBound - pageSize;
                    String recordIDs = recordID(String.Format("select top {0} {1} from {2} {3} order by {4} ", pageLowerBound, "ID", myVw, whereString, orderString), pageUpperBound);
                    cmdRecord = new OleDbCommand(String.Format("select {0} from {1} where id in ({2}) order by {3} ", showString, myVw, recordIDs, orderString), Conn);

                }
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmdRecord);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                return dt;
            }
        }
    }
}
