using RulesEngines= RulesEngine;
using RulesEngine.Models;
using PTC.RulesEngine.Core.Models;
using PTC.RulesEngine.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTC.RulesEngine.Core.Services
{
    public class RulesEngineService : IRulesEngineService
    {
        private readonly IRulesRepository _rulesRepository;

        public RulesEngineService(IRulesRepository rulesRepository)
        {
            _rulesRepository = rulesRepository;
        }

        public async Task<List<RuleResult>> ExecuteRulesAsync<T>(string workflowName, T inputData,string inputName)
        {
            var engine = await GetOrCreateEngineAsync(workflowName);
            var ruleParams = new RuleParameter(inputName, inputData);

            var results = await engine.ExecuteAllRulesAsync(workflowName, ruleParams);

            return results.Select(r => new RuleResult
            {
                RuleName = r.Rule.RuleName,
                IsSuccess = r.IsSuccess,
                Message = r.IsSuccess ? r.Rule.SuccessEvent : r.Rule.ErrorMessage ?? string.Empty,
                InputData = inputData,
                OutputData = r.IsSuccess ? r.Rule.Properties ?? new Dictionary<string, object>(): new Dictionary<string, object>()
            }).ToList();
        }

       
        private async Task<RulesEngines.RulesEngine> GetOrCreateEngineAsync(string workflowName)
        {
            var workflow = await _rulesRepository.GetWorkflowByNameAsync(workflowName);
            if (workflow == null)
            {
                throw new ArgumentException($"Workflow '{workflowName}' not found");
            }

            var engine = new RulesEngines.RulesEngine(new[] { workflow });
            return engine;
        }
    }
}
