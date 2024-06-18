using OrderBook;

namespace TradingEngineServer.OrderBook;

public class Order : IOrderCore
{
    public Order(IOrderCore orderCore, bool isBuySide, uint volume, long price)
    {
        IsBuySide = isBuySide;
        InitialVolume = volume;
        CurrentVolume = volume;
        Price = price;

        _orderCore = orderCore;
    }

    public Order(ModifyOrder modifyOrder) : this(modifyOrder, modifyOrder.IsBuySide, modifyOrder.ModifyVolume, modifyOrder.ModifyPrice)
    {
        
    }

    public bool IsBuySide {get; private set;}
    public uint InitialVolume {get; private set;}
    public uint CurrentVolume {get; private set;}
    public long Price {get; private set;}

    public long OrderID => _orderCore.OrderID;
    public string UserName => _orderCore.UserName;
    public int SecurityID => _orderCore.SecurityID;
    
    public void increaseVolume(uint volume)
    {
        CurrentVolume += volume;
    }

    public void decreaseVolume(uint volume){
        if (volume > CurrentVolume){
            throw new InvalidOperationException($"Volume change > curent volume for OrderID = {OrderID}");
        }
        CurrentVolume -= volume;
    }

    private readonly IOrderCore _orderCore;

}
