using Newtonsoft.Json;
using RulesEngine.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace PTC.RulesEngine.Core.Repository
{
    public class RulesRepository : IRulesRepository
    {
        private readonly string _rulesFilePath;

        public RulesRepository(string rulesFilePath)
        {
            _rulesFilePath = rulesFilePath;
        }

        public async Task<List<Rule>> GetRulesAsync(string workflowName)
        {
            var workflows = await GetWorkflowsAsync();
            var workflow = workflows.FirstOrDefault(w => w.WorkflowName == workflowName);
            return workflow.Rules != null ? workflow.Rules.ToList() : new List<Rule>();
        }

        public async Task<List<Workflow>> GetWorkflowsAsync()
        {
            if (!File.Exists(_rulesFilePath))
            {
                throw new FileNotFoundException($"Rules file not found: {_rulesFilePath}");
            }

            var jsonContent = File.ReadAllText(_rulesFilePath);
            var workflows = JsonConvert.DeserializeObject<List<Workflow>>(jsonContent);
            return workflows ?? new List<Workflow>();
        }

        public async Task<Workflow> GetWorkflowByNameAsync(string workflowName)
        {
            var workflows = await GetWorkflowsAsync();
            return workflows.FirstOrDefault(w => w.WorkflowName == workflowName);
        }
    }
}