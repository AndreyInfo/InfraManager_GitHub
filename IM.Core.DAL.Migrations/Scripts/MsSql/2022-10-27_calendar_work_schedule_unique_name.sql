if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_CalendarWorkSchedule_UniqueName' and object_id = OBJECT_ID('CalendarWorkSchedule')))
BEGIN
	CREATE UNIQUE INDEX UI_CalendarWorkSchedule_UniqueName on CalendarWorkSchedule (Name)
end;