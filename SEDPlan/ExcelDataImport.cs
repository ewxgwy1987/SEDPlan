using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;


namespace SEDPlan
{
    public class ExcelDataImport
    {
        #region Class Field and Property

        private DataTable m_dtImportData;
        private string sheetname;

        private const string XCFG_CONNSTR = "connectionString";
        private const string XCFG_VARTYPE = "VariableType";

        // SC TAGS
        private const string XCFG_SC = "SAComponent";
        private const string XCFG_SC_COLUMNS = "SCColumns";
        private const string XCFG_SC_PARAMETERS = "SCParameters";
        private const string XCFG_SC_STP = "SCSTP";

        // SV Tags
        private const string XCFG_SV = "SAVariable";
        private const string XCFG_SV_COLUMNS = "SVColumns";
        private const string XCFG_SV_PARAMETERS = "SVParameters";
        private const string XCFG_SV_STP = "SVSTP";

        // STD Tags
        private const string XCFG_STD = "StandardParts";
        private const string XCFG_STD_COLUMNS = "STDColumns";
        private const string XCFG_STD_PARAMETERS = "STDParameters";
        private const string XCFG_STD_STP = "STDSTP";

        // BP Tags
        private const string XCFG_BP = "BOMPlan";
        private const string XCFG_BP_COLUMNS = "BPColumns";
        private const string XCFG_BP_PARAMETERS = "BPParameters";
        private const string XCFG_BP_STP = "BPSTP";

        private const string PARA_VAR = "VAR";
        private const string PARA_WEIGHT = "WEIGHT";
        private const string TP_SC = "SC";
        private const string TP_SV = "SV";
        private const string TP_STD = "STD";
        private const string TP_BP = "BP";

        private string connstr;
        private string[] Var_Types;

        // SC
        private string[] SC_ColNames;
        private string[] SC_ParaNames;
        private string SC_STPName;

        // SV 
        private string[] SV_ColNames;
        private string[] SV_ParaNames;
        private string SV_STPName;

        // STD
        private string[] STD_ColNames;
        private string[] STD_ParaNames;
        private string STD_STPName;

        // BP
        private string[] BP_ColNames;
        private string[] BP_ParaNames;
        private string BP_STPName;

        // SA component
        //private const int SC_COLCNT = 8;
        //private const string[] SC_VARIABLE_TYPES = {"L","LS","A°"};
        //private const string SC_PARTSNAME_COLNAME = "NAME";
        //private const string SC_SPEC_COLNAME = "DESCRIPTION";
        //private const string SC_LHS_COLNAME = "LHS";
        //private const string SC_RHS_COLNAME = "RHS";
        //private const string SC_PCE_COLNAME = "PCE";
        //private const string SC_WEIGHT_COLNAME = "WEIGHT (lbs/pce)";
        //private const string SC_PROCESS_COLNAME = "REMARKS";

        private string showerrstr;

        public event EventHandler<ErrorEventArgs> OnErrorHappened;

        // The name of current class 
        private static readonly string _className =
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        // Create a logger for use in this class
        private static readonly log4net.ILog _logger =
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion


        #region Constructor, Dispose, Finalize and Destructor

        public ExcelDataImport(XmlNode xconfig, string connstr)
        {
            //this.connstr = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xconfig, XCFG_CONNSTR, "");
            this.connstr = connstr;

            string str_VARTYPE = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xconfig, XCFG_VARTYPE, "");
            this.Var_Types = str_VARTYPE.Split(',');

