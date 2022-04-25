using Marten.Multitenancy.Domain.Base;

namespace Marten.Multitenancy.Domain.Cart;

public class CartAggregate : AggregateBase
{
    public List<string> Items { get; set; }  = new();

    public void InitializeCart()
    {
        var id = Guid.NewGuid().ToString();
        var @event = new Events.V1.CartInitialized(id);
        Apply(@event);
        AddUncommittedEvent(@event);
    }

    public void AddItem(string itemId)
    {
        var @event = new Events.V1.ItemAdded(Id, itemId);
        Apply(@event);
        AddUncommittedEvent(@event);
    }

    public void RemoveItem(string itemId)
    {
        var @event = new Events.V1.ItemRemoved(Id, itemId);
        Apply(@event);
        AddUncommittedEvent(@event);
    }

    public void Apply(Events.V1.CartInitialized @event)
    {
        Id = @event.CartId;
    }
    
    public void Apply(Events.V1.ItemAdded @event)
    {
        Items.Add(@event.ItemId);
    }
    
    public void Apply(Events.V1.ItemRemoved @event)
    {
        Items.Remove(@event.ItemId);
    }
}