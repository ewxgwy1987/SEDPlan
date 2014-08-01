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
        private string projectno;
        private string sheetname;

        private const string XCFG_CONNSTR = "connectionString";
        private const string XCFG_VARTYPE = "VariableType";
        private const string XCFG_SQLREVISION = "SqlRevision";

        // SV Tags
        private const string XCFG_DDV = "DDVariable";
        private const string XCFG_DDV_COLUMNS = "DDVColumns";
        private const string XCFG_DDV_PARAMETERS = "DDVParameters";
        private const string XCFG_DDV_STP = "DDVSTP";

        // STD Tags
        private const string XCFG_STD = "StandardParts";
        private const string XCFG_STD_COLUMNS = "STDColumns";
        private const string XCFG_STD_PARAMETERS = "STDParameters";
        private const string XCFG_STD_STP = "STDSTP";

        // FW tags
        private const string XCFG_FW = "FixedWeight";
        private const string XCFG_FW_COLUMNS = "FWColumns";
        private const string XCFG_FW_PARAMETERS = "FWParameters";
        private const string XCFG_FW_STP = "FWSTP";

        // DDT Tags
        private const string XCFG_DDT = "DrawingDesingTypes";
        private const string XCFG_DDT_COLUMNS = "DDTColumns";
        private const string XCFG_DDT_PARAMETERS = "DDTParameters";
        private const string XCFG_DDT_STP = "DDTSTP";


        // SC TAGS
        private const string XCFG_SC = "SAComponent";
        private const string XCFG_SC_COLUMNS = "SCColumns";
        private const string XCFG_SC_PARAMETERS = "SCParameters";
        private const string XCFG_SC_STP = "SCSTP";


        // BP Tags
        private const string XCFG_BP = "BOMPlan";
        private const string XCFG_BP_COLUMNS = "BPColumns";
        private const string XCFG_BP_VARNAMES = "BPVarNames";
        private const string XCFG_BP_PARAMETERS = "BPParameters";
        private const string XCFG_BP_STP = "BPSTP";

        // SAIF Tags
        private const string XCFG_SAIF = "SAInfomation";
        private const string XCFG_SAIF_COLUMNS = "SIColumns";
        private const string XCFG_SAIF_PARAMETERS = "SIParameters";
        private const string XCFG_SAIF_STP = "SISTP";

        private const string PARA_VAR = "VAR";
        private const string PARA_WEIGHT = "WEIGHT";
        private const string TP_DDV = "DDV";
        private const string TP_STD = "STD";
        private const string TP_FW = "FW";
        private const string TP_DDT = "DDT";
        private const string TP_SC = "SC";
        private const string TP_BP = "BP";
        private const string TP_SAIF = "SAIF";

        private string connstr;
        private string[] Var_Types;

        // DDV 
        private string[] DDV_ColNames;
        private string[] DDV_ParaNames;
        private string DDV_STPName;
        private string DDV_SQL_REVISION;

        // STD
        private string[] STD_ColNames;
        private string[] STD_ParaNames;
        private string STD_STPName;
        private string STD_SQL_REVISION;

        // FW
        private string[] FW_ColNames;
        private string[] FW_ParaNames;
        private string FW_STPName;
        private string FW_SQL_REVISION;

        // DDT
        private string[] DDT_ColNames;
        private string[] DDT_ParaNames;
        private string DDT_STPName;
        private string DDT_SQL_REVISION;

        // SC
        private string[] SC_ColNames;
        private string[] SC_ParaNames;
        private string SC_STPName;
        private string SC_SQL_REVISION;

        // BP
        private string[] BP_ColNames;
        private string[] BP_VarNames;
        private string[] BP_ParaNames;
        private string BP_STPName;
        private string BP_SQL_REVISION;

        // SAIF
        private string[] SAIF_ColNames;
        private string[] SAIF_ParaNames;
        private string SAIF_STPName;
        private string SAIF_SQL_REVISION;

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

            // Fetch DD Variable configuration
            XmlNode xml_DDV = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_DDV);
            string str_SVCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_DDV, XCFG_DDV_COLUMNS, "");
            this.DDV_ColNames = str_SVCols.Split(',');
            string str_SVParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_DDV, XCFG_DDV_PARAMETERS, "");
            this.DDV_ParaNames = str_SVParas.Split(',');
            this.DDV_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_DDV, XCFG_DDV_STP, "");
            this.DDV_SQL_REVISION = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_DDV, XCFG_SQLREVISION, "");

            // Feth Standard Parts configuration
            XmlNode xml_STD = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_STD);
            string str_STDCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_STD, XCFG_STD_COLUMNS, "");
            this.STD_ColNames = str_STDCols.Split(',');
            string str_STDParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_STD, XCFG_STD_PARAMETERS, "");
            this.STD_ParaNames = str_STDParas.Split(',');
            this.STD_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_STD, XCFG_STD_STP, "");
            this.STD_SQL_REVISION = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_STD, XCFG_SQLREVISION, "");

            // (FW)Fetch Fixed Weight configuration
            XmlNode xml_FW = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_FW);
            string str_FWCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_FW, XCFG_FW_COLUMNS, "");
            this.FW_ColNames = str_FWCols.Split(',');
            string str_FWParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_FW, XCFG_FW_PARAMETERS, "");
            this.FW_ParaNames = str_FWParas.Split(',');
            this.FW_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_FW, XCFG_FW_STP, "");
            this.FW_SQL_REVISION = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_FW, XCFG_SQLREVISION, "");

            // (DDT)Fetch Detail Design Types configuration
            XmlNode xml_DDT = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_DDT);
            string str_DDTCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_DDT, XCFG_DDT_COLUMNS, "");
            this.DDT_ColNames = str_DDTCols.Split(',');
            string str_DDTParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_DDT, XCFG_DDT_PARAMETERS, "");
            this.DDT_ParaNames = str_DDTParas.Split(',');
            this.DDT_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_DDT, XCFG_DDT_STP, "");
            this.DDT_SQL_REVISION = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_DDT, XCFG_SQLREVISION, "");

            // Fetch SA Component configuration
            XmlNode xml_SC = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_SC);
            string str_SCCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SC, XCFG_SC_COLUMNS, "");
            this.SC_ColNames = str_SCCols.Split(',');
            string str_SCParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SC, XCFG_SC_PARAMETERS, "");
            this.SC_ParaNames = str_SCParas.Split(',');
            this.SC_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SC, XCFG_SC_STP, "");
            this.SC_SQL_REVISION = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SC, XCFG_SQLREVISION, "");

            // Fetch BOM Plan configuration
            XmlNode xml_BP = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_BP);
            string str_BPCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_BP, XCFG_BP_COLUMNS, "");
            this.BP_ColNames = str_BPCols.Split(',');
            string str_BPVarNames = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_BP, XCFG_BP_VARNAMES, "");
            this.BP_VarNames = str_BPVarNames.Split(',');
            string str_BPParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_BP, XCFG_BP_PARAMETERS, "");
            this.BP_ParaNames = str_BPParas.Split(',');
            this.BP_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_BP, XCFG_BP_STP, "");
            this.BP_SQL_REVISION = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_BP, XCFG_SQLREVISION, "");

            // (SAIF)Fetch SA Information configuration
            XmlNode xml_SAIF = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xconfig, XCFG_SAIF);
            string str_SAIFCols = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SAIF, XCFG_SAIF_COLUMNS, "");
            this.SAIF_ColNames = str_SAIFCols.Split(',');
            string str_SAIFParas = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SAIF, XCFG_SAIF_PARAMETERS, "");
            this.SAIF_ParaNames = str_SAIFParas.Split(',');
            this.SAIF_STPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SAIF, XCFG_SAIF_STP, "");
            this.SAIF_SQL_REVISION = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xml_SAIF, XCFG_SQLREVISION, "");
   
        }

        public bool Init(DataTable dtImport, string sheetname, string projectID)
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
                    this.projectno = projectID;
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

                int revision = this.getNewRevision(sqlcmd, tabletype);
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
                            case TP_DDV:
                                paras = AnalyzeDataRow_DDV(dr, revision.ToString(), crtime);
                                break;
                            case TP_STD:
                                paras = AnalyzeDataRow_STD(dr, revision.ToString(), crtime);
                                break;
                            case TP_FW:
                                paras = AnalyzeDataRow_FW(dr, revision.ToString(), crtime);
                                break;
                            case TP_DDT:
                                paras = AnalyzeDataRow_DDT(dr, revision.ToString(), crtime);
                                break;
                            case TP_SC:
                                paras = AnalyzeDataRow_SC(dr, revision.ToString(), crtime);
                                break;     
                            case TP_BP:
                                paras = AnalyzeDataRow_BP(dr, revision.ToString(), crtime);
                                break;
                            case TP_SAIF:
                                paras = AnalyzeDataRow_SAIF(dr, revision.ToString(), crtime);
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

        private string DataRowColumnSearch(DataColumnCollection dtCols, string colname)
        {
            string res_colname = null;
            string tmp_colname = "";
            int ch_idx = -1;

            if (colname == null || colname == "")
                return res_colname;

            foreach (DataColumn col in dtCols)
            {
                ch_idx = col.ColumnName.IndexOf('(');
                if (ch_idx > -1)
                {
                    tmp_colname = col.ColumnName.Substring(0, ch_idx);
                    if (tmp_colname.Length > 0 && tmp_colname.Trim().ToUpper() == colname.ToUpper())
                    {
                        res_colname = col.ColumnName;
                        break;
                    }
                }
                else
                {
                    if (col.ColumnName.Trim().ToUpper() == colname.ToUpper())
                    {
                        res_colname = col.ColumnName;
                        break;
                    }
                }
            }

            return res_colname;
        }

        private string[] AnalyzeDataRow_DDV(DataRow dr, string revision, string created_time)
        {
            string drcolname = "";
            string[] paras = new string[this.DDV_ColNames.Length + 3];
            paras[0] = this.sheetname.Trim(); // sheet name is DD ID
            int i = 1;
            foreach (string colname in this.DDV_ColNames)
            {
                paras[i] = "";
                drcolname = DataRowColumnSearch(dr.Table.Columns, colname);
                if (drcolname != null && drcolname.Length > 0)
                    paras[i] = ((string)dr[drcolname]).Trim();
                else
                {
                    if (colname.ToUpper().IndexOf("WEIGHT") != -1)
                    {
                        string errstr = "Import Data Error: Cannot find the specified column - " + colname + " for ExcelType: Detail Design Variabls(A, L & LS)";
                        throw new Exception(errstr);
                    }
                }
                i++;
            }
            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_STD(DataRow dr, string revision, string created_time)
        {
            string drcolname = "";
            string[] paras = new string[this.STD_ColNames.Length + 2];
            //paras[0] = this.sheetname.Trim();

            int i = 0;
            foreach (string colname in this.STD_ColNames)
            {
                paras[i] = "";
                drcolname = DataRowColumnSearch(dr.Table.Columns, colname);
                if (drcolname != null && drcolname.Length > 0)
                    paras[i] = ((string)dr[drcolname]).Trim();
                else
                {
                    string errstr = "Import Data Error: Cannot find the specified column - " + colname + " for ExcelType: Standard Parts";
                    throw new Exception(errstr);
                }
                i++;
            }
            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_FW(DataRow dr, string revision, string created_time)
        {
            string drcolname = "";
            string[] paras = new string[this.FW_ColNames.Length + 2];
            //paras[0] = this.sheetname.Trim();

            int i = 0;
            foreach (string colname in this.FW_ColNames)
            {
                paras[i] = "";
                drcolname = DataRowColumnSearch(dr.Table.Columns, colname);
                if (drcolname != null && drcolname.Length > 0)
                    paras[i] = ((string)dr[drcolname]).Trim();
                else
                {
                    string errstr = "Import Data Error: Cannot find the specified column - " + colname + " for ExcelType: Detail Design Fixed Weight";
                    throw new Exception(errstr);
                }
                i++;
            }
            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_DDT(DataRow dr, string revision, string created_time)
        {
            string drcolname = "";
            string[] paras = new string[this.DDT_ColNames.Length + 3];
            paras[0] = this.sheetname.Trim(); // sheet name is SA ID
            int i = 1;
            foreach (string colname in this.DDT_ColNames)
            {
                paras[i] = "";
                drcolname = DataRowColumnSearch(dr.Table.Columns, colname);
                if (drcolname != null && drcolname.Length > 0)
                    paras[i] = ((string)dr[drcolname]).Trim();
                else
                {
                    string errstr = "Import Data Error: Cannot find the specified column - " + colname + " for ExcelType: SA Detail Design Types";
                    throw new Exception(errstr);
                }
                i++;
            }
            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_SC(DataRow dr,string revision, string created_time)
        {
            string drcolname = "";
            string[] paras = new string[this.SC_ColNames.Length + 3];
            paras[0] = this.sheetname.Trim(); // sheet name is SA ID

            int i = 1;
            foreach (string colname in this.SC_ColNames)
            {
                paras[i] = "";
                if (colname == PARA_VAR)
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
                        // some variables may not be shown in excel
                        //else
                        //{
                        //    string errstr = "Import Data Error: Cannot find the variable type - " + vartype + " for ExcelType: SA Component";
                        //    throw new Exception(errstr);
                        //}
                    }
                }
                else
                {
                    if (dr.Table.Columns.Contains(colname))
                        paras[i] = ((string)dr[colname]).Trim();
                    else
                    {
                        string errstr = "Import Data Error: Cannot find the specified column - " + colname + " for ExcelType: SA Component";
                        throw new Exception(errstr);
                    }
                }

                i++;
            }

            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_BP(DataRow dr, string revision, string created_time)
        {
            string drcolname = "";
            string[] paras = new string[this.BP_ColNames.Length + 2 + 4];
            paras[0] = this.projectno;
            paras[1] = this.sheetname.Trim(); // sheetname is the plan name
            int i = 2;

            // the non-var columns 
            foreach (string colname in this.BP_ColNames)
            {
                paras[i] = "";
                drcolname = DataRowColumnSearch(dr.Table.Columns, colname);
                if (drcolname != null && drcolname.Length > 0)
                    paras[i] = ((string)dr[drcolname]).Trim();
                else
                {
                    string errstr = "Import Data Error: Cannot find the specified column - " + colname + " for ExcelType: BOM Plan";
                    throw new Exception(errstr);
                }
                i++;
            }

            // the var columns
            string varnames = "";
            string vars = "";
            int j = 0;
            foreach (string varname in this.BP_VarNames)
            {
                if (dr.Table.Columns.Contains(varname))
                {
                    varnames += varname;
                    vars += ((string)dr[varname]).Trim();
                    if (j != this.BP_VarNames.Length - 1)
                    {
                        varnames += ",";
                        vars += ",";
                    }
                }
                else
                {
                    string errstr = "Import Data Error: Cannot find the variable name - " + varname + " for ExcelType: BOM Plan";
                    throw new Exception(errstr);
                }
                j++;
            }
            paras[i] = varnames;
            paras[i + 1] = vars;
            paras[paras.Length - 2] = revision;
            paras[paras.Length - 1] = created_time;
            return paras;
        }

        private string[] AnalyzeDataRow_SAIF(DataRow dr, string revision, string created_time)
        {
            string drcolname = "";
            string[] paras = new string[this.SAIF_ColNames.Length + 2];
            //paras[0] = this.sheetname.Trim();

            int i = 0;
            foreach (string colname in this.SAIF_ColNames)
            {
                paras[i] = "";
                drcolname = DataRowColumnSearch(dr.Table.Columns, colname);
                if (drcolname != null && drcolname.Length > 0)
                    paras[i] = ((string)dr[drcolname]).Trim();
                else
                {
                    string errstr = "Import Data Error: Cannot find the specified column - " + colname + " for ExcelType: SA Information";
                    throw new Exception(errstr);
                }
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
                    case TP_DDV:
                        paranames = this.DDV_ParaNames;
                        sqlcmd.CommandText = DDV_STPName;
                        break;
                    case TP_STD:
                        paranames = this.STD_ParaNames;
                        sqlcmd.CommandText = STD_STPName;
                        break;
                    case TP_FW:
                        paranames = this.FW_ParaNames;
                        sqlcmd.CommandText = FW_STPName;
                        break;
                    case TP_DDT:
                        paranames = this.DDT_ParaNames;
                        sqlcmd.CommandText = DDT_STPName;
                        break;
                    case TP_SC:
                        paranames = this.SC_ParaNames;
                        sqlcmd.CommandText = SC_STPName;
                        break;
                    case TP_BP:
                        paranames = this.BP_ParaNames;
                        sqlcmd.CommandText = BP_STPName;
                        break;
                    case TP_SAIF:
                        paranames = this.SAIF_ParaNames;
                        sqlcmd.CommandText = SAIF_STPName;
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
                    sPararameter[i] = new SqlParameter("@" + paranames[i], SqlDbType.NVarChar, 500);
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

        private int getNewRevision(SqlCommand sqlcmd, string tabletype)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            string sqlstr = "";
            try
            {
                switch (tabletype)
                {
                    case TP_DDV:
                        sqlstr = this.DDV_SQL_REVISION + "'" + this.sheetname + "';";
                        break;
                    case TP_STD:
                        sqlstr = this.STD_SQL_REVISION;
                        break;
                    case TP_FW:
                        sqlstr = this.FW_SQL_REVISION;
                        break;
                    case TP_DDT:
                        sqlstr = this.DDT_SQL_REVISION + "'" + this.sheetname + "';";;
                        break;
                    case TP_SC:
                        sqlstr = this.SC_SQL_REVISION + "'" + this.sheetname + "';";
                        break;
                    case TP_BP:
                        sqlstr = this.BP_SQL_REVISION + " where Plan_Name='" + this.sheetname + "' AND Project_No='" + this.projectno + "';";
                        break;
                    case TP_SAIF:
                        sqlstr = this.SAIF_SQL_REVISION;
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
