IF 'Schedule' NOT IN (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
begin
	CREATE TABLE [dbo].[Schedule](
		[ID] [uniqueidentifier] NOT NULL,
		[StartAt] [datetime] NULL,
		[Interval] [int] NULL,
		[FinishAt] [datetime] NULL,
		[ScheduleType] [int] NOT NULL,
		[DaysOfWeek] [nvarchar](50) NULL,
		[Months] [nvarchar](50) NULL,
		[ScheduleTaskEntityID] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_ScheduleTask] FOREIGN KEY([ScheduleTaskEntityID])
	REFERENCES [dbo].[ScheduleTask] ([ID])
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK_Schedule_ScheduleTask]
end
GO