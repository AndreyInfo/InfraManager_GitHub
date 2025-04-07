namespace IM.Core.Import.BLL.Interface.Import;

public class ParsersData
{
    private readonly Dictionary<string, ParserData> _parserDatas;
    private readonly ParserData _parentParserData;

    public ParsersData()
    {
        _parserDatas = new(){};
        _parentParserData = new();
    }
    public ParsersData(Dictionary<string, ParserData> parserDatas, ParserData parentParserData)
    {
        _parserDatas = parserDatas;
        _parentParserData = parentParserData;
    }

    public ParserData this[string className]
    {
        get
        {
            if (className == null)
                return _parentParserData;
            if (_parserDatas.ContainsKey(className))
                return _parserDatas[className];
            var parserData = new ParserData();
            _parserDatas[className] = parserData;
            return parserData;
        }
    }

    public ParsersData FilterCLasses(Func<string, bool> predicate)
    {
        var keyValuePairs = _parserDatas.Where(x => predicate(x.Key));
        var parserDatas = keyValuePairs.ToDictionary(x => x.Key, x => x.Value);
        ParserData parentParserData;
        if (predicate(null))
        {
            parentParserData = _parentParserData;
        }
        else
        {
            parentParserData = new();
        }
        return new ParsersData(parserDatas, parentParserData);
    }

    public IEnumerable<ParserData> GetParserDatum() => _parserDatas.Values;
}