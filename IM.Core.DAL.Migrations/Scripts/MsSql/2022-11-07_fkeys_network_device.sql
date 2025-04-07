IF (OBJECT_ID(N'dbo.FK_Активное устройство_Комната', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Активное устройство] WITH NOCHECK
        ADD CONSTRAINT [FK_Активное устройство_Комната]
        FOREIGN KEY ([RoomID])
        REFERENCES [dbo].[Комната] ([Идентификатор])

    ALTER TABLE [dbo].[Активное устройство] CHECK CONSTRAINT [FK_Активное устройство_Комната]
END
GO