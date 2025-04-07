if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_IncidentResult' and object_id = OBJECT_ID('[dbo].[IncidentResult]')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_IncidentResult on [dbo].IncidentResult (Name) 
				where Removed = 0;
        end;