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
using System.Windows.Navigation;
using System.Windows.Shapes;

using WinForms = System.Windows.Forms;

namespace dynamics
{
    /// <summary>
    /// Логика взаимодействия для FileChooserPage.xaml
    /// </summary>
    public partial class FileChooserPage : Page
    {
        public FileChooserPage()
        {
            InitializeComponent();
        }

        public FileChooserPage(string error)
        {
            InitializeComponent();
            folerSercherTextBlock.Text = error;

        }
        private void onFolderSearcherClick(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            //folderDialog.SelectedPath = @"c:\Projects\data\#data";
            folderDialog.SelectedPath = @"c:\";
            WinForms.DialogResult result = folderDialog.ShowDialog();

            if (result == WinForms.DialogResult.OK)
            {
                string sPath = folderDialog.SelectedPath;
                CheckTablePage checkTablePage = new CheckTablePage(sPath);
                Application.Current.MainWindow.Content = checkTablePage;
            }
        }
    }
}
