
using RulesEngine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTC.RulesEngine.Core.Repository
{
    public interface IRulesRepository
    {
        Task<List<Rule>> GetRulesAsync(string workflowName);
        Task<List<Workflow>> GetWorkflowsAsync();
        Task<Workflow> GetWorkflowByNameAsync(string workflowName); 
    }
}