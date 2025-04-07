
IF EXISTS ( SELECT 1 FROM sys.columns
					 WHERE Name = N'[ShiftNumber]'
					 AND Object_ID = Object_ID(N'dbo.[CalendarWorkScheduleItem]'))
BEGIN
alter table [CalendarWorkScheduleItem]
	alter column [ShiftNumber] tinyint NULL
END
