using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamics
{
    public class CheckObject
    {
        public string uid { get; set; }
        public string nameObject { get; set; }
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public string schemeNumber { get; set; }
        public string intervalTime { get; set; }
        public List<List<string>> checksList { get; set; }

        public CheckObject(string uid, string nameObject, string timeStart, string timeEnd, string schemeNumber, string intervalTime, List<List<string>> checksList)
        {
            this.uid = uid;
            this.nameObject = nameObject;
            this.timeStart = timeStart;
            this.timeEnd = timeEnd;
            this.schemeNumber = schemeNumber;
            this.intervalTime = intervalTime;        
            this.checksList = checksList;
        }
    }
}
