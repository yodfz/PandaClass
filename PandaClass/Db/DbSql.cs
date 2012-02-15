using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PandaClass
{
    /// <summary>
    /// 微软MS SQL操作(支持 SQL 2005以上版本)
    /// </summary>
    public partial class DbSql
    {
        /// <summary>
        /// 基本操作
        /// </summary>
        public class Base
        {
            /// <summary>
            /// 数据库连接字符串
            /// </summary>
            private String ConnString
            {
                get;
                set;
            }
            public Base()
            {
                ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString.ToString();
            }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="_ConnString">数据库连接字符串</param>
            public Base(String _ConnString)
            {
                ConnString = _ConnString;
            }
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="Host">主机地址</param>
            /// <param name="DbName">数据库名称</param>
            /// <param name="Uid">登录账户</param>
            /// <param name="Pwd">登录密码</param>
            public Base(String Host, String DbName, String Uid, String Pwd)
            {
                StringBuilder Str = new StringBuilder("server=");
                Str.Append(Host.ToString());
                Str.Append(";uid=");
                Str.Append(Uid);
                Str.Append(";Pwd=");
                Str.Append(Pwd);
                Str.Append(";database=");
                Str.Append(DbName);
                ConnString = Str.ToString();
            }
            /// <summary>
            /// 执行SQL
            /// </summary>
            /// <param name="Sql">SQL语句</param>
            /// <returns></returns>
            public Boolean ConnExecute(String Sql)
            {
                using (SqlConnection Conn = new SqlConnection(ConnString))
                {
                    using (SqlCommand Cmd = new SqlCommand(Sql, Conn))
                    {
                        Conn.Open();
                        return (Cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            /// <summary>
            /// 执行SQL
            /// </summary>
            /// <param name="Sql">SQL语句</param>
            /// <param name="Par">参数 可以 xxx1,xxx2,xxx3</param>
            /// <returns></returns>
            public Boolean ConnExecute(String Sql, params Par[] Par)
            {
                using (SqlConnection Conn = new SqlConnection(ConnString))
                {
                    using (SqlCommand Cmd = new SqlCommand(Sql, Conn))
                    {
                        Conn.Open();
                        for (int i = 0; i < Par.Length; i++)
                        {
                            Cmd.Parameters.AddWithValue("@" + Par[i].Field, Par[i].Value);
                        }
                        return (Cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }

            /// <summary>
            /// 执行SQL
            /// </summary>
            /// <param name="Sql">SQL语句</param>
            /// <param name="Par">参数 可以 xxx1,xxx2,xxx3</param>
            /// <returns></returns>
            public Boolean ConnExecute(String Sql, List<Par> Par)
            {
                using (SqlConnection Conn = new SqlConnection(ConnString))
                {
                    using (SqlCommand Cmd = new SqlCommand(Sql, Conn))
                    {
                        Conn.Open();
                        for (int i = 0; i < Par.Count; i++)
                        {
                            Cmd.Parameters.AddWithValue("@" + Par[i].Field, Par[i].Value);
                        }
                        return (Cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }

            /// <summary>
            /// 执行SQL
            /// </summary>
            /// <param name="Sql">SQL语句</param>
            /// <param name="CmdType">执行类型 可以是SQL语句或者 存储过程</param>
            /// <param name="Par">参数 可以 xxx1,xxx2,xxx3</param>
            public Boolean ConnExecute(String Sql, CommandType CmdType, params Par[] Par)
            {
                using (SqlConnection Conn = new SqlConnection(ConnString))
                {
                    using (SqlCommand Cmd = new SqlCommand(Sql, Conn))
                    {
                        Conn.Open();
                        for (int i = 0; i < Par.Length; i++)
                        {
                            Cmd.CommandType = CmdType;
                            Cmd.Parameters.AddWithValue("@" + Par[i].Field, Par[i].Value);

                        }
                        return (Cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            /// <summary>
            /// 获取一个模型
            /// </summary>
            /// <param name="_Model">获取条件</param>
            /// <param name="ModelType">模型类型</param>
            /// <returns></returns>
            public DataTable Get(String Sql)
            {
                using (SqlConnection Conn = new SqlConnection(ConnString))
                {
                    using (SqlDataAdapter Da = new SqlDataAdapter(Sql, Conn))
                    {
                        Conn.Open();
                        DataTable Dt = new DataTable();
                        Da.Fill(Dt);
                        return Dt;
                    }
                }
            }
            /// <summary>
            /// 获取一张表
            /// </summary>
            /// <param name="_Model">获取条件</param>
            /// <param name="ModelType">模型类型</param>
            /// <returns></returns>
            public DataTable Get(String Sql, List<Par> Par)
            {
                using (SqlConnection Conn = new SqlConnection(ConnString))
                {
                    using (SqlDataAdapter Da = new SqlDataAdapter(Sql, Conn))
                    {
                        Conn.Open();
                        for (int i = 0; i < Par.Count; i++)
                        {

                            Da.SelectCommand.Parameters.AddWithValue("@" + Par[i].Field, Par[i].Value);

                        }
                        DataTable Dt = new DataTable();
                        Da.Fill(Dt);
                        return Dt;
                    }
                }
            }

            /// <summary>
            /// 获取一个模型
            /// </summary>
            /// <param name="_Model">获取条件</param>
            /// <param name="CmdType">执行类型 可以是SQL语句或者 存储过程</param>
            /// <param name="ModelType">模型类型</param>
            /// <returns></returns>
            public DataTable Get(String Sql, CommandType CmdType, List<Par> Par)
            {
                using (SqlConnection Conn = new SqlConnection(ConnString))
                {
                    using (SqlDataAdapter Da = new SqlDataAdapter(Sql, Conn))
                    {
                        Conn.Open();
                        Da.SelectCommand.CommandType = CmdType;
                        for (int i = 0; i < Par.Count; i++)
                        {

                            Da.SelectCommand.Parameters.AddWithValue("@" + Par[i].Field, Par[i].Value);

                        }
                        DataTable Dt = new DataTable();
                        Da.Fill(Dt);
                        return Dt;
                    }
                }
            }
        }
        /// <summary>
        /// 字段
        /// </summary>
        public class Par
        {
            public Par()
            {
            }
            /// <summary>
            /// 键值
            /// </summary>
            /// <param name="_Field">名称</param>
            /// <param name="_Value">对应值</param>
            public Par(String _Field, object _Value)
            {
                Field = _Field;
                Value = _Value.toString();
                if (String.IsNullOrWhiteSpace(Value))
                {
                    Value = String.Empty;
                }
                Connector = Operator.Connector.And;
                Operation = Operator.Operation.Equal;
            }
            /// <summary>
            /// 键值
            /// </summary>
            /// <param name="_Field">名称</param>
            /// <param name="_Value">对应值</param>
            /// <param name="_Connector">Sql Where 连接符</param>
            public Par(String _Field, object _Value, Operator.Connector _Connector)
            {
                Field = _Field;
                Value = _Value.toString();
                if (String.IsNullOrWhiteSpace(Value))
                {
                    Value = String.Empty;
                }
                Connector = _Connector;
                Operation = Operator.Operation.Equal;
            }
            /// <summary>
            /// 键值
            /// </summary>
            /// <param name="_Field">名称</param>
            /// <param name="_Value">对应值</param>
            /// <param name="_Connector">Sql Where 连接符</param>
            /// <param name="_Operation">Sql 操作符</param>
            public Par(String _Field, object _Value, Operator.Connector _Connector, Operator.Operation _Operation)
            {
                Field = _Field;
                Value = _Value.toString();
                if (String.IsNullOrWhiteSpace(Value))
                {
                    Value = String.Empty;
                }
                Connector = _Connector;
                Operation = _Operation;
            }



            /// <summary>
            /// 名称
            /// </summary>
            public String Field
            {
                get;
                set;
            }
            /// <summary>
            /// 值
            /// </summary>
            public String Value
            {
                get;
                set;
            }
            /// <summary>
            /// 连接符
            /// </summary>
            public Operator.Connector Connector
            {
                get;
                set;
            }

            public Operator.Operation Operation
            {
                get;
                set;
            }
            /// <summary>
            /// 数据类型(未启用
            /// )
            /// </summary>
            public Type DbType
            {
                get;
                set;
            }
        }
        /// <summary>
        /// 操作符
        /// </summary>
        public class Operator
        {
            /// <summary>
            /// SQL WHERE 连接符
            /// </summary>
            public enum Connector
            {
                And = 1,
                Or
            }
            /// <summary>
            /// SQL 操作符
            /// </summary>
            public enum Operation
            {
                /// <summary>
                /// 等于
                /// </summary>
                Equal = 1,
                /// <summary>
                /// 不等于
                /// </summary>
                UnEqual,
                /// <summary>
                /// 大于
                /// </summary>
                GreatThan,
                /// <summary>
                /// 大于等于
                /// </summary>
                GreatAndEqual,
                /// <summary>
                /// 小于
                /// </summary>
                LessThan,
                /// <summary>
                /// 小于等于
                /// </summary>
                LessAndEqual,
                /// <summary>
                /// 类似
                /// </summary>
                Like,
                /// <summary>
                /// 包含
                /// </summary>
                In,
                /// <summary>
                /// 不包含
                /// </summary>
                NotIn,
                /// <summary>
                /// 全文索引
                /// </summary>
                CONTAINS
            }
            public static String GetOperation(Operation Oprn, String Field, String Value)
            {
                switch (Oprn)
                {
                    case Operation.CONTAINS:
                        {
                            return "CONTAINS(" + Field + ",@Where_" + Field + ") ";
                        }
                    case Operation.Equal:
                        {
                            return Field + "=@Where_" + Field + " ";
                        }
                    case Operation.GreatAndEqual:
                        {
                            return Field + ">=@Where_" + Field + " ";
                        }
                    case Operation.GreatThan:
                        {
                            return Field + ">@Where_" + Field + " ";
                        }
                    case Operation.In:
                        {
                            return Field + " in (" + Value + ")" + " ";
                        }
                    case Operation.LessAndEqual:
                        {
                            return Field + "<=@Where_" + Field + " ";
                        }
                    case Operation.LessThan:
                        {
                            return Field + "<@Where_" + Field + " ";
                        }
                    case Operation.Like:
                        {
                            return Field + " like %@Where_" + Field + "% ";
                        }
                    case Operation.NotIn:
                        {
                            return Field + " not in(" + Value + ") ";
                        }
                    case Operation.UnEqual:
                        {
                            return Field + "<>@Where_" + Field + " ";
                        }
                    default:
                        {
                            throw new Exception("检测到无法识别的操作符,请检查类库的完整性!");
                        }
                }
            }

            public static String GetOperation(Operation Oprn, String Field, String Value,int i)
            {
                switch (Oprn)
                {
                    case Operation.CONTAINS:
                        {
                            return "CONTAINS(" + Field + ",@Where_" + Field + "_" + i.ToString() + ") ";
                        }
                    case Operation.Equal:
                        {
                            return Field + "=@Where_" + Field + "_" + i.ToString() + " ";
                        }
                    case Operation.GreatAndEqual:
                        {
                            return Field + ">=@Where_" + Field + "_" + i.ToString() + " ";
                        }
                    case Operation.GreatThan:
                        {
                            return Field + ">@Where_" + Field + "_" + i.ToString() + " ";
                        }
                    case Operation.In:
                        {
                            return Field + " in (" + Value + ")" + " ";
                        }
                    case Operation.LessAndEqual:
                        {
                            return Field + "<=@Where_" + Field + "_" + i.ToString() + " ";
                        }
                    case Operation.LessThan:
                        {
                            return Field + "<@Where_" + Field + "_" + i.ToString() + " ";
                        }
                    case Operation.Like:
                        {
                            return Field + " like %@Where_" + Field + "_" + i.ToString() + "% ";
                        }
                    case Operation.NotIn:
                        {
                            return Field + " not in(" + Value + ") ";
                        }
                    case Operation.UnEqual:
                        {
                            return Field + "<>@Where_" + Field + "_" + i.ToString() + " ";
                        }
                    default:
                        {
                            throw new Exception("检测到无法识别的操作符,请检查类库的完整性!");
                        }
                }
            }
        }
        /// 待修订规则:2011-01-30 21:55 赵逸风
        /// 给所有SQL 查询加入
        ///  and ()
        ///  判断是否为第一个条件
        ///  如果是第一个条件则放弃一个SQL链接字符语句
        ///  如
        ///  and(a=xxx)
        ///  and(a=xxx or b=xxx)
        /// 
        /// <summary>
        /// 对数据库角色进行派生
        /// </summary>
        public class Derived
        {
            /// <summary>
            /// 数据库表名称
            /// </summary>
            private String DataTableName = "";
            private Type ModelType;
            private DbSql.Base MSSQL;

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="DTN">表名</param>
            public Derived(String DTN, Type _ModelType)
            {
                DataTableName = DTN;
                ModelType = _ModelType;
                //默认采用
                MSSQL = new DbSql.Base(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString.ToString());

            }
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="DTN">表名</param>
            /// <param name="ConnString">连接字符串</param>
            public Derived(String DTN, Type _ModelType, String ConnString)
            {
                DataTableName = DTN;
                ModelType = _ModelType;
                MSSQL = new DbSql.Base(ConnString);
            }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="DTN">表名</param>
            /// <param name="Host">主机地址</param>
            /// <param name="DbName">数据库名称</param>
            /// <param name="Uid">登录帐号</param>
            /// <param name="Pwd">登录密码</param>
            public Derived(String DTN, Type _ModelType, String Host, String DbName, String Uid, String Pwd)
            {
                DataTableName = DTN;
                ModelType = _ModelType;
                MSSQL = new DbSql.Base(Host, DbName, Uid, Pwd);
            }
            /// <summary>
            /// 添加
            /// </summary>
            /// <param name="Par">添加的参数</param>
            /// <returns></returns>
            public Boolean Insert(params DbSql.Par[] Par)
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    StringBuilder TempSql = new StringBuilder();
                    Sql.Append("insert into ");
                    Sql.Append(DataTableName);
                    Sql.Append(" (");
                    for (int i = 0; i < Par.Length; i++)
                    {
                        Sql.Append(Par[i].Field.ToString() + ",");
                        TempSql.Append("@" + Par[i].Field.ToString() + ",");

                    }
                    Sql.Remove(Sql.Length - 1, 1);
                    TempSql.Remove(TempSql.Length - 1, 1);
                    Sql.Append(" ) values(");
                    Sql.Append(TempSql.ToString());
                    Sql.Append(")");
                    return MSSQL.ConnExecute(Sql.ToString(), Par);

                }
                catch (Exception Err)
                {
                    throw new Exception("执行插入语句出现异常,错误信息:" + Err.Message.ToString());
                }

            }

            /// <summary>
            /// 添加(扩展方法:允许返回新添加的ID值)
            /// </summary>
            /// <param name="Par">添加的参数</param>
            /// <returns></returns>
            public long InsertEx(List<DbSql.Par> Par)
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    StringBuilder TempSql = new StringBuilder();
                    Sql.Append("insert into ");
                    Sql.Append(DataTableName);
                    Sql.Append(" (");
                    for (int i = 0; i < Par.Count; i++)
                    {
                        Sql.Append(Par[i].Field.ToString() + ",");
                        TempSql.Append("@" + Par[i].Field.ToString() + ",");

                    }
                    Sql.Remove(Sql.Length - 1, 1);
                    TempSql.Remove(TempSql.Length - 1, 1);
                    Sql.Append(" ) values(");
                    Sql.Append(TempSql.ToString());
                    Sql.Append(");select @@IDENTITY");
                    DataTable MyDt = MSSQL.Get(Sql.ToString(), Par);
                    long Recordset = 0;
                    if (MyDt.Rows.Count == 1)
                    {
                        Recordset = Convert.ToInt64(MyDt.Rows[0][0]);
                    }
                    return Recordset;

                }
                catch (Exception Err)
                {
                    throw new Exception("执行插入语句出现异常,错误信息:" + Err.Message.ToString());
                }

            }

            /// <summary>
            /// 添加
            /// </summary>
            /// <param name="Par">添加的参数</param>
            /// <returns></returns>
            public Boolean Insert(List<DbSql.Par> Par)
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    StringBuilder TempSql = new StringBuilder();
                    Sql.Append("insert into ");
                    Sql.Append(DataTableName);
                    Sql.Append(" (");
                    for (int i = 0; i < Par.Count; i++)
                    {
                        Sql.Append(Par[i].Field.ToString() + ",");
                        TempSql.Append("@" + Par[i].Field.ToString() + ",");

                    }
                    Sql.Remove(Sql.Length - 1, 1);
                    TempSql.Remove(TempSql.Length - 1, 1);
                    Sql.Append(" ) values(");
                    Sql.Append(TempSql.ToString());
                    Sql.Append(")");
                    return MSSQL.ConnExecute(Sql.ToString(), Par);

                }
                catch (Exception Err)
                {
                    throw new Exception("执行插入语句出现异常,错误信息:" + Err.Message.ToString());
                }

            }
            /// <summary>
            /// 修改
            /// </summary>
            /// <param name="Where">修改条件</param>
            /// <param name="Par">修改的参数</param>
            /// <returns></returns>
            public Boolean Update(List<DbSql.Par> Where, List<DbSql.Par> Par)
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    DbSql.Par[] PostKey = new DbSql.Par[Where.Count + Par.Count];
                    Sql.Append("update ");
                    Sql.Append(DataTableName);
                    Sql.Append(" set ");
                    //修改参数
                    for (int i = 0; i < Par.Count; i++)
                    {
                        Sql.Append(Par[i].Field.ToString() + "=@" + Par[i].Field.ToString() + ",");
                        PostKey[i] = new DbSql.Par(Par[i].Field, Par[i].Value);

                    }
                    Sql.Remove(Sql.Length - 1, 1);
                    Sql.Append(" where 1=1 ");
                    if (Where.Count > 0)
                    {
                        Sql.Append(" and (");
                    }
                    //添加条件
                    for (int i = 0; i < Where.Count; i++)
                    {
                        if (i > 0)
                        {
                            Sql.Append(Where[i].Connector.ToString() + " ");
                        }
                        Sql.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                        PostKey[Par.Count + i] = new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation);
                    }
                    if (Where.Count > 0)
                    {
                        Sql.Append(")");
                    }
                    return MSSQL.ConnExecute(Sql.ToString(), PostKey);
                }
                catch (Exception Err)
                {
                    throw new Exception("更新异常:" + Err.Message.ToString());
                }
            }
            /// <summary>
            /// 删除
            /// </summary>
            /// <param name="Where">删除的条件</param>
            /// <returns></returns>
            public Boolean Delete(List<DbSql.Par> Where)
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    DbSql.Par[] PostKey = new DbSql.Par[Where.Count];
                    Sql.Append("delete from ");
                    Sql.Append(DataTableName);
                    Sql.Append(" where 1=1 ");
                    if (Where.Count > 0)
                    {
                        Sql.Append(" and (");
                    }
                    //添加条件
                    for (int i = 0; i < Where.Count; i++)
                    {
                        if (i > 0)
                        {
                            Sql.Append(Where[i].Connector.ToString() + " ");
                        }
                        Sql.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                        PostKey[i] = new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation);
                    }
                    if (Where.Count > 0)
                    {
                        Sql.Append(")");
                    }
                    return MSSQL.ConnExecute(Sql.ToString(), PostKey);
                }
                catch (Exception Err)
                {
                    throw new Exception("删除异常:" + Err.Message.ToString());
                }
            }
            /// <summary>
            /// 查询
            /// </summary>
            /// <param name="Where">查询条件</param>
            /// <returns></returns>
            public object GetInfo(List<DbSql.Par> Where, String OrderBy)
            {
                //try
                //{
                StringBuilder Sql = new StringBuilder();
                List<DbSql.Par> PostKey = new List<DbSql.Par>();

                Sql.Append("select * from ");
                Sql.Append(DataTableName);
                Sql.Append(" where 1=1 ");
                if (Where.Count > 0)
                {
                    Sql.Append(" and (");
                }
                //添加条件
                for (int i = 0; i < Where.Count; i++)
                {
                    if (i > 0)
                    {
                        Sql.Append(Where[i].Connector.ToString() + " ");
                    }
                    Sql.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                    PostKey.Add(new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation));
                }
                if (Where.Count > 0)
                {
                    Sql.Append(")");
                }
                if (!String.IsNullOrEmpty(OrderBy))
                {
                    Sql.Append(" order by ");
                    Sql.Append(OrderBy);
                }

                //进行类型转换
                NameObjectCollection OModel = new NameObjectCollection();
                DataTable Dt = MSSQL.Get(Sql.ToString(), PostKey);
                if (Dt.Rows.Count >= 1)
                {
                    for (int i = 0; i < Dt.Columns.Count; i++)
                    {
                        OModel.Add(Dt.Columns[i].ColumnName.ToString(), Dt.Rows[0][i].ToString());
                    }
                    return OModel.ConvertToModel(ModelType);
                }
                else
                {
                    return null;
                }



                //}
                //catch (Exception Err)
                //{
                //    throw new Exception("读取信息异常:" + Err.Message.ToString());
                //}
            }
            /// <summary>
            /// 查询
            /// </summary>
            /// <param name="Where">查询条件</param>
            /// <param name="ModelType">模型类型</param>
            /// <param name="OrderBy">排序条件</param>
            /// <param name="Fields">需要显示的字段名称</param>
            /// <returns></returns>
            public object GetInfo(List<DbSql.Par> Where, String OrderBy, params String[] Fields)
            {
                try
                {
                    if (Fields.Length > 0)
                    {
                        StringBuilder Sql = new StringBuilder();
                        List<DbSql.Par> PostKey = new List<DbSql.Par>();
                        Sql.Append("select ");
                        for (int i = 0; i < Fields.Length; i++)
                        {
                            Sql.Append(Fields + ",");
                        }
                        Sql.Remove(Sql.Length - 1, 1);
                        Sql.Append(" from ");
                        Sql.Append(DataTableName);
                        Sql.Append(" where 1=1 ");
                        if (Where.Count > 0)
                        {
                            Sql.Append(" and (");
                        }
                        //添加条件
                        for (int i = 0; i < Where.Count; i++)
                        {
                            if (i > 0)
                            {
                                Sql.Append(Where[i].Connector.ToString() + " ");
                            }
                            Sql.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                            PostKey.Add(new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation));
                        }
                        if (Where.Count > 0)
                        {
                            Sql.Append(")");
                        }
                        if (!String.IsNullOrEmpty(OrderBy))
                        {
                            Sql.Append(" order by ");
                            Sql.Append(OrderBy);
                        }

                        //进行类型转换
                        NameObjectCollection OModel = new NameObjectCollection();
                        DataTable Dt = MSSQL.Get(Sql.ToString(), PostKey);
                        if (Dt.Rows.Count >= 1)
                        {
                            for (int i = 0; i < Dt.Columns.Count; i++)
                            {
                                OModel.Add(Dt.Columns[i].ColumnName.ToString(), Dt.Rows[0][i].ToString());
                            }
                        }
                        return OModel.ConvertToModel(ModelType);
                    }
                    else
                    {
                        throw new Exception("显示字段为空,Fields is null!");
                    }

                }
                catch (Exception Err)
                {
                    throw new Exception("读取信息异常:" + Err.Message.ToString());
                }
            }
            /// <summary>
            /// 返回一个列表
            /// </summary>
            /// <param name="Where">条件</param>
            /// <returns></returns>
            public List<object> GetListInfo(List<DbSql.Par> Where, String OrderBy = "")
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    List<DbSql.Par> PostKey = new List<DbSql.Par>();

                    Sql.Append("select * from ");
                    Sql.Append(DataTableName);
                    Sql.Append(" where 1=1 ");
                    if (Where.Count > 0)
                    {
                        Sql.Append(" and (");
                    }
                    //添加条件
                    for (int i = 0; i < Where.Count; i++)
                    {
                        if (i > 0)
                        {
                            Sql.Append(Where[i].Connector.ToString() + " ");
                        }

                        Sql.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                        PostKey.Add(new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation));
                    }
                    if (Where.Count > 0)
                    {
                        Sql.Append(")");
                    }
                    if (!String.IsNullOrEmpty(OrderBy))
                    {
                        Sql.Append(" order by ");
                        Sql.Append(OrderBy);
                    }
                    //进行类型转换
                    NameObjectCollection OModel = new NameObjectCollection();
                    List<object> Objs = new List<object>();
                    DataTable Dt = MSSQL.Get(Sql.ToString(), PostKey);
                    for (int k = 0; k < Dt.Rows.Count; k++)
                    {
                        OModel.Clear();
                        for (int i = 0; i < Dt.Columns.Count; i++)
                        {
                            OModel.Add(Dt.Columns[i].ColumnName.ToString(), Dt.Rows[k][i].ToString());
                        }
                        Objs.Add(OModel.ConvertToModel(ModelType));
                    }
                    return Objs;
                }
                catch (Exception Err)
                {
                    throw new Exception("读取列表信息异常:" + Err.Message.ToString());
                }
            }
            /// <summary>
            /// 返回一个分页
            /// </summary>
            /// <param name="Where">条件</param>
            /// <returns></returns>
            public List<object> GetPage(List<DbSql.Par> Where, int PageSize, int PageIndex, String OrderBy, out int TotalPages, params String[] Fields)
            {
                StringBuilder Sql = new StringBuilder();
                StringBuilder SqlWhere = new StringBuilder();
                List<DbSql.Par> PostKey = new List<DbSql.Par>();
                DataTable Dt = new DataTable();
                //循环条件
                for (int i = 0; i < Where.Count; i++)
                {
                    if (i > 0)
                    {
                        SqlWhere.Append(Where[i].Connector.ToString() + " ");
                    }
                    SqlWhere.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                    PostKey.Add(new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation));
                }

                Sql.Append("select count(*) from ");
                Sql.Append(DataTableName);
                Sql.Append(" where 1=1 ");
                if (Where.Count > 0)
                {
                    Sql.Append(" and (");
                }
                Sql.Append(SqlWhere);
                if (Where.Count > 0)
                {
                    Sql.Append(")");
                }
                Dt = MSSQL.Get(Sql.ToString(), PostKey);
                double TotalRecords = 0;
                if (Dt.Rows.Count == 1)
                {
                    StringBuilder SqlFields = new StringBuilder();
                    TotalRecords = Convert.ToDouble(Dt.Rows[0][0].ToString());
                    TotalPages = Convert.ToInt16(Math.Ceiling(TotalRecords / PageSize));
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        SqlFields.Append(Fields[i].ToString() + ",");
                    }
                    SqlFields.Remove(Sql.Length - 1, 1);

                    if (PageIndex <= 0)
                    {
                        PageIndex = 1;
                    }
                    if (PageIndex > Convert.ToInt32(TotalPages))
                    {
                        PageIndex = Convert.ToInt32(TotalPages);
                    }
                    Sql.Clear();

                    Sql.Append("select ")
                       .Append(SqlFields)
                       .Append(" from (select top(")
                       .Append((PageIndex * PageSize).ToString())
                       .Append(")")
                       .Append(SqlFields)
                       .Append(",row_number() over(order by ")
                       .Append(OrderBy)
                       .Append(") as rowNumber from ")
                       .Append(DataTableName)
                       .Append(" where 1=1 ");
                    if (Where.Count > 0)
                    {
                        Sql.Append(" and (");
                    }
                    Sql.Append(SqlWhere);
                    if (Where.Count > 0)
                    {
                        Sql.Append(")");
                    }
                    Sql.Append(") t where t.rowNumber >= (")
                       .Append(((PageIndex - 1) * PageSize + 1))
                       .Append(")");

                    NameObjectCollection OModel = new NameObjectCollection();
                    List<object> Objs = new List<object>();
                    Dt = MSSQL.Get(Sql.ToString(), PostKey);
                    for (int k = 0; k < Dt.Rows.Count; k++)
                    {
                        for (int i = 0; i < Dt.Columns.Count; i++)
                        {
                            OModel.Add(Dt.Columns[i].ColumnName.ToString(), Dt.Rows[k][i].ToString());
                        }
                        Objs.Add(OModel.ConvertToModel(ModelType));
                    }
                    return Objs;
                }
                else
                {
                    TotalPages = 0;
                    return new List<object>();
                }

            }
            /// <summary>
            /// 返回一个分页
            /// </summary>
            /// <param name="Where">条件</param>
            /// <returns></returns>
            public List<T> GetPageEX<T>(List<DbSql.Par> Where, int PageSize, int PageIndex, String OrderBy, out int TotalPages, params String[] Fields)
            {
                StringBuilder Sql = new StringBuilder();
                StringBuilder SqlWhere = new StringBuilder();
                List<DbSql.Par> PostKey = new List<DbSql.Par>();
                DataTable Dt = new DataTable();
                //循环条件
                for (int i = 0; i < Where.Count; i++)
                {
                    if (i > 0)
                    {
                        SqlWhere.Append(Where[i].Connector.ToString() + " ");
                    }
                    SqlWhere.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value,i));
                    PostKey.Add(new DbSql.Par("Where_" + Where[i].Field +"_" + i.ToString(), Where[i].Value, Where[i].Connector, Where[i].Operation));
                }

                Sql.Append("select count(*) from ");
                Sql.Append(DataTableName);
                Sql.Append(" where 1=1 ");
                if (Where.Count > 0)
                {
                    Sql.Append(" and (");
                }
                Sql.Append(SqlWhere);
                if (Where.Count > 0)
                {
                    Sql.Append(")");
                }
                Dt = MSSQL.Get(Sql.ToString(), PostKey);
                double TotalRecords = 0;
                if (Dt.Rows.Count == 1)
                {
                    StringBuilder SqlFields = new StringBuilder();
                    TotalRecords = Convert.ToDouble(Dt.Rows[0][0].ToString());
                    TotalPages = Convert.ToInt16(Math.Ceiling(TotalRecords / PageSize));
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        SqlFields.Append(Fields[i].ToString() + ",");
                    }
                    SqlFields.Remove(SqlFields.Length - 1, 1);

                    if (PageIndex <= 0)
                    {
                        PageIndex = 1;
                    }
                    if (PageIndex > Convert.ToInt32(TotalPages))
                    {
                        PageIndex = Convert.ToInt32(TotalPages);
                    }
                    Sql.Clear();

                    Sql.Append("select ")
                       .Append(SqlFields)
                       .Append(" from (select top(")
                       .Append((PageIndex * PageSize).ToString())
                       .Append(")")
                       .Append(SqlFields)
                       .Append(",row_number() over(order by ")
                       .Append(OrderBy)
                       .Append(") as rowNumber from ")
                       .Append(DataTableName)
                       .Append(" where 1=1 ");
                    if (Where.Count > 0)
                    {
                        Sql.Append(" and (");
                    }
                    Sql.Append(SqlWhere);
                    if (Where.Count > 0)
                    {
                        Sql.Append(")");
                    }
                    Sql.Append(") t where t.rowNumber >= (")
                       .Append(((PageIndex - 1) * PageSize + 1))
                       .Append(")");
                    
                    
                    List<T> Objs = new List<T>();
                    Dt = MSSQL.Get(Sql.ToString(), PostKey);
                    for (int k = 0; k < Dt.Rows.Count; k++)
                    {
                        NameObjectCollection OModel = new NameObjectCollection();
                        for (int i = 0; i < Dt.Columns.Count; i++)
                        {
                            OModel.Add(Dt.Columns[i].ColumnName.ToString(), Dt.Rows[k][i].ToString());
                        }
                        Objs.Add((T)OModel.ConvertToModel(ModelType));
                    }
                    return Objs;
                }
                else
                {
                    TotalPages = 0;
                    return new List<T>();
                }

            }
            
            
            /// <summary>
            /// 返回分页
            /// </summary>
            /// <param name="Where">条件</param>
            /// <param name="PageSize">分页大小</param>
            /// <param name="PageIndex">页码</param>
            /// <param name="OrderBy">排序规则</param>
            /// <param name="TotalPages">总页码</param>
            /// <param name="Fields">显示字段</param>
            /// <returns></returns>
            public DataTable GetPageDataTable(List<DbSql.Par> Where, int PageSize, int PageIndex, String OrderBy, out int TotalPages, params String[] Fields)
            {
                StringBuilder Sql = new StringBuilder();
                StringBuilder SqlWhere = new StringBuilder();


                DataTable Dt = new DataTable();


                Sql.Append("select count(*) from ");
                Sql.Append(DataTableName);
                Sql.Append(" where 1=1 ");
                //Sql.Append(SqlWhere);

                //循环条件
                List<DbSql.Par> PostKey = new List<DbSql.Par>();
                if (Where.Count > 0)
                {
                    Sql.Append(" and (");
                }
                for (int i = 0; i < Where.Count; i++)
                {
                    if (i > 0)
                    {
                        SqlWhere.Append(Where[i].Connector.ToString() + " ");
                    }
                    SqlWhere.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                    PostKey.Add(new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation));
                }
                Sql.Append(SqlWhere);
                if (Where.Count > 0)
                {
                    Sql.Append(")");
                }

                Dt = MSSQL.Get(Sql.ToString(), PostKey);

                double TotalRecords = 0;
                if (Dt.Rows.Count == 1)
                {
                    StringBuilder SqlFields = new StringBuilder();
                    TotalRecords = Convert.ToDouble(Dt.Rows[0][0].ToString());
                    TotalPages = Convert.ToInt16(Math.Ceiling(TotalRecords / PageSize));
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        SqlFields.Append(Fields[i].ToString() + ",");
                    }
                    if (SqlFields.Length > 0)
                    {
                        SqlFields.Remove(SqlFields.Length - 1, 1);
                    }
                    if (PageIndex <= 0)
                    {
                        PageIndex = 1;
                    }
                    if (PageIndex > Convert.ToInt32(TotalPages))
                    {
                        PageIndex = Convert.ToInt32(TotalPages);
                    }
                    Sql.Clear();

                    Sql.Append("select ")
                       .Append(SqlFields)
                       .Append(" from (select top(")
                       .Append((PageIndex * PageSize).ToString())
                       .Append(")")
                       .Append(SqlFields)
                       .Append(",row_number() over(order by ")
                       .Append(OrderBy)
                       .Append(") as rowNumber from ")
                       .Append(DataTableName)
                       .Append(" where 1=1 ");
                    if (Where.Count > 0)
                    {
                        Sql.Append(" and (");
                    }
                    Sql.Append(SqlWhere);
                    if (Where.Count > 0)
                    {
                        Sql.Append(")");
                    }
                    Sql.Append(") t where t.rowNumber >= (")
                       .Append(((PageIndex - 1) * PageSize + 1))
                       .Append(")");

                    NameObjectCollection OModel = new NameObjectCollection();
                    List<object> Objs = new List<object>();
                    Dt = MSSQL.Get(Sql.ToString(), PostKey);
                    return Dt;
                }
                else
                {
                    TotalPages = 0;
                    return new DataTable();
                }

            }

            /// <summary>
            /// 返回一个列表
            /// </summary>
            /// <param name="Where">条件</param>
            /// <returns></returns>
            public DataTable GetList(List<DbSql.Par> Where, int TopIndex = 0)
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    List<DbSql.Par> PostKey = new List<DbSql.Par>();

                    Sql.Append("select " + (TopIndex > 0 ? " top " + TopIndex.toString() : "") + " * from ");
                    Sql.Append(DataTableName);
                    Sql.Append(" where 1=1 ");

                    //添加条件
                    for (int i = 0; i < Where.Count; i++)
                    {
                        Sql.Append(Where[i].Connector.ToString() + " ");
                        Sql.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                        PostKey.Add(new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation));
                    }


                    //进行类型转换
                    NameObjectCollection OModel = new NameObjectCollection();
                    List<object> Objs = new List<object>();
                    DataTable Dt = MSSQL.Get(Sql.ToString(), PostKey);

                    return Dt;
                }
                catch (Exception Err)
                {
                    throw new Exception("读取列表信息异常:" + Err.Message.ToString());
                }
            }

            /// <summary>
            /// 返回一个列表
            /// </summary>
            /// <param name="Where">条件</param>
            /// <returns></returns>
            public DataTable GetList(List<DbSql.Par> Where, String OrderBy, int TopIndex = 0)
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    List<DbSql.Par> PostKey = new List<DbSql.Par>();

                    Sql.Append("select " + (TopIndex > 0 ? " top " + TopIndex.toString() : "") + " * from ");
                    Sql.Append(DataTableName);
                    Sql.Append(" where 1=1 ");

                    //添加条件
                    for (int i = 0; i < Where.Count; i++)
                    {
                        Sql.Append(Where[i].Connector.ToString() + " ");
                        Sql.Append(DbSql.Operator.GetOperation(Where[i].Operation, Where[i].Field, Where[i].Value));
                        PostKey.Add(new DbSql.Par("Where_" + Where[i].Field, Where[i].Value, Where[i].Connector, Where[i].Operation));
                    }

                    if (!String.IsNullOrEmpty(OrderBy))
                    {
                        Sql.Append(" order by ");
                        Sql.Append(OrderBy);
                    }
                    //进行类型转换
                    NameObjectCollection OModel = new NameObjectCollection();
                    List<object> Objs = new List<object>();
                    DataTable Dt = MSSQL.Get(Sql.ToString(), PostKey);
                    //for (int k = 0 ; k < Dt.Rows.Count ; k++)
                    //{
                    //    for (int i = 0 ; i < Dt.Columns.Count ; i++)
                    //    {
                    //        OModel.Add(Dt.Columns[i].ColumnName.ToString() , Dt.Rows[k][i].ToString());
                    //    }
                    //    Objs.Add(OModel.ConvertToModel(ModelType));
                    //}
                    return Dt;
                }
                catch (Exception Err)
                {
                    throw new Exception("读取列表信息异常:" + Err.Message.ToString());
                }
            }
        }
        /// <summary>
        /// 字段数据字典集合
        /// </summary>
        public sealed class NameObjectCollection : NameObjectCollectionBase
        {
            public object this[string name]
            {
                get
                {
                    return base.BaseGet(name);

                }
                set
                {
                    base.BaseSet(name, value);
                }
            }
            public object this[int index]
            {
                get
                {
                    return base.BaseGet(index);
                }
                set
                {
                    base.BaseSet(index, value);
                }
            }

            public NameObjectCollection()
            {
            }

            public NameObjectCollection(string[] keys, object[] values)
            {
                for (int i = 0; keys != null && i < keys.Length; i++)
                {

                    if (i > values.Length - 1)
                    {
                        break;
                    }
                    else
                    {
                        Add(keys[i], values[i]);
                    }
                }
            }
            public void Add(string name, object value)
            {
                base.BaseAdd(name, value);
            }
            public void RemoveAt(int index)
            {
                base.BaseRemoveAt(index);
            }
            public void Remove(string name)
            {
                base.BaseRemove(name);
            }
            public void Clear()
            {
                base.BaseClear();
            }
            public object ConvertToModel(Type modeltype)
            {
                if (this == null || base.Count <= 0)
                {
                    return null;
                }
                else
                {
                    object model = modeltype.Assembly.CreateInstance(modeltype.ToString());
                    System.Reflection.PropertyInfo[] pinfolist = modeltype.GetProperties();
                    if (pinfolist == null || pinfolist.Length <= 0)
                    {
                        return model;
                    }
                    else
                    {
                        for (int i = 0; i < pinfolist.Length; i++)
                        {
                            object value = base.BaseGet(pinfolist[i].Name);
                            if (Convert.IsDBNull(value) || value == null || String.IsNullOrWhiteSpace(value.ToString()))
                            {
                                continue;
                            }
                            else
                            {
                                pinfolist[i].SetValue(model, Convert.ChangeType(value, pinfolist[i].PropertyType), null);
                            }
                        }
                        return model;
                    }
                }
            }
            public bool HasKey(string key)
            {
                foreach (string k in base.Keys)
                {
                    if (k == key)
                        return true;
                }
                return false;
            }
            public bool HasKey(params string[] key)
            {
                foreach (string k in key)
                {
                    if (!this.HasKey(k))
                        return false;
                }
                return true;
            }
        }
    }
}
