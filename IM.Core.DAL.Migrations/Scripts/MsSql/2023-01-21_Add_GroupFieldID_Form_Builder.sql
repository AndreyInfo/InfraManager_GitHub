IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[WorkflowActivityFormField]') AND NAME='GroupFieldID')
  ALTER TABLE [dbo].[WorkflowActivityFormField]
  ADD [GroupFieldID] [uniqueidentifier] null
GO
