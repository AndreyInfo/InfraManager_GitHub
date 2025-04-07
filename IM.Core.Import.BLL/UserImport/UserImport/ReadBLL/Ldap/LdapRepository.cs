using System.Text;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Import.Ldap;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Ldap;
using IM.Core.Import.BLL.Ldap.LDAPPages;
using InfraManager;
using Microsoft.Scripting.Runtime;
using Novell.Directory.Ldap;

namespace IM.Core.Import.BLL.Ldap;

public class LdapRepository : ILdapRepository, ISelfRegisteredService<ILdapRepository>
{
    //todo:реализовать компоновщик по узлам или опрос по классам 
    //todo:оптимизировать логику
    private const int DefaultHopLimit = 10;
    private const int MaxMsLimit = 0;
    private const string ObjectClassAttribute = "objectClass";
    private const string DnAttribute = "dn";
    private const string ObjectClassSeparator = ";";
    private readonly ILocalLogger<LdapRepository> _logger;
    private readonly ILdapRequestFactory _factory;
    private readonly IScriptDataParser<ConcordanceObjectType> _parser;

    public LdapRepository(ILocalLogger<LdapRepository> logger, ILdapRequestFactory factory, IScriptDataParser<ConcordanceObjectType> parser)
    {
        _logger = logger;
        _factory = factory;
        _parser = parser;
    }

    private IEnumerable<LdapNodeData> LoadLdapTree(string url, string login, string password,
        LdapLoadRequestData ldapLoadRequestData, IMLdapTreeContext context, ParsersData parserData, Dictionary<string,LdapNodeData> nodes)
    {
        var ldapUrl = new LdapUrlData(url);
        var ldapUrlDn = ldapUrl.Dn;

        //поиск родителей
        var dnData = new DnData(ldapUrlDn);
        var enumerable = dnData.GetParents().ToList();
        using var parentDns = enumerable.GetEnumerator();

        //загрузка общих родителей
        var foundParentNode = GetBaseLdapNodeDescription(login,
            password,
            ldapLoadRequestData,
            parentDns,
            ldapUrl,
            parserData,
            nodes);

        //загрузка корня
        var parents = foundParentNode.Parents;
        var parentsData = parents.GetParents();
        var baseNodeResult =
            TryReadNodeFromLdap(login, password, ldapLoadRequestData, ldapUrl, parserData, parentsData);
        if (baseNodeResult == null)
            yield break;
        context.Readed(baseNodeResult.Dn);
        yield return baseNodeResult;

        //загрузка ветвей
        var baseNode = foundParentNode.AddChild(baseNodeResult.Dn, baseNodeResult.Attributes);
        foreach (var ldapData in ReadTreeFromLdap(login, password, ldapLoadRequestData, baseNode.Parents, ldapUrl,
                     context, parserData))
            yield return ldapData;
    }

