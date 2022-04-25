namespace Marten.Multitenancy.Api;

public interface ITenantResolver
{
    public string TenantId { get; set; }
}