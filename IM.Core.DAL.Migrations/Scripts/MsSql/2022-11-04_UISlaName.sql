if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_SLA_Name' and object_id = OBJECT_ID('SLA')))
        BEGIN
            CREATE UNIQUE INDEX UI_SLA_Name on SLA (Name);
        end;