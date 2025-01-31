﻿using TradingServerEngine.Logging;

namespace TradingEngineServer.Logging
{
    public record LogInformation(LogLevel LogLevel, string Module, string Message, DateTime Now, int ThreadId, string ThreadName);
}

/***
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { };
}
***/