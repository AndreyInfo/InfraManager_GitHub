using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Settings;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public class WebUserSettingsConfiguration : IEntityTypeConfiguration<WebUserSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserSettings> entity)
        {
            entity.ToTable("web_user_settings", Options.Scheme);

            entity.HasKey(e => e.UserID).HasName("pk_web_user_settings");

            entity.Property(e => e.UserID)
                .IsRequired()
                .HasColumnName("user_id");

            entity.Property(e => e.CultureName)
                .HasColumnName("culture_name")
                .HasMaxLength(84)
                .IsRequired();

            entity.Property(e => e.ViewNameSD)
                .HasColumnName("view_name_sd")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.IncomingCallProcessing)
                .HasColumnName("incoming_call_processing")
                .IsRequired();

            entity.Property(e => e.TimeSheetFilter)
                .HasColumnName("time_sheet_filter")
                .IsRequired();

            entity.Property(e => e.ViewNameAsset)
                .HasColumnName("view_name_asset")
                .HasMaxLength(50).IsRequired();

            entity.Property(e => e.AssetFiltrationData)
                .HasColumnName("asset_filtration_data")
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(e => e.UseCompactMenuOnly)
                .HasColumnName("use_compact_menu_only")
                .IsRequired();

            entity.Property(e => e.ViewNameFinance)
                .HasColumnName("view_name_finance")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.FinanceBudgetID)
                .HasColumnName("finance_budget_id");

            entity.Property(e => e.ListViewGridLines)
                .HasColumnName("list_view_grid_lines")
                .IsRequired();

            entity.Property(e => e.ListViewMulticolor)
                .HasColumnName("list_view_multicolor")
                .IsRequired();

            entity.Property(e => e.ListViewCompactMode)
                .HasColumnName("list_view_compact_mode")
                .IsRequired();
        }
    }
}