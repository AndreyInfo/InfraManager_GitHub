using System.Dynamic;
using IM.Core.Import.BLL.Interface.Import;
using Microsoft.Extensions.Primitives;

namespace IM.Core.Import.BLL.Import.CheckNotFoundFields;

public class FieldsChecker:DynamicObject
{
    private readonly List<string> _passedAttributes;
    private readonly IReadOnlyDictionary<string, string> _fields;
    private readonly CheckerTypeEnum _checkerTypeEnum;

    public FieldsChecker(List<string> passedAttributes, IReadOnlyDictionary<string, string> fields,
        CheckerTypeEnum checkerTypeEnum)
    {
        _passedAttributes = passedAttributes;
        _fields = fields;
        _checkerTypeEnum = checkerTypeEnum;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        var tryGetMember = binder.Name;
        if (_fields.ContainsKey(tryGetMember))
        {
            result = _fields[tryGetMember];
            return true;
        }

        if (_passedAttributes.Contains(tryGetMember))
        {
            result = string.Empty;
            return true;
        }

        throw new FieldNotFoundException(tryGetMember, _checkerTypeEnum);
    }
}