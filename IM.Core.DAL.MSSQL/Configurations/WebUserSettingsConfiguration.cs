using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class WebUserSettingsConfiguration: IEntityTypeConfiguration<WebUserSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserSettings> entity)
        {
            entity.ToTable("WebUserSettings", "dbo");
            entity.HasKey(e => e.UserID);

            entity.Property(e => e.UserID)
                .IsRequired()
                .HasColumnName("UserID");

            entity.Property(e => e.CultureName)
                .HasColumnName("CultureName")
                .HasMaxLength(84)
                .IsRequired();

            entity.Property(e => e.ViewNameSD)
                .HasColumnName("ViewNameSD")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.IncomingCallProcessing)
                .HasColumnName("IncomingCallProcessing")
                .IsRequired();

            entity.Property(e => e.TimeSheetFilter)
                .HasColumnName("TimeSheetFilter")
                .IsRequired();

            entity.Property(e => e.ViewNameAsset)
                .HasColumnName("ViewNameAsset")
                .HasMaxLength(50).IsRequired();

            entity.Property(e => e.AssetFiltrationData)
                .HasColumnName("AssetFiltrationData")
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(e => e.UseCompactMenuOnly)
                .HasColumnName("UseCompactMenuOnly")
                .IsRequired();

            entity.Property(e => e.ViewNameFinance)
                .HasColumnName("ViewNameFinance")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.FinanceBudgetID)
                .HasColumnName("FinanceBudgetID");

            entity.Property(e => e.ListViewGridLines)
                .HasColumnName("ListView_GridLines")
                .IsRequired();

            entity.Property(e => e.ListViewMulticolor)
                .HasColumnName("ListView_Multicolor")
                .IsRequired();

            entity.Property(e => e.ListViewCompactMode)
                .HasColumnName("ListView_CompactMode")
                .IsRequired();
        }
    }
}
