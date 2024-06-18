using OrderBook;

namespace TradingEngineServer.OrderBook;

public class ModifyOrder : IOrderCore
{
    public ModifyOrder(IOrderCore orderCore, long modifyPrice, uint modifyVolume, bool isBuySide)
    {
        _orderCore = orderCore;
        ModifyPrice = modifyPrice;
        ModifyVolume = modifyVolume; 
        IsBuySide = isBuySide;
    }

    public long OrderID => _orderCore.OrderID;
    public string UserName => _orderCore.UserName;
    public int SecurityID => _orderCore.SecurityID;

    public long ModifyPrice {get; private set;}
    public uint ModifyVolume {get; private set;}
    public bool IsBuySide {get; private set;}

    private readonly IOrderCore _orderCore;

    public CancelOrder ToCancelOrder()
    {
        return new CancelOrder(this);
    }

    public Order toNewOrder()
    {
        return new Order(this);
    }
}
