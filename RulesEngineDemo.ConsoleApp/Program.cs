using PTC.RulesEngine.Core.Models;
using PTC.RulesEngine.Core.Repository;
using PTC.RulesEngine.Core.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PTC.RulesEngine.ConsoleApp
{
    class Program
    {
        static RulesRepository _rulesRepository;
        static bool _reloadRequested = false;
        static bool _exitRequested = false;

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Rules Engine Demo ===\n");
            Console.WriteLine("Type 'reload' to reload rules, 'exit' to quit at any time.");

            var rulesFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rules", "sample-rules.json");
            _rulesRepository = new RulesRepository(rulesFilePath);
            var rulesEngineService = new RulesEngineService(_rulesRepository);
            var train = new Train { KeyTrain = "Y", Type = "Freight", Symbol = "ALTALT" };
            var restriction = new Restriction { ReasonCode = "DR", Speed = 29, Track = "Main1", Subdivision = "Afton", Type = "A" };

            // Start input listener in a separate thread
            var inputThread = new Thread(InputListener);
            inputThread.Start();

            int i = 1;
            while (!_exitRequested)
            {
                if (_reloadRequested)
                {
                    _rulesRepository.ReloadRules();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Rules cache cleared. Latest rules are loaded for next execution.");
                    Console.ResetColor();
                    _reloadRequested = false;
                }
                Console.WriteLine($"======== Rules service called count({i++}) =======\n");
                await ExecuteRulesAsync(rulesEngineService, train, restriction);
                await Task.Delay(7000); // Execute every 5 seconds
            }
            Console.WriteLine("Exiting...");
        }

        static void InputListener()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input != null)
                {
                    if (input.Trim().ToLower() == "exit")
                    {
                        _exitRequested = true;
                        break;
                    }
                    if (input.Trim().ToLower() == "reload")
                    {
                        _reloadRequested = true;
                    }
                }
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