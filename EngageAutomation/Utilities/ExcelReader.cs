using Microsoft.Office.Interop.Excel;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;

namespace Engage.Automation.Utilities
{
    class ExcelReader
    {
        static OleDbConnection conn = null;
        static OleDbCommand cmd = null;
        static OleDbDataAdapter adapter = null;

        static Microsoft.Office.Interop.Excel.Application excel = null;
        static Workbooks workbooks = null;
        static Workbook workbook = null;

        /* ______________Fetching the Entire Excel Sheet Data in to the Dataset_____________________*/
        public static DataSet Excel_Reader(string AppName)
        {
            DataSet datasetexcel = new DataSet();
            //string Path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            //string TestDataFolder = Path + "\\" + TestPath + "\\";
            //string strFile = TestDataFolder + TestExcel + ".xlsx";

            string excelString = ConfigurationManager.AppSettings["excel_location"];

            if (excelString.Equals(string.Empty))
                excelString = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestData\\" + AppName + "_TestData.xlsx";//File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestData\\" + AppName + "_TestData.xlsx");
            else
                excelString = excelString + "\\" + AppName + "_TestData.xlsx";// File.ReadAllText(excelString + "\\" + AppName + "_TestData.xlsx");

            try
            {
                string strConnectionString = "";

                if (excelString.Trim().EndsWith(".xlsx"))
                {
                    strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\"", excelString);  //Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\";"
                }
                else if (excelString.Trim().EndsWith(".xls"))
                {
                    strConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES\"", excelString);  //Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";"
                }

                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.DisplayAlerts = false;
                workbooks = excel.Application.Workbooks;
                workbook = workbooks.Open(excelString);

                using (conn = new OleDbConnection(strConnectionString))
                {
                    conn.Open();

                    using (cmd = conn.CreateCommand())
                    {
                        using (adapter = new OleDbDataAdapter())
                        {
                            foreach (Microsoft.Office.Interop.Excel.Worksheet wsheet in workbook.Worksheets)
                            {
                                cmd.CommandText = "SELECT * FROM [" + wsheet.Name + "$]";
                                adapter.SelectCommand = cmd;
                                adapter.Fill(datasetexcel.Tables.Add(wsheet.Name));
                                if (wsheet != null) Marshal.ReleaseComObject(wsheet);
                            }
                        }
                    }
                }
                conn.Close();
                conn.Dispose();

                workbook.Close();
                workbook = null;
                workbooks.Close();
                workbooks = null;
                excel.Quit();
                excel = null;

                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (workbooks != null) Marshal.ReleaseComObject(workbooks);
                if (excel != null) Marshal.ReleaseComObject(excel);

                CommonFunctions.KillProcess("excel");
                CommonFunctions.KillProcess("EXCEL");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (workbooks != null) Marshal.ReleaseComObject(workbooks);
                if (excel != null) Marshal.ReleaseComObject(excel);
            }
            return datasetexcel;
        }
    }
}
