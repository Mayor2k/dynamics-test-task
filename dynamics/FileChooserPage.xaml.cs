using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class FileChooserPage : Page
    {
        public FileChooserPage()
        {
            InitializeComponent();
        }

        private void onFolderSearcherClick(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = @"c:\Projects\data\#data";
            WinForms.DialogResult result = folderDialog.ShowDialog();

            if (result == WinForms.DialogResult.OK)
            {
                string sPath = folderDialog.SelectedPath;

                DirectoryInfo[] workDirectories;
                try
                {
                    DirectoryInfo objectsDirectoryInfo = new DirectoryInfo(sPath + @"\card\PKE");
                    workDirectories = objectsDirectoryInfo.GetDirectories();
                    NavigationService.Navigate(new CheckTablePage(workDirectories));
                }
                catch (DirectoryNotFoundException)
                {
                    folerSercherTextBlock.Text = "Вы выбрали неверную директорию!";
                    return;
                }
            }
        }
    }
}
