using Marten.Events.Projections;

namespace Marten.Multitenancy.Projections.Cart;

public class CartViewProjection : ViewProjection<CartDocument, string>
{
    public CartViewProjection()
    {
        Identity<Domain.Cart.Events.V1.CartInitialized>(e => e.CartId);
        Identity<Domain.Cart.Events.V1.ItemAdded>(e => e.CartId);
        Identity<Domain.Cart.Events.V1.ItemRemoved>(e => e.CartId);
    }
    
    public void Apply(Domain.Cart.Events.V1.CartInitialized e, CartDocument cart)
    {
        cart.Id = e.CartId;
    }
    
    public void Apply(Domain.Cart.Events.V1.ItemAdded e, CartDocument cart)
    {
        cart.Items.Add(e.ItemId);
    }
    
    public void Apply(Domain.Cart.Events.V1.ItemRemoved e, CartDocument cart)
    {
        cart.Items.Remove(e.ItemId);
    }
}

public class CartDocument
{
    public string Id { get; set; }
    public List<string> Items { get; set; }
}