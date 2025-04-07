using System;

namespace InfraManager.BLL.ServiceCatalogue.Rules;

public class RuleParameterValue
{

    #region Ctors

    public RuleParameterValue(ValueType type, object value)
    {
        Type = type;
        Value = value;
    }

    public RuleParameterValue(object value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));

        switch (Value)
        {
            case bool:
                Type = ValueType.Boolean;
                break;

            case decimal:
                Type = ValueType.Decimal;
                break;

            case string:
                Type = ValueType.String;
                break;

            case DateTime:
                Type = ValueType.UtcDateTime;
                break;

            case Guid:
                Type = ValueType.Guid;
                break;

            case Tuple<int, Guid>:
                Type = ValueType.ClassIDAndGuid;
                break;

            case Tuple<RuleParameterValue[], bool>[]:
                Type = ValueType.TableRow;
                break;

            default:
                throw new NotSupportedException(Value.GetType().Name);
        }
    }

    public RuleParameterValue()
    {
    }

    #endregion
    
    public ValueType Type { get; set; }
    public object Value { get; set; }
}

public enum ValueType : byte
{
    Boolean = 0,
    Decimal = 1,
    String = 2,
    UtcDateTime = 3,
    Guid = 4,
    ClassIDAndGuid = 5,
    TableRow = 6
}