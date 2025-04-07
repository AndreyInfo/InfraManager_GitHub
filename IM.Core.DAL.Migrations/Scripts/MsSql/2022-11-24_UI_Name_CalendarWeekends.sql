if (NOT EXISTS(SELECT 0 FROM sys.indexes where name = 'UI_CalendarWeekend_Name' and object_id = OBJECT_ID('[CalendarWeekend]')))
        BEGIN
            CREATE UNIQUE INDEX UI_CalendarWeekend_Name on CalendarWeekend ([Name]);
        end;