    private LdapNodeDescription GetBaseLdapNodeDescription(string login, string password,
        LdapLoadRequestData ldapLoadRequestData, IEnumerator<string> parentDns, LdapUrlData ldapUrl,
        ParsersData parserData, Dictionary<string,LdapNodeData> ldapNodeDatas)
    {
        LdapNodeDescription? foundParentNode = null;
        while (parentDns.MoveNext())
        {
            var topParent = parentDns.Current;
            var parentUrl = ldapUrl.GetOtherDnUrlData(topParent)!;
            var topParentNode = new LdapNodeDescription(parentUrl);
            var parentNode =
                TryReadNodeFromLdap(login, password, ldapLoadRequestData, parentUrl, parserData, topParentNode.Parents.GetParents());
            if (parentNode != null)
            {
                foundParentNode = topParentNode.AddChild(parentNode.Dn, parentNode.Attributes);
                ldapNodeDatas[parentNode.Dn] = parentNode;
                break;
            }
        }

        if (foundParentNode != null)
        {
            while (parentDns.MoveNext())
            {
                var topParent = parentDns.Current;
                var parentUrl = ldapUrl.GetOtherDnUrlData(topParent);
                var subParentNode =
                    TryReadNodeFromLdap(login, password, ldapLoadRequestData, parentUrl, parserData, foundParentNode.Parents.GetParents());
                if (subParentNode != null)
                {
                    foundParentNode = foundParentNode.AddChild(subParentNode.Dn, subParentNode.Attributes);
                    ldapNodeDatas[subParentNode.Dn] =  subParentNode;
                }
                else
                {
                    foundParentNode = foundParentNode.AddChild(subParentNode.Dn, new Dictionary<string, string>());
                }
            }
        }
        else
        {
            foundParentNode = new LdapNodeDescription(ldapUrl);
        }

        return foundParentNode;
    }

    
    private LdapNodeData? TryReadNodeFromLdap(string login,
        string password,
        LdapLoadRequestData ldapLoadRequestData,
        LdapUrlData parentUrl,
        ParsersData parserData,
        List<Dictionary<string, string>> parents)
    {
        LdapNodeData? parentNode;
        try
        {
            parentNode = ReadLdapNode(login, password, ldapLoadRequestData, parentUrl, parserData, parents)
                .SingleOrDefault();
        }
        catch (LdapException e)
        {
            _logger.Verbose($"ERR: Ошибка LDAP при чтении узла {parentUrl.Dn}. \n {e}");
            parentNode = null;
        }

        return parentNode;
    }

    private IEnumerable<LdapNodeData> ReadTreeFromLdap(string login, string password,
        [NotNull] LdapLoadRequestData ldapLoadRequestData,
        LdapParents nodeDnWithParent, LdapUrlData ldapUrl, IMLdapTreeContext ldapContext, ParsersData parserData)
    {
        var topRequest = new LdapNodeDescription(ldapUrl, nodeDnWithParent);
        var ldapContextTree = ldapContext.Tree;
        ldapContextTree.AddPlanned(topRequest);
        while (ldapContextTree.MoveNext())
        {
            var current = ldapContextTree.Current!;
            var url = current.Url;
            List<LdapNodeData> redNodes;
            try
            {
                LdapParents parents = current.Parents;
                var currentUrl = ldapUrl.GetOtherDnUrlData(url.Dn);
                redNodes = ReadLdapChilds(login, password, ldapLoadRequestData, currentUrl, ldapContext, parserData, parents.GetParents());
            }
            catch (Exception e)
            {
                _logger.Information($"Не удалось прочитать узел {url.Dn}");
                _logger.Error(e,e.Message);
               continue;
            }
          
            foreach (var ldapNodeData in redNodes)
            {
                ldapContextTree.AddPlanned(ldapNodeData, current);
                yield return ldapNodeData;
            }
        }
        
    }

    private List<LdapNodeData> ReadLdapNode(string login, string password, LdapLoadRequestData ldapLoadRequestData,
        LdapUrlData ldapUrl, ParsersData parserData, List<Dictionary<string, string>> parents)
    {
        return ReadLdapNodeInternal(login, password, ldapLoadRequestData, ldapUrl, LdapScopeEnum.Base, null, parserData, parents);
    }

    private List<LdapNodeData> ReadLdapChilds(string login, string password, LdapLoadRequestData ldapLoadRequestData,
        LdapUrlData ldapUrl, IMLdapTreeContext context, ParsersData parserData, List<Dictionary<string, string>> parents)
    {
        return ReadLdapNodeInternal(login, password, ldapLoadRequestData, ldapUrl, LdapScopeEnum.One, context, parserData, parents);
    }

    private List<LdapNodeData> ReadLdapNodeInternal(string login, string password,
        LdapLoadRequestData ldapLoadRequestData,
        LdapUrlData ldapUrl,
        LdapScopeEnum scope, IMLdapTreeContext context, ParsersData parserData, List<Dictionary<string, string>> parents)
    {
        var address = ldapUrl.Address;
        var classes = ldapLoadRequestData?.Classes;
        var request = _factory.GetRequestData(address, login, password, LdapConnection.LdapV3, classes);
        var nodes = FollowReference(request, context, ldapLoadRequestData, scope, parserData, parents)
            .ToList();

        foreach (var node in nodes)
        {
            node.Address = ldapUrl.GetOtherDnUrlData(node.Dn).Address;
        }
        
        return nodes;
    }