            // Fetch SA Component configuration
            XmlNode xml_SC = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_SC);
            string str_SCCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SC, XCFG_SC_COLUMNS, "");
            this.SC_ColNames = str_SCCols.Split(',');
            string str_SCParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SC, XCFG_SC_PARAMETERS, "");
            this.SC_ParaNames = str_SCParas.Split(',');
            this.SC_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SC, XCFG_SC_STP, "");

            // Fetch SA Variable configuration
            XmlNode xml_SV = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_SV);
            string str_SVCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SV, XCFG_SV_COLUMNS, "");
            this.SV_ColNames = str_SVCols.Split(',');
            string str_SVParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SV, XCFG_SV_PARAMETERS, "");
            this.SV_ParaNames = str_SVParas.Split(',');
            this.SV_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SV, XCFG_SV_STP, "");

            // Fetch BOM Plan configuration
            XmlNode xml_BP = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_BP);
            string str_BPCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_BP, XCFG_BP_COLUMNS, "");
            this.BP_ColNames = str_BPCols.Split(',');
            string str_BPParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_BP, XCFG_BP_PARAMETERS, "");
            this.BP_ParaNames = str_BPParas.Split(',');
            this.BP_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_BP, XCFG_BP_STP, "");

            // Feth Standard Parts configuration
            XmlNode xml_STD = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_STD);
            string str_STDCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_STD, XCFG_STD_COLUMNS, "");
            this.STD_ColNames = str_STDCols.Split(',');
            string str_STDParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_STD, XCFG_STD_PARAMETERS, "");
            this.STD_ParaNames = str_STDParas.Split(',');
            this.STD_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_STD, XCFG_STD_STP, "");
            
        }

        public bool Init(DataTable dtImport, string sheetname)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";
            bool res = true;
            this.showerrstr = "";

            try
            {
                if (dtImport != null && sheetname != null && sheetname != "")
                {
                    this.m_dtImportData = dtImport;
                    this.sheetname = sheetname;
                }
                else
                {
                    showerrstr += "No data can be imported.";
                    errstr += showerrstr;
                    _logger.Error(errstr);
                    ShowError(showerrstr);
                    return false;
                }
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
                return false;
            }
            return res;
        }

        #endregion


        #region Member Function

        private void ShowError(string errormsg)
        {
            ErrorEventArgs new_errargs = new ErrorEventArgs();
            new_errargs.ErrorMsg = errormsg;
            EventHandler<ErrorEventArgs> temp = OnErrorHappened;
            if (temp != null)
                temp(this, new_errargs);
        }

        private void CleanError()
        {
            this.showerrstr = "";
        }

        public bool ImportData(string tabletype)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";
            bool res = true;

            SqlConnection sqlconn = null;
            SqlCommand sqlcmd = null;

            try
            {
                sqlconn = new SqlConnection(connstr);
                sqlconn.Open();
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlconn;

                int revision = this.getNewRevision(this.sheetname, sqlcmd, tabletype);
                string crtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                SqlTransaction sqltran = sqlconn.BeginTransaction();
                sqlcmd.Transaction = sqltran;
                try
                {
                    // here can use transaction
                    foreach (DataRow dr in this.m_dtImportData.Rows)
                    {
                        string[] paras = null;
                        switch (tabletype)
                        {
                            case TP_SC:
                                paras = AnalyzeDataRow_SC(dr, revision.ToString(), crtime);
                                break;
                            case TP_SV:
                                paras = AnalyzeDataRow_SV(dr, revision.ToString(), crtime);
                                break;
                            case TP_STD:
                                paras = AnalyzeDataRow_STD(dr, revision.ToString(), crtime);
                                break;
                            case TP_BP:
                                paras = AnalyzeDataRow_BP(dr, revision.ToString(), crtime);
                                break;
                            default:
                                paras = null;
                                break;
                        }

                        if (paras == null)
                        {
                            showerrstr += "unknown excel type\n";
                            errstr += showerrstr;
                            _logger.Error(errstr);
                            ShowError(showerrstr);
                            return false;
                        }

                        if (paras!= null && !InsertToDatabase(paras, sqlcmd, tabletype))
                        {
                            showerrstr += "Import Data Failed.\n";
                            errstr += showerrstr;
                            _logger.Error(errstr);
                            ShowError(showerrstr);
                            res = false;
                        }

                        if (res == false)
                            break;
                    }
                    sqltran.Commit();
                }
                catch (Exception exp)
                {
                    try
                    {
                        sqltran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        errstr += exRollback.ToString();
                        _logger.Error(errstr);
                        showerrstr += exRollback.Message;
                        ShowError(showerrstr);
                        return false;
                    }
                    throw exp;
                }

            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
                return false;
            }
            finally
            {
                if (sqlconn != null && sqlconn.State == ConnectionState.Open)
                {
                    sqlconn.Close();
                    sqlconn.Dispose();
                    if (sqlcmd != null)
                    {
                        sqlcmd.Dispose();
                    }
                }
            }
            return res;

        }

        private string[] AnalyzeDataRow_SC(DataRow dr,string revision, string created_time)
        {
            string[] paras = new string[this.SC_ColNames.Length + 3];
            paras[0] = this.sheetname.Trim();

            int i = 1;
            foreach (string paraname in this.SC_ColNames)
            {
                paras[i] = "";
                if (paraname == PARA_VAR)
                {
                    foreach (string vartype in this.Var_Types)
                    {
                        if (dr.Table.Columns.Contains(vartype))
                        {
                            if (dr[vartype] != null && ((string)dr[vartype]) != "")
                            {
                                paras[i] = ((string)dr[vartype]).Trim();
                                break;
                            }
                        }
                    }
                }
                else
                    paras[i] = ((string)dr[paraname]).Trim();

                i++;
            }

            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_SV(DataRow dr, string revision, string created_time)
        {
            string[] paras = new string[this.SV_ColNames.Length + 3];
            paras[0] = this.sheetname.Trim();
            int i = 1;
            foreach (string colname in this.SV_ColNames)
            {
                paras[i] = "";
                if (dr.Table.Columns.Contains(colname))
                    paras[i] = ((string)dr[colname]).Trim();
                i++;
            }
            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_STD(DataRow dr, string revision, string created_time)
        {
            string[] paras = new string[this.STD_ColNames.Length + 2];
            //paras[0] = this.sheetname.Trim();

            int i = 0;
            foreach (string colname in this.STD_ColNames)
            {
                paras[i] = "";
                if (dr[colname] != null)
                    paras[i] = ((string)dr[colname]).Trim();
                i++;
            }
            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_BP(DataRow dr, string revision, string created_time)
        {
            string[] paras = new string[this.BP_ColNames.Length + 3];
            paras[0] = this.sheetname.Trim();
            int i = 1;
            foreach (string colname in this.BP_ColNames)
            {
                paras[i] = "";
                if (dr[colname] != null)
                    paras[i] = ((string)dr[colname]).Trim();
                i++;
            }
            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private bool InsertToDatabase(string[] paras, SqlCommand sqlcmd, string tabletype)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";
            bool res = false;
            

            try
            {
                if (sqlcmd.Parameters.Count > 0)
                    sqlcmd.Parameters.Clear();
                SqlParameter[] sPararameter = new SqlParameter[paras.Length];
                string[] paranames;

                switch (tabletype)
                {
                    case TP_SC:
                        paranames = this.SC_ParaNames;
                        sqlcmd.CommandText = SC_STPName;
                        break;
                    case TP_SV:
                        paranames = this.SV_ParaNames;
                        sqlcmd.CommandText = SV_STPName;
                        break;
                    case TP_STD:
                        paranames = this.STD_ParaNames;
                        sqlcmd.CommandText = STD_STPName;
                        break;
                    case TP_BP:
                        paranames = this.BP_ParaNames;
                        sqlcmd.CommandText = BP_STPName;
                        break;
                    default:
                        paranames = null;
                        sqlcmd.CommandText = "";
                        break;
                }

                if (sqlcmd.CommandText == "" || paranames == null)
                {
                    showerrstr += "unknown excel type\n";
                    errstr += showerrstr;
                    _logger.Error(errstr);
                    ShowError(showerrstr);
                    return false;
                }

                int i;
                for (i = 0; i < paras.Length; i++)
                {
                    sPararameter[i] = new SqlParameter("@" + paranames[i], SqlDbType.NVarChar, 100);
                    sPararameter[i].Direction = ParameterDirection.Input;
                    sPararameter[i].Value = paras[i];
                    sqlcmd.Parameters.Add(sPararameter[i]);
                }

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.ExecuteNonQuery();

                res = true;

            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
                res = false;
            }
           
            return res;
        }

        private int getNewRevision(string sheetname, SqlCommand sqlcmd, string tabletype)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            string sqlstr = "";
            try
            {
                switch (tabletype)
                {
                    case TP_SC:
                        sqlstr = "select max(Revision) as MAXREV from SA_Component where SA_ID='" + sheetname + "';";
                        break;
                    case TP_SV:
                        sqlstr = "select max(Revision) as MAXREV from SA_Variable_Map where SA_ID='" + sheetname + "';";
                        break;
                    case TP_STD:
                        //sqlstr = "select max(Revision) as MAXREV from STD_Parts where STD_ImportName='" + sheetname + "';";
                        sqlstr = "select max(Revision) as MAXREV from STD_Parts;";
                        break;
                    case TP_BP:
                        sqlstr = "select max(Revision) as MAXREV from BOM_Plan where Plan_Name='" + sheetname + "';";
                        break;
                    default:
                        sqlstr = "";
                        break;
                }

                if (sqlstr != "")
                {
                    DataTable dt_loc = new DataTable();
                    sqlcmd.CommandText = sqlstr;
                    sqlcmd.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlcmd.ExecuteReader();
                    if (reader.HasRows)
                        dt_loc.Load(reader);

                    if (dt_loc.Rows.Count > 0 && dt_loc.Columns.Count > 0)
                    {
                        if (dt_loc.Rows[0][0] == DBNull.Value)
                            return 0;
                        else
                            return ((int)dt_loc.Rows[0][0]) + 1;
                    }
                    else
                        return 0;
                }
                else
                {
                    showerrstr += "unknown excel type\n";
                    errstr += showerrstr;
                    _logger.Error(errstr);
                    ShowError(showerrstr);
                    return -1;
                }
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
                return -1;
            }
        }

       
        #endregion
    }
}
