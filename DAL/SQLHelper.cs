using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace RAFE.DAL
{
    public class SQLHelper
    {
        #region Member variables

        static string databaseOwner = "dbo";	// overwrite in web.config     

        #endregion

        #region Connection

        public static SqlConnection getConnection()
        {
            SqlConnection conn;
            try
            {

                string constr = GetConnectionString();
                conn = new SqlConnection(constr);

            }
            catch
            {
                throw new Exception("SQL Connection String is invalid.");
            }
            return conn;
        }

        public static string GetConnectionString()
        {
            string strLocation = string.Empty;
            strLocation = System.Configuration.ConfigurationManager.AppSettings["connectionString"].ToString();
            return strLocation.Trim();
        }



        #endregion

        #region Common Proceudures

        public static DataTable AddSerialNumber(DataTable table)
        {
            DataColumn identity = new DataColumn("SrNo", typeof(int));
            identity.AutoIncrement = true;
            identity.AutoIncrementSeed = 1;
            identity.AutoIncrementStep = 1;
            table.Columns.Add(identity);
            table.AcceptChanges();

            return table;

        }

        public static ArrayList GetArrayList(string procedureName, SqlCommand command, string ColumnName)
        {
            SqlConnection connection = getConnection();
            SqlDataReader dr = null;

            ArrayList Arr = new ArrayList();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Arr.Add(Convert.ToInt32(dr[ColumnName].ToString()));
                }


                dr.Close();
                connection.Close();

                return Arr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();

                command.Dispose();
                connection.Dispose();
            }

        }

        public static ArrayList GetArrayListStringValues(string procedureName, SqlCommand command, string ColumnName)
        {
            SqlConnection connection = getConnection();
            SqlDataReader dr = null;

            ArrayList Arr = new ArrayList();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Arr.Add(dr[ColumnName].ToString());
                }


                dr.Close();
                connection.Close();

                return Arr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();

                command.Dispose();
                connection.Dispose();
            }

        }

        //Execute Stored Procedure And Returns Dataview as a resultset
        public static DataTable GetDataTable(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable table = null;


            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                command.Dispose();
                if (connection.State == ConnectionState.Open) connection.Close();
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
            {
                table = ds.Tables[0];
            }
            return table;
        }

        //Execute Stored Procedure And Returns Dataview as a resultset
        public static DataTable GetDataTable(string procedureName, SqlCommand command, string tableName)
        {
            SqlConnection connection = getConnection();

            SqlDataAdapter da = new SqlDataAdapter();

            DataTable dt = new DataTable();


            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;

                da.SelectCommand = command;


                da.Fill(dt);
                dt.TableName = tableName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                command.Dispose();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return dt;
        }

        public static DataTable GetDataTable(string sSQl)
        {
            SqlConnection connection = getConnection();
            DataTable datatable;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sSQl, connection);
                datatable = new DataTable();
                da.Fill(datatable);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }

            return datatable;

        }

        public static DataTable GetDataTable(string sqlQuery, string TableName)
        {
            SqlConnection connection = getConnection();
            DataTable datatable;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
                datatable = new DataTable();
                da.Fill(datatable);
                datatable.TableName = TableName.Trim();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }

            return datatable;

        }

        public static int ExecuteSP(string procedureName, SqlCommand command)
        {
            int i = 0;
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }

        public static int ExecuteSQL(string strQuery, SqlCommand command)
        {
            int i = 0;
            SqlConnection connection = getConnection();

            try
            {

                command.CommandText = strQuery;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.Text;

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }

        public static int ExecuteSQL(string strQuery)
        {
            SqlCommand command = new SqlCommand();
            int i = 0;
            SqlConnection connection = getConnection();

            try
            {

                command.CommandText = strQuery;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.Text;

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }

        //'vishal  it gets count return by a stored procedure
        public static int GetSPCount(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }

        //'vishal  it gets count return by a stored procedure
        public static string ExecuteScalarString(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                return Convert.ToString(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
        }

        //'vishal  it gets count return by a stored procedure
        public static Int32 ExecuteScalar(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            Int32 intReturnValue = 0;

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                intReturnValue = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }

            return intReturnValue;
        }

        //'vishal  it gets count return by a stored procedure
        public static string ExecuteScalarString(string sql)
        {
            SqlConnection connection = getConnection();
            SqlCommand command = new SqlCommand();
            string retValue = "";

            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;


                connection.Open();
                retValue = Convert.ToString(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return retValue;
        }

        //'vishal  it gets count return by a stored procedure
        public static Int32 ExecuteScalar(string sql)
        {
            SqlConnection connection = getConnection();
            SqlCommand command = new SqlCommand();
            Int32 retValue = 0;

            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;


                connection.Open();
                retValue = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return retValue;
        }

        //gets one Argument return by a stored procedure
        public static string GetSPOutPut(string ProcedureName, SqlCommand command, int OutputParameterIndex)
        {
            SqlDataReader dr = null;
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                dr.Close();
                connection.Close();
                return command.Parameters[OutputParameterIndex].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                if (!dr.IsClosed) dr.Close();
                dr.Dispose();

                command.Dispose();
                connection.Dispose();
            }
        }

        //gets one Argument return by a stored procedure
        public static string GetSPOutPut1(string ProcedureName, SqlCommand command, string OutputParameter)
        {
            SqlDataReader dr = null;
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                dr.Close();
                connection.Close();

                return command.Parameters[OutputParameter].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                if (!dr.IsClosed) dr.Close();
                dr.Dispose();

                command.Dispose();
                connection.Dispose();
            }
        }

        //gets one Argument return by a stored procedure

        public static Hashtable GetSPOutPut(string ProcedureName, SqlCommand command, params string[] OutputParameter)
        {
            SqlDataReader dr = null;
            SqlConnection connection = getConnection();

            try
            {
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                dr.Close();
                connection.Close();


                //create hash table to return all output parameters
                Hashtable htable = new Hashtable();
                string parameterValue = string.Empty;

                foreach (string optPara in OutputParameter)
                {
                    parameterValue = command.Parameters[optPara].Value.ToString();
                    htable.Add(optPara, parameterValue);
                }

                return htable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                if (!dr.IsClosed) dr.Close();
                dr.Dispose();

                command.Dispose();
                connection.Dispose();
            }
        }

        //Execute Stored Procedure And Returns Dataview as a resultset
        public static DataView GetDataView(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                command.CommandTimeout = 2000;

                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                command.Dispose();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return ds.Tables[0].DefaultView;
        }

        public static DataView GetDataView(string sqlQuery)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            DataView dv = new DataView();

            try
            {
                da = new SqlDataAdapter(sqlQuery, connection);
                da.Fill(ds);
                dv.Table = ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
            return dv;
        }

        public static DataSet GetDataSet(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                command.CommandTimeout = 120;
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                da.SelectCommand = command;
                da.Fill(ds);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                connection.Dispose();
                command.Dispose();
            }
            return ds;
        }

        public static DataSet GetDataSet(string sqlQuery)
        {
            SqlConnection connection = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                da = new SqlDataAdapter(sqlQuery, connection);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return ds;
        }

        //Execute Stored Procedure And Returns data Adapter as a resultset
        public static SqlDataAdapter GetDataAdapter(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = command;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
                command.Dispose();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return da;
        }

        public static SqlDataReader GetDataReader(string procedureName, SqlCommand command)
        {
            SqlConnection connection = getConnection();
            SqlDataReader dr = null;
            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (connection.State == ConnectionState.Open) connection.Close();
                //command.Connection.Close();
                //command.Connection.Dispose();
                //command.Dispose();

            }
            return dr;
        }

        #endregion

        #region Get_HashTable_Of_UserRights

        public static Hashtable GetHashTable(string procedureName, SqlCommand command, string ObjectKey, string ObjectValue)
        {
            SqlConnection connection = getConnection();
            SqlDataReader dr = null;

            Hashtable Hashtable = new Hashtable();

            try
            {
                command.CommandText = databaseOwner + "." + procedureName;
                command.Connection = connection;

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Hashtable.Add(dr[ObjectKey], dr[ObjectValue]);
                }


                dr.Close();
                connection.Close();

                return Hashtable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();

                command.Dispose();
                connection.Dispose();
            }

        }
        #endregion
    }
}