    private IEnumerable<LdapNodeData> FollowReference(ILdapRequestData requestData,
        IMLdapTreeContext context,
        LdapLoadRequestData ldapScriptData,
        LdapScopeEnum scope,
        ParsersData parserData,
        List<Dictionary<string, string>> parents)
    {
        var connectionData = requestData.ConnectionData;
        var bindData = requestData.BindData;
        var searchData = requestData.SearchData;
        var searchAttributes = new[] {ObjectClassAttribute};

        using var connection = new LdapConnection();
        if (connectionData is not null)
        {
            try
            {
                connection.Connect(connectionData.HostName, connectionData.Port);
            }
            catch (Exception e)
            {
                _logger.Information("Ошибка при подключении к LDAP");
                _logger.Error(e, e.Message);
                throw;
            }
        }

        var classes = ldapScriptData.Classes;
        try
        {
            Bind(bindData, searchData, connection);

            var cookie = LdapPagedResultsControl.GetEmptyCookie;

            while (true)
            {
                var constraints = connection.Constraints;
                var searchConstraints = GetConstraints(bindData, searchData, cookie, connection.Constraints);

                connection.Constraints = searchConstraints;

                var searchDataMask = ""; 
                _logger.Information($"Поиск в узле {searchData.BaseNode}");
                var results = connection.Search(searchData.BaseNode,
                    (int) scope,
                    searchDataMask,
                    searchAttributes,
                    false,
                    searchConstraints);
                
                //получение страницы
                while (results.HasMore())
                {
                    LdapEntry? currentResult;

                    try
                    {
                        currentResult = results.Next();
                    }
                    catch (LdapReferralException e)
                    {
                        if (e.LdapErrorMessage is not null)
                            _logger.Error(e,e.LdapErrorMessage);
                        _logger.Information("Следование по ссылке LDAP");
                        var referenceUrls = e.GetReferrals();
                        context?.AddReferences(referenceUrls);
                        continue;
                    }
                    catch (LdapException e)
                    {
                        if (e.Message == "No Such Object")
                        {
                            _logger.Information(
                                $"Ошибка LDAP {e.LdapErrorMessage}. Объект не найден. Возможно нет доступа. Пропуск.");
                            continue;
                        }

                        _logger.Information("Неизвестная ошибка при чтении ветки. Пропуск.");
                        _logger.Error(e,e.Message);
                        continue;
                    }

                    var currentObjectClass =
                        (LdapAttribute?) GetLdapStringAttribute(currentResult, ObjectClassAttribute);
                    var classesValue = currentObjectClass?.StringValueArray ?? Array.Empty<string>();

                    Dictionary<string, string> currentResultAttributes;
                    var currentResultDn = currentResult.Dn;
                    if (classes.Any() && classesValue.Any())
                    {
                        try
                        {
                            currentResultAttributes = LoadAttributesForNodeClasses(requestData,
                                ldapScriptData,
                                currentResultDn,
                                classesValue,
                                classes,
                                connection, parserData, parents);
                        }
                        catch (Exception e)
                        {
                            _logger.Information("Ошибка при опросе параметров. Пропуск.");
                            _logger.Error(e,e.Message);
                            continue;
                        }
                       
                    }
                    else
                    {
                        _logger.Information(
                            $"Не удается прочитать данные для {currentResultDn}. Будет доступно только поле dn.");
                        currentResultAttributes = new();
                        currentResultAttributes.Add(DnAttribute, currentResultDn);
                    }

                    if (!currentResultAttributes.ContainsKey(ObjectClassAttribute))
                    {
                        var classesString = string.Join(';', classesValue).TrimEnd();
                        currentResultAttributes.Add(ObjectClassAttribute, classesString);
                    }

                    if (!currentResultAttributes.ContainsKey(DnAttribute))
                    {
                        currentResultAttributes.Add(DnAttribute, currentResultDn);
                    }


                    var result = new LdapNodeData(currentResultDn, parents,
                        currentResultAttributes);
                    yield return result;
                }

                if (!GetCookie(results, ref cookie))
                    break;
            }
        }
        finally
        {
            connection.Disconnect();
        }
    }

