if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_CalendarHoliday_Name' and object_id = OBJECT_ID('CalendarHoliday')))
        BEGIN
            CREATE UNIQUE INDEX UI_CalendarHoliday_Name on CalendarHoliday (Name);
        end;