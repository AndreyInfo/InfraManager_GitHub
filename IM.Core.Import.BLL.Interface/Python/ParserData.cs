using IM.Core.Import.BLL.Import;

namespace IM.Core.Import.BLL.Interface.Import
{
    public class ParserData
    {
        private readonly Dictionary<string, ParserMethodData> _scripts;

        public ParserData()
        {
            _scripts = new();
        }
        
        private ParserData(Dictionary<string, ParserMethodData> scripts)
        {
            _scripts = scripts;
        }

        public void Add(string name, string source, Func<dynamic, IEnumerable<dynamic>, dynamic> function)
        {
            var scriptSource = new ParserMethodData(name, source, function);
            _scripts[name] = scriptSource;
        }

        public ParserMethodData? this[string name] => _scripts.ContainsKey(name) ? _scripts[name] : null;

        public IEnumerable<ParserMethodData> GetScripts() => _scripts.Values;

        public ParserData Filter(Func<ParserMethodData, bool> predicate)
        {
            var keyValuePairs = _scripts.Where(x=>predicate(x.Value));
            var parserMethodDatas = keyValuePairs.ToDictionary(x=>x.Key,x=>x.Value);
            return new ParserData(parserMethodDatas);
        }
    }
}