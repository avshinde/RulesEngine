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
                var Train = new TrainInput
                {
                    Subdivision = "North",
                    Track = "T1",
                    Speed = 55,
                    ReasonCode = "Weather",
                    TypeRestriction = "B",
                    KeyTrain = "N",
                    Route = "ABCDEF",
                    Source = "SRC",
                    Destination = "DST",
                    CarType = "C01",
                    TrainType = "Freight"
                };
                Console.WriteLine($"======================Key Train================================================");
                await RunTrainRulesEvaluation(rulesEngineService, Train);
                Console.WriteLine($"======================================================================");
                Train = new TrainInput
                {
                    Subdivision = "North",
                    Track = "T1",
                    Speed = 55,
                    ReasonCode = "Weather",
                    TypeRestriction = "B",
                    KeyTrain = "Y",
                    Route = "ABCDEF",
                    Source = "SRC",
                    Destination = "DST",
                    CarType = "C01",
                    TrainType = "Freight"
                };
                Console.WriteLine($"=======================NonKey Train===============================================");
                await RunTrainRulesEvaluation(rulesEngineService, Train);
                Console.WriteLine($"======================================================================");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static async Task RunTrainRulesEvaluation(RulesEngineService rulesEngineService, TrainInput Train)
        {
            var resultList = await rulesEngineService.ExecuteRulesAsync<TrainInput>("BOSOverrideRestrictionSpeed", Train, "TrainInput");

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
                    Console.WriteLine("OutputData: None");

                Console.WriteLine(new string('-', 40));
               
            }
        }
    }
}