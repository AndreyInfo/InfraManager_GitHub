IF NOT EXISTS (SELECT 1 -- нет InitiatorID
               FROM information_schema.columns
               WHERE table_name = N'Problem' AND column_name = N'InitiatorID')
BEGIN
    ALTER TABLE [dbo].[Problem] ADD [InitiatorID] uniqueidentifier;
END
GO

IF (OBJECT_ID(N'dbo.FK_Problem_Initiator', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Problem]
        ADD CONSTRAINT [FK_Problem_Initiator]
        FOREIGN KEY ([InitiatorID])
        REFERENCES [dbo].[Пользователи] ([IMObjID])
END
GO


IF NOT EXISTS (SELECT 1 -- нет QueueID
               FROM information_schema.columns
               WHERE table_name = N'Problem' AND column_name = N'QueueID')
BEGIN
    ALTER TABLE [dbo].[Problem] ADD [QueueID] uniqueidentifier;
END
GO

IF (OBJECT_ID(N'dbo.FK_Problem_Queue', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Problem]
        ADD CONSTRAINT [FK_Problem_Queue]
        FOREIGN KEY ([QueueID])
        REFERENCES [dbo].[Queue] ([ID])
END
GO


IF NOT EXISTS (SELECT 1 -- нет ExecutorID
               FROM information_schema.columns
               WHERE table_name = N'Problem' AND column_name = N'ExecutorID')
BEGIN
    ALTER TABLE [dbo].[Problem] ADD [ExecutorID] uniqueidentifier;
END
GO

IF (OBJECT_ID(N'dbo.FK_Problem_Executor', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Problem]
        ADD CONSTRAINT [FK_Problem_Executor]
        FOREIGN KEY ([ExecutorID])
        REFERENCES [dbo].[Пользователи] ([IMObjID])
END
GO


IF NOT EXISTS (SELECT 1 -- нет ServiceID
               FROM information_schema.columns
               WHERE table_name = N'Problem' AND column_name = N'ServiceID')
BEGIN
    ALTER TABLE [dbo].[Problem] ADD [ServiceID] uniqueidentifier;
END
GO

IF (OBJECT_ID(N'dbo.FK_Problem_Service', 'F') IS NULL)
    BEGIN
        ALTER TABLE [dbo].[Problem]
            ADD CONSTRAINT [FK_Problem_Service]
            FOREIGN KEY ([ServiceID])
            REFERENCES [dbo].[Service] ([ID])
    END
GO