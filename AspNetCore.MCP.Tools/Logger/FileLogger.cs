namespace AspNetCore.MCP.API.Logger;

public sealed class FileLogger : IFileLogger, IDisposable
{
    #region Fields
    private readonly StreamWriter _writer;
    private readonly Lock _lock = new();
    private readonly string _logPath;
    #endregion

    #region Methods
    public FileLogger(string logDirectory = "logs")
    {
        Directory.CreateDirectory(logDirectory);
        _logPath = Path.Combine(logDirectory, $"app-{DateTime.Now:yyyy-MM-dd}.log");
        _writer = new StreamWriter(_logPath, append: true) { AutoFlush = true };
        Log("Logger initialized. Application starting.");
    }

    public void Log(string message)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        lock (_lock)
        {
            _writer.WriteLine(line);
            Console.WriteLine(line);
        }
    }

    public void LogError(string message, Exception? ex = null)
    {
        var fullMsg = ex is null ? message : $"{message} | Exception: {ex.Message}\n{ex.StackTrace}";
        Log(fullMsg);
    }

    public void Dispose()
    {
        _writer?.Flush();
        _writer?.Dispose();
    }

    #endregion
}

