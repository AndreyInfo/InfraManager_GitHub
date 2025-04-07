IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[UserSession]') AND NAME='Location')
  ALTER TABLE [dbo].[UserSession]
  ADD Location [smallint] not null default 2
GO

IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[UserSession]') AND NAME='LicenceType')
  ALTER TABLE [dbo].[UserSession]
  ADD [LicenceType] [smallint] not null default 1
GO