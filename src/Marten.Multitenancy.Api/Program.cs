using Oakton;

namespace Marten.Multitenancy.Api;

public class Program
{
    public static Task<int> Main(string[] args)
    {
        return CreateHostBuilder(args).RunOaktonCommands(args);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    private static void ConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder builder)
    {
        builder.AddJsonFile("appsettings.local.json", true);

        var configuration = builder.Build();

    }
}