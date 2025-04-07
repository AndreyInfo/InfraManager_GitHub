using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

[ListViewItem(ListView.SupplierContactPerson)]
public class SupplierContactPersonColumns
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Surname))]
    public string Surname { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Patronymic))]
    public string Patronymic { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Phone))]
    public string Phone { get; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.SecondPhone))]
    public string SecondPhone { get; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.Email))]
    public string Email { get; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.Position))]
    public string Position { get; }

    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.Note))]
    public string Note { get; }

    [ColumnSettings(8, 100)]
    [Label(nameof(Resources.Supplier))]
    public string Supplier { get; }
}