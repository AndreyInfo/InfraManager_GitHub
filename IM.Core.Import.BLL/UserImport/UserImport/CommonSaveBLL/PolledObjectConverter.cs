using System.Reflection;
using IM.Core.Import.BLL.Import.Helpers;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using InfraManager.ServiceBase.ImportService;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.Import.Enum;

public class PolledObjectConverter : IPolledObjectConverter,ISelfRegisteredService<IPolledObjectConverter>
{
    private readonly ILocalLogger<PolledObjectConverter> _logger;
    
    public PolledObjectConverter(ILocalLogger<PolledObjectConverter> logger)
    {
        _logger = logger;
    }

    public long GetNumberFromPolledString(PolledObjectDetails polledObjects)
    {
        long polledObjectNumber = 0;
        foreach (var item in polledObjects.GetType().GetProperties())
        {
            if ((bool)item.GetValue(polledObjects))
            {
                var number = (long)System.Enum.Parse(typeof(ObjectType), item.Name);
                polledObjectNumber |= number;
            }
        }
        return polledObjectNumber;
    }
    public PolledObjectDetails GetPolledObjects(
        long bitMask
    )
    {
        _logger.Verbose($"Аргумент bitMask {bitMask.ToString("X")}");
        var polledObjectEnum = System.Enum.GetValues(typeof(ObjectType)).Cast<long>();
        _logger.Verbose("Получены Enumы");
        var polledObject = new PolledObjectDetails();

        foreach (var item in polledObjectEnum)
        {
            _logger.Verbose($"Item {item}");
            if ((bitMask & item) > 0)
            {
                var stringProperty = ((ObjectType)item).ToString();
                _logger.Verbose($"Найден флаг {stringProperty}");
                PropertyInfo property = polledObject.GetType().GetProperty(stringProperty);

                if (property != null)
                {
                    _logger.Verbose("property != null");
                    property.SetValue(polledObject, true, null);
                    _logger.Verbose("property установлен");
                }
            }
        }
        _logger.Verbose("Возврат объекта");
        return polledObject;
    }
}