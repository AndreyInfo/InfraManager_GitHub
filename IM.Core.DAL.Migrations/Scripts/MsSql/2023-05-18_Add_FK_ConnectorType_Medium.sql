IF (OBJECT_ID(N'dbo.FK_ConnectorType_Medium', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Виды разъемов] WITH NOCHECK
        ADD CONSTRAINT [FK_ConnectorType_Medium]
        FOREIGN KEY ([MediumID])
        REFERENCES [dbo].[Виды среды передачи] ([Идентификатор]);

    ALTER TABLE [dbo].[Виды разъемов] CHECK CONSTRAINT FK_ConnectorType_Medium;
END
GO