namespace Marten.Multitenancy.Domain.Base;

public abstract class AggregateBase
{
    public string Id { get; set; }
    public int Version { get; set; }
    private readonly List<object> _uncommittedEvents = new();
    
    public IEnumerable<object> GetUncommittedEvents()
    {
        return _uncommittedEvents;
    }

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    protected void AddUncommittedEvent(object @event)
    {
        _uncommittedEvents.Add(@event);
    }
}