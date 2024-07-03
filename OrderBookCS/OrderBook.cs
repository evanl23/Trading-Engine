namespace TradingEngineServer.OrderBookCS;
using TradingEngineServer.Instrument;
using TradingEngineServer.OrderBook;

public class OrderBook : IRetrievalOrderBook
{
    private readonly Security _instrument;
    private readonly SortedSet<Limit> _askLimits = new SortedSet<Limit>(AskLimitSort.Comparar);
    private readonly SortedSet<Limit> _bidLimits = new SortedSet<Limit>(BidLimitSort.Comparer);
    private readonly Dictionary<long, OrderBookEntry> _orders = new Dictionary<long, OrderBookEntry>(); 

    public OrderBook(Security instrument)
    {
        _instrument = instrument;
    }

    public int OrderCount => _orders.Count;
    public bool ContainsOrder(long orderID)
    {
        return _orders.ContainsKey(orderID);
    }
    public void AddOrder(Order order){
        var BaseLimit = new Limit(order.Price);
        addOrder(order, BaseLimit, order.IsBuySide ? _bidLimits : _askLimits, _orders);
    }

    private static void addOrder(Order order, Limit baselimit, SortedSet<Limit> limitLevels, Dictionary<long, OrderBookEntry> orders)
    {
        if (limitLevels.TryGetValue(baselimit, out Limit limit))
        {
            OrderBookEntry orderBookEntry = new OrderBookEntry(order, baselimit);
            if (limit.Head == null)
            {
                limit.Head = orderBookEntry;
                limit.Tail = orderBookEntry;
            } else 
            {
                OrderBookEntry tail = limit.Tail;
                tail.Next = orderBookEntry;
                orderBookEntry.Prev = tail;
                limit.Tail = orderBookEntry;
            }
            orders.Add(order.OrderID, orderBookEntry);
        } else
        {
            limitLevels.Add(baselimit);
            OrderBookEntry orderBookEntry = new OrderBookEntry(order, baselimit);
            baselimit.Head = orderBookEntry;
            baselimit.Tail = orderBookEntry;
            orders.Add(order.OrderID, orderBookEntry);
        }
    }

    public void ChangeOrder(ModifyOrder modifyOrder)
    {
        if (_orders.TryGetValue(modifyOrder.OrderID, out OrderBookEntry obe))
        {
            RemoveOrder(modifyOrder.ToCancelOrder());
            addOrder(modifyOrder.toNewOrder(), obe.ParentLimit, modifyOrder.IsBuySide ? _bidLimits : _askLimits, _orders);
        }
    }

    public void RemoveOrder(CancelOrder cancelOrder)
    {
        if (_orders.TryGetValue(cancelOrder.OrderID, out var obe))
        {
            removeOrder(cancelOrder, obe, _orders);
        }
    }

    private static void removeOrder(CancelOrder cancelOrder, OrderBookEntry orderBookEntry, Dictionary<long, OrderBookEntry> orders)
    {
        if (orderBookEntry.Prev != null && orderBookEntry.Next != null)
        {
            orderBookEntry.Next.Prev = orderBookEntry.Prev;
            orderBookEntry.Prev.Next = orderBookEntry.Next;
        } else if (orderBookEntry.Prev != null)
        {
            orderBookEntry.Prev.Next = null;
        } else if (orderBookEntry.Next != null)
        {
            orderBookEntry.Next.Prev = null; 
        }

        if (orderBookEntry.ParentLimit.Head == orderBookEntry && orderBookEntry.ParentLimit.Tail == orderBookEntry)
        {
            orderBookEntry.ParentLimit.Head = null;
            orderBookEntry.ParentLimit.Tail = null;
        } else if (orderBookEntry.ParentLimit.Head == orderBookEntry && orderBookEntry.ParentLimit.Tail != orderBookEntry)
        {
            orderBookEntry.ParentLimit.Head = orderBookEntry.Next; 
        } else if (orderBookEntry.ParentLimit.Head != orderBookEntry && orderBookEntry.ParentLimit.Tail == orderBookEntry)
        {
            orderBookEntry.ParentLimit.Tail = orderBookEntry.Prev;
        } 

        orders.Remove(cancelOrder.OrderID);
    }

    public List<OrderBookEntry> GetAskOrders()
    {
        List<OrderBookEntry> orders = new List<OrderBookEntry>();
        foreach (var bid in _bidLimits)
        {
            if (bid.IsEmpty)
            {
                continue;
            } else {
                OrderBookEntry headPointer = bid.Head;
                while (headPointer != null) {
                    orders.Add(headPointer);
                    headPointer = headPointer.Next;
                }
            }
        }
        return orders;    
    }

    public List<OrderBookEntry> GetBidOrders()
    {
        List<OrderBookEntry> orders = new List<OrderBookEntry>();
        foreach (var ask in _askLimits)
        {
            if (ask.IsEmpty)
            {
                continue;
            } else {
                OrderBookEntry headPointer = ask.Head;
                while (headPointer != null) {
                    orders.Add(headPointer);
                    headPointer = headPointer.Next;
                }
            }
        }
        return orders;
    }

    public OrderBookSpread GetSpread()
    {
        long? bestAsk = null, bestBid = null;
        if (_askLimits.Any() && !_askLimits.Min.IsEmpty)
        {
            bestAsk = _askLimits.Min.Price;
        } 

        if (_bidLimits.Any() && !_bidLimits.Max.IsEmpty)
        {
            bestBid = _bidLimits.Max.Price;
        }
        return new OrderBookSpread(bestBid, bestAsk);
    }
}
