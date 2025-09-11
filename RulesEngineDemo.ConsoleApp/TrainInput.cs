using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineDemo.ConsoleApp
{
    public class TrainInput
    {
        public string Subdivision { get; set; }
        public string Track { get; set; }
        public double Speed { get; set; }            // current speed
        public string ReasonCode { get; set; }      // e.g. "Weather"
        public string TypeRestriction { get; set; } // e.g. A-Z or restriction type
        public string KeyTrain { get; set; }          // Y / N
        public string Route { get; set; }           // 6-char
        public string Source { get; set; }          // 3-char
        public string Destination { get; set; }     // 3-char
        public string CarType { get; set; }  //  3-char codes
        public string TrainType { get; set; }    // string
    }
}
