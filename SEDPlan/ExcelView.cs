using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Data;

using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace SEDPlan
{
    public class ExcelView
    {
        #region Class Field and Property

        private string m_filepath;
        private string m_sheetname;
        private Application m_xlsapp;
        private Workbook m_xlswbk;
        private Worksheet m_worksheet;
        private Range m_DataRange;
        private System.Data.DataTable m_dtImportData;

        private int start_rowidx = -1;
        private int start_colidx = -1;
        private int end_rowidx = -1;
        private int end_colidx = -1;
        private int crr_rowidx = -1;
        private int crr_colidx = -1;

        private const int EMPTY_ROWNUM = 3;
        private const int EMPTY_COLNUM = 3;
        private const int MAXROWNUM = 2000;
        private const int MAXCOLNUM = 2000;

        private string[] m_rowheads;
        private string[] m_colheads;

        public string FilePath
        {
            get
            {
                return this.m_filepath;
            }
        }

        public string SheetName
        {
            get
            {
                return this.m_sheetname;
            }
        }

        public string[] RowHeads
        {
            get
            {
                return this.m_rowheads;
            }
        }

        public string[] ColHeads
        {
            get
            {
                return this.m_colheads;
            }
        }

        public System.Data.DataTable ImportData
        {
            get
            {
                return m_dtImportData;
            }
        }

        private string showerrstr;
        public string ErrorInfo
        {
            get
            {
                return this.showerrstr;
            }
        }

        public event EventHandler<ErrorEventArgs> OnErrorHappened;

        // The name of current class 
        private static readonly string _className =
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        // Create a logger for use in this class
        private static readonly log4net.ILog _logger =
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region Constructor, Dispose, Finalize and Destructor

        public void Init(string filepath, string sheetname)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";
            int i;
            this.showerrstr = "";
            crr_rowidx = -1;
            start_rowidx = -1;
            start_colidx = -1;
            end_rowidx = -1;
            end_colidx = -1;

            try
            {
                this.m_filepath = Path.GetFullPath(filepath).Trim();
                if (!FileExists(this.m_filepath))
                {
                    showerrstr += "The File:" + this.m_filepath + " does not exist.\n";
                    errstr += showerrstr;
                    _logger.Error(errstr);
                    ShowError(showerrstr);
                    return;
                }

                this.m_sheetname = sheetname.Trim();
                m_xlsapp = new Application();
                m_xlswbk = m_xlsapp.Workbooks.Open(this.m_filepath,
                true, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                if (this.m_sheetname == "")
                    this.m_sheetname = ((Worksheet)m_xlswbk.Sheets[1]).Name;

                // look for the specified worksheet named sheetname
                i = 1;
                foreach (Worksheet crr_wksht in this.m_xlswbk.Sheets)
                {
                    if (crr_wksht.Name == this.m_sheetname)
                        break;
                    else
                        i++;
                }
                if (i > this.m_xlswbk.Sheets.Count)
                {
                    showerrstr += "Cannot Find Specified worksheet.\n"
                        +"File Path:" + this.m_filepath + ".\n"
                        +"WorkSheet:" + sheetname + ".\n";
                    errstr += showerrstr;
                    _logger.Error(errstr);
                    ShowError(showerrstr);
                    return;
                }
                else
                {
                    this.m_worksheet = (Worksheet)m_xlswbk.Sheets[i];
                }

                if (this.m_worksheet != null)
                {
                    // Search Data Area
                    this.GetDataRange();
                }
                //else
                //{
                //}

                if (this.m_DataRange != null && this.m_DataRange.Rows.Count > 0 && this.m_colheads != null && this.m_colheads.Length > 0)
                {
                    // Fetch data from excel
                    this.FetchData();
                }
                //else
                //{
                //    showerrstr += "Cannot find Data Range.\n"
                //        + "File Path:" + this.m_filepath + ".\n"
                //        + "WorkSheet:" + SAID + ".\n";
                //    ShowError(showerrstr);
                //    _logger.Error(showerrstr);
                //    return;
                //}

                if (this.m_dtImportData != null && this.m_dtImportData.Rows.Count > 0)
                {
                    _logger.Info("ExcelView Initialization complete");
                }
                //else
                //{
                //    showerrstr += "Cannot fetch data.\n"
                //        + "File Path:" + this.m_filepath + ".\n"
                //        + "WorkSheet:" + SAID + ".\n";
                //    ShowError(showerrstr);
                //    _logger.Error(showerrstr);
                //    return;
                //}
                this.CloseExcel();
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
            }
        }

        ~ExcelView()
        {
            Dispose();
        }

        private void CloseExcel()
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            try
            {
                this.m_DataRange = null;
                this.m_worksheet = null;

                if (this.m_xlswbk != null)
                {
                    this.m_xlswbk.Close();

                    this.m_xlswbk = null;
                }

                if (this.m_xlsapp != null)
                {
                    this.m_xlsapp.Quit();
                    this.m_xlsapp = null;
                }
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
            }
        }

        public void Dispose()
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            try
            {
                CloseExcel();

                if (this.m_dtImportData != null)
                {
                    this.m_dtImportData.Dispose();
                    this.m_dtImportData = null;
                }
                _logger.Info("SEDPlan Quit!");
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
            }
        }

        public void ClearData()
        {
            CloseExcel();
            if (this.m_dtImportData != null)
            {
                this.m_dtImportData.Clear();
                this.m_dtImportData = null;
            }

            if (this.m_dtImportData != null)
            {
                this.m_dtImportData.Dispose();
                this.m_dtImportData = null;
            }
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

        private bool FileExists(string filepath)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            try
            {
                FileInfo file = new FileInfo(this.m_filepath);
                return file.Exists;
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
                return false;
            }

        }

        private void GetDataRange()
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            try
            {

                int i = 1, j = 1;
                // Find starting row and column
                for (i = 1; i <= EMPTY_ROWNUM; i++)
                {
                    for (j = 1; j <= EMPTY_COLNUM; j++)
                    {
                        Range startcell = (Range)m_worksheet.Cells[i, j];
                        if (startcell.Value != null && startcell.Value != "")
                        {
                            this.start_rowidx = i;
                            this.start_colidx = j;
                            break;
                        }
                    }
                    if (start_rowidx != -1 && start_colidx != -1)
                        break;
                }

                if (i > EMPTY_ROWNUM && j > EMPTY_COLNUM)
                {
                    showerrstr += "Cannot find valid data in first 3 rows and columns.\n"
                        + "File Path:" + this.m_filepath + ".\n"
                        + "WorkSheet:" + this.m_sheetname + ".\n";
                    errstr += showerrstr;
                    _logger.Error(errstr);
                    ShowError(showerrstr);
                    return;
                }

                int colempty = 0;
                int rowempty = 0;
                // Find ending row
                for (i = start_rowidx; i <= MAXROWNUM; i++)
                {
                    Range rowhead = m_worksheet.Cells[i, start_colidx];
                    if (rowhead.Value == null || rowhead.Text.ToString() == "")
                        rowempty++;
                    else
                        rowempty = 0;
                    if (rowempty == EMPTY_ROWNUM)
                        break;
                }
                this.end_rowidx = i - rowempty;

                // Find endingcolumn 
                for (j = start_colidx; j <= MAXCOLNUM; j++)
                {
                    Range colhead = (Range)m_worksheet.Cells[start_rowidx, j];
                    if (colhead.Value == null || colhead.Text.ToString() == "")
                        colempty++;
                    else
                        colempty = 0;
                    if (colempty == EMPTY_COLNUM)
                        break;
                }
                this.end_colidx = j - colempty;

                // Fetch Data Range
                if (start_rowidx != -1 && start_colidx != -1 && end_rowidx != -1 && end_colidx != -1)
                {
                    Array array_str;
                    char ch_start_colidx = (char)((int)'A' + start_colidx - 1);
                    char ch_end_colidx = (char)((int)'A' + end_colidx - 1);

                    this.m_DataRange = m_worksheet.get_Range(ch_start_colidx + (start_rowidx + 1).ToString(), ch_end_colidx + end_rowidx.ToString());

                    Range rg_colheads = m_worksheet.get_Range(ch_start_colidx + start_rowidx.ToString(), ch_end_colidx + start_rowidx.ToString());
                    array_str = (Array)rg_colheads.Cells.Value;
                    this.m_colheads = new string[array_str.Length];
                    i = 0;
                    foreach (object item in array_str)
                    {
                        this.m_colheads[i] = (string)item;
                        i++;
                    }
                }
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
            }

        }

        private void FetchData()
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            try
            {
                this.m_dtImportData = new System.Data.DataTable();
                foreach (string colname in this.m_colheads)
                {
                    DataColumn newcol = new DataColumn(colname, typeof(string));
                    this.m_dtImportData.Columns.Add(newcol);
                }

                Array rowarr;
                bool emptyrow;
                string[] rowcells;
                int i;
                foreach (Range crrrow in this.m_DataRange.Rows)
                {
                    emptyrow = true;
                    rowarr = (Array)crrrow.Cells.Value;
                    rowcells = new string[rowarr.Length];
                    i = 0;
                    foreach (object item in rowarr)
                    {
                        if (item != null)
                            rowcells[i] = item.ToString();
                        else
                            rowcells[i] = "";

                        if (emptyrow == true && rowcells[i] != null && rowcells[i] != "")
                            emptyrow = false;
                        i++;
                    }


                    if (emptyrow != true)
                    {
                        this.m_dtImportData.Rows.Add(rowcells);
                    }
                }
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
            }

        }
        #endregion
    }
}
