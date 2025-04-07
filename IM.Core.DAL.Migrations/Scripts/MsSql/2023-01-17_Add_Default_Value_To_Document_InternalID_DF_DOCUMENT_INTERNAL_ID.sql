if not exists
            (select *
                from sys.all_columns c
                         join sys.tables t on t.object_id = c.object_id
                         join sys.schemas s on s.schema_id = t.schema_id
                         join sys.default_constraints d on c.default_object_id = d.object_id
                where t.name = 'Document'
                  and c.name = 'InternalID'
                  and s.name = 'dbo')
BEGIN
ALTER TABLE [dbo].[Document] ADD  CONSTRAINT [DF_DOCUMENT_INTERNAL_ID]  DEFAULT (NEXT VALUE FOR DocumentInternalID) FOR [InternalID]    
END

