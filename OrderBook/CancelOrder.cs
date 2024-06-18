using OrderBook;

namespace TradingEngineServer.OrderBook;

public class CancelOrder : IOrderCore
{
    public CancelOrder(IOrderCore orderCore)
    {
        _orderCore = orderCore;
    }

    public long OrderID => _orderCore.OrderID;
    public string UserName => _orderCore.UserName;
    public int SecurityID => _orderCore.SecurityID;

    private readonly IOrderCore _orderCore;
}
