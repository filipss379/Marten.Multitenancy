namespace Marten.Multitenancy.Domain.Cart;

public static class Events
{
    public static class V1
    {
        public class CartInitialized
        {
            public CartInitialized(string cartId)
            {
                CartId = cartId;
            }

            public string CartId { get; }                
        }
        
        public class ItemAdded
        {
            public ItemAdded(string cartId, string itemId)
            {
                CartId = cartId;
                ItemId = itemId;
            }

            public string CartId { get; }
            public string ItemId { get; }
        }
        
        public class ItemRemoved
        {
            public ItemRemoved(string cartId, string itemId)
            {
                CartId = cartId;
                ItemId = itemId;
            }
            public string CartId { get; }
            public string ItemId { get; }
        }
    }
}