    private static LdapAttribute GetLdapStringAttribute(LdapEntry currentResult, string attributeName)
    {
        LdapAttribute attribute;
        try
        {
            attribute = currentResult.GetAttribute(attributeName);
        }
        catch (KeyNotFoundException e)
        {
            attribute = null;
        }


        return attribute;
    }

    private Dictionary<string, string> LoadAttributesForNodeClasses(ILdapRequestData requestData,
        LdapLoadRequestData ldapRequestData,
        string dn,
        string[] classesFromLdap,
        string[] classesFromConfiguration,
        LdapConnection connection, ParsersData parserData, List<Dictionary<string, string>> parents)
    {
        _logger.Information($"Опрос атрибутов {dn}");
        var dict = new Dictionary<string, string>();
        var validClasses = classesFromLdap.Select(x => x.ToLower()).Intersect(classesFromConfiguration).ToArray();
        HashSet<string> attributesData = new();
        string[] attributes;
        dict[DnAttribute] = dn;
        dict[ObjectClassAttribute] = string.Join(ObjectClassSeparator, classesFromLdap);

        foreach (var currentClass in validClasses.DefaultIfEmpty())
        {
            var ldapClassRequestData = ldapRequestData[currentClass];
            var searchAttribute = ldapClassRequestData.GetAttributes();
            attributesData.UnionWith(searchAttribute);

            attributes = attributesData.ToArray();
            bool notParsed;
            List<string> passedAttributes = attributes.ToList();
            List<string> parentPassedAttributes = new();
            _logger.Information("Загрузка данных и поиск полей");
            do
            {
                if (currentClass != null)
                    AddToDictionary(requestData, dn, connection, new[] {currentClass}, attributes, dict);
                var parsersData = parserData[currentClass];
                if (parserData == null)
                {
                    _logger.Information("Не найдена информация для парсинга");
                    break;
                }
                notParsed = !_parser.TryParseObjectDictionary(parsersData,
                    dict,
                    parents,
                    parentPassedAttributes,
                    passedAttributes,
                    out var result,
                    out var fieldName, 
                    out var typeEnum);
                if (notParsed)
                {
                    attributes = new[] {fieldName};
                    switch (typeEnum)
                    {
                        case CheckerTypeEnum.Record:
                            _logger.Information($"Обнаружено необходимое для парсинга record поле {fieldName}. Добавление в список опроса");
                            ldapRequestData[currentClass].AddAttribute(fieldName);
                            passedAttributes.Add(fieldName);
                            break;
                        case CheckerTypeEnum.Parent:
                            _logger.Information($"Обнаружено необходимое для парсинга parent поле {fieldName}. Добавление в список опроса");
                            ldapRequestData[null].AddAttribute(fieldName);
                            foreach (var parent in parents)
                            {
                                var classes = parent[ObjectClassAttribute].Split(ObjectClassSeparator);
                                AddToDictionary(requestData,parent[DnAttribute],connection,classes,attributes,parent);
                            }
                            parentPassedAttributes.Add(fieldName);
                            break;
                        default:
                            throw new NotSupportedException(
                                $"{Enum.GetName(typeof(CheckerTypeEnum), typeEnum)} не поддерживается");
                    }
                    
                }
            } while (notParsed);
            _logger.Information("Опрос атрибутов узла завершен");
        }

        return dict;
    }

