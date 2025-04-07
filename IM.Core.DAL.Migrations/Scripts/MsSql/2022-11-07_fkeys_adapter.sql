IF (OBJECT_ID(N'dbo.FK_Adapter_Type', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Adapter] WITH NOCHECK
        ADD CONSTRAINT [FK_Adapter_Type]
        FOREIGN KEY([AdapterTypeID])
        REFERENCES [dbo].[AdapterType] ([AdapterTypeID])

    ALTER TABLE [dbo].[Adapter] CHECK CONSTRAINT [FK_Adapter_Type]
END
GO

IF (OBJECT_ID(N'dbo.FK_Adapter_TerminalDevice', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Adapter] WITH NOCHECK
        ADD CONSTRAINT [FK_Adapter_TerminalDevice]
        FOREIGN KEY ([TerminalDeviceID])
        REFERENCES [dbo].[Оконечное оборудование] ([Идентификатор])

    ALTER TABLE [dbo].[Adapter] CHECK CONSTRAINT [FK_Adapter_TerminalDevice]
END
GO

IF (OBJECT_ID(N'dbo.FK_Adapter_NetworkDevice', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Adapter] WITH NOCHECK
        ADD CONSTRAINT [FK_Adapter_NetworkDevice]
        FOREIGN KEY ([NetworkDeviceID])
        REFERENCES [dbo].[Активное устройство] ([Идентификатор])
    
    ALTER TABLE [dbo].[Adapter] CHECK CONSTRAINT [FK_Adapter_NetworkDevice]
END
GO

IF (OBJECT_ID(N'dbo.FK_Adapter_Room', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Adapter] WITH NOCHECK
        ADD CONSTRAINT [FK_Adapter_Room]
        FOREIGN KEY ([RoomID])
        REFERENCES [dbo].[Комната] ([Идентификатор])

    ALTER TABLE [dbo].[Adapter] CHECK CONSTRAINT [FK_Adapter_Room]
END
GO

IF (OBJECT_ID(N'dbo.FK_Adapter_SlotType', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Adapter] WITH NOCHECK
        ADD CONSTRAINT [FK_Adapter_SlotType]
        FOREIGN KEY ([SlotTypeID])
        REFERENCES [dbo].[Тип слота] ([Идентификатор])
    
    ALTER TABLE [dbo].[Adapter] CHECK CONSTRAINT [FK_Adapter_SlotType]
END
GO