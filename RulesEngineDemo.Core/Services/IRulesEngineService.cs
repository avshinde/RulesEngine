using PTC.RulesEngine.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTC.RulesEngine.Core.Services
{
    public interface IRulesEngineService
    {
        Task<List<RuleResult>> ExecuteRulesAsync<T>(string workflowName, T inputData, string inputName);
    }
}