using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using WinForms = System.Windows.Forms;

namespace dynamics
{
    public partial class CurrentCheckTableWindow : System.Windows.Window
    {
        public Dictionary<string, List<string>> schemes = new Dictionary<string, List<string>>()
        {
                { "1", new List<string>{ "pke_cxema", "TimeTek", "UA", "IA", "PA", "QA", "SA", "Freq", "sigmaUy" } },
                { "2", new List<string>{ "pke_cxema", "TimeTek", "UAB", "UBC", "UCA", "IAB",
                    "IBC", "ICA", "IA", "IB", "IC", "PO", "PP", "QO", "QP", "SO", "SP", "UO",
                    "UP", "IO", "IP", "KO", "Freq", "sigmaUy", "sigmaUyAB", "sigmaUyBC", "sigmaUyCA" }
                },
                { "3", new List<string>{ "pke_cxema", "TimeTek", "UAB", "UBC", "UCA ", "IA", "IB", "IC",
                    "UA", "UB", "UC", "PO", "PP", "PH", "QO", "QP", "QH", "SO", "SP", "SH", "UO", "UP",
                    "UH", "IO", "IP", "IH", "KO", "KH", "Freq", "sigmaUy", "sigmaUyA", "sigmaUyB", "sigmaUyC" }
                }
        };
        private CheckObject checkObject;
        public CurrentCheckTableWindow(CheckObject currentObject)
        {
            InitializeComponent();
            checkObject = currentObject;
            for (int currentColumn = 0; currentColumn < schemes[checkObject.schemeNumber].Count; currentColumn++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = schemes[checkObject.schemeNumber][currentColumn];
                column.Binding = new Binding("[" + currentColumn.ToString() + "]");
                rootDataGrid.Columns.Add(column);
            }

            for (int currentRow = 0; currentRow < checkObject.checksList.Count; currentRow++)
            {
                rootDataGrid.Items.Add(checkObject.checksList[currentRow]);
            }

            Console.WriteLine(schemes[checkObject.schemeNumber].Count);
        }

        private void exportToExelClick(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                SelectedPath = @"C:\"
            };
            WinForms.DialogResult result = folderDialog.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                string sPath = folderDialog.SelectedPath;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    MessageBox.Show("Excel не установлен на Вашем компьютере!");
                    return;
                }

                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue); ;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                for (int currentExcelColumn = 0; currentExcelColumn < schemes[checkObject.schemeNumber].Count; currentExcelColumn++)
                {
                    xlWorkSheet.Cells[1, currentExcelColumn + 1] = schemes[checkObject.schemeNumber][currentExcelColumn];
                }

                for (int currentExcelRow = 0; currentExcelRow < checkObject.checksList.Count; currentExcelRow++)
                {
                    for (int currentExcelColumn = 0; currentExcelColumn < checkObject.checksList[currentExcelRow].Count; currentExcelColumn++)
                    {
                        xlWorkSheet.Cells[currentExcelRow + 2, currentExcelColumn + 1] = checkObject.checksList[currentExcelRow][currentExcelColumn];
                    }
                }

                xlWorkBook.SaveAs(sPath + @"\" + checkObject.uid + ".xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue,
                misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);


                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);

                MessageBox.Show("Файл успешно создан в: " + sPath + @"\" + checkObject.uid + ".xlsx");
            }
        }
    }
}
