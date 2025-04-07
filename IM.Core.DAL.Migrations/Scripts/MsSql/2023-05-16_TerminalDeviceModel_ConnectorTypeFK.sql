IF (OBJECT_ID(N'dbo.FK_TerminalDeviceModel_ConnectorType', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Типы оконечного оборудования] WITH NOCHECK
        ADD CONSTRAINT [FK_TerminalDeviceModel_ConnectorType]
        FOREIGN KEY ([ИД разъема])
        REFERENCES [dbo].[Виды разъемов] ([Идентификатор]);

    ALTER TABLE [dbo].[Типы оконечного оборудования] CHECK CONSTRAINT FK_TerminalDeviceModel_ConnectorType;
END
GO
