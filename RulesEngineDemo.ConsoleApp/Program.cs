using PTC.RulesEngine.Core.Models;
using PTC.RulesEngine.Core.Repository;
using PTC.RulesEngine.Core.Services;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace PTC.RulesEngine.ConsoleApp
{
    class Program
    {
        static RulesRepository _rulesRepository;

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Rules Engine Demo ===\n");
            Console.WriteLine("Set ReloadRules=true in app.config to reload rules. Set to false to use cache.");

            var rulesFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rules", "sample-rules.json");
            _rulesRepository = new RulesRepository(rulesFilePath);
            var rulesEngineService = new RulesEngineService(_rulesRepository);
            var train = new Train { KeyTrain = "Y", Type = "Freight", Symbol = "ALTALT" };
            var restriction = new Restriction { ReasonCode = "DR", Speed = 29, Track = "Main1", Subdivision = "Afton", Type = "A" };

            int i = 1;
            while (true)
            {
                bool reloadFlag = false;
                try
                {
                    var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var config = ConfigurationManager.OpenExeConfiguration(exePath);
                    var reloadValue = config.AppSettings.Settings["ReloadRules"]?.Value;
                    reloadFlag = bool.TryParse(reloadValue, out bool reload) && reload;
                }
                catch { }
                if (reloadFlag)
                {
                    _rulesRepository.ReloadRules();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Rules cache cleared. Latest rules are loaded for next execution.");
                    Console.ResetColor();
                    // Set flag to false so reload only happens once
                    var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var config = ConfigurationManager.OpenExeConfiguration(exePath);
                    config.AppSettings.Settings["ReloadRules"].Value = "false";
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                Console.WriteLine($"======== Rules service called count({i++}) =======\n");
                await ExecuteRulesAsync(rulesEngineService, train, restriction);
                await Task.Delay(7000); // Execute every 7 seconds
            }
        }

        public static async Task ExecuteRulesAsync(RulesEngineService rulesEngineService, params object[] inputs)
        {
            var resultList = await rulesEngineService.ExecuteRulesAsync("BOSOverrideRestrictionSpeed", inputs);

            foreach (var res in resultList)
            {
                Console.WriteLine($"Rule: {res.RuleName}");
                Console.WriteLine($"Success: {res.IsSuccess}");
                Console.WriteLine($"Message: {res.Message}");

                if (res.OutputData != null && res.OutputData.Count > 0)
                {
                    Console.WriteLine("OutputData:");
                    foreach (var kvp in res.OutputData)
                    {
                        Console.WriteLine($"   {kvp.Key}: {kvp.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("OutputData: None");
                }

                Console.WriteLine(new string('-', 40));
            }
        }
    }
}