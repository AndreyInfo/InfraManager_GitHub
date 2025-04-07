using System.Collections;

public class SingleOrVector:IConvertible
{
    private readonly object? _obj;

    private T? GetScalar<T>(IFormatProvider? provider)
    {
        return (T?) GetScalar(typeof(T), provider);
    }
    private object? GetScalar(Type type, IFormatProvider? provider)
    {
        
        object value;
        if (_obj is not string && _obj is IEnumerable enumerable)
        {
            var objects = enumerable.Cast<object>().ToList();
            if (objects.Count >= 1)
            {
                value = objects.First();
                
            }
            else
            {
                value = null;
            }
        }
        else
        {
            value = _obj;
        }
        // возможна рекурсия
        return Convert.ChangeType(value, type, provider);
    }

    public SingleOrVector(object? obj)
    {
        _obj = obj;
        
    }

    public TypeCode GetTypeCode()
    {
        return TypeCode.Object;
    }

    public bool ToBoolean(IFormatProvider? provider)
    {
        return GetScalar<bool>(provider);
    }

    public byte ToByte(IFormatProvider? provider)
    {
        return GetScalar<byte>(provider);
    }

    public char ToChar(IFormatProvider? provider)
    {
        return GetScalar<char>(provider);
    }

    public DateTime ToDateTime(IFormatProvider? provider)
    {
        return GetScalar<DateTime>(provider);
    }

    public decimal ToDecimal(IFormatProvider? provider)
    {
        return GetScalar<decimal>(provider);
    }

    public double ToDouble(IFormatProvider? provider)
    {
        return GetScalar<double>(provider);
    }

    public short ToInt16(IFormatProvider? provider)
    {
        return GetScalar<short>(provider);
    }

    public int ToInt32(IFormatProvider? provider)
    {
        return GetScalar<int>(provider);
    }

    public long ToInt64(IFormatProvider? provider)
    {
        return GetScalar<long>(provider);
    }

    public sbyte ToSByte(IFormatProvider? provider)
    {
        return GetScalar<sbyte>(provider);
    }

    public float ToSingle(IFormatProvider? provider)
    {
        return GetScalar<float>(provider);
    }

    public string ToString(IFormatProvider? provider)
    {
        return GetScalar<string>(provider);
    }

    public object? ToType(Type conversionType, IFormatProvider? provider)
    {
        
        if (typeof(IEnumerable).IsAssignableFrom(conversionType) && conversionType.IsGenericType)
        {
            var enumerableType = conversionType.GenericTypeArguments.First();
            var method = typeof(Enumerable).GetMethod("Cast");
            var genericMethod = method.MakeGenericMethod(new[] {enumerableType});
            if (_obj is not string && _obj is IEnumerable enumerable)
            {
                var objects = enumerable.Cast<object>().Select(x => Convert.ChangeType(x, enumerableType, provider)).ToList();
                
                var convertedEnumerable = genericMethod.Invoke(typeof(Enumerable),new[] {objects});
               
                return convertedEnumerable;
            }
            var convertedScalar = Convert.ChangeType(_obj, enumerableType, provider);
            object?[] objectArrayOfConverted = {convertedScalar};
            var convertedResult = genericMethod.Invoke(typeof(Enumerable), new []{objectArrayOfConverted});
            return convertedResult;
        }
        return GetScalar(conversionType, provider);
    }

    public ushort ToUInt16(IFormatProvider? provider)
    {
        return GetScalar<ushort>(provider);
    }

    public uint ToUInt32(IFormatProvider? provider)
    {
        return GetScalar<uint>(provider);
    }

    public ulong ToUInt64(IFormatProvider? provider)
    {
        return GetScalar<ulong>(provider);
    }
}