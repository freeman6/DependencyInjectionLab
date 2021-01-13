using System;
using Microsoft.Extensions.DependencyInjection;

namespace Lab01
{
    class Program
    {
        private readonly IService serviceA;

        public Program(IService serviceA)
        {
            this.serviceA = serviceA;
        }

        static void Main(string[] args)
        {
            // Step 01 建立 Service Collection
            var services = new ServiceCollection();

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
            Console.WriteLine("Running in Before");
            serviceA.DoSomething();
            Console.WriteLine("Running in After");
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
        public void DoSomething()
            => Console.WriteLine("ServiceB is doing something.");
    }

}