    private void AddToDictionary(ILdapRequestData requestData, string dn, LdapConnection connection, string?[] validClasses,
        string[] attributes, Dictionary<string, string> dict)
    {
        var request = _factory.GetSearchClassData(requestData, dn, validClasses);

        var results2 = connection.Search(request.SearchData.BaseNode,
            LdapConnection.ScopeBase,
            request.SearchData.Mask,
            attributes,
            false);

        LdapEntry entry;
        if (results2.HasMore())
        {
            entry = results2.Next();
            LoadData(dict, entry);
        }
    }

    private static void LoadData(Dictionary<string, string> dict, LdapEntry? entry)
    {
        if (entry is null)
            throw new ArgumentNullException(nameof(entry));

        dict[DnAttribute] = entry.Dn;
        //Get the attribute set of the entry
        var attributeSet = entry.GetAttributeSet();
        foreach (var attribute in attributeSet)
        {
            var attributeName = attribute.Name;
            var attributeVal = GetAttributeValue(attribute);
            dict[attributeName] = attributeVal;
        }
    }

    private static bool GetCookie(ILdapSearchResults results, ref byte[] cookie)
    {
        var controls = (results as LdapSearchResults)?.ResponseControls;
        if (controls == null)
            return false;
        var pageControl = (LdapPagedResultsControl) controls?.FirstOrDefault(x => x is LdapPagedResultsControl);
        if (pageControl?.IsEmptyCookie() ?? false)
            return false;
        cookie = pageControl.Cookie;
        return true;
    }

    private static LdapSearchConstraints Bind(IBindData bindData, ISearchData searchData, LdapConnection connection)
    {
        var searchConstraints = GetConstraints(bindData, searchData, LdapPagedResultsControl.GetEmptyCookie, null);
        connection.Bind(LdapConnection.LdapV3, bindData.Dn, bindData.Password, searchConstraints);
        return searchConstraints;
    }

    private static LdapSearchConstraints GetConstraints(IBindData bindData, ISearchData searchData, byte[] cookie,
        LdapConstraints? prevConstraints)
    {
        var handler = (ILdapAuthHandler) new LdapAuthHandler(bindData.Dn, bindData.Password);

        LdapConstraints constraints =
            prevConstraints ?? new LdapConstraints(MaxMsLimit, true, handler, DefaultHopLimit);

        var control = new LdapPagedResultsControl(searchData.PageSize, cookie);

        var searchConstraints = new LdapSearchConstraints(constraints)
        {
            BatchSize = 0,
        };

        searchConstraints.SetControls(control);
        return searchConstraints;
    }

    private static string GetAttributeValue(LdapAttribute attribute)
    {
        return attribute.Name switch
        {
            "objectGUID" => (new Guid(attribute.ByteValue)).ToString(),
            "objectSid" => Convert.ToBase64String(attribute.ByteValue),
            _ => GetStringValue(attribute)
        };
    }

    private static string GetStringValue(LdapAttribute attribute)
    {
        var size = attribute.Size();
        return size > 1
            ? attribute.StringValueArray.Aggregate(new StringBuilder(), (a, c) => a.Append(c).Append(';'),
                a => a.ToString())
            : attribute.StringValue;
    }

    public IEnumerable<ILdapDataEntry> GetAllAttributes(string url, string login, string password,
        ParsersData parserData,
        LdapLoadRequestData ldapLoadRequestData)
    {
        var forest = new IMLdapForestContext();
        forest.AddAddress(url);
        var ldapNodeDatas = new Dictionary<string, LdapNodeData>();
        while (forest.MoveNext())
        {
            var ldapDataEntries = LoadLdapTree(forest.CurrentAddress, login, password, ldapLoadRequestData,
                forest.Current, parserData, ldapNodeDatas);
            foreach (var current in ldapDataEntries)
            {
                var currentDn = current.Dn;
                if (ldapNodeDatas.ContainsKey(currentDn))
                    ldapNodeDatas.Remove(currentDn);
                yield return current;
            }
        }

        foreach (var ldapNodeData in ldapNodeDatas.Values)
        {
            yield return ldapNodeData;
        }
    }
}