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
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace dynamics
{
    public partial class CheckTablePage : Page
    {
        public CheckTablePage(string path)
        {
            InitializeComponent();

            DirectoryInfo[] workDirectories;
            try
            {
                DirectoryInfo objectsDirectoryInfo = new DirectoryInfo(path + @"\card\PKE");
                workDirectories = objectsDirectoryInfo.GetDirectories();
            }
            catch (DirectoryNotFoundException)
            {
                FileChooserPage fileChooserPage = new FileChooserPage("Вы выбрали невернуб директорию!");
                Application.Current.MainWindow.Content = fileChooserPage;
                return;

            }

            Dictionary<string, string[]> uids = new Dictionary<string, string[]>();
            //перебор директорий с объектами
            for (int currentObjectDirNum = 0; currentObjectDirNum < workDirectories.Length; currentObjectDirNum++)
            {
                //перебор директорий с временем проверок объекта
                for (int currentObjectTimeDirNum = 0; currentObjectTimeDirNum < workDirectories[currentObjectDirNum].GetDirectories().Length; currentObjectTimeDirNum++)
                {
                    //перебор файлов с проверками
                    for (int currentCheckFileNum = 0; currentCheckFileNum < workDirectories[currentObjectDirNum].GetDirectories()[currentObjectTimeDirNum].GetFiles().Length; currentCheckFileNum++)
                    {
                        XmlDocument currentCheckDocument = new XmlDocument();
                        currentCheckDocument.Load(workDirectories[currentObjectDirNum].GetDirectories()[currentObjectTimeDirNum].GetFiles()[currentCheckFileNum].FullName);
                        XmlElement currentCheckElement = currentCheckDocument.DocumentElement;
                        foreach (XmlNode currentCheckNode in currentCheckElement)
                        {
                            if (currentCheckNode.Name == "Param_Check_PKE" && !uids.ContainsKey(currentCheckElement.GetAttribute("UID")))
                            {
                                string[] objectInfo = new string[5]
                                {
                                    currentCheckNode.Attributes.GetNamedItem("nameObject").Value,
                                    currentCheckNode.Attributes.GetNamedItem("TimeStart").Value,
                                    currentCheckNode.Attributes.GetNamedItem("TimeStop").Value,
                                    currentCheckNode.Attributes.GetNamedItem("active_cxema").Value,
                                    currentCheckNode.Attributes.GetNamedItem("averaging_interval_time").Value
                                };
                                uids.Add(currentCheckElement.GetAttribute("UID"), objectInfo);

                                rootGrid.RowDefinitions.Add(new RowDefinition());
                                for (int currentColumn = 0; currentColumn < objectInfo.Length; currentColumn++)
                                {
                                    TextBlock textBlock = new TextBlock();
                                    if (currentColumn == 1 || currentColumn == 2)
                                    {
                                        DateTime dataTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(objectInfo[currentColumn])).LocalDateTime;
                                        textBlock.Text = dataTime.ToString();
                                    }
                                    else
                                    {
                                        textBlock.Text = objectInfo[currentColumn];
                                    }
                                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                                    textBlock.SetValue(Grid.ColumnProperty, currentColumn);
                                    textBlock.SetValue(Grid.RowProperty, uids.Keys.Count);
                                    rootGrid.Children.Add(textBlock);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
