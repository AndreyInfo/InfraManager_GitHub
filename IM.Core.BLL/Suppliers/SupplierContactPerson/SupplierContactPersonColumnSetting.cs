using InfraManager.BLL.ColumnMapper;
using SupplierContactPersonEntity = InfraManager.DAL.Suppliers.SupplierContactPerson;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

internal sealed class SupplierContactPersonColumnSetting :
    IColumnMapperSetting<SupplierContactPersonEntity, SupplierContactPersonColumns>
    , ISelfRegisteredService<IColumnMapperSetting<SupplierContactPersonEntity, SupplierContactPersonColumns>>
{
    public void Configure(IColumnMapperSettingsBase<SupplierContactPersonEntity, SupplierContactPersonColumns> configurer)
    {
        configurer.ShouldBe(x => x.Position, x => x.Position.Name);
        configurer.ShouldBe(x => x.Supplier, x => x.Supplier.Name);
    }
}