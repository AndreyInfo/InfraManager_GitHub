using IM.Core.Import.BLL.Interface.Import.Log;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.UserImport.Log;

public class ImLocalLoggerAdapter<T> : ILocalLogger<T> where T: class
{
    private readonly ILogger<T> _classLogger;
    private readonly IImportContext _importContext;
    public ImLocalLoggerAdapter(ILogger<T> classLogger,IImportContext importContextLogger)
    {
        _classLogger = classLogger;
        _importContext = importContextLogger;
    }

    public void Information(string message)
    {
        _classLogger.LogInformation(message);
        _importContext.AdditionalContextLogger?.Information(message);
    }

    public void Error(Exception e, string data)
    {
        _classLogger.LogError(e,data);
        _importContext.AdditionalContextLogger?.Error(e,data);
    }

    public void Error(string message, string data, Exception exception)
    {
        _classLogger.LogError(message,data,exception);
        _importContext.AdditionalContextLogger?.Error(message, data, exception);
    }

    public void Verbose(string message)
    {
        _classLogger.LogTrace(message);
        _importContext.AdditionalContextLogger?.Verbose(message);
    }
}