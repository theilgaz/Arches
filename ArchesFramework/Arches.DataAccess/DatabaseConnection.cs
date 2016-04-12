using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Arches.DataAccess
{
    /// <summary>
    /// Database Class
    /// </summary>
    public class DatabaseConnection
    {
        DataTable _dt;
        SqlDataAdapter _da;
        SqlCommand _cmd;
        SqlConnection _conn;

        /// <summary>
        /// Creating a new connection with the name of ConnectionString in ConnectionStrings in which web.config file.
        /// </summary>
        /// <param name="ConnectionString"></param>
        public DatabaseConnection(String ConnectionString)
        {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
        }

        /// <summary>
        /// Creating a new connection with serverName, dbName, username, password using SqlConnection class.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="dbName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public DatabaseConnection(String serverName, String dbName, String username, String password)
        {
            _conn = new SqlConnection("server =" + serverName + "; Initial Catalog =" + dbName + "; Persist Security Info = True; User ID =" + username + "; password =" + password + ";MultipleActiveResultSets=true;");
        }

        /// <summary>
        /// It executes Select queries and Views and retrieves data as DataTable. 
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public DataTable Select(String SQL)
        {
            _da = new SqlDataAdapter(SQL, _conn);
            _dt = new DataTable("EntTable");
            _da.Fill(_dt);
            return _dt;
        }

        /// <summary>
        /// It executes Insert, Update and Delete queries.
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public int ExecQuery(String SQL)
        {
            int result = 0;

            try
            {
                _cmd = new SqlCommand(SQL, _conn);
                _conn.Open();
                result = _cmd.ExecuteNonQuery();
            }
            catch
            {
                result = -1;
            }
            _conn.Close();
            return result;
        }

        /// <summary>
        /// It executes {SPName} with {parameters} and returns int (1 successful, 0 no row affected, -1 failed, -2 process failed).
        /// </summary>
        /// <param name="SPName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int Execute(String SPName, SqlParameter[] parameters)
        {
            int val = 0;

            _cmd = new SqlCommand(SPName, _conn);


            _cmd.CommandType = CommandType.StoredProcedure;
            _conn.Open();

            foreach (var param in parameters)
            {
                _cmd.Parameters.Add(param);
            }

            try
            {
                val = _cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                val = -2;
                throw e;
            }

            _conn.Close();
            return val;
        }

        /// <summary>
        /// It executes {SPName} with {parameters} and retrieves data as DataTable.
        /// </summary>
        /// <param name="SPName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteWithReturn(String SPName, SqlParameter[] parameters)
        {
            _dt = new DataTable("EntTable");
            _cmd = new SqlCommand(SPName, _conn);
            _da = new SqlDataAdapter();

            _cmd.CommandType = CommandType.StoredProcedure;
            //conn.Open();

            foreach (var param in parameters)
            {
                _cmd.Parameters.Add(param);
            }

            _da.SelectCommand = _cmd;
            _da.Fill(_dt);

            //conn.Close();
            return _dt;
        }
    }
}
