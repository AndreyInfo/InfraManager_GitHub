if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Sla_SlaReference' and object_id = OBJECT_ID('SLAReference')))
        BEGIN
            CREATE UNIQUE INDEX UI_Sla_SlaReference on SLAReference (ObjectID, SLAID);
        end;