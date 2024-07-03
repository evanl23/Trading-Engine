namespace TradingEngineServer.OrderBookCS;

public interface IMatchingOrderBook : IRetrievalOrderBook
{
    MatchResult Matching();
}
