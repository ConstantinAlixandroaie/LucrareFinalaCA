using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LucrareFinalaCA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            var host = CreateWebHostBuilder(args).Build();
            //The code below is used to seed the database and add the administrator role to one account. 
            //it is not necesarry after first run of the application on one particular server
            //a separate page to assign roles should be created.
            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    //var context = services.GetRequiredService<ApplicationDbContext>();
            //    //context.Database.Migrate();

            //    //requires using Microsoft.Extensions.Configuration;
            //    var config = host.Services.GetRequiredService<IConfiguration>();
            //    //Set password with the Secret Manager tool.
            //    //dotnet user-secrets set SeedUserPW<pw>

            //    var testUserPw = config["SeedUserPW"];
            //    try
            //    {
            //        SeedData.Initialize(services).Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex.Message, "An error occurred seeding the DB.");
            //    }
            //}
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
