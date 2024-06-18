using OrderBook;

namespace TradingEngineServer.OrderBook;

public class OrderCore : IOrderCore
{
    public OrderCore(long orderID, string userName, int securityID)
    {
        OrderID = orderID;
        UserName = userName;
        SecurityID = securityID;
    }

    public long OrderID {get; private set;}
    public string UserName {get; private set;}
    public int SecurityID {get; private set;}
}
