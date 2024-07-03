namespace TradingEngineServer.OrderBookCS;

public interface IReadOnlyOrderBook
{
    bool ContainsOrder(long orderID);
    OrderBookSpread GetSpread();
    int OrderCount {get;}
}
