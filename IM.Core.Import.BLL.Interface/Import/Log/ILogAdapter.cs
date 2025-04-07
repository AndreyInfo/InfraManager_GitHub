namespace IM.Core.Import.BLL.Interface.Import.Log;

public interface ILogAdapter
{
    void Information(string message);
    void Error(Exception e, string data);
    void Error(string message, string data, Exception exception);
    void Verbose(string message);
    
}