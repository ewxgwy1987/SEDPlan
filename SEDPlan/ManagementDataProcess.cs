using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace SEDPlan
{
    public class ManagementDataProcess
    {
        #region Class Field and Property

        private string sqlconn_str;

        private const string sqlstr_getSAPrss = "select Process_ID, Process_Name from SA_Process";
        private const string sqlstr_getPrjInfo = "select ProjectNo, ProjectName from ProjectInfo";

        private const string sqlstr_InsertSAPrss = "INSERT INTO SA_Process VALUES((SELECT MAX(Process_ID)+1 FROM SA_Process),@ProcessName)";
        private const string sqlstr_SelectSAPrss = "SELECT Process_ID FROM SA_Process WHERE Process_ID=@ProcessID";
        private const string sqlstr_DeleteSAPrss = "DELETE FROM SA_Process WHERE Process_ID=@ProcessID";
        private const string sqlstr_UpdateSAPrss = "UPDATE SA_Process SET Process_Name=@ProcessName WHERE Process_ID=@ProcessID";

        private const string sqlstr_InsertProjInfo = "INSERT INTO ProjectInfo VALUES(@ProjNo,@ProjName)";
        private const string sqlstr_SelectProjInfo = "SELECT ProjectNo FROM ProjectInfo WHERE ProjectNo=@ProjNo";
        private const string sqlstr_DeleteProjInfo = "DELETE FROM ProjectInfo WHERE ProjectNo=@ProjNo";
        private const string sqlstr_UpdateProjInfo = "UPDATE ProjectInfo SET ProjectName=@ProjName WHERE ProjectNo=@ProjNo";

        // The name of current class 
        private static readonly string _className =
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        // Create a logger for use in this class
        private static readonly log4net.ILog _logger =
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructor, Dispose, Finalize and Destructor

        public ManagementDataProcess(string connstr)
        {
            this.sqlconn_str = connstr;
        }

        #endregion

        #region Member Function

        #region Database Operation

        /// <summary>
        /// Executes a T-sql statement in SqlServer without data returned from DB.
        /// It has Exception handling, so no need to process exception when executes T-sql in SqlServer.
        /// </summary>
        /// <param name="commandText">The T-sql statement.</param>
        /// <param name="sqlParas">The Array of SqlParameter type transfered into T-sql statement.</param>
        /// <param name="dbBehavior">Describes what is done for T-sql, which is written into log file.</param>
        /// <returns>Type:bool True if T-sql is executed successfully, otherwise false.</returns>
        private bool DBExecuteNonQuery(string commandText, SqlParameter[] sqlParas, string dbBehavior)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">" + " DB Operation:<" + dbBehavior + ">\n";

            bool res = false;

            SqlConnection sqlconn = null;
            SqlCommand sqlcmd = null;
            try
            {
                sqlconn = new SqlConnection(sqlconn_str);
                sqlconn.Open();
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlconn;

                sqlcmd.CommandText = commandText;
                sqlcmd.CommandType = CommandType.Text;

                if (sqlParas != null && sqlParas.Length > 0)
                {
                    foreach (SqlParameter sqlpara in sqlParas)
                    {
                        sqlcmd.Parameters.Add(sqlpara);
                    }
                }
                sqlcmd.ExecuteNonQuery();
                res = true;
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                res = false;
            }
            finally
            {
                if (sqlconn != null && sqlconn.State == ConnectionState.Open)
                {
                    sqlconn.Close();
                    sqlconn.Dispose();
                    if (sqlcmd != null)
                        sqlcmd.Dispose();
                }
            }

            return res;
        }

        /// <summary>
        /// Executes a T-sql statement in SqlServer to query data from DB.
        /// It has Exception handling, so no need to process exception when executes T-sql in SqlServer.
        /// </summary>
        /// <param name="commandText">The T-sql statement.</param>
        /// <param name="sqlParas">The Array of SqlParameter type transfered into T-sql statement.</param>
        /// <param name="dbBehavior">Describes what is done for T-sql, which is written into log file.</param>
        /// <returns>Type:DataTable The data queried from DB.</returns>
        private DataTable DBQuery(string commandText, SqlParameter[] sqlParas, string dbBehavior)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">" + " DB Operation:<" + dbBehavior + ">\n";

            DataTable dtres = new DataTable();

            SqlConnection sqlconn = null;
            SqlCommand sqlcmd = null;
            SqlDataAdapter sqlAdapter = null;
            try
            {
                sqlconn = new SqlConnection(sqlconn_str);
                sqlconn.Open();
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlconn;

                sqlcmd.CommandText = commandText;
                sqlcmd.CommandType = CommandType.Text;

                if (sqlParas != null && sqlParas.Length > 0)
                {
                    foreach (SqlParameter sqlpara in sqlParas)
                    {
                        sqlcmd.Parameters.Add(sqlpara);
                    }
                }
                sqlAdapter = new SqlDataAdapter(sqlcmd);
                sqlAdapter.Fill(dtres);
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
            }
            finally
            {
                if (sqlconn != null && sqlconn.State == ConnectionState.Open)
                {
                    sqlconn.Close();
                    sqlconn.Dispose();
                    if (sqlcmd != null)
                        sqlcmd.Dispose();
                    if (sqlAdapter != null)
                        sqlAdapter.Dispose();
                }
            }

            return dtres;
        }

        #endregion

        #region SA Process Operation

        /// <summary>
        /// Return all Data about SA Process
        /// </summary>
        /// <returns>Type:DataTable  The data with Process_ID and  Process_Name from SA_Process in DB</returns>
        public DataTable GetSAProcess()
        {
            SqlParameter[] sqlParas = new SqlParameter[0];

            return DBQuery(sqlstr_getSAPrss, sqlParas, "Get All SAProcesses");
        }

        /// <summary>
        /// Insert a new SA Process into DB
        /// </summary>
        /// <param name="prssname">The new SA process name</param>
        /// <returns>Type:bool True if add a new SA process into DB successfully; otherwise, false</returns>
        public bool AddSAProcess(string prssname)
        {
            SqlParameter[] sqlParas = new SqlParameter[1];
            sqlParas[0] = new SqlParameter("@ProcessName", SqlDbType.NVarChar, 100);
            sqlParas[0].Value = prssname;

            return DBExecuteNonQuery(sqlstr_InsertSAPrss, sqlParas, "Insert SAProcess. Process Name:" + prssname);
        }

        /// <summary>
        /// Check whether a SA Process exists in DB or not.
        /// </summary>
        /// <param name="prssno">The SA Process ID</param>
        /// <returns>Type:bool True if the SA process exists in DB; otherwise, false</returns>
        public bool SAPrssExists(int prssno)
        {
            SqlParameter[] sqlParas = new SqlParameter[1];
            sqlParas[0] = new SqlParameter("@ProcessID", SqlDbType.BigInt);
            sqlParas[0].Value = prssno;

            DataTable dtres = DBQuery(sqlstr_SelectSAPrss, sqlParas, "Select SAProcess. Process ID:" + prssno);
            if (dtres.Rows.Count == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Delete a SA Process from DB
        /// </summary>
        /// <param name="prssno">The SA Process ID</param>
        /// <returns>Type:bool True if the SA process is deleted successfully in DB; otherwise, false</returns>
        public bool DeleteSAProcess(int prssno)
        {
            SqlParameter[] sqlParas = new SqlParameter[1];
            sqlParas[0] = new SqlParameter("@ProcessID", SqlDbType.BigInt);
            sqlParas[0].Value = prssno;

            return DBExecuteNonQuery(sqlstr_DeleteSAPrss, sqlParas, "Delete SAProcess. Process ID:" + prssno);
        }

        /// <summary>
        /// Update a SA Process Info in DB with SA Process ID
        /// </summary>
        /// <param name="prssno">The SA Process ID</param>
        /// <param name="prssname">The SA Process Name</param>
        /// <returns>Type:bool True if the SA process is updated successfully in DB; otherwise, false</returns>
        public bool UpdateSAProcess(int prssno, string prssname)
        {
            SqlParameter[] sqlParas = new SqlParameter[2];
            sqlParas[0] = new SqlParameter("@ProcessID", SqlDbType.BigInt);
            sqlParas[0].Value = prssno;
            sqlParas[1] = new SqlParameter("@ProcessName", SqlDbType.NVarChar, 100);
            sqlParas[1].Value = prssname;

            return DBExecuteNonQuery(sqlstr_UpdateSAPrss, sqlParas, "Update SAProcess. Process ID:" + prssno.ToString() + ";Process Name:" + prssname);
        }

        #endregion

        #region ProjectInfo Operation

        /// <summary>
        /// Return all Data about project info
        /// </summary>
        /// <returns>Type:DataTable The data with ProjectNo and ProjectName from ProjectInfo in DB</returns>
        public DataTable GetProjectInfo()
        {
            SqlParameter[] sqlParas = new SqlParameter[0];

            return DBQuery(sqlstr_getPrjInfo, sqlParas, "Get All Projects");
        }

        /// <summary>
        /// Check whether a project exists in DB or not.
        /// </summary>
        /// <param name="projno">The project no.</param>
        /// <returns>Type:bool True if the project exists in DB; otherwise, false</returns>
        public bool ProjectExists(string projno)
        {
            SqlParameter[] sqlParas = new SqlParameter[1];
            sqlParas[0] = new SqlParameter("@ProjNo", SqlDbType.NVarChar, 20);
            sqlParas[0].Value = projno;

            DataTable dtres = DBQuery(sqlstr_SelectProjInfo, sqlParas, "Select Project");
            if (dtres.Rows.Count == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Insert a new project into DB
        /// </summary>
        /// <param name="projno">The new project no.</param>
        /// <param name="projname">The new project name</param>
        /// <returns>Type:bool True if add the new project into DB successfully; otherwise, false</returns>
        public bool AddProject(string projno, string projname)
        {
            SqlParameter[] sqlParas = new SqlParameter[2];
            sqlParas[0] = new SqlParameter("@ProjNo", SqlDbType.NVarChar, 20);
            sqlParas[0].Value = projno;
            sqlParas[1] = new SqlParameter("@ProjName", SqlDbType.NVarChar, 200);
            sqlParas[1].Value = projname;

            return DBExecuteNonQuery(sqlstr_InsertProjInfo, sqlParas, "Insert Project");
        }

        /// <summary>
        /// Delete a project from DB
        /// </summary>
        /// <param name="projno">The new project no.</param>
        /// <returns>Type:bool True if the project is deleted successfully in DB; otherwise, false</returns>
        public bool DeleteProject(string projno)
        {
            SqlParameter[] sqlParas = new SqlParameter[1];
            sqlParas[0] = new SqlParameter("@ProjNo", SqlDbType.NVarChar, 20);
            sqlParas[0].Value = projno;

            return DBExecuteNonQuery(sqlstr_DeleteProjInfo, sqlParas, "Delete Project");
        }

        /// <summary>
        /// Update a project info in DB with project no.
        /// </summary>
        /// <param name="projno">The new project no.</param>
        /// <param name="projname">The new project name</param>
        /// <returns>Type:bool True if the SA process is updated successfully in DB; otherwise, false</returns>
        public bool UpdateProject(string projno, string projname)
        {
            SqlParameter[] sqlParas = new SqlParameter[2];
            sqlParas[0] = new SqlParameter("@ProjNo", SqlDbType.NVarChar, 20);
            sqlParas[0].Value = projno;
            sqlParas[1] = new SqlParameter("@ProjName", SqlDbType.NVarChar, 200);
            sqlParas[1].Value = projname;

            return DBExecuteNonQuery(sqlstr_UpdateProjInfo, sqlParas, "Insert Project. Project No:" + projno + ";Project Name:" + projname);
        }

        #endregion

        #endregion

    }
}
