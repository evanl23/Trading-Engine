namespace TradingEngineServer.Rejects;

public enum RejectReason
{
    unknown,
    OrderNotFound,
    InstrumentNotFound,
    AttemptingToModifyWrongSide,
    
}
