IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[Values]') AND NAME='Order')
  ALTER TABLE [dbo].[Values]
  ADD "Order" INT NOT NULL DEFAULT 0
GO