if 'DF_Document_InternalID' not in (select name from sys.default_constraints)
begin 
	ALTER TABLE [dbo].[Document] ADD CONSTRAINT [DF_Document_InternalID] DEFAULT NEXT VALUE FOR [dbo].[DocumentInternalID] FOR [InternalID];
end