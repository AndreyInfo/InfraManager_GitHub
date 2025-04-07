IF (OBJECT_ID(N'dbo.FK_ProblemType_Form', 'F') IS NULL)
    BEGIN
        ALTER TABLE [dbo].[ProblemType] WITH NOCHECK
            ADD CONSTRAINT [FK_ProblemType_Form]
            FOREIGN KEY ([FormID])
            REFERENCES [dbo].[WorkflowActivityForm] ([ID])

        ALTER TABLE [dbo].[ProblemType] CHECK CONSTRAINT [FK_ProblemType_Form]
    END
GO

IF (OBJECT_ID(N'dbo.FK_WorkOrderTemplate_Form', 'F') IS NULL)
    BEGIN
        ALTER TABLE [dbo].[WorkOrderTemplate] WITH NOCHECK
            ADD CONSTRAINT [FK_WorkOrderTemplate_Form]
            FOREIGN KEY ([FormID])
            REFERENCES [dbo].[WorkflowActivityForm] ([ID])

        ALTER TABLE [dbo].[WorkOrderTemplate] CHECK CONSTRAINT [FK_WorkOrderTemplate_Form]
    END
GO

IF (OBJECT_ID(N'dbo.FK_RFC_Form', 'F') IS NULL)
    BEGIN
        ALTER TABLE [dbo].[RFC] WITH NOCHECK
            ADD CONSTRAINT [FK_RFC_Form]
            FOREIGN KEY ([FormID])
            REFERENCES [dbo].[WorkflowActivityForm] ([ID])

        ALTER TABLE [dbo].[RFC] CHECK CONSTRAINT [FK_RFC_Form]
    END
GO

IF (OBJECT_ID(N'dbo.FK_SLA_Form', 'F') IS NULL)
    BEGIN
        ALTER TABLE [dbo].[SLA] WITH NOCHECK
            ADD CONSTRAINT [FK_SLA_Form]
            FOREIGN KEY ([FormID])
            REFERENCES [dbo].[WorkflowActivityForm] ([ID])

        ALTER TABLE [dbo].[SLA] CHECK CONSTRAINT [FK_SLA_Form]
    END
GO

IF (OBJECT_ID(N'dbo.FK_MassiveIncidentType_Form', 'F') IS NULL)
    BEGIN
        ALTER TABLE [dbo].[MassiveIncidentType] WITH NOCHECK
            ADD CONSTRAINT [FK_MassiveIncidentType_Form]
            FOREIGN KEY ([FormID])
            REFERENCES [dbo].[WorkflowActivityForm] ([ID])

        ALTER TABLE [dbo].[MassiveIncidentType] CHECK CONSTRAINT [FK_MassiveIncidentType_Form]
    END
GO