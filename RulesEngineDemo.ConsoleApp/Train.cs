using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTC.RulesEngine.ConsoleApp
{
    public class Train
    {
        public string Type { get; set; }       // e.g. "Passenger", "Freight"
        public string KeyTrain { get; set; }        // "Y" or "N"
        public string Symbol { get; set; }     // Unique symbol/code for train
    }
}
