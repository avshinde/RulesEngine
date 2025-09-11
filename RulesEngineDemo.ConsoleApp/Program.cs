using PTC.RulesEngine.Core.Models;
using PTC.RulesEngine.Core.Repository;
using PTC.RulesEngine.Core.Services;
using RulesEngineDemo.ConsoleApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PTC.RulesEngine.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Rules Engine Demo ===\n");

            try
            {
                var rulesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rules", "sample-rules.json");
                var rulesRepository = new RulesRepository(rulesFilePath);
                var rulesEngineService = new RulesEngineService(rulesRepository);
                var train = new Train { KeyTrain = "Y", Type = "Freight", Symbol = "ALTALT" };
                var restriction = new Restriction { ReasonCode = "DR", Speed = 29, Track = "Main1", Subdivision = "Afton", Type = "A" };

                await ExecuteRulesAsync(rulesEngineService, train, restriction);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
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