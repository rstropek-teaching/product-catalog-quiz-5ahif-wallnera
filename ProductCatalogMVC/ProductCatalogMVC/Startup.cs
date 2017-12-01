using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogMVC.Services;

namespace ProductCatalogMVC
{
    public class Startup
    {
        // Database: https://www-productbase-ef71.restdb.io 
        // Tutorial from https://restdb.io/docs/quick-start
        // Further help https://restdb.io/docs/rest-api-code-examples#restdb

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProductRepository>(new ProductRepository());
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseMvc(routes => routes.MapRoute(
                name: "default",
                template: "{controller=Product}/{action=Index}/{id?}"));
        }
    }
}
