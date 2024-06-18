using OrderBook;

namespace TradingEngineServer.Rejects;

public sealed class RejectCreator
{
    public static Reject GenerateReject(IOrderCore order, RejectReason reason)
    {
        return new Reject(order, reason);
    }
}
