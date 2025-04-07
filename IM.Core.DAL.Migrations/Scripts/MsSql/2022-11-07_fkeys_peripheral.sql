IF (OBJECT_ID(N'dbo.FK_Peripheral_Type', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Peripheral] WITH NOCHECK
        ADD CONSTRAINT [FK_Peripheral_Type]
        FOREIGN KEY ([PeripheralTypeID])
        REFERENCES [dbo].[PeripheralType] ([PeripheralTypeID])

    ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_Type]
END
GO

IF (OBJECT_ID(N'dbo.FK_Peripheral_TerminalDevice', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Peripheral] WITH NOCHECK
        ADD CONSTRAINT [FK_Peripheral_TerminalDevice]
        FOREIGN KEY ([TerminalDeviceID])
        REFERENCES [dbo].[Оконечное оборудование] ([Идентификатор])

    ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_TerminalDevice]
END
GO

IF (OBJECT_ID(N'dbo.FK_Peripheral_NetworkDevice', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Peripheral] WITH NOCHECK
        ADD CONSTRAINT [FK_Peripheral_NetworkDevice]
        FOREIGN KEY([NetworkDeviceID])
        REFERENCES [dbo].[Активное устройство] ([Идентификатор])

    ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_NetworkDevice]
END
GO

IF (OBJECT_ID(N'dbo.FK_Peripheral_Room', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Peripheral] WITH NOCHECK
        ADD CONSTRAINT [FK_Peripheral_Room]
        FOREIGN KEY ([RoomID])
        REFERENCES [dbo].[Комната] ([Идентификатор])

    ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_Room]
END
GO