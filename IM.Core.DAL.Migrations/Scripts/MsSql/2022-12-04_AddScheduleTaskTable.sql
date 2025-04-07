IF 'ScheduleTask' NOT IN (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
	CREATE TABLE [dbo].[ScheduleTask](
		[ID] [uniqueidentifier] NOT NULL,
		[Name] [nchar](250) NOT NULL,
		[TaskType] [int] NOT NULL,
		[TaskSettingID] [uniqueidentifier] NULL,
		[TaskSettingName] [nchar](250) NULL,
		[Note] [nchar](1000) NULL,
		[IsEnabled] [bit] NOT NULL,
		[UseAccount] [bit] NOT NULL,
		[NextRunAt] [datetime] NULL,
		[FinishRunAt] [datetime] NULL,
		[TaskState] [int] NOT NULL,
		[CredentialID] [int] NULL,
	 CONSTRAINT [PK_ScheduleTask] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF 'ScheduleTask' NOT IN (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
	ALTER TABLE [dbo].[ScheduleTask]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleTask_UserAccount] FOREIGN KEY([CredentialID])
	REFERENCES [dbo].[UserAccount] ([ID])
GO

IF 'ScheduleTask' NOT IN (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
	ALTER TABLE [dbo].[ScheduleTask] CHECK CONSTRAINT [FK_ScheduleTask_UserAccount]
GO
