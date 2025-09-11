using System;
using System.Collections.Generic;

namespace PTC.RulesEngine.Core.Models
{
    public class RuleResult
    {
        public string RuleName { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public object InputData { get; set; }
        public Dictionary<string, object>  OutputData{ get; set; } 
    }
}