using Newtonsoft.Json;
using RulesEngine.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace PTC.RulesEngine.Core.Repository
{
    public class RulesRepository : IRulesRepository
    {
        private readonly string _rulesFilePath;
        private List<Workflow> _cachedWorkflows = null;
        private DateTime _lastFileWriteTime = DateTime.MinValue;

        public RulesRepository(string rulesFilePath)
        {
            _rulesFilePath = rulesFilePath;
        }

        public async Task<List<Rule>> GetRulesAsync(string workflowName)
        {
            var workflows = await GetWorkflowsAsync();
            var workflow = workflows.FirstOrDefault(w => w.WorkflowName == workflowName);
            return workflow?.Rules != null ? workflow.Rules.ToList() : new List<Rule>();
        }

        public async Task<List<Workflow>> GetWorkflowsAsync()
        {
            if (!File.Exists(_rulesFilePath))
            {
                throw new FileNotFoundException($"Rules file not found: {_rulesFilePath}");
            }

            var fileWriteTime = File.GetLastWriteTimeUtc(_rulesFilePath);
            if (_cachedWorkflows == null || fileWriteTime != _lastFileWriteTime)
            {
                var jsonContent = File.ReadAllText(_rulesFilePath);
                _cachedWorkflows = JsonConvert.DeserializeObject<List<Workflow>>(jsonContent) ?? new List<Workflow>();
                _lastFileWriteTime = fileWriteTime;
            }
            foreach (var workflow in _cachedWorkflows)
            {
                if (workflow.Rules != null && workflow.Rules.Count() > 0)
                {
                    workflow.Rules = workflow.Rules
                        .OrderBy(r => r.Properties != null && r.Properties.ContainsKey("Priority") ? Convert.ToInt32(r.Properties["Priority"]) : int.MaxValue)
                        .ToList();
                }
            }
            return _cachedWorkflows;
        }

        public async Task<Workflow> GetWorkflowByNameAsync(string workflowName)
        {
            var workflows = await GetWorkflowsAsync();
            return workflows.FirstOrDefault(w => w.WorkflowName == workflowName);
        }

        // Method to clear cache (call this when file watcher triggers)
        public void ClearCache()
        {
            _cachedWorkflows = null;
            _lastFileWriteTime = DateTime.MinValue;
        }
    }
}