namespace TradingEngineServer.OrderBook;

public class Limit
{
    public Limit(long price) 
    {
        Price = price;
    }
    public uint GetLevelOrderCount() // The number of separate orders on that level
    {
        uint orderCount = 0;
        OrderBookEntry headPointer = Head;
        while (headPointer != null)
        {
            if (headPointer.CurrentOrder.CurrentVolume != 0) { orderCount++; }
            headPointer = headPointer.Next;
        }
        return orderCount;
    }

    public uint GetLevelOrderQuantity() // The total quantity from all orders on that level
    {
        uint orderQuantity = 0;
        OrderBookEntry headPointer = Head;
        while (headPointer != null)
        {
            orderQuantity += headPointer.CurrentOrder.CurrentVolume;
            headPointer = headPointer.Next;
        }
        return orderQuantity;
    }

    public List<OrderRecord> GetLevelOrderRecords()
    {
        List<OrderRecord> orderRecords = new List<OrderRecord>();
        OrderBookEntry headPointer = Head;
        uint theoreticalQeuePosition = 0;
        while (headPointer != null) 
        {
            var currentOrder = headPointer.CurrentOrder;
            if (currentOrder.CurrentVolume != 0)
            {
                orderRecords.Add(new OrderRecord(currentOrder.OrderID, currentOrder.CurrentVolume, this.Price, 
                currentOrder.IsBuySide, currentOrder.UserName, currentOrder.SecurityID, theoreticalQeuePosition));
            }
            theoreticalQeuePosition++;
            headPointer = headPointer.Next; 
        }
        
        return orderRecords;
    }

    public long Price {get; private set;}
    public OrderBookEntry Head {get; set;}
    public OrderBookEntry Tail {get; set;}
    public bool IsEmpty 
    {
        get{ return Head==null && Tail==null; }
    }
    public Side Side 
    {
        get{ if(IsEmpty){return Side.Unknown;} else { return Head.CurrentOrder.IsBuySide ? Side.Bid : Side.Ask; }}
    }
}
