using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineDemo.ConsoleApp
{
    public class Restriction
    {
        public string Type { get; set; }        // e.g. "SpeedLimit", "Weather"
        public string Subdivision { get; set; } // Subdivision name/code
        public double Speed { get; set; }       // Max allowed speed
        public string Track { get; set; }                 // Track(s) affected
        public string ReasonCode { get; set; }    // Reason code from mainframe system
    }
}
