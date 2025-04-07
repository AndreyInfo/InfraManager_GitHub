using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class UserConfiguration : UserConfigurationBase
{
    protected override string KeyName => "pk_users";

    protected override string LoginNameUI => "ui_login_users";

    protected override string EmailUI => "ui_email_users";

    protected override string WorkplaceFK => "fk_users_workplace";

    protected override string PositionFK => "fk_users_positions";

    protected override string SIDDefaultValue => "('')";

    protected override string IMObjIDDefaultValue => "(gen_random_uuid())";

    protected override string ExternalIDDefaultValue => "('')";

    protected override string DefaultIDValue => "nextval('pk_user_id_seq')";

    protected override void ConfigureDataBase(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", Options.Scheme);

        builder.HasXminRowVersion(e => e.RowVersion);
        builder.Property(e => e.Admin).HasColumnName("admin");
        builder.Property(e => e.NetworkAdmin).HasColumnName("network_admin");
        builder.Property(e => e.Removed).HasColumnName("removed");
        builder.Property(e => e.SupportAdmin).HasColumnName("support_admin");
        builder.Property(e => e.SupportEngineer).HasColumnName("support_engineer");
        builder.Property(e => e.SupportOperator).HasColumnName("support_operator");
        builder.Property(e => e.ID).HasColumnName("identificator");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Surname).HasColumnName("surname");
        builder.Property(e => e.Patronymic).HasColumnName("patronymic");
        builder.Property(e => e.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
        builder.Property(e => e.ComplementaryGuidId).HasColumnName("complementary_guid_id");
        builder.Property(e => e.ComplementaryId).HasColumnName("complementary_id");
        builder.Property(e => e.Email).HasColumnName("email");
        builder.Property(e => e.SDWebPassword).HasColumnName("sd_web_password");
        builder.Property(e => e.SID).HasColumnName("s_id");
        builder.Property(e => e.TimeZoneID).HasColumnName("time_zone_id");
        builder.Property(e => e.SDWebAccessIsGranted).HasColumnName("sd_web_access_is_granted");
        builder.Property(e => e.VisioID).HasColumnName("visio_id");
        builder.Property(e => e.RoomID).HasColumnName("room_id");
        builder.Property(e => e.Initials).HasColumnName("initials");
        builder.Property(e => e.Pager).HasColumnName("pager");
        builder.Property(e => e.Note).HasColumnName("notes");
        builder.Property(e => e.Phone).HasColumnName("phone");
        builder.Property(e => e.Phone1).HasColumnName("phone1");
        builder.Property(e => e.Phone2).HasColumnName("phone2");
        builder.Property(e => e.Phone3).HasColumnName("phone3");
        builder.Property(e => e.Phone4).HasColumnName("phone4");
        builder.Property(e => e.Fax).HasColumnName("fax");
        builder.Property(e => e.Photo).HasColumnType("image").HasColumnName("photo");
        builder.Property(e => e.ExternalID).HasColumnName("external_id");
        builder.Property(e => e.IMObjID).HasColumnName("im_obj_id");
        builder.Property(e => e.IsLockedForOsi).HasColumnName("is_locked_for_osi");
        builder.Property(e => e.LoginName).HasColumnName("login_name");
        builder.Property(e => e.PositionID).HasColumnName("position_id");
        builder.Property(e => e.WorkplaceID).HasColumnName("workplace_id");
        builder.Property(e => e.ManagerID).HasColumnName("manager_id");
        builder.Property(e => e.Number).HasColumnName("number");
        builder.Property(e => e.PeripheralDatabaseId).HasColumnName("peripheral_database_id");
        builder.Property(e => e.SubdivisionID).HasColumnName("department_id");
        builder.Property(x => x.WorkplaceID).HasColumnName("workplace_id");
        builder.Property(x => x.PositionID).HasColumnName("position_id");
    }
}