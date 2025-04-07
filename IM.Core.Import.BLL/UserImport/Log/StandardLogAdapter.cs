using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Log;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.UserImport.Log;

public class StandardLogAdapter:ILogAdapter
{
    private readonly ILogger _logger;

    public StandardLogAdapter(ILogger logger)
    {
        _logger = logger;
    }

    public void Information(string message)
    {
        _logger.LogInformation(message);
    }

    public void Error(Exception e, string data)
    {
        _logger.LogError(e, data);
    }

    public void Error(string message, string data, Exception exception)
    {
        _logger.LogError(message, data, exception);
    }

    public void Verbose(string message)
    {
        _logger.LogTrace(message);
    }
}