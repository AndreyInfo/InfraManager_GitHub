IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[MassiveIncidentType]') AND NAME='IconName')
  ALTER TABLE [dbo].[MassiveIncidentType]
  ADD [IconName] varchar(100) not null
GO