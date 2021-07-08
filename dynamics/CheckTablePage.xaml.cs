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
        public Dictionary<string, CheckObject> uids = new Dictionary<string, CheckObject>();
        public CheckTablePage(DirectoryInfo[] workDirectories)
        {
            InitializeComponent();

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
                                objectInfo[1] = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(objectInfo[1])).UtcDateTime.ToString();
                                objectInfo[2] = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(objectInfo[2])).UtcDateTime.ToString();

                                CheckObject currentCheckObject = new CheckObject(
                                    currentCheckElement.GetAttribute("UID"),
                                    objectInfo[0],
                                    objectInfo[1],
                                    objectInfo[2],
                                    objectInfo[3],
                                    objectInfo[4],
                                    new List<List<string>>()
                                );
                                uids.Add(currentCheckElement.GetAttribute("UID"), currentCheckObject);
                                rootDataGrid.Items.Add(currentCheckObject);
                            }
                            else if (currentCheckNode.Name == "Result_Check_PKE")
                            {
                                CheckObject checkObject = uids[currentCheckElement.GetAttribute("UID")];
                                List<string> checksList = new List<string>();
                                for (int currentCheckElementNum = 0; currentCheckElementNum < currentCheckNode.Attributes.Count; currentCheckElementNum++)
                                {
                                    XmlNode currentNode = currentCheckNode.Attributes.Item(currentCheckElementNum);
                                    string currentNodeValue = currentNode.Value;
                                    if (currentNode.Name == "TimeTek")
                                    {
                                        currentNodeValue = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(currentNodeValue)).UtcDateTime.ToString();
                                    }
                                    checksList.Add(currentNodeValue);
                                }
                                checkObject.checksList.Add(checksList);
                            }
                        }
                    }
                }
            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            CheckObject currentObject = (CheckObject)row.Item;
            CurrentCheckTableWindow currentCheckTableWindow = new CurrentCheckTableWindow(currentObject);
            currentCheckTableWindow.Show();
        }
    }
}
