using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Users;

[ListViewItem(ListView.UsersList)]
public sealed class UserForTable
{
    [ColumnSettings(1)]
    [Label(nameof(Resources.Surname))]
    public string SurName { get; init; }

    [ColumnSettings(2)]
    [Label(nameof(Resources.PersonalName))]
    public string Name { get; init; }

    [ColumnSettings(3)]
    [Label(nameof(Resources.Patronymic))]
    public string Patronymic { get; init; }

    [ColumnSettings(4)]
    [Label(nameof(Resources.UserBuilding))]
    public string Building { get; init; }

    [ColumnSettings(5)]
    [Label(nameof(Resources.UserFloor))]
    public string Floor { get; init; }

    [ColumnSettings(6)]
    [Label(nameof(Resources.UserRoom))]
    public string Room { get; init; }

    [ColumnSettings(7)]
    [Label(nameof(Resources.UserOrganization))]
    public string Organization { get; init; }

    [ColumnSettings(8)]
    [Label(nameof(Resources.UserSubdivision))]
    public string Department { get; init; }

    [ColumnSettings(9)]
    [Label(nameof(Resources.Login))]
    public string LoginName { get; init; }

    [ColumnSettings(10)]
    [Label(nameof(Resources.SecondPhone))]
    public string Phone1 { get; init; }

    [ColumnSettings(11)]
    [Label(nameof(Resources.UserEmail))]
    public string Email { get; init; }

    [ColumnSettings(12)]
    [Label(nameof(Resources.UserPhone))]
    public string Phone { get; init; }

    [ColumnSettings(13)]
    [Label(nameof(Resources.UserWorkplace))]
    public string WorkplaceName { get; init; }

    [ColumnSettings(14)]
    [Label(nameof(Resources.AdminTools_PersonalLicence_UserNumber))]
    public string Number { get; init; }
}