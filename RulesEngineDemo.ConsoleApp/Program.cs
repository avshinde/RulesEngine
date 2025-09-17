using PTC.RulesEngine.Core.Models;
using PTC.RulesEngine.Core.Repository;
using PTC.RulesEngine.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PTC.RulesEngine.ConsoleApp
{
    class Program
    {
        static FileSystemWatcher _watcher;
        static bool _rulesChanged = false;
        static RulesRepository _rulesRepository;

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Rules Engine Demo ===\n");

            var rulesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rules", "sample-rules.json");
            _rulesRepository = new RulesRepository(rulesFilePath);
            var rulesEngineService = new RulesEngineService(_rulesRepository);
            var train = new Train { KeyTrain = "Y", Type = "Freight", Symbol = "ALTALT" };
            var restriction = new Restriction { ReasonCode = "DR", Speed = 29, Track = "Main1", Subdivision = "Afton", Type = "A" };

            // Setup file watcher
            SetupFileWatcher(rulesFilePath);
            int i = 1;
            while (true)
            {
                if (_rulesChanged)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Rules file changed. Clearing cache... engine will load latest rules...");
                    Console.ForegroundColor = ConsoleColor.White;
                    _rulesRepository.ClearCache(); // Clear cache so new rules are loaded
                    _rulesChanged = false;
                }
                Console.WriteLine($"======== Rules service called count({i++}) =======\n\n");
                await ExecuteRulesAsync(rulesEngineService, train, restriction);

                // add thread sleep for 15 seconds
                await Task.Delay(15000);
                i++;
            }
        }

        static void SetupFileWatcher(string rulesFilePath)
        {
            var dir = Path.GetDirectoryName(rulesFilePath);
            var file = Path.GetFileName(rulesFilePath);
            _watcher = new FileSystemWatcher(dir, file)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
            };
            _watcher.Changed += (s, e) => { _rulesChanged = true; };
            _watcher.Created += (s, e) => { _rulesChanged = true; };
            _watcher.Renamed += (s, e) => { _rulesChanged = true; };
            _watcher.EnableRaisingEvents = true;
            
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