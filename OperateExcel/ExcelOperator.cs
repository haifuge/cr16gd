using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Text;
using System;
using System.IO;
using System.Data;

namespace OperateExcel
{
    public class ExcelOperator
    {
        //将数据写入已存在Excel
        public static string ExportCompany(System.Data.DataTable data, string tempPath)
        {
            Application xApp = new Application();

            //2.得到workbook对象，打开已有的文件
            Workbook xBook = xApp.Workbooks.Open(tempPath+ "/ExcelTemplate/CompanyTemplate.xlsx",
                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            //3.指定要操作的Sheet
            Worksheet xSheet = (Worksheet)xBook.Sheets[1];
            //在第一列的左边插入一列  1:第一列
            //xlShiftToRight:向右移动单元格   xlShiftDown:向下移动单元格
            //Range Columns = (Range)xSheet.Columns[1, System.Type.Missing];
            //Columns.Insert(XlInsertShiftDirection.xlShiftToRight, Type.Missing);

            //4.向相应对位置写入相应的数据
            //xSheet.Cells[0][0] = result;
            int row = 0;
            int column = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                row = i + 2;
                for(int j=0;j<data.Columns.Count;j++)
                {
                    column = j + 1;
                    if(j== data.Columns.Count-1)
                    {
                        if (data.Rows[i][j].ToString() == "1")
                        {
                            xSheet.Cells[row, column] = "启用";
                        }
                        else
                            xSheet.Cells[row, column] = "停用";
                    }
                    else
                        xSheet.Cells[row, column] = data.Rows[i][j].ToString();
                }
            }
            string fpath = tempPath + "ExcelFiles";
            if (!Directory.Exists(fpath))
                Directory.CreateDirectory(fpath);
            string fName = fpath+"/Companys" + DateTime.Now.ToShortDateString().Replace(":","").Replace(" ","").Replace("/","")+".xlsx";
            if (File.Exists(fName))
                File.Delete(fName);
            //5.保存保存WorkBook
            xBook.SaveAs(fName);
            //6.从内存中关闭Excel对象
            xSheet = null;
            xBook.Close();
            xBook = null;
            //关闭EXCEL的提示框
            xApp.DisplayAlerts = false;
            //Excel从内存中退出
            xApp.Quit();
            xApp = null;
            return fName;
        }

        public static string ExportCompanysStat(System.Data.DataTable data, string tempPath)
        {
            Application xApp = new Application();

            //2.得到workbook对象，打开已有的文件
            Workbook xBook = xApp.Workbooks.Open(tempPath + "/ExcelTemplate/CompanyStatTemplate.xlsx",
                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            //3.指定要操作的Sheet
            Worksheet xSheet = (Worksheet)xBook.Sheets[1];

            int row = 0;
            int column = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                row = i + 2;
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    column = j + 1;
                    xSheet.Cells[row, column] = data.Rows[i][j].ToString();
                }
            }
            string fpath = tempPath + "ExcelFiles";
            if (!Directory.Exists(fpath))
                Directory.CreateDirectory(fpath);
            string fName = fpath + "/CompanysStat" + DateTime.Now.ToShortDateString().Replace(":", "").Replace(" ", "").Replace("/", "") + ".xlsx";
            if (File.Exists(fName))
                File.Delete(fName);
            //5.保存保存WorkBook
            xBook.SaveAs(fName);
            //6.从内存中关闭Excel对象
            xSheet = null;
            xBook.Close();
            xBook = null;
            //关闭EXCEL的提示框
            xApp.DisplayAlerts = false;
            //Excel从内存中退出
            xApp.Quit();
            xApp = null;
            return fName;
        }
    }
}
