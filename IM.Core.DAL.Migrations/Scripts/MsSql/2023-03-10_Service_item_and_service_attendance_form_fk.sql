IF (OBJECT_ID(N'dbo.FK_ServiceItem_FormID', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[ServiceItem] WITH NOCHECK
        ADD CONSTRAINT [FK_ServiceItem_FormID]
        FOREIGN KEY ([FormID])
        REFERENCES [dbo].[WorkflowActivityForm] ([ID])

    ALTER TABLE [dbo].[ServiceItem] CHECK CONSTRAINT [FK_ServiceItem_FormID]
END
GO

IF (OBJECT_ID(N'dbo.FK_ServiceAttendance_FormID', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[ServiceAttendance] WITH NOCHECK
        ADD CONSTRAINT [FK_ServiceAttendance_FormID]
        FOREIGN KEY ([FormID])
        REFERENCES [dbo].[WorkflowActivityForm] ([ID])

    ALTER TABLE [dbo].[ServiceAttendance] CHECK CONSTRAINT [FK_ServiceAttendance_FormID]
END
GO