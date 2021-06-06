using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sentinel.Core;

namespace Sentinel.CLI
{
    public class HangfireServer : IModule
    {
        public void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.RegisterSentinelCore()
                        .AddHangfireServer(options =>
                        {
                            options.WorkerCount = 0;
                        });

                    services.AddHostedService<TestHostedService>();
                }).Build();
            Console.WriteLine("Queueing job");
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello World"));
        }

        private class TestHostedService : BackgroundService
        {
            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                await Task.Run(() =>
                {
                    Console.ReadLine();
                    Console.WriteLine("Queueing job");
                    BackgroundJob.Enqueue(() => Console.WriteLine("Hello World"));
                });

            }
        }
    }
}