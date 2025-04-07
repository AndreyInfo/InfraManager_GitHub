IF (OBJECT_ID(N'dbo.FK_TerminalDevice_Room', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Оконечное оборудование] WITH NOCHECK
        ADD CONSTRAINT [FK_TerminalDevice_Room]
        FOREIGN KEY ([ИД комнаты])
        REFERENCES [dbo].[Комната] ([Идентификатор])

    ALTER TABLE [dbo].[Оконечное оборудование] CHECK CONSTRAINT [FK_TerminalDevice_Room]
END
GO