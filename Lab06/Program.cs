using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Unity;
using Unity.Microsoft.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lab06
{
    class Program
    {
        private static IUnityContainer unityContainer;
        private readonly IService serviceA;
        private readonly ILogger<Program> logger;
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();

            Console.ReadLine();
        }
        public Program(ILogger<Program> logger, ServiceA serviceA)
        {
            this.logger = logger;
            this.serviceA = serviceA;
        }
        public void Run()
        {
            logger.LogInformation("Program is running.");
            serviceA.DoSomething();
            logger.LogInformation("Program is completed.");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            unityContainer = new UnityContainer();
            return Host.CreateDefaultBuilder(args)
                .UseUnityServiceProvider(unityContainer)
                .ConfigureContainer<IUnityContainer>(services =>
                {
                    services.RegisterType<Program>();
                    services.RegisterType<IService, ServiceA>("ServiceA");
                    services.RegisterType<IService, ServiceB>("ServiceB");
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
