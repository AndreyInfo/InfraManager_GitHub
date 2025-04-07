IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[Schedule]') AND NAME='LastAt')
  ALTER TABLE [dbo].[Schedule]
  ADD LastAt [datetime] NULL
GO

IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[Schedule]') AND NAME='NextAt')
  ALTER TABLE [dbo].[Schedule]
  ADD NextAt [datetime] NULL
GO