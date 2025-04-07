IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[WorkflowActivityFormField]') AND NAME='ColumnFieldID')
  ALTER TABLE [dbo].[WorkflowActivityFormField] ADD [ColumnFieldID] [uniqueidentifier] null

  ALTER TABLE [dbo].[WorkflowActivityFormField] ADD CONSTRAINT FK_ColumnField FOREIGN KEY ([ColumnFieldID]) REFERENCES [dbo].[WorkflowActivityFormField](ID);
GO

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'FK_GroupField' and object_id = OBJECT_ID('WorkflowActivityFormField')))
        BEGIN
		ALTER TABLE [dbo].[WorkflowActivityFormField] ADD CONSTRAINT FK_GroupField FOREIGN KEY ([GroupFieldID]) REFERENCES [dbo].[WorkflowActivityFormField](ID);
        end;