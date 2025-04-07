using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ViewUserConfiguration : IEntityTypeConfiguration<ViewUser>
    {
        public void Configure(EntityTypeBuilder<ViewUser> builder)
        {
            builder.ToView("view_user", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.IntID)
                .HasColumnName("int_id");
            builder.Property(x => x.ID)
                .HasColumnName("id");
            builder.Property(x => x.Family)
                .HasColumnName("family");
            builder.Property(x => x.Name)
                .HasColumnName("name");
            builder.Property(x => x.Patronymic)
                .HasColumnName("patronymic");
            builder.Property(x => x.Phone)
                .HasColumnName("phone");
            builder.Property(x => x.PhoneInternal)
                .HasColumnName("phone_internal");
            builder.Property(x => x.Email)
                .HasColumnName("email");
            builder.Property(x => x.Fax)
                .HasColumnName("fax");
            builder.Property(x => x.Pager)
                .HasColumnName("pager");
            builder.Property(x => x.Note)
                .HasColumnName("note");
            builder.Property(x => x.Login)
                .HasColumnName("login");
            builder.Property(x => x.SID)
                .HasColumnName("s_id");
            builder.Property(x => x.SDWebAccessIsGranted)
                .HasColumnName("sd_web_access_is_granted");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(x => x.Removed)
                .HasColumnName("removed");
            builder.Property(x => x.RoomID)
                .HasColumnName("room_id");
            builder.Property(x => x.RoomName)
                .HasColumnName("room_name");
            builder.Property(x => x.BuildingID)
                .HasColumnName("building_id");
            builder.Property(x => x.BuildingName)
                .HasColumnName("building_name");
            builder.Property(x => x.FloorID)
                .HasColumnName("floor_id");
            builder.Property(x => x.FloorName)
                .HasColumnName("floor_name");
            builder.Property(x => x.PositionID)
                .HasColumnName("position_id");
            builder.Property(x => x.PositionName)
                .HasColumnName("position_name");
            builder.Property(x => x.DivisionID)
                .HasColumnName("division_id");
            builder.Property(x => x.DivisionName)
                .HasColumnName("division_name");
            builder.Property(x => x.OrganizationID)
                .HasColumnName("organization_id");
            builder.Property(x => x.OrganizationName)
                .HasColumnName("organization_name");
            builder.Property(x => x.WorkplaceID)
                .HasColumnName("work_place_id");
            builder.Property(x => x.WorkplaceName)
                .HasColumnName("work_place_name");
            builder.Property(x => x.Number)
                .HasColumnName("number");
            builder.Property(x => x.TimeZoneID)
                .HasColumnName("time_zone_id");
            builder.Property(x => x.TimeZoneName)
                .HasColumnName("time_zone_name");
            builder.Property(x => x.CalendarWorkScheduleID)
                .HasColumnName("calendar_work_schedule_id");
            builder.Property(x => x.CalendarWorkScheduleName)
                .HasColumnName("calendar_work_schedule_name");
            builder.Property(x => x.ExternalID)
                .HasColumnName("external_id");
            builder.Property(x => x.IsLockedForOSI)
                .HasColumnName("is_locked_for_osi");
            builder.Property(x => x.ManagerID)
                .HasColumnName("manager_id");
            builder.Property(x => x.ManagerName)
                .HasColumnName("manager_name");
        }
    }
}