using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class CurrentCheckTableWindow : Window
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
        }

        private void exportToExelClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(checkObject.nameObject);
        }
    }
}
