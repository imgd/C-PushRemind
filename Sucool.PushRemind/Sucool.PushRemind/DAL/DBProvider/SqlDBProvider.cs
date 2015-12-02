using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections;
using System.Text;



namespace Sucool.PushRemind
{
    
    public abstract class SqlDBProvider
    {
        public static string connectionstring = ConfigHelper.GetConfigConnnString("DBCONNECTION");
        /// <summary>
        /// 构造
        /// </summary>
        public SqlDBProvider()
        { }

        #region 公共方法
        /// <summary>
        /// 判断是否存在某表的某个字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <returns>是否存在</returns>
        public static bool ColumnExists(string tableName, string columnName)
        {
            string sql = "Select Count(1) From syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object res = SqlDBProvider.GetSingle(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }

        /// <summary>
        /// 获取数据表中最大ID编号,程序自动在最大ID编号上+1
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="tableName">数据表名</param>
        /// <returns>最大ID编号</returns>
        public static int GetMaxID(string fieldName, string tableName)
        {
            string sql = "Select max(" + fieldName + ")+1 From " + tableName;
            object obj = SqlDBProvider.GetSingle(sql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// 检测是否存在于某个值或记录
        /// Copyright 2008 Code By y.xiaobin
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns>逻辑值 True,False</returns>
        public static bool Exists(string strSql)
        {
            object obj = SqlDBProvider.GetSingle(strSql);
            int result;
            if ((Object.Equals(obj, null) || Object.Equals(obj, DBNull.Value)))
            {
                result = 0;
            }
            else
            {
                result = int.Parse(obj.ToString());
            }

            if (result == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 检测表是否存在
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>逻辑值 True,False</returns>
        public static bool TabExists(string TableName)
        {
            string sql = "Select Count(*) From sysobjects WHERE [id]=object_id[N'[" + TableName + "]') and OBJECTPROPERTY(id,N'IsUserTable')=1";
            object obj = SqlDBProvider.GetSingle(sql);
            int result;

            if ((Object.Equals(obj, null)) || (Object.Equals(obj, DBNull.Value)))
            {
                result = 0;
            }
            else
            {
                result = int.Parse(obj.ToString());
            }

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 检测是否存在于某个值(带参数)
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parms">存储过程参数</param>
        /// <returns>逻辑值True,False</returns>
        public static bool Exists(string strSql, params SqlParameter[] parms)
        {
            object obj = SqlDBProvider.GetSingle(strSql, parms);
            int result;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, DBNull.Value)))
            {
                result = 0;
            }
            else
            {
                result = int.Parse(obj.ToString());
            }

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region 执行简单SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <returns>影响记录数</returns>
        public static int ExecuteSql(string sqlString)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句,返回影响的记录数带超时时间设置
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="Times">超时时间</param>
        /// <returns>影响记录数</returns>
        public static int ExecuteSqlByTime(string sqlString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        #region 执行Sql和Oracle滴混合事务
        /// <summary>
        /// 执行Sql和Oracle滴混合事务
        /// </summary>
        /// <param name="list">SQL命令行列表</param>
        /// <param name="oracleCmdSqlList">Oracle命令行列表</param>
        /// <returns>执行结果 0-由于SQL造成事务失败 -1 由于Oracle造成事务失败 1-整体事务执行成功</returns>
        public static int ExecuteSqlTran(List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (CommandInfo myDE in list)
                    {
                        string cmdText = myDE.CommandText;
                        SqlParameter[] parms = (SqlParameter[])myDE.param;
                        PrepareCommand(cmd, connection, null, cmdText, parms);
                        if (myDE.EffentNextType == EffentNextType.SolicitationEvent)
                        {
                            if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                tx.Rollback();
                                throw new Exception("违背要求" + myDE.CommandText + "必须符合Select count(..的格式");
                            }
                            object obj = cmd.ExecuteScalar();
                            bool isHave = false;
                            if (obj == null && obj == DBNull.Value)
                            {
                                isHave = false;
                            }

                            isHave = Convert.ToInt32(obj) > 0;
                            if (isHave)
                            {
                                //引发事件
                                myDE.OnSolicitationEvent();
                            }
                        }

                        if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                        {
                            if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "必须符合select count(..的格式");
                            }
                            object obj = cmd.ExecuteScalar();
                            bool isHave = false;
                            if (obj == null && obj == DBNull.Value)
                            {
                                isHave = false;
                            }
                            isHave = Convert.ToInt32(obj) > 0;
                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须大于0");
                            }

                            if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须等于0");
                            }
                            continue;
                        }

                        #region Oracle 操作
                        //string oraConnectionString = PubConstant.GetConnectionString("ConnectionStringPPC");
                        //bool res = OracleHelper.ExecuteSqlTran(oraConnectionString, oracleCmdSqlList);
                        //if (!res)
                        //{
                        //    tx.Rollback();
                        //    throw new Exception("Oracle执行失败");
                        //    // return -1;
                        //}
                        tx.Commit();
                        return 1;
                        #endregion
                    }
                    tx.Commit();
                    return 1;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    tx.Rollback();
                    throw e;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    throw e;
                }
            }
        }
        #endregion

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }



        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sqlString, string content)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand(sqlString, connection);
                SqlParameter myParams = new SqlParameter("@content", SqlDbType.NText);
                myParams.Value = content;

                cmd.Parameters.Add(myParams);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>返回一个对象</returns>
        public static object ExecuteSqlGet(string sqlString, string content)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand(sqlString, connection);
                SqlParameter myParams = new SqlParameter("@content", SqlDbType.NText);
                myParams.Value = content;
                cmd.Parameters.Add(myParams);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, DBNull.Value)))
                        return null;
                    else
                        return obj;
                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行查询语句,返回SqlDataReader(注意:调用该方法后,一定要对SqlDataReader进行Close)
        /// </summary>
        /// <param name="sqlString">SQL查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sqlString)
        {
            SqlConnection connection = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand(sqlString, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
                //using ()
                //{
                //    return myReader;
                //}
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 执行查询语句,返回SqlDataReader(注意:调用该方法后,一定要对SqlDataReader进行Close)
        /// </summary>
        /// <param name="sqlString">SQL查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sqlString, out SqlCommand cmd)
        {
            SqlConnection connection = new SqlConnection(connectionstring);
            cmd = new SqlCommand(sqlString, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
                //using ()
                //{
                //    return myReader;
                //}
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 执行查询语句,返回SqlDataReader(注意:调用该方法后,一定要对SqlDataReader进行Close)
        /// </summary>
        /// <param name="sqlString">SQL查询语句</param>
        /// <param name="parms"></param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sqlString, SqlParameter[] parms)
        {
            SqlConnection connection = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand();

            try
            {
                PrepareCommand(cmd, connection, null, sqlString, parms);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                cmd.Parameters.Clear();
                return myReader;
            }
            catch (SqlException e)
            {
                connection.Close();
                cmd.Parameters.Clear();
                throw e;
            }
            finally
            {
                cmd.Dispose();
                //connection.Close();
                //connection.Dispose();
            }

        }
        /// <summary>
        /// 执行查询语句,返回SqlDataReader(注意:调用该方法后,一定要对SqlDataReader进行Close)
        /// </summary>
        /// <param name="sqlString">SQL查询语句</param>
        /// <param name="parms"></param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sqlString, SqlParameter[] parms, out SqlCommand cmd)
        {
            SqlConnection connection = new SqlConnection(connectionstring);
            cmd = new SqlCommand();

            try
            {
                PrepareCommand(cmd, connection, null, sqlString, parms);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                cmd.Parameters.Clear();
                return myReader;
            }
            catch (SqlException e)
            {
                connection.Close();
                cmd.Parameters.Clear();
                throw e;
            }
            finally
            {
                cmd.Dispose();
                //connection.Close();
                //connection.Dispose();
            }

        }

        /// <summary>
        /// 执行SQL查询语句,返回DataSet数据集
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>返回DataSet数据集</returns>
        public static DataSet ExecuteSelect(string sqlString)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter myAdapter = new SqlDataAdapter(sqlString, connection);
                    myAdapter.Fill(ds);
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return ds;
        }



        /// <summary>
        /// 执行SQL查询语句,返回DataSet数据集
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="Times">超时时间</param>
        /// <returns>返回DataSet数据集</returns>
        public static DataSet ExecuteSelect(string sqlString, int Times)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    using (SqlDataAdapter myAdapter = new SqlDataAdapter(sqlString, connection))
                    {
                        myAdapter.SelectCommand.CommandTimeout = Times;
                        myAdapter.Fill(ds);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return ds;
        }

        #endregion




        #region 执行带参数的方法

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteSelect(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                    return ds;
                }
            }
        }
        /// <summary>
        /// 执行SQL语句,返回影响记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="param">SqlParameter集合</param>
        /// <returns>影响记录数</returns>
        public static int ExecuteNonQuery(string sqlString, params SqlParameter[] param)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, param);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 执行SQL语句,返回影响记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="param">SqlParameter集合</param>
        /// <returns>影响记录数</returns>
        public static int ExecuteNonQuery(string sqlString, IList<SqlParameter> param)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, param);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句,实现数据库事务
        /// </summary>
        /// <param name="sqlStringList">SQL语句的哈希表(key为sql语句,value是该语句的SqlParameter[])</param>
        public static int ExecuteSelectTran(Hashtable sqlStringList)
        {
            using (SqlConnection connnection = new SqlConnection(connectionstring))
            {
                connnection.Open();
                using (SqlTransaction trans = connnection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int count = 0;
                        foreach (DictionaryEntry myDE in sqlStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] param = (SqlParameter[])myDE.Value;
                            PrepareCommand(cmd, connnection, trans, cmdText, param);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            count = count + val;
                        }
                        trans.Commit();
                        return count;
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 执行多条SQL语句,实现数据库事务
        /// </summary>
        /// <param name="cmdList">SQL语句的哈希表(key为sql语句,value是该语句的SqlParameter[])</param>
        /// <returns></returns>
        public static int ExecuteSelectTran(List<CommandInfo> cmdList)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int count = 0;
                        foreach (CommandInfo myDE in cmdList)
                        {
                            string cmdText = myDE.CommandText;
                            SqlParameter[] Parms = (SqlParameter[])myDE.param;
                            PrepareCommand(cmd, connection, trans, cmdText, Parms);
                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    trans.Rollback();
                                    return 0;
                                }

                                object obj = cmd.ExecuteScalar();
                                bool isHanve = false;
                                if (obj == null && obj == DBNull.Value)
                                {
                                    isHanve = false;
                                }
                                isHanve = Convert.ToInt32(obj) > 0;

                                if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && !isHanve)
                                {
                                    trans.Rollback();
                                    return 0;
                                }

                                if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHanve)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                continue;
                            }
                            int val = cmd.ExecuteNonQuery();
                            count += val;
                            if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return count;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSelectTranWithIndentity(System.Collections.Generic.List<CommandInfo> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (CommandInfo myDE in SQLStringList)
                        {
                            string cmdText = myDE.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.param;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSelectTranWithIndentity(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <param name="Times">超时时间设置</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（int）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（int）</returns>
        public static int GetSingleAExecuteNonQuery(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommandA(cmd, connection, null, SQLString, cmdParms);
                        return cmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 执行相关SqlParameter数组参数
        /// </summary>
        /// <param name="cmd">SqlCommand集合</param>
        /// <param name="conn">数据连接</param>
        /// <param name="trans">事务</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParms">SqlParameter集</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        /// <summary>
        /// 执行相关SqlParameter数组参数
        /// </summary>
        /// <param name="cmd">SqlCommand集合</param>
        /// <param name="conn">数据连接</param>
        /// <param name="trans">事务</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParms">SqlParameter集</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, IList<SqlParameter> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 执行相关SqlParameter数组参数
        /// </summary>
        /// <param name="cmd">SqlCommand集合</param>
        /// <param name="conn">数据连接</param>
        /// <param name="trans">事务</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParms">SqlParameter集</param>
        private static void PrepareCommandA(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }

        #endregion

        #region 存储过程操作
        /// <summary>
        /// 执行存储过程,返回SqlDataReader(注意:调用该方法后,一定要对SqlDataReader进行Close)
        /// </summary>
        /// <param name="storedProcName">存储过过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader StroredProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionstring);
            SqlDataReader myReader;
            connection.Open();
            SqlCommand cmd = TectonicSqlCommand(connection, storedProcName, parameters);
            cmd.CommandType = CommandType.StoredProcedure;
            myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return myReader;
        }

        /// <summary>
        /// 执行存储过程,返回DataSet数据集
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet数据集</returns>
        public static DataSet StroredProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter myAdapter = new SqlDataAdapter();
                myAdapter.SelectCommand = TectonicSqlCommand(connection, storedProcName, parameters);
                myAdapter.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        /// <summary>
        /// 执行存储过程,返回DataSet数据集
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <param name="Times">超时时间</param>
        /// <returns>DataSet数据集</returns>
        public static DataSet StroredProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                DataSet ds = new DataSet();
                connection.Open();
                SqlDataAdapter myAdapter = new SqlDataAdapter();
                myAdapter.SelectCommand = TectonicSqlCommand(connection, storedProcName, parameters);
                myAdapter.SelectCommand.CommandTimeout = Times;
                myAdapter.Fill(ds, tableName);
                connection.Close();
                return ds;
            }
        }
        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int StroredProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffcted)
        {
            int result;
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = TectonicIntCommand(connection, storedProcName, parameters);
                rowsAffcted = cmd.ExecuteNonQuery();
                result = (int)cmd.Parameters["ReturnValue"].Value;
                connection.Close();
                cmd.Dispose();
            }
            return result;
        }


        /// <summary>
        /// 构建SqlCommand 对象(用来返回一个结果集,而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand TectonicSqlCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(storedProcName, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //myCommand.Parameters.Clear(); 
            cmd.Parameters.Clear();
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    //检查未分配值的输出参数,将其分配以DBNull.value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            return cmd;
        }

        /// <summary>
        /// 创建SqlCommand对象实例(用来返回一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        private static SqlCommand TectonicIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand cmd = TectonicSqlCommand(connection, storedProcName, parameters);
            cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return cmd;
        }

        #endregion


        #region 分页操作
        /// <summary>
        /// 储存过程分页(固定储存过程名称)
        /// </summary>
        /// <param name="pageindex">每页显示的记录数</param>
        /// <param name="pagesize">当前要显示的页数</param>
        /// <param name="primaryName">主键或者标识列名</param>
        /// <param name="selectName">筛选字段(SELECT字名，不包含select关键字)如：*或id,userid等</param>
        /// <param name="tableName">需要操作的表名(from子句，不包含from关键字)如：mytable</param>
        /// <param name="whereStr">where子句，不包含where关键字，可为""或者id>10等</param>
        /// <param name="orderName">order by 子句，不包含order by 子句，如id desc,order</param>
        /// <param name="rowsCount">返回记录总数</param>
        /// <param name="pageCount">返回总页数</param>
        /// <returns></returns>
        public static DataSet RunProcedurePager(int pageindex, int pagesize, string primaryName, string selectName, string tableName, string whereStr, string orderName, out int rowsCount, out int pageCount)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@PrimaryName",SqlDbType.NVarChar,100),
                    new SqlParameter("@SelectName",SqlDbType.NVarChar,2000),
                    new SqlParameter("@TableName",SqlDbType.NVarChar,1000),
                    new SqlParameter("@WhereStr",SqlDbType.NVarChar,2000),
                    new SqlParameter("@OrderName",SqlDbType.NVarChar,1000),
                    new SqlParameter("@RowsCount",SqlDbType.Int,4),
                    new SqlParameter("@PageCount",SqlDbType.Int,4)
                                        };
            parameters[0].Value = pageindex;
            parameters[1].Value = pagesize;
            parameters[2].Value = primaryName;
            parameters[3].Value = selectName;
            parameters[4].Value = tableName;
            parameters[5].Value = whereStr;
            parameters[6].Value = orderName;
            parameters[7].Direction = ParameterDirection.InputOutput;
            parameters[8].Direction = ParameterDirection.InputOutput;

            DataSet ds = StroredProcedure("[SN_Pager]", parameters, "sn_table");
            rowsCount = Convert.ToInt32(parameters[7].Value);
            pageCount = Convert.ToInt32(parameters[8].Value);
            return ds;
        }

        /// <summary>
        /// 储存过程分页(重载一）
        /// </summary>
        /// <param name="pageindex">每页显示的记录数</param>
        /// <param name="pagesize">当前要显示的页数</param>
        /// <param name="primaryName">主键或者标识列名</param>
        /// <param name="selectName">筛选字段(SELECT字名，不包含select关键字)如：*或id,userid等</param>
        /// <param name="tableName">需要操作的表名(from子句，不包含from关键字)如：mytable</param>
        /// <param name="whereStr">where子句，不包含where关键字，可为""或者id>10等</param>
        /// <param name="orderName">order by 子句，不包含order by 子句，如id desc,order</param>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="rowsCount">返回记录总数</param>
        /// <returns></returns>
        public static DataSet RunProcedurePager(int pageindex, int pagesize, string primaryName, string selectName, string tableName, string whereStr, string orderName, string proceName, out int rowsCount)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@PrimaryName",SqlDbType.NVarChar,100),
                    new SqlParameter("@SelectName",SqlDbType.NVarChar,2000),
                    new SqlParameter("@TableName",SqlDbType.NVarChar,1000),
                    new SqlParameter("@WhereStr",SqlDbType.NVarChar,2000),
                    new SqlParameter("@OrderName",SqlDbType.NVarChar,1000),
                    new SqlParameter("@RowsCount",SqlDbType.Int,4),
                    new SqlParameter("@PageCount",SqlDbType.Int,4)
                                        };
            parameters[0].Value = pageindex;
            parameters[1].Value = pagesize;
            parameters[2].Value = primaryName;
            parameters[3].Value = selectName;
            parameters[4].Value = tableName;
            parameters[5].Value = whereStr;
            parameters[6].Value = orderName;
            parameters[7].Direction = ParameterDirection.InputOutput;
            parameters[8].Direction = 0;
            DataSet ds = StroredProcedure(proceName, parameters, "sn_table");
            rowsCount = Convert.ToInt32(parameters[7].Value);
            return ds;
        }


        /// <summary>
        /// 分页处理
        /// </summary>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">页面大小 设置为0，读取所有</param>
        /// <param name="tablename">表名</param>
        /// <param name="tablecoulumn">显示字段</param>
        /// <param name="w">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="_index">索引编号ID</param>
        /// <param name="rowsCount">返回总记录数</param>
        /// <returns></returns>
        public static DataSet Pager(int pageindex, int pagesize, string tablename, string tablecoulumn, string w, string orderBy, string _index, out int rowsCount)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(150);
                if (pageindex == 0)
                    pageindex = 1;

                sb.Append("select top ").Append(pagesize);
                sb.Append(string.Format(" {0} FROM {1} ", tablecoulumn, tablename));
                string swh = "";
                if (!string.IsNullOrEmpty(w))
                {
                    swh = string.Format("{0} and", w);
                }

                string swhere = "";
                if (w.ToLower().IndexOf("where") == -1 && w.Length > 0)
                    swhere = string.Format("where {0}", w);

                sb.Append(string.Format(" {3} where {0} not in(select top {1} {0} from {2} {4})", _index, (pageindex - 1) * pagesize, tablename, swh, swhere));


                //计算记录总数
                rowsCount = Convert.ToInt32(SqlDBProvider.GetSingle(string.Format("select count(*) from {0} {1}", tablename, w)));
                return SqlDBProvider.ExecuteSelect(sb.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 执行分页的查询
        /// code y.xiaobin
        /// </summary>
        /// <param name="SqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，如:a.id,a.name,b.score</param>
        /// <param name="SqlTablesAndWhere">查询的表如果包含查询条件，也将条件带上，但不要包含order by子句，也不要包含"from"关键字，如:students a inner join achievement b on a.... where ....</param>
        /// <param name="IndexField">用以分页的不能重复的索引字段名，最好是主表的自增长字段，如果是多表查询，请带上表名或别名，如:a.id</param>
        /// <param name="OrderASC">排序方式,如果为true则按升序排序,false则按降序排</param>
        /// <param name="PageIndex">当前页的页码</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="RecordCount">输出参数，返回查询的总记录条数</param>
        /// <param name="PageCount">输出参数，返回查询的总页数</param>
        /// <returns>返回查询结果</returns>
        public static DataSet Pager(string SqlAllFields, string SqlTablesAndWhere, string IndexField, bool OrderASC, int PageIndex, int PageSize, out int RecordCount, out int PageCount)
        {
            RecordCount = 0;
            PageCount = 0;
            if (PageIndex < 1)
            {
                PageIndex = 1;
            }
            if (PageSize <= 0)
            {
                PageSize = 10;
            }
            string strOrder = " asc";
            if (OrderASC)
                strOrder = " desc";
            DataSet ds = null;
            using (SqlConnection connect_ = new SqlConnection(connectionstring))
            {
                try
                {
                    connect_.Open();
                    string SqlCount = "select count(*) from " + SqlTablesAndWhere;
                    SqlCommand cmd = new SqlCommand(SqlCount, connect_);
                    RecordCount = (int)cmd.ExecuteScalar();
                    if (RecordCount > 0)
                    {
                        if (RecordCount % PageSize == 0)
                        {
                            PageCount = RecordCount / PageSize;
                        }
                        else
                        {
                            PageCount = RecordCount / PageSize + 1;
                        }
                        if (PageIndex > PageCount)
                        {
                            PageIndex = PageCount;
                        }
                        string Sql = null;
                        if (PageIndex == 1)
                        {
                            Sql = "select top " + PageSize + " " + SqlAllFields + " from " + SqlTablesAndWhere + " order by " + IndexField + strOrder;
                        }
                        else
                        {
                            Sql = "select top " + PageSize + " " + SqlAllFields + " from " + SqlTablesAndWhere;
                            if (SqlTablesAndWhere.ToLower().IndexOf(" where ") > 0)
                            {
                                Sql += " and (";
                            }
                            else
                            {
                                Sql += " where (";
                            }
                            Sql += IndexField + " not in (select top " + (PageIndex - 1) * PageSize + " " + IndexField + " from " + SqlTablesAndWhere + " order by " + IndexField + strOrder + "))";
                            Sql += " order by " + IndexField + strOrder;
                        }
                        SqlDataAdapter ap = new SqlDataAdapter(Sql, connect_);
                        ds = new DataSet();
                        ap.Fill(ds);
                    }
                    else
                    {
                        PageCount = 0;
                    }
                }
                catch (SqlException SE)
                {
                    throw new Exception(SE.ToString());
                }
                finally
                {
                    if (connect_.State != ConnectionState.Closed)
                    {
                        connect_.Close();
                    }
                }
            }
            return ds;
        }

        

        /// <summary>
        /// 执行有自定义排序的分页的查询
        /// </summary>
        /// <param name="connection">SQL数据库连接对象</param>
        /// <param name="SqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，如:a.id,a.name,b.score</param>
        /// <param name="SqlTablesAndWhere">查询的表如果包含查询条件，也将条件带上，但不要包含order by子句，也不要包含"from"关键字，如:students a inner join achievement b on a.... where ....</param>
        /// <param name="IndexField">用以分页的不能重复的索引字段名，最好是主表的自增长字段，如果是多表查询，请带上表名或别名，如:a.id</param>
        /// <param name="OrderASC">排序方式,如果为true则按升序排序,false则按降序排</param>
        /// <param name="OrderFields">排序字段以及方式如：a.OrderID desc,CnName desc</OrderFields>
        /// <param name="PageIndex">当前页的页码</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="RecordCount">输出参数，返回查询的总记录条数</param>
        /// <param name="PageCount">输出参数，返回查询的总页数</param>
        /// <returns>返回查询结果</returns>
        public static DataSet ExecutePage(SqlConnection connection, string SqlAllFields, string SqlTablesAndWhere, string IndexField, string OrderFields, int PageIndex, int PageSize, out int RecordCount, out int PageCount, params SqlParameter[] commandParameters)
        {
            RecordCount = 0;
            PageCount = 0;
            if (PageSize <= 0)
            {
                PageSize = 10;
            }
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string SqlCount = "select count(*) from " + SqlTablesAndWhere;
            SqlCommand cmd = new SqlCommand(SqlCount, connection);
            if (commandParameters != null)
            {
                foreach (SqlParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
            RecordCount = (int)cmd.ExecuteScalar();
            if (RecordCount % PageSize == 0)
            {
                PageCount = RecordCount / PageSize;
            }
            else
            {
                PageCount = RecordCount / PageSize + 1;
            }
            if (PageIndex > PageCount)
                PageIndex = PageCount;
            if (PageIndex < 1)
                PageIndex = 1;
            string Sql = null;
            if (PageIndex == 1)
            {
                Sql = "select top " + PageSize + " " + SqlAllFields + " from " + SqlTablesAndWhere + " " + OrderFields;
            }
            else
            {
                Sql = "select top " + PageSize + " " + SqlAllFields + " from ";
                if (SqlTablesAndWhere.ToLower().IndexOf(" where ") > 0)
                {
                    string _where = System.Text.RegularExpressions.Regex.Replace(SqlTablesAndWhere, @"\ where\ ", " where (", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
                    Sql += _where + ") and (";
                }
                else
                {
                    Sql += SqlTablesAndWhere + " where (";
                }
                Sql += IndexField + " not in (select top " + (PageIndex - 1) * PageSize + " " + IndexField + " from " + SqlTablesAndWhere + " " + OrderFields;
                Sql += ")) " + OrderFields;
            }
            cmd.CommandText = Sql;
            SqlDataAdapter ap = new SqlDataAdapter();
            ap.SelectCommand = cmd;
            DataSet st = new DataSet();
            ap.Fill(st, "PageResult");
            cmd.Parameters.Clear();
            return st;
        }

        /// <summary>
        /// 执行有自定义排序的分页的查询
        /// </summary>
        /// <param name="connectionString">SQL数据库连接字符串</param>
        /// <param name="SqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，如:a.id,a.name,b.score</param>
        /// <param name="SqlTablesAndWhere">查询的表如果包含查询条件，也将条件带上，但不要包含order by子句，也不要包含"from"关键字，如:students a inner join achievement b on a.... where ....</param>
        /// <param name="IndexField">用以分页的不能重复的索引字段名，最好是主表的自增长字段，如果是多表查询，请带上表名或别名，如:a.id</param>
        /// <param name="OrderASC">排序方式,如果为true则按升序排序,false则按降序排</param>
        /// <param name="OrderFields">排序字段以及方式如：a.OrderID desc,CnName desc</OrderFields>
        /// <param name="PageIndex">当前页的页码</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="RecordCount">输出参数，返回查询的总记录条数</param>
        /// <param name="PageCount">输出参数，返回查询的总页数</param>
        /// <returns>返回查询结果</returns>
        public static DataSet ExecutePage(string SqlAllFields, string SqlTablesAndWhere, string IndexField, string OrderFields, int PageIndex, int PageSize, out int RecordCount, out int PageCount, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                return ExecutePage(connection, SqlAllFields, SqlTablesAndWhere, IndexField, OrderFields, PageIndex, PageSize, out RecordCount, out PageCount, commandParameters);
            }
        }
        /// <summary>
        /// 返回分页的T-sql语句
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="fields">查询字段</param>
        /// <param name="tableName">数据表 可以多表连接</param>
        /// <param name="key">主键字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式 desc,asc  默认为asc</param>
        /// <param name="orderbykey">排序字段 如果值为"" 那么按照主键字段排序</param>
        /// <param name="count">数据条数</param>
        /// <returns></returns>
        public static StringBuilder getSqlPager(int pageSize, int pageIndex, string fields, string tableName, string key, string where, string orderby, string orderbykey, out int count)
        {
            StringBuilder _sql = new StringBuilder();
            #region 排序方式为asc  则找出大于等于key的后pageSize则找出大于等于key的后pageSize调数据数据  排序方式为desc  则找出小于等于key的前pageSize条数据
            _sql.AppendFormat(" select top {0} {1}  from {2}", pageSize, fields, tableName);
            _sql.AppendFormat(" where {0} {1} ", key, (orderby == "asc" || orderby == "" ? ">=" : "<="));
            _sql.AppendFormat(" (");
            #region 排序方式为asc  查询前(pageIndex - 1)*pageSize+1最大的数据    排序方式为desc 查询前(pageIndex - 1)*pageSize+1最小的数据
            _sql.AppendFormat(" select {0}(keyId) as maxid from ", (orderby == "asc" || orderby == "" ? "max" : "min"));
            _sql.AppendFormat(" (");
            #region 查询前(pageIndex - 1) * pageSize + 1
            _sql.AppendFormat(" select top {0} {1} as keyId from {2}", ((pageIndex - 1) * pageSize + 1), key, tableName);
            _sql.AppendFormat(" where 1=1 " + (where.Trim().Length > 0 ? (" and " + where) : ""));
            _sql.AppendFormat(" order by {0} {1}", (orderbykey.Length < 1 ? key : orderbykey), orderby);
            #endregion
            _sql.AppendFormat(" ) as tmp");
            #endregion
            _sql.AppendFormat(" )");
            _sql.AppendFormat(" " + (where.Trim().Length > 0 ? (" and " + where) : ""));
            _sql.AppendFormat(" order by {0} {1}", (orderbykey.Length < 1 ? key : orderbykey), orderby);
            #endregion
            string conutsql = "select ";
            conutsql += " count(" + key + ") ";
            conutsql += "  from  " + tableName + " where 1=1 ";
            if (where != "")
            {
                conutsql += " and " + where;
            }
            object obj = SqlDBProvider.GetSingle(conutsql.ToString());
            int _conut = (obj == null) ? 0 : int.Parse(obj.ToString());
            count = _conut;
            return _sql;
        }


        /// <summary>
        /// 返回分页的T-sql语句 
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="fields">查询字段</param>
        /// <param name="tableName">数据表 可以多表连接</param>
        /// <param name="key">主键字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式 desc,asc  默认为asc</param>
        /// <param name="orderbykey">排序字段 如果值为"" 那么按照主键字段排序</param>
        /// <param name="count">数据条数</param>
        /// <returns></returns>
        public static StringBuilder getNotInSqlPager(int pageSize, int pageIndex, string fields, string tableName, string key, string where, string orderby, string orderbykey, out int count)
        {
            StringBuilder _sql = new StringBuilder();

            _sql.AppendFormat("select top {0} {1} from ", pageSize, fields);
            _sql.AppendFormat(" {0} ", tableName);
            _sql.AppendFormat(" where {0} {1} not in  ", (where.Length < 1 ? "" : where + " and "), key);
            _sql.AppendFormat("(");
            _sql.AppendFormat("select top ({0}*({1})) {2} from {3}", pageSize, (pageIndex >= 1 ? pageIndex - 1 : pageIndex), key, tableName);
            _sql.AppendFormat(" where {0} 1=1 ", (where.Length < 1 ? "" : where + " and "));
            _sql.AppendFormat(" order by {0} {1} ", orderbykey, orderby);
            _sql.AppendFormat(")");
            _sql.AppendFormat(" order by {0} {1} ", orderbykey, orderby);
            string conutsql = "select ";
            conutsql += " count(" + key + ") ";
            conutsql += "  from  " + tableName + " where 1=1 ";
            if (where != "")
            {
                conutsql += " and " + where;
            }
            object obj = SqlDBProvider.GetSingle(conutsql.ToString());
            int _conut = (obj == null) ? 0 : int.Parse(obj.ToString());
            count = _conut;
            return _sql;
        }

        /// <summary>
        /// 返回分页的T-sql语句
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="fields">查询字段</param>
        /// <param name="tableName">数据表 可以多表连接</param>
        /// <param name="key">主键字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式 desc,asc  默认为asc</param>
        /// <param name="orderbykey">排序字段 如果值为"" 那么按照主键字段排序</param>
        /// <param name="count">数据条数</param>
        /// <returns></returns>
        public static StringBuilder getSqlPager(int pageSize, int pageIndex, string fields, string tableName, string key, string where, string orderby, string orderbykey, SqlParameter[] para, out int count)
        {
            StringBuilder _sql = new StringBuilder();
            #region 排序方式为asc  则找出大于等于key的后pageSize则找出大于等于key的后pageSize调数据数据  排序方式为desc  则找出小于等于key的前pageSize条数据
            _sql.AppendFormat(" select top {0} {1}  from {2}", pageSize, fields, tableName);
            _sql.AppendFormat(" where {0} {1} ", key, (orderby == "asc" || orderby == "" ? ">=" : "<="));
            _sql.AppendFormat(" (");
            #region 排序方式为asc  查询前(pageIndex - 1)*pageSize+1最大的数据    排序方式为desc 查询前(pageIndex - 1)*pageSize+1最小的数据
            _sql.AppendFormat(" select {0}(keyId) as maxid from ", (orderby == "asc" || orderby == "" ? "max" : "min"));
            _sql.AppendFormat(" (");
            #region 查询前(pageIndex - 1) * pageSize + 1
            _sql.AppendFormat(" select top {0} {1} as keyId from {2}", (((pageIndex == 0 ? 1 : pageIndex) - 1) * pageSize + 1), key, tableName);
            _sql.AppendFormat(" where 1=1 " + (where.Trim().Length > 0 ? (" and " + where) : ""));
            _sql.AppendFormat(" order by {0} {1}", (orderbykey.Length < 1 ? key : orderbykey), orderby);
            #endregion
            _sql.AppendFormat(" ) as tmp");
            #endregion
            _sql.AppendFormat(" )");
            _sql.AppendFormat(" " + (where.Trim().Length > 0 ? (" and " + where) : ""));
            _sql.AppendFormat(" order by {0} {1}", (orderbykey.Length < 1 ? key : orderbykey), orderby);
            #endregion
            string conutsql = "select ";
            conutsql += " count(" + key + ") ";
            conutsql += "  from  " + tableName + " where 1=1 ";
            if (where != "")
            {
                conutsql += " and " + where;
            }
            object obj = SqlDBProvider.GetSingle(conutsql.ToString(), para);
            int _conut = (obj == null) ? 0 : int.Parse(obj.ToString());
            count = _conut;
            return _sql;
        }

        /// <summary>
        /// by王强:2013-07-30 16:46
        /// 最新排序返回分页的T-sql语句
        /// 没有创建索引的字段禁止进行排序
        /// 如果没有数据 不会返回可执行sql语句
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="fields">查询字段,可以带别名,eg:ProDataBasic.id,t_name,Merchant.Nickname</param>
        /// <param name="tableName">数据表 可以多表连接eg:ProDataBasic inner join Enterprise on T_Businesses=Merchant.Id</param>
        /// <param name="key">主键字段:该字段在查询结果中必须是唯一，否则会出现未知异常,eg:ProDataBasic.id</param>
        /// <param name="where">查询条件eg:ProDataBasic.id>100</param>
        /// <param name="orderby">排序方式  默认为 主键字段降序eg:id desc, addtime asc</param>
        /// <param name="count">数据条数</param>
        /// <returns></returns>
        public static StringBuilder NewGetSqlPager(int pageSize, int pageIndex, string fields, string tableName, string key, string where, string orderby, out int count)
        {

            StringBuilder _sql = new StringBuilder();
            #region 查询数据总条数,需要查询的主键id
            _sql.Append(" with tablealllist(keyid,row ) ");
            _sql.Append(" as( ");
            _sql.Append(" select " + key + " keyid ,ROW_NUMBER() over(order by " + (orderby.TrimEnd().Length == 0 ? key : orderby) + ") row from  ");
            _sql.Append(" " + tableName + "  ");
            if (where.TrimEnd().Length > 0)
            {
                _sql.Append(" where " + where);
            }
            _sql.Append(" ),procount(pcount) ");
            _sql.Append(" as( ");
            _sql.Append(" select max(row) from tablealllist ");
            _sql.Append(" ),tablelist(keyid) ");
            _sql.Append(" as( ");
            _sql.Append(" select keyid from tablealllist where row between " + ((pageIndex - 1) * pageSize + 1) + " and  " + ((pageIndex) * pageSize) + "  ");
            _sql.Append(" )select keyid,pcount from tablelist,procount  ");
            List<int> prolistid = new List<int>();
            count = 0;

            StringBuilder keyidswhere = new StringBuilder();
            using (SqlDataReader dr = SqlDBProvider.ExecuteReader(_sql.ToString()))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int keyid = int.Parse(dr["keyid"].ToString());
                        prolistid.Add(keyid);
                        keyidswhere.Append(keyid + ",");
                        count = int.Parse(dr["pcount"].ToString());
                    }
                    keyidswhere.Remove(keyidswhere.ToString().Length - 1, 1);
                }
                dr.Close();
            }
            #endregion 查询数据总条数,需要查询的主键id
            _sql.Clear();
            if (prolistid.Count > 0)
            {
                _sql.Append(" select ");
                _sql.Append(" " + fields);
                _sql.Append(" from " + tableName);
                _sql.Append(" where " + key + " in ( " + keyidswhere.ToString() + " ) order by charindex(','+convert(nvarchar," + key + ")+',','," + keyidswhere.ToString() + ",')");
            }
            else
            {
                _sql = null;
            }
            return _sql;


        }
        #endregion

        #region  扩展方法

        #region SqlBulkCopy 批量 插入/更新数据集

        #region 插入 DataRow[]
        /// <summary>
        /// 批量 插入/更新数据集  执行SQL语句,返回影响记录数
        /// </summary>          
        /// <param name="tableName">需要插入数据的表名</param>
        /// <param name="rows">DataRow[]</param>
        /// <returns>返回执行结果状态</returns>
        #endregion
        public static bool SqlBulkCopyRange(string tableName, DataRow[] rows)
        {

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = tableName;

                        if (rows != null && rows.Length > 0)
                            bulkCopy.WriteToServer(rows);

                        return true;

                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                    finally
                    {
                        bulkCopy.Close();
                        connection.Close();
                    }
                }
            }
        }

        #region 插入 DataTable
        /// <summary>
        /// 批量 插入/更新数据集  执行SQL语句,返回影响记录数
        /// </summary>          
        /// <param name="tableName">需要插入数据的表名</param>
        /// <param name="table">DataTable</param>
        /// <returns>返回执行结果状态</returns>
        #endregion
        public static bool SqlBulkCopyRange(string tableName, DataTable table)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = tableName;

                        if (table != null && table.Rows.Count > 0)
                            bulkCopy.WriteToServer(table);

                        return true;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                    finally
                    {
                        bulkCopy.Close();
                        connection.Close();
                    }
                }
            }
        }

        #region 插入 IDataReader
        /// <summary>
        /// 批量 插入/更新数据集  执行SQL语句,返回影响记录数
        /// </summary>          
        /// <param name="tableName">需要插入数据的表名</param>
        /// <param name="reader">IDataReader</param>
        /// <returns>返回执行结果状态</returns>
        #endregion
        public static bool SqlBulkCopyRange(string tableName, IDataReader reader)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = tableName;

                        if (reader != null)
                        {
                            bulkCopy.WriteToServer(reader);
                            reader.Close();
                        }
                        return true;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                    finally
                    {
                        bulkCopy.Close();
                        connection.Close();
                    }
                }
            }
        }

        #region 匹配的行状态 插入 DataTable
        /// <summary>
        /// 批量 插入/更新数据集  执行SQL语句,返回影响记录数
        /// </summary>          
        /// <param name="tableName">需要插入数据的表名</param>
        /// <param name="table">DataTable</param>
        /// <returns>返回执行结果状态</returns>
        #endregion
        public static bool SqlBulkCopyRange(string tableName, DataTable table, DataRowState rowState)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = tableName;

                        if (table != null && table.Rows.Count > 0)
                            bulkCopy.WriteToServer(table, rowState);

                        return true;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                    finally
                    {
                        bulkCopy.Close();
                        connection.Close();
                    }
                }
            }
        }

        #endregion
        #endregion


        #region  Other

        /// <summary>
        /// 批量提交订单
        /// 采用存储过程事务
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句和参数</param>	
        /// <returns>
        /// return  SubStorePruduceList.count 订单提交成功
        ///         0 提交失败 存储过程异常
        ///        -2 提交失败 库存不足
        ///        -3 提交失败 程序异常
        /// </returns>
        public static int SubmitOrderTrans(List<CommandInfo> SubStorePruduceList)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                int results = 0;
                try
                {
                    for (int n = 0; n < SubStorePruduceList.Count; n++)
                    {
                        string strsql = SubStorePruduceList[n].CommandText;
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            DbParameter[] cmdPara = SubStorePruduceList[n].param;
                            if (cmdPara != null)
                            {
                                foreach (SqlParameter parameter in cmdPara)
                                {
                                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                                        (parameter.Value == null))
                                    {
                                        parameter.Value = DBNull.Value;
                                    }
                                    cmd.Parameters.Add(parameter);
                                }
                            }

                            int resu = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Parameters.Clear();

                            //单个订单提交失败
                            //rollback
                            if (resu != 1)
                            {
                                results = resu;
                                tx.Rollback();
                                return results;
                            }
                            results += resu;
                        }
                    }
                    tx.Commit();
                    return (results == SubStorePruduceList.Count) ? 1 : 0;
                }
                catch
                {
                    tx.Rollback();
                    return -3;//程序异常
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 枚举类型
    /// </summary>
    public enum EffentNextType
    {
        /// <summary>
        /// 对其它语句无任何影响
        /// </summary>
        None,
        /// <summary>
        /// 当前语句必须为"Select count(1) From .."格式,如果存在则继续执行,不存在回滚事务
        /// </summary>
        WhenHaveContine,
        /// <summary>
        /// 当前语句必须为"Select count(1) From .."格式,如果不存在则继续执行,存在回滚事务
        /// </summary>
        WhenNoHaveContine,
        /// <summary>
        /// 当前语句影响到的行数必须大于0,否则回滚事务
        /// </summary>
        ExcuteEffectRows,
        /// <summary>
        /// 引发事情
        /// 当前语句必须为"Select count(1) From .."格式,如果不存在则继续执行,存在回滚事务
        /// </summary>
        SolicitationEvent
    }
    /// <summary>
    /// 
    /// </summary>
    public class CommandInfo
    {
        public object ShareObject = null;
        public object OriginalData = null;
        event EventHandler _solicitationEvent;

        public event EventHandler SolicitationEvent
        {
            add
            {
                _solicitationEvent += value;
            }
            remove
            {
                _solicitationEvent -= value;
            }
        }

        public void OnSolicitationEvent()
        {
            if (_solicitationEvent != null)
            {
                _solicitationEvent(this, new EventArgs());
            }
        }

        public string CommandText;
        public System.Data.Common.DbParameter[] param;

        public EffentNextType EffentNextType = EffentNextType.None;

        public CommandInfo()
        { }

        public CommandInfo(string sqlText, System.Data.SqlClient.SqlParameter[] para)
        {
            this.CommandText = sqlText;
            this.param = para;
        }

        public CommandInfo(string sqlText, System.Data.SqlClient.SqlParameter[] para, EffentNextType type)
        {
            this.CommandText = sqlText;
            this.param = para;
            this.EffentNextType = type;
        }
    }
}
