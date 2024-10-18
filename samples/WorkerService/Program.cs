using Milkshake.Configuration;
using WorkerService1;

namespace WorkerService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddSingleton<MyInstance>();

        builder.Configuration.AddJsonFile("appsettings.json");
        builder.Services.AddMilkshake(builder.Configuration);

        var host = builder.Build();
        host.Run();
    }
}