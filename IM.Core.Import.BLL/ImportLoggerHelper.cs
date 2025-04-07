using Microsoft.Extensions.Logging;


namespace IM.Core.Import.BLL
{
    public static class ImportLoggerHelper
    {
        public static void LogError(this ILogger logger, string message, string param, Exception exception)
        {
            logger.LogInformation($"ERR {message} {param}");
            logger.LogError($"{message} {param}", exception);
        }
    }
}
