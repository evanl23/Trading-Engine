namespace TradingEngineServer.OrderBookCS;

public class OrderBookSpread
{
    public OrderBookSpread(long? bid, long? ask)
    {
        Bid = bid;
        Ask = ask;
    }

    public long? Bid {get; private set;}
    public long? Ask {get; private set;}

    public long? Spread 
    {
        get
        {
            if (Bid.HasValue && Ask.HasValue)
            {
                return Bid.Value-Ask.Value;
            }
            else
            {
                return null;
            }
        }
    }
}
