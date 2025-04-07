using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class UserConfiguration : UserConfigurationBase
{
    protected override string KeyName => "PK_Пользователи";

    protected override string LoginNameUI => "UI_Login_Users";

    protected override string EmailUI => "UI_Email_Users";

    protected override string WorkplaceFK => "FK_Пользователи_Рабочее место";

    protected override string PositionFK => "FK_Пользователи_Должности";

    protected override string SIDDefaultValue => "('')";

    protected override string IMObjIDDefaultValue => "(newid())";

    protected override string ExternalIDDefaultValue => "('')";
    protected override string DefaultIDValue => "(NEXT VALUE FOR [pk_users_seq])";


    protected override void ConfigureDataBase(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Пользователи", "dbo");

        builder.Property(e => e.Admin).HasColumnName("Admin");
        builder.Property(e => e.NetworkAdmin).HasColumnName("NetworkAdmin");
        builder.Property(e => e.Removed).HasColumnName("Removed");
        builder.Property(e => e.SupportAdmin).HasColumnName("SupportAdmin");
        builder.Property(e => e.SupportEngineer).HasColumnName("SupportEngineer");
        builder.Property(e => e.SupportOperator).HasColumnName("SupportOperator");
        builder.Property(e => e.ID).HasColumnName("Идентификатор");
        builder.Property(e => e.Name).HasColumnName("Имя");
        builder.Property(e => e.Surname).HasColumnName("Фамилия");
        builder.Property(e => e.Patronymic).HasColumnName("Отчество");
        builder.Property(e => e.CalendarWorkScheduleID).HasColumnName("CalendarWorkScheduleID");
        builder.Property(e => e.ComplementaryGuidId).HasColumnName("ComplementaryGuidID");
        builder.Property(e => e.ComplementaryId).HasColumnName("ComplementaryID");
        builder.Property(e => e.Email).HasColumnName("Email");
        builder.Property(e => e.SDWebPassword).HasColumnName("SDWebPassword");
        builder.Property(e => e.SID).HasColumnName("SID");
        builder.Property(e => e.TimeZoneID).HasColumnName("TimeZoneID");
        builder.Property(e => e.SDWebAccessIsGranted).HasColumnName("SDWebAccessIsGranted");
        builder.Property(e => e.VisioID).HasColumnName("Visio_ID");
        builder.Property(e => e.RoomID).HasColumnName("ИД комнаты");
        builder.Property(e => e.Initials).HasColumnName("Инициалы");
        builder.Property(e => e.Pager).HasColumnName("Пейджер");
        builder.Property(e => e.Note).HasColumnName("Примечания");
        builder.Property(e => e.Phone).HasColumnName("Телефон");
        builder.Property(e => e.Phone1).HasColumnName("Телефон1");
        builder.Property(e => e.Phone2).HasColumnName("Телефон2");
        builder.Property(e => e.Phone3).HasColumnName("Телефон3");
        builder.Property(e => e.Phone4).HasColumnName("Телефон4");
        builder.Property(e => e.Fax).HasColumnName("Факс");
        builder.Property(e => e.Photo).HasColumnType("image").HasColumnName("Фото");
        builder.Property(e => e.ExternalID).HasColumnName("ExternalID");
        builder.Property(e => e.IMObjID).HasColumnName("IMObjID");
        builder.Property(e => e.IsLockedForOsi).HasColumnName("IsLockedForOSI");
        builder.Property(e => e.LoginName).HasColumnName("Login name");
        builder.Property(e => e.ManagerID).HasColumnName("ManagerID");
        builder.Property(e => e.Number).HasColumnName("Number");
        builder.Property(e => e.PeripheralDatabaseId).HasColumnName("PeripheralDatabaseID");
        builder.Property(e => e.SubdivisionID).HasColumnName("ИД подразделения");
        builder.Property(e => e.WorkplaceID).HasColumnName("ИД рабочего места");
        builder.Property(e => e.PositionID).HasColumnName("ИД должности");
        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .HasColumnType("timestamp")
            .HasColumnName("RowVersion");
    }
}