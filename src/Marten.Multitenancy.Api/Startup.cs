using LamarCodeGeneration;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Multitenancy.Persistance.Repositories;
using Marten.Multitenancy.Projections.Cart;

namespace Marten.Multitenancy.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMarten(opts =>
        {
            var tenants = _configuration.GetSection("Tenants").GetChildren();
            opts.MultiTenantedDatabases(x =>
            {
                foreach (var tenant in tenants)
                {
                    x.AddSingleTenantDatabase(tenant.Value, tenant.Key);
                }
            });
            //opts.Connection("");
            opts.Events.StreamIdentity = StreamIdentity.AsString;

            opts.Events.AddEventType(typeof(Domain.Cart.Events.V1.CartInitialized));
            opts.Events.AddEventType(typeof(Domain.Cart.Events.V1.ItemAdded));
            opts.Events.AddEventType(typeof(Domain.Cart.Events.V1.ItemRemoved));
    
            opts.Projections.Add(new CartViewProjection(), ProjectionLifecycle.Inline);

            opts.GeneratedCodeMode = TypeLoadMode.Auto;
        });
        services.AddScoped<IEventStoreRepository, EventStoreRepository>(provider =>
        {
            return new EventStoreRepository(provider.GetService<IDocumentStore>(), provider.GetService<ITenantResolver>().TenantId);
        });

        services.AddControllers();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}