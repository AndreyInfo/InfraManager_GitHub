IF NOT EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'CurrentExecutingScheduleID' AND Object_ID = Object_ID(N'dbo.ScheduleTask'))
BEGIN
    alter table [dbo].[ScheduleTask] ADD CurrentExecutingScheduleID uniqueidentifier null;
	alter table [dbo].[ScheduleTask] ADD CONSTRAINT FK_Schedule_CurrentExecutingScheduleID FOREIGN KEY(CurrentExecutingScheduleID) REFERENCES [dbo].[Schedule]([ID])
END
go