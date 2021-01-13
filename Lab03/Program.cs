using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Lab03
{
    class Program
    {
        private readonly IService serviceA;
        private readonly ILogger<Program> logger;
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
            Console.ReadLine();
        }
        public Program(ILogger<Program> logger, IService serviceA)
        {
            this.logger = logger;
            this.serviceA = serviceA;
        }
        public void Run()
        {
            logger.LogCritical("Program is running.");
            serviceA.DoSomething();
            logger.LogCritical("Program is completed.");
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
        private readonly ServiceB serviceB;
        public ServiceA(ServiceB serviceB)
        {
            this.serviceB = serviceB;
        }

        public void DoSomething()
            => serviceB.DoSomething();
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
