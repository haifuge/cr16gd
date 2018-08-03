using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace OperateExcel
{
    public class ExcelOperator
    {
        //将数据写入已存在Excel
        public static void writeExcel(System.Data.DataTable data, string filepath)
        {
            Application xApp = new Application();

            //2.得到workbook对象，打开已有的文件
            Workbook xBook = xApp.Workbooks.Open(filepath,
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
            for (int i = 0; i < data.Columns.Count; i++)
                xSheet.Cells[0][i] = data.Columns[i].ColumnName;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                row = i + 1;
                for(int j=0;j<data.Columns.Count;j++)
                {
                    xSheet.Cells[row][j] = data.Rows[i][j].ToString();
                }
            }

            //5.保存保存WorkBook
            xBook.Save();
            //6.从内存中关闭Excel对象
            xSheet = null;
            xBook.Close();
            xBook = null;
            //关闭EXCEL的提示框
            xApp.DisplayAlerts = false;
            //Excel从内存中退出
            xApp.Quit();
            xApp = null;
        }
    }
}
