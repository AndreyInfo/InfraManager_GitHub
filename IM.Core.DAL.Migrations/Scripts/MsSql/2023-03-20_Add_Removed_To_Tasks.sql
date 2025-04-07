IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[UIADSetting]') AND NAME='Removed')
  ALTER TABLE [dbo].[UIADSetting]
  ADD Removed [bit] NOT NULL DEFAULT 'FALSE'
GO

IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[UICSVSetting]') AND NAME='Removed')
  ALTER TABLE [dbo].[UICSVSetting]
  ADD Removed [bit] NOT NULL DEFAULT 'FALSE'
GO

IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[UISetting]') AND NAME='Removed')
  ALTER TABLE [dbo].[UISetting]
  ADD Removed [bit] NOT NULL DEFAULT 'FALSE'
GO