namespace TradingEngineServer.OrderBookCS;
using TradingEngineServer.OrderBook;

public interface IOrderEntry : IReadOnlyOrderBook
{
    void AddOrder(Order order);
    void ChangeOrder(ModifyOrder modifyOrder);
    void RemoveOrder(CancelOrder cancelOrder); 
}
