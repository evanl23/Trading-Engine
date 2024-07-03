using TradingEngineServer.OrderBook;

namespace TradingEngineServer.OrderBookCS;

public interface IRetrievalOrderBook : IOrderEntry
{
    List<OrderBookEntry> GetAskOrders();
    List<OrderBookEntry> GetBidOrders();
    
}
