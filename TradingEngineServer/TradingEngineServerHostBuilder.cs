﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Core
{
    public sealed class TradingEngineServerHostBuilder
    {
        public static IHost BuildTradingEngineServer() 
            => Host.CreateDefaultBuilder().ConfigureServices((context, services) => 
            {
                // Start with configuration
                services.AddOptions();
                services.Configure<TradingEngineServerConfiguration>(context.Configuration.GetSection(nameof(TradingEngineServerConfiguration)));
                services.Configure<LoggerConfiguration>(context.Configuration.GetSection(nameof(LoggerConfiguration)));

                // Add singleton objects
                services.AddSingleton<ITradingEngineServer, TradingEngineServer>();
                services.AddSingleton<ITextLogger, TextLogger>();

                // Add hosted service 
                services.AddHostedService<TradingEngineServer>();

            }).Build();
    }
}