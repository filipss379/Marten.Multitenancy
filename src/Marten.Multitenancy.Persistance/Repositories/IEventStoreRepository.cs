using Marten.Multitenancy.Domain.Base;

namespace Marten.Multitenancy.Persistance.Repositories;

public interface IEventStoreRepository : IDisposable
{
    Task Init(AggregateBase aggregate);

    Task Append(AggregateBase aggregate);

    Task<T> Load<T>(string streamId, int? version = null) where T : AggregateBase;

    Task<IReadOnlyList<T>> GetEventsOfTheGivenType<T>() where T : class;
}