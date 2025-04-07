IF COL_LENGTH('dbo.CalendarWorkScheduleDefault', 'TimeEnd') IS NOT NULL
BEGIN
	declare @schema_name nvarchar(256) = N'dbo';
	declare @table_name nvarchar(256) = N'CalendarWorkScheduleDefault';
	declare @col_name nvarchar(256) = N'TimeEnd';
	declare @Command  nvarchar(1000);
	
	select @Command = 'ALTER TABLE ' + @schema_name + '.[' + @table_name + '] DROP CONSTRAINT ' + d.name
		from sys.tables t
		join sys.default_constraints d on d.parent_object_id = t.object_id
		join sys.columns c on c.object_id = t.object_id and c.column_id = d.parent_column_id
	where t.name = @table_name
		and t.schema_id = schema_id(@schema_name)
		and c.name = @col_name;

    execute (@Command);

	ALTER TABLE [CalendarWorkScheduleDefault]
	ADD CONSTRAINT DF_TimeEnd DEFAULT '1900-01-01T18:00:00' FOR TimeEnd;

	Update [CalendarWorkScheduleDefault] SET TimeEnd = DATEADD(MINUTE, TimeSpanInMinutes, TimeStart) 
END