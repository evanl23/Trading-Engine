namespace TradingEngineServer.OrderBook;


public class OrderBookEntry
{
    public OrderBookEntry (Order currentOrder, Limit parentLimit)
    {
        CurrentOrder = currentOrder;
        ParentLimit = parentLimit;
        CreationTime = DateTime.UtcNow;
    }

    public DateTime CreationTime {get; private set;}
    public Order CurrentOrder {get;private set;}
    public Limit ParentLimit {get; private set;}
    public OrderBookEntry Next {get; set;}
    public OrderBookEntry Prev {get; set;}
}
