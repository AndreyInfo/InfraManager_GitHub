if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_RfsResult' and object_id = OBJECT_ID('[dbo].[RFSResult]')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_RfsResult on [dbo].RFSResult (Name) 
				where Removed = 0;
        end;