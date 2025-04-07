IF (OBJECT_ID(N'dbo.FK_TerminalDeviceModel_TechnologyType', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Типы оконечного оборудования] WITH NOCHECK
        ADD CONSTRAINT [FK_TerminalDeviceModel_TechnologyType]
        FOREIGN KEY ([ИД технологии])
        REFERENCES [dbo].[Виды технологий] ([Идентификатор]);

    ALTER TABLE [dbo].[Типы оконечного оборудования] CHECK CONSTRAINT FK_TerminalDeviceModel_TechnologyType;
END
GO