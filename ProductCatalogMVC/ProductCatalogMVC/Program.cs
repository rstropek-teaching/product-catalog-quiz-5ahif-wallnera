using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ProductCatalogMVC
{
    public class Program
    {
        // Database https://www-productbase-ef71.restdb.io
        // Tutorial from https://restdb.io/docs/quick-start
        // Further help https://restdb.io/docs/rest-api-code-examples#restdb
        // and for Postman https://restdb.io/blog/postman-with-restdb

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
