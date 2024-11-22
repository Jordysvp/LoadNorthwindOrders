using LoadNortwindOrders.Data.Context;
using LoadNortwindOrders.Data.Interfaces;
using LoadNortwindOrders.Data.Service;
using LoadNortwindOrders.WorkerService;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) => {

            services.AddDbContextPool<NorthwindContext>(options =>
                                                      options.UseSqlServer(hostContext.Configuration.GetConnectionString("DbNorthwind")));

            services.AddDbContextPool<NorthwindOrdersContext>(options =>
                                                      options.UseSqlServer(hostContext.Configuration.GetConnectionString("DbNortwindOrders")));


            services.AddScoped<IDataServiceNortwindOrders, DataServiceNortwindOrders>();

            services.AddHostedService<Worker>();
        });
}
