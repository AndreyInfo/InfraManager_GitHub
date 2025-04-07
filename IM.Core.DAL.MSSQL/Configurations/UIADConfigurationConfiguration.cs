
	using Inframanager.DAL.ActiveDirectory.Import;
	using Inframanager.DAL.EntityConfigurations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	namespace InfraManager.DAL.Postgres.Configurations
	{
		internal class UIADConfigurationConfiguration:UIADConfigurationConfigurationBase
		{
		
			protected override string PrimaryKeyName => "PK_UIADConfiguration";
			
			protected override void ConfigureDatabase(EntityTypeBuilder<UIADConfiguration> builder)
			{
				builder.ToTable("UIADConfiguration","dbo");
				
				builder.Property(x=>x.ID).HasColumnName("ID");

builder.Property(x=>x.Name).HasColumnName("Name");

builder.Property(x=>x.ShowUsersInADTree).HasColumnName("ShowUsersInAdTree");

builder.Property(x=>x.Note).HasColumnName("Note");


			}
		}
	}
	