using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Lab08
{
    class Program
    {
        public IService serviceA;
        public IService serviceB;
        private static IHost host;
        private readonly ILogger<Program> logger;
        static void Main(string[] args)
        {
            host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
            Console.ReadLine();
        }
        public Program(ILogger<Program> logger)
        {
            this.logger = logger;
        }
        public void Run()
        {
            serviceA = host.Services.GetServices<IService>().FirstOrDefault(x => x.GetType().Equals(typeof(ServiceA))); ;
            serviceB = host.Services.GetServices<IService>().FirstOrDefault(x => x.GetType().Equals(typeof(ServiceB))); ;

            logger.LogInformation("Program is running.");
            serviceA.DoSomething();
            serviceB.DoSomething();
            logger.LogInformation("Program is completed.");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging(config => config.AddConsole());
                    services.AddTransient<Program>();
                    services.AddTransient<IService, ServiceA>();
                    services.AddTransient<IService, ServiceB>();
                });
        }
    }

    public interface IService
    {
        void DoSomething();
    }

    public class ServiceA : IService
    {
        private readonly ILogger<ServiceA> logger;
        public ServiceA(ILogger<ServiceA> logger)
        {
            this.logger = logger;
        }
        public void DoSomething()
            => logger.LogInformation("ServiceA is doing something.");
    }

    public class ServiceB : IService
    {
        private readonly ILogger<ServiceB> logger;
        public ServiceB(ILogger<ServiceB> logger)
        {
            this.logger = logger;
        }
        public void DoSomething()
            => logger.LogInformation("ServiceB is doing something.");
    }

}
