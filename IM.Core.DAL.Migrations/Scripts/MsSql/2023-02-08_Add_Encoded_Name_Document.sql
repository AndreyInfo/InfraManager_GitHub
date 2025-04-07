IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[Document]') AND NAME='EncodedName')
  ALTER TABLE [dbo].[Document]
  ADD [EncodedName] [nvarchar](250) NULL
GO
