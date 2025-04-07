if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_CalendarWorkSchedule_Number_Shift' and object_id = OBJECT_ID('CalendarWorkScheduleShift')))
BEGIN
	CREATE UNIQUE INDEX UI_CalendarWorkSchedule_Number_Shift on CalendarWorkScheduleShift (CalendarWorkScheduleID, Number);
end;

IF NOT EXISTS (SELECT 1
	FROM sys.objects
	WHERE name ='DF_CalendarWorkScheduleShift')
BEGIN 
	ALTER TABLE [CalendarWorkScheduleShift]
		ADD CONSTRAINT DF_CalendarWorkScheduleShift DEFAULT NEWID() for ID;
END;