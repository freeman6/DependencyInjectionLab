using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lab02
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
            services.AddTransient<IService, ServiceA>();
            services.AddTransient<IService, ServiceB>();

            // Step 03 建立 ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Step 04 取得相關服務
            serviceProvider.GetRequiredService<Program>().Run();
        }

        public void Run()
        {
            logger.LogCritical("Running in Before");
            serviceA.DoSomething();
            logger.LogCritical("Running in After");
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
