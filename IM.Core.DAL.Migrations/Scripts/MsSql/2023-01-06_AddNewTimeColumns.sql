IF COL_LENGTH('dbo.CalendarWorkScheduleDefault', 'ExclusionTimeStart') IS NOT NULL
BEGIN
    EXEC sp_rename 'dbo.CalendarWorkScheduleDefault.ExclusionTimeStart', 'DinnerTimeStart';
END

IF COL_LENGTH('dbo.CalendarWorkScheduleDefault', 'DinnerTimeEnd') IS NULL
BEGIN
	ALTER TABLE [CalendarWorkScheduleDefault]
	ADD DinnerTimeEnd SMALLDATETIME NOT NULL DEFAULT GETDATE()
END

IF COL_LENGTH('dbo.CalendarWorkScheduleDefault', 'TimeEnd') IS NULL
BEGIN
	ALTER TABLE [CalendarWorkScheduleDefault]
	ADD TimeEnd SMALLDATETIME NOT NULL DEFAULT GETDATE();
END