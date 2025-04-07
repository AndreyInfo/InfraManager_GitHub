using IM.Core.Import.BLL.Interface.Import.ITAsset;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.Import.OSU;

public static class ImportHelper
{
    public static ITAssetImportDetails GetModelForITAsset(Dictionary<string, string?> resultInterpreter)
        => GetModel<ITAssetImportDetails,string>(resultInterpreter);

    public static SCImportDetail GetModelForSC(Dictionary<string, string?> resultInterpreter)
        => GetModel<SCImportDetail,string>(resultInterpreter);

    public static ImportModel GetImportModel(Dictionary<string, object?> resultInterpreter) =>
        GetModel<ImportModel, object>(resultInterpreter);
    
    public static TModel GetModel<TModel, TElement>(Dictionary<string, TElement?> resultInterpreter) where TModel:new()
    {
        TModel model = new TModel();
        UpdateModel(resultInterpreter.Select(x=>new KeyValuePair<string, object>(x.Key,x.Value)), model);
        return model;
    }
    

    public static T UpdateModel<T>(IEnumerable<KeyValuePair<string, object>> resultInterpretator, T model)
    {
        var modelType = model.GetType();
        foreach (var field in resultInterpretator)
        {
            var property = modelType.GetProperty(field.Key);
            var type = property.PropertyType;
            var convertingValue = new SingleOrVector(field.Value);
            var converted = Convert.ChangeType(convertingValue, type);
            property.SetValue(model, converted, null);
        }

        return model;
    }
    
    public static void PrintConfiguration(ILookup<string, ScriptDataDetails<ConcordanceObjectType>> scripts, ILogAdapter logger)
    {
        logger.Information("Конфигурация:");
        foreach (var script in scripts)
        {
            logger.Information($" Конфигурация для {script.Key}:");
            PrintConfiguration(script, logger);
        }
    }

    public static void PrintConfiguration(IEnumerable<ScriptDataDetails<ConcordanceObjectType>> script, ILogAdapter logger)
    {
        foreach (var classData in script)
        {
            logger.Information($"  {classData.FieldName}:");

            foreach (var line in classData.Script.Split("\n"))
            {
                logger.Information($"   {line}");
            }
        }
    }
}