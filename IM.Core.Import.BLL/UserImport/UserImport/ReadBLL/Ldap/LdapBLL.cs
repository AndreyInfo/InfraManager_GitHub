using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Import.OSU;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Ldap;
using InfraManager;

namespace IM.Core.Import.BLL.Ldap;

public  class LdapBLL : ILdapBLL, ISelfRegisteredService<ILdapBLL>
{
    private const string ObjectSid = "objectSid";
    private const string DnKeyName = "dn";
    private readonly ILdapRepository _repository;
    private readonly IScriptDataParser<ConcordanceObjectType> _parser;
    private readonly ILocalLogger<LdapBLL> _logger;

    public LdapBLL(ILdapRepository repository,
        IScriptDataParser<ConcordanceObjectType> parser, 
        ILocalLogger<LdapBLL> logger)
    {
        _repository = repository;
        _parser = parser;
        _logger = logger;
    }

    string[] GetParents(string s)
    {
        return s.Split(",").Select(x => x.Split("="))
            .Select(x => new {Key = x[0], Value = x[1]}).Reverse().SkipWhile(x => x.Key.ToLower() == "dc")
            .Select(x => x.Value).ToArray();
    }

    public bool CheckUniqueDN(string userName, string password, string dn, string path)
    {
        var url = GetUrl(dn, path);
        
        var data = _repository.GetAllAttributes(url, userName,password, new ParsersData()).Take(2).ToArray();
    
        return data.Length == 1;
    }

    private static string GetUrl(string dn, string path)
    {
        var pathTrimmed = path.TrimEnd('/');
        var url = $"{pathTrimmed}/{dn}";
        return url;
    }


    public IEnumerable<ImportModel> GetImportModels(string path,
        string accountName,
        string password,
        ILookup<string, ScriptDataDetails<ConcordanceObjectType>> userScriptsByClass,
        Func<ImportModel, string> getManagerIdentificator,
        LdapLoadRequestData classes = null)
    {
        var scripts = LoadImportModelScript(userScriptsByClass);

        var data = _repository.GetAllAttributes(path, accountName, password, scripts, classes).ToList();

        _logger.Verbose($"Загрузка из LDAP:");
        _logger.Information($"Загрузка из LDAP:");
        var managerKeyDictionary = new Dictionary<string, string>();
        foreach (var element in data)
        {
            var userEntryAttributes = element.Attributes;
            
            var parents = element.Parents;
            
            var objectClasses = GetObjectClasses(element);

            if (PrintInformation(element, parents)) 
                continue;
            
            var record = LoadImportModel(scripts, userEntryAttributes, objectClasses, parents);
           
            if (record == null)
            {
                _logger.Information("Пустой результат интерпретации.");
                _logger.Information("Пустой результат интерпретации.");

                continue;
            }
            
            var recordData = record.ToString();
            var pivotLines = recordData.Split("\n").Select(x=>$" {x}");
            var importModel = string.Join("\n",pivotLines);
            var message = $"Интерпретировано как запись:\n{importModel}\n";
            _logger.Information(message);

            var recordUserManager = record.UserManager;
            if (!string.IsNullOrWhiteSpace(recordUserManager))
            {
                if (managerKeyDictionary.ContainsKey(recordUserManager))
                {
                    record.UserManager = managerKeyDictionary[recordUserManager];
                }
                else
                {
                    managerKeyDictionary[recordUserManager] = String.Empty;
                    string? key = recordUserManager;
                    if (data.Any(x => x.Dn == recordUserManager))
                    {
                        var node = data.FirstOrDefault(x => x.Dn == recordUserManager);
                        key = GetKey(getManagerIdentificator, node, scripts);
                    }
                    else if (!string.IsNullOrWhiteSpace(recordUserManager))
                    {

                        var url = new LdapUrlData(element.Address);
                        var currentUrl = url.GetOtherDnUrlData(recordUserManager).Address;
                        var manager = _repository.GetAllAttributes(currentUrl, accountName, password, scripts, classes)
                            .FirstOrDefault();
                        key = GetKey(getManagerIdentificator, manager, scripts);
                    }

                    record.UserManager = key;
                    managerKeyDictionary[recordUserManager] = key;
                }
                _logger.Information($"UserManager присвоено {record.UserManager}");
            }

            yield return record;
        }
    }

