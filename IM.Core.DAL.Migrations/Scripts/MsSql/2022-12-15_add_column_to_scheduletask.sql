IF NOT EXISTS (SELECT * FROM syscolumns
  WHERE ID=OBJECT_ID('[dbo].[ScheduleTask]') AND NAME='LastStartAt')
  ALTER TABLE [dbo].[ScheduleTask]
  ADD LastStartAt [datetime] NULL
GO