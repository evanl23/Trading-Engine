using OrderBook;

namespace TradingEngineServer.Rejects;

public class Reject : IOrderCore
{
    public Reject(IOrderCore reject, RejectReason reason)
    {
        _orderCore = reject;
        Reason = reason;
    }

    public RejectReason Reason {get; private set;}
    public long OrderID => _orderCore.OrderID;
    public string UserName => _orderCore.UserName;
    public int SecurityID => _orderCore.SecurityID;
    private readonly IOrderCore _orderCore;
}
