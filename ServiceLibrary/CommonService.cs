using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServiceLibrary
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddServiceAB(this IServiceCollection services)
        {
            services.AddLogging(config => config.AddConsole());
            services.AddTransient<IService, ServiceA>();
            services.AddTransient<IService, ServiceB>();
            return services;
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
