IF NOT EXISTS(SELECT 1 FROM sys.columns
          WHERE Name = N'ID'
          AND Object_ID = Object_ID(N'dbo.[CalendarWorkScheduleDefault]'))
BEGIN 
	ALTER TABLE [CalendarWorkScheduleDefault]
		ADD ID uniqueidentifier NOT NULL default NEWID();
END

if NOT EXISTS (SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					WHERE C.TABLE_NAME = 'CalendarWorkScheduleDefault'
						AND C.CONSTRAINT_SCHEMA = 'dbo' 
						AND C.CONSTRAINT_NAME = 'PK_CalendarWorkScheduleDefault')
BEGIN 
	ALTER TABLE [CalendarWorkScheduleDefault]
		ADD CONSTRAINT PK_CalendarWorkScheduleDefault PRIMARY KEY CLUSTERED (ID);
END