
namespace IM.Core.Import.BLL.Import.Array;

internal record ComplexKey<TFirst,TSecond>(TFirst Required,  TSecond Optional):IIsSet
    where TFirst:IIsSet
    where TSecond:IIsSet
{
    public static bool IsSet(bool required , bool optional) => required;
    //todo:создать имена для ключей и кастомный вывод
    public bool IsSet()
    {
        return Required.IsSet();
    }

    public string GetKey()
    {
        return $"{Required.GetKey()}-{Optional.GetKey()}";
    }
}