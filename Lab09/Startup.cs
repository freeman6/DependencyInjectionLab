using Lab09.Interface;
using Lab09.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab09
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IShoppingCartRepository, ShoppingCartResolver>();
            services.AddSingleton<ShoppingCartCache>();
            services.AddSingleton<ShoppingCartDB>();
            services.AddSingleton<ShoppingCartAPI>();

            services.AddTransient<Func<string, IShoppingCart>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "API":
                        return serviceProvider.GetService<ShoppingCartAPI>();
                    case "DB":
                        return serviceProvider.GetService<ShoppingCartDB>();
                    default:
                        return serviceProvider.GetService<ShoppingCartCache>();
                }
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
