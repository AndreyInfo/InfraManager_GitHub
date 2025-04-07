if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Massive_incident_type_name' and object_id = OBJECT_ID('MassiveIncidentType')))
        BEGIN
            CREATE UNIQUE INDEX UI_Massive_incident_type_name on MassiveIncidentType (Name);
        end;