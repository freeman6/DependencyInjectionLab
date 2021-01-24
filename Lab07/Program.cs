using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab03
{

    class Program
    {
        private readonly IService service;
        private readonly ILogger<Program> logger;
        private readonly IConfiguration config;

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
            
            Console.ReadLine();
        }
        public Program(ILogger<Program> logger, ServiceResolver ServiceResolver)
        {
            this.logger = logger;
            this.service = ServiceResolver.GetService();
        }
        public void Run()
        {
            logger.LogInformation("Program is running.");
            service.DoSomething();
            logger.LogInformation("Program is completed.");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging(config => config.AddConsole());
                    services.AddTransient<Program>();
                    services.AddTransient<ServiceResolver>();
                    services.AddTransient<ServiceA>();
                    services.AddTransient<ServiceB>();
                    services.AddTransient<Func<string, IService>>(provider => key =>
                    {
                        switch (key)
                        {
                            case "A":
                                return provider.GetService<ServiceA>();
                            case "B":
                                return provider.GetService<ServiceB>();
                            default:
                                return provider.GetService<ServiceA>();
                        }
                    });
                })
                .ConfigureAppConfiguration(config =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("Appsettings.json", true, true);
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

    public class ServiceResolver
    {
        private readonly Func<string, IService> service;
        private readonly IConfiguration config;
        public ServiceResolver(Func<string, IService> service, IConfiguration config)
        {
            this.service = service;
            this.config = config;
        }

        public IService GetService() => service(config["ServiceType"]);
    }
}