    private string? GetKey(Func<ImportModel, string> getManagerIdentificator, ILdapDataEntry? manager, ParsersData scripts)
    {
        if (manager != null)
        {
            var managerAttributes = manager.Attributes;
            var managerObjectClasses = GetObjectClasses(manager);
            var managerParents = manager.Parents;
            var managerRecord = LoadImportModel(scripts, managerAttributes, managerObjectClasses,
                managerParents);
            if (managerRecord != null)
            {
                return getManagerIdentificator(managerRecord);
            }
        }

        return null;
    }

    private bool PrintInformation(ILdapDataEntry element,
        IEnumerable<IReadOnlyDictionary<string, string>> parents)
    {
        _logger.Verbose($" Загружен:{element.Dn}");
        _logger.Information($" Загружен:{element.Dn}");

        _logger.Verbose($"  Атрибуты:");
        _logger.Information($"  Атрибуты:");

        foreach (var attribute in element.Attributes.Where(x=>x.Key!= DnKeyName))
        {
            _logger.Verbose($"   {attribute.Key}={attribute.Value}");
            _logger.Information($"   {attribute.Key}={attribute.Value}");
        }
        
        _logger.Verbose("   Родители:");
        _logger.Information("   Родители:");

        int parentCount = 0;
        foreach (var parent in parents)
        {
            _logger.Information($"    Родитель[{parentCount}]: {parent[DnKeyName]}");
            foreach (var field in parent.Where(x => x.Key != DnKeyName))
            {
                var fieldName = LogHelper.ToOutputFormat(field.Key);
                var fieldValue = LogHelper.ToOutputFormat(field.Value);
                var message = $"     {fieldName} = {fieldValue}";
                _logger.Verbose(message);
                _logger.Information(message);
            }
            parentCount++;
        }

        return false;
    }

    private static string[]? GetObjectClasses(ILdapDataEntry element)
    {
        var objectClassesString = element.Attributes.FirstOrDefault(x => x.Key.ToLower() == "objectclass")
            .Value?.ToLower();

        var objectClasses = objectClassesString?
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.ToLower()).ToArray();
        return objectClasses;
    }

    private ParsersData LoadImportModelScript(ILookup<string, ScriptDataDetails<ConcordanceObjectType>> userScriptsByClass)
    {
        Dictionary<string, ParserData> scriptDictionary = new();
        foreach (var objectClass in userScriptsByClass)
        {
            var scripts = objectClass;
            var scriptData = _parser.GetParserData(scripts);
            scriptDictionary.Add(objectClass.Key,scriptData);
        }
        
        var parentParserData = new ParserData();
        return new ParsersData(scriptDictionary, parentParserData);
    }
    
    private ImportModel? LoadImportModel(ParsersData scriptDictionary,
        IReadOnlyDictionary<string, string>? userEntryAttributes, string[] objectClasses, IEnumerable<IReadOnlyDictionary<string,string>> parents)
    {
        ImportModel? record = null;

        var parserDatas = (IEnumerable<ParserData>?) scriptDictionary.FilterCLasses(x=>objectClasses.Contains(x)).GetParserDatum();
        foreach (var objectClass in parserDatas)
        {
            var attributeNames = userEntryAttributes.Keys.ToArray();
            
            var data = userEntryAttributes.Values.ToArray();
            
            var classData = _parser.ParseToObjectDictionary(
                objectClass,
                attributeNames,
                data,
                parents: parents);

            record = record == null
                ? ImportHelper.GetImportModel(classData)
                : ImportHelper.UpdateModel(classData,
                    record);
            
            if (userEntryAttributes.ContainsKey(ObjectSid))
            {
                record.UserSID = userEntryAttributes[ObjectSid];
            }
        }
        return record;
    }
}