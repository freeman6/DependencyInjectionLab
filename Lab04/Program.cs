using System;
using Microsoft.Extensions.DependencyInjection;
using ServiceLibrary;

namespace Lab04
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
            services.AddServiceAB();

            // Step 03 建立 ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Step 04 取得相關服務
            serviceProvider.GetRequiredService<Program>().Run();
        }

        public void Run()
        {
            serviceA.DoSomething();
        }

    }

}
