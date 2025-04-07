namespace IM.Core.Import.BLL.Interface.Import;

public interface IFieldsService
{
    IReadOnlyCollection<string> GetCommonFields();
}