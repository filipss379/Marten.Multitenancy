using Marten.Multitenancy.Domain.Base;

namespace Marten.Multitenancy.Persistance.Repositories;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IDocumentStore _store;
    private readonly string _tenantId;

    public EventStoreRepository(IDocumentStore store, string tenantId)
    {
        _store = store;
        _tenantId = tenantId;
    }

    public async Task Init(AggregateBase aggregate)
    {
        await using var session = _store.OpenSession(_tenantId);
        var events = aggregate.GetUncommittedEvents().ToArray();
        session.Events.StartStream(aggregate.Id, events);
        await session.SaveChangesAsync();
        aggregate.ClearUncommittedEvents();
    }

    public async Task Append(AggregateBase aggregate)
    {
        await using var session = _store.OpenSession(_tenantId);
        var events = aggregate.GetUncommittedEvents().ToArray();
        session.Events.Append(aggregate.Id, events);
        await session.SaveChangesAsync();
        aggregate.ClearUncommittedEvents();
    }
        
    public async Task<T> Load<T>(string streamId, int? version = null) where T : AggregateBase
    {
        await using var session = _store.QuerySession(_tenantId);
        var aggregateProjection = await session.Events.AggregateStreamAsync<T>(streamId, version ?? 0);
        return aggregateProjection ?? throw new StreamNotFoundException("Stream with given id does not exist.");
    }

    public async Task<IReadOnlyList<T>> GetEventsOfTheGivenType<T>() where T : class
    {
        await using var session = _store.OpenSession(_tenantId);
        return await session.Events.QueryRawEventDataOnly<T>().ToListAsync();
    }

    public void Dispose()
    {
        _store?.Dispose();
    }
}

public class StreamNotFoundException : Exception
{
    public StreamNotFoundException(string message) : base(message)
    {

    }
}