
IF (EXISTS (SELECT *
                 FROM INFORMATION_SCHEMA.TABLES
                 WHERE TABLE_SCHEMA = 'dbo'
                 AND  TABLE_NAME = 'CalendarWorkScheduleDefault'))
BEGIN
    ALTER TABLE [CalendarWorkScheduleDefault] ALTER COLUMN [DinnerTimeEnd] smalldatetime NULL
END