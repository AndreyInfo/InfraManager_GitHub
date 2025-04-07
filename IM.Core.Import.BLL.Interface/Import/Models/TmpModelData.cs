using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;

namespace IM.Core.Import.BLL.Interface.Import.Models
{
    //TODO:сделать легковес
    public class TmpModelData:IModelDataTryGet, IModelDataKeys, IModelDataContains, ICommonData
    {
        private readonly Dictionary<string?, string?> _data = new();

        public TmpModelData(IReadOnlyDictionary<string?, string?> data)
        {
            _data = data.ToDictionary(x => x.Key, x => x.Value);
        }
        
        public bool TryGetValue(string key, out string value)
        {
            return _data.TryGetValue(key, out value);
        }

        public IEnumerable<string> Keys => _data.Keys;

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public string this[string value] => _data[value];

        public string ExternalID
        {
            get => _data[CommonFieldNames.ModelExternalID];
            set =>_data[CommonFieldNames.ManufacturerExternalID] = value;
        }

        public string Name
        {
            get => _data[CommonFieldNames.ModelName];
            set => _data[CommonFieldNames.ModelName] = value;
        }

        public string ModelNote
        {
            get => _data[CommonFieldNames.ModelDescription];
            set => _data[CommonFieldNames.ModelDescription] = value;
        }

        public string ModelProductNumber
        {
            get => _data[CommonFieldNames.ModelProductName];
            set => _data[CommonFieldNames.ModelProductName] = value;
        }

        public bool ModelCanBuy
        {
            get => _data[CommonFieldNames.ModelCanBuy] switch{ "Да"=>true,"Нет"=> false, _=>throw new ArgumentOutOfRangeException()};
            set => _data[CommonFieldNames.ModelCanBuy] = value ? "Да" : "Нет";
        }

        public string TypeExternalID
        {
            get => _data[CommonFieldNames.TypeExternalID];
            set => _data[CommonFieldNames.TypeExternalID] = value;
        }

        public string TypeExternalName
        {
            get => _data[CommonFieldNames.TypeExternalName];
            set => _data[CommonFieldNames.TypeExternalName] = value;
        }

        public string UnitExternalID
        {
            get => _data[CommonFieldNames.UnitExternalID];
            set => _data[CommonFieldNames.UnitExternalID] = value;
        }

        public string UnitExternalName
        {
            get => _data[CommonFieldNames.UnitName];
            set => _data[CommonFieldNames.UnitName] = value;
        }

        public string ManufacturerExternalID
        {
            get => _data[CommonFieldNames.ManufacturerExternalID];
            set => _data[CommonFieldNames.ManufacturerExternalID] = value;
        }

        public string ManufacturerName
        {
            get => _data[CommonFieldNames.ManufacturerName];
            set => _data[CommonFieldNames.ManufacturerName] = value;
        }

        public string Parameters
        {
            get => _data[CommonFieldNames.Paramters];
            set => _data[CommonFieldNames.Paramters] = value;
        }
    }
}