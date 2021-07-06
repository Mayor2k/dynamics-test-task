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
                pathTextBlock.Text = path;
            }
            catch (DirectoryNotFoundException)
            {
                pathTextBlock.Text = "You are choose the wrong directory!";
                return;

            }

            Dictionary<string, string[]> uids = new Dictionary<string, string[]>();
            //перебор директорий с объектами
            for (int currentObjectDirNum = 0; currentObjectDirNum < workDirectories.Length; currentObjectDirNum++)
            {
                //Console.WriteLine(workDirectories[currentObjectDirNum].Name);

                //перебор директорий с временем проверок объекта
                for (int currentObjectTimeDirNum = 0; currentObjectTimeDirNum < workDirectories[currentObjectDirNum].GetDirectories().Length; currentObjectTimeDirNum++)
                {
                    //Console.WriteLine("> " + workDirectories[currentObjectDirNum].GetDirectories()[currentObjectTimeDirNum].Name);

                    //перебор файлов с проверками
                    for (int currentCheckFileNum = 0; currentCheckFileNum < workDirectories[currentObjectDirNum].GetDirectories()[currentObjectTimeDirNum].GetFiles().Length; currentCheckFileNum++)
                    {
                        //String uid, nameObject, timeStart, timeEnd, intervalTime, schemeNumber;
                        //Console.WriteLine("> > " + workDirectories[currentObjectDirNum].GetDirectories()[currentObjectTimeDirNum].GetFiles()[currentCheckFileNum].Name);
                        XmlDocument currentCheckDocument = new XmlDocument();
                        currentCheckDocument.Load(workDirectories[currentObjectDirNum].GetDirectories()[currentObjectTimeDirNum].GetFiles()[currentCheckFileNum].FullName);
                        XmlElement currentCheckElement = currentCheckDocument.DocumentElement;
                        //Console.WriteLine("> > " + currentCheckElement.GetAttribute("UID"));
                        string[] objectInfo = new string[5];
                        foreach (XmlNode currentCheckNode in currentCheckElement)
                        {
                            if (currentCheckNode.Name == "Param_Check_PKE" && !uids.ContainsKey(currentCheckElement.GetAttribute("UID")))
                            {
                                objectInfo[0] = currentCheckNode.Attributes.GetNamedItem("TimeStart").Value;
                                objectInfo[1] = currentCheckNode.Attributes.GetNamedItem("TimeStop").Value;
                                objectInfo[2] = currentCheckNode.Attributes.GetNamedItem("nameObject").Value;
                                objectInfo[3] = currentCheckNode.Attributes.GetNamedItem("averaging_interval_time").Value;
                                objectInfo[4] = currentCheckNode.Attributes.GetNamedItem("active_cxema").Value;
                                uids.Add(currentCheckElement.GetAttribute("UID"), objectInfo);
                            }
                        }
                    }
                }
            }
            Console.WriteLine(uids);
        }
    }
}
