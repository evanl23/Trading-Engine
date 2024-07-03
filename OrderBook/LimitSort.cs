namespace TradingEngineServer.OrderBook;

public class BidLimitSort : IComparer<Limit>
{
    public static IComparer<Limit> Comparer { get; } = new BidLimitSort();
    public int Compare (Limit x, Limit y)
    {
        if (x.Price == y.Price) {return 0;}
        else if (x.Price > y.Price) {return -1;}
        else {return 1;}
    }
}

public class AskLimitSort : IComparer<Limit>
{
    public static IComparer<Limit> Comparar {get; } = new AskLimitSort();
    public int Compare (Limit x, Limit y)
    {
        if (x.Price == y.Price) {return 0;}
        else if (x.Price > y.Price) {return 1;}
        else {return -1;}
    }
}
