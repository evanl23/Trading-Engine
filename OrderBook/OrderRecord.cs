namespace TradingEngineServer.OrderBook;

public record OrderRecord(long OrderId, uint Quantity, long price, bool IsBuySide, 
                        string Username, int SecurityId, uint TheoreticalQeuePosition)
{

}
