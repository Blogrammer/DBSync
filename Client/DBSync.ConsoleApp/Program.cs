﻿using System;
using Dotmim.Sync;
using Dotmim.Sync.Sqlite;
using System.Threading.Tasks;
using Dotmim.Sync.SqlServer;

namespace DBSync.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Be sure the web api has started. Then click enter..");
            Console.ReadLine();
            await SynchronizeAsync();
        }

        private static async Task SynchronizeAsync()
        {
             var clientConnectionString = @"Data Source=C:\DBName.db;Version=3;";
              var serverConnectionString = @"Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
            var serverProvider = new SqlSyncProvider(serverConnectionString);
          
            var clientProvider = new SqliteSyncProvider(clientConnectionString);

            var agent = new SyncAgent(clientProvider, serverProvider, new string[] { "People" });

            do
            {
                try
                {
                    var progress = new SynchronousProgress<ProgressArgs>(args => Console.WriteLine($"{args.PogressPercentageString}:\t{args.Message}"));

                    var result = await agent.SynchronizeAsync(progress);

                    Console.WriteLine(result);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (Console.ReadKey().Key != ConsoleKey.Escape);

            Console.WriteLine("End");
        }
    }
}
