using TradingEngineServer.Logging.LoggingConfiguration;
using TradingServerEngine.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks.Dataflow;
using System.IO;

namespace TradingEngineServer.Logging;

public class TextLogger : AbstractLogger, ITextLogger
{
    private readonly LoggerConfiguration _loggingConfiguration;
    public TextLogger(IOptions<LoggerConfiguration> loggingConfiguration) : base()
    {
        _loggingConfiguration = loggingConfiguration.Value ?? throw new ArgumentNullException(nameof(loggingConfiguration));
        if (_loggingConfiguration.LoggerType != LoggerType.Text)
        {
            throw new InvalidOperationException($"{nameof(TextLogger)} doesn't match LoggerType of {_loggingConfiguration.LoggerType}");
        }

        var now = DateTime.Now;
        string LogDirectory = Path.Combine(_loggingConfiguration.TextLoggerConfiguration.Directory, $"{now:yyyy-MM-dd}");
        string baseLogName = Path.ChangeExtension($"{_loggingConfiguration.TextLoggerConfiguration.FileName}-{now:HH-mm-ss}", 
                                            _loggingConfiguration.TextLoggerConfiguration.FileExtension);
        string filePath = Path.Combine(LogDirectory, baseLogName);
        
        Directory.CreateDirectory(LogDirectory);

        _ = Task.Run(() => LogAsync(filePath, _logQueue, _tokenSource.Token));
    }

    private static async void LogAsync(string filePath, BufferBlock<LogInformation> logQueue, CancellationToken token)
    {
        using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
        using var sw = new StreamWriter(fs){AutoFlush = true};
        try
        {
            while (true)
            {
                var logItem = await logQueue.ReceiveAsync(token).ConfigureAwait(false);
                string formattedMessage = FormatLogItem(logItem);
                await sw.WriteAsync(formattedMessage).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        { }
    }

    private static string FormatLogItem(LogInformation logItem)
    {
        return $"[{logItem.Now: yyyy-MM-dd HH-mm-ss.fffffff}] [{logItem.ThreadName,-30}:{logItem.ThreadId:000}] "
        + $"[{logItem.LogLevel}] {logItem.Message}";
    }

    protected override void Log(LogLevel logLevel, string module, string message)
    {
        _logQueue.Post(new LogInformation(logLevel, module, message, DateTime.Now,
                        Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
    }

    ~TextLogger()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        lock (_lock){
            if (_dispose)
                return;
            _dispose = true;
        }
        
        if (disposing)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
    }

    private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
    private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
    private readonly object _lock = new object();
    private bool _dispose = false;
}
