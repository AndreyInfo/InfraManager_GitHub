using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Log;

namespace IM.Core.Import.BLL.UserImport.Log;

public class ProtocolLogAdapter:ILogAdapter
{
    private readonly IProtocolLogger _logger;

    public ProtocolLogAdapter(IProtocolLogger logger)
    {
        _logger = logger;
    }

    public void Information(string message)
    {
        _logger?.Information(message);
    }

    public void Error(Exception e, string data)
    {
        _logger.Error(e,data);
    }

    public void Error(string message, string data, Exception exception)
    {
        _logger.Error(exception,$"{message}, {data}");
    }

    public void Verbose(string message)
    {
        _logger?.Verbose(message);
    }
}