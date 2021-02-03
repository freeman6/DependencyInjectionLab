using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Lab10
{
    class Program
    {
        private readonly ILogger<Program> logger;
        private readonly IService serviceA;

        public Program(ILogger<Program> logger, IService serviceA)
        {
            this.logger = logger;
            this.serviceA = serviceA;
        }

        static void Main(string[] args)
        {
            // Step 01 建立 Service Collection
            var services = new ServiceCollection()
                           .AddLogging(config => config.AddConsole());

            // Step 02 加入服務
            services.AddTransient<Program>();
            services.AddTransient<IService>(x => new ServiceA("RECO"));
            services.AddTransient<IService, ServiceB>();
            services.AddTransient<IService>(x => new ServiceB(services.BuildServiceProvider().GetService<ILogger<ServiceB>>(), "RECO....."));

            // Step 03 建立 ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Step 04 取得相關服務
            serviceProvider.GetRequiredService<Program>().Run();
        }

        public void Run()
        {
            //logger.LogCritical("Running in Before");
            serviceA.DoSomething();
            //logger.LogCritical("Running in After");
        }
    }

    public interface IService
    {
        void DoSomething();
    }

    public class ServiceA : IService
    {
        private string name;
        public ServiceA(string name)
        {
            this.name = name;
        }

        public void DoSomething()
            => Console.WriteLine($"Hi, {name}, ServiceA is doing something.");
    }

    public class ServiceB : IService
    {
        private readonly ILogger<ServiceB> logger;
        private string name;
        public ServiceB(ILogger<ServiceB> logger, string name)
        {
            this.logger = logger;
            this.name = name;
        }
        public void DoSomething()
            => logger.LogInformation($"Hi, {name}, ServiceB is doing something.");
    }
}
