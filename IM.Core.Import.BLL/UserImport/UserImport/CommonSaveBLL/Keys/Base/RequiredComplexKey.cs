namespace IM.Core.Import.BLL.Import.Array;

internal record RequiredComplexKey<TFirst,TSecond>(TFirst First, TSecond Second) :IIsSet
where TFirst:IIsSet
where TSecond:IIsSet
{
    public bool IsSet()
    {
        return First.IsSet() && Second.IsSet();
    }

    public virtual string GetKey()
    {
        return $"{First.GetKey()} {Second.GetKey()}";
    }
}