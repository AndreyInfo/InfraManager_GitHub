IF (OBJECT_ID(N'dbo.FK_RFC_Urgency', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[RFC] WITH NOCHECK
        ADD CONSTRAINT [FK_RFC_Urgency]
        FOREIGN KEY ([UrgencyID])
        REFERENCES [dbo].[Urgency] ([ID])

    ALTER TABLE [dbo].[RFC] CHECK CONSTRAINT [FK_RFC_Urgency]
END
GO

IF (OBJECT_ID(N'dbo.FK_RFC_Influence', 'F') IS NULL)
BEGIN
ALTER TABLE [dbo].[RFC] WITH NOCHECK
    ADD CONSTRAINT [FK_RFC_Influence]
    FOREIGN KEY ([InfluenceID])
    REFERENCES [dbo].[Influence] ([ID])

ALTER TABLE [dbo].[RFC] CHECK CONSTRAINT [FK_RFC_Influence]
END
GO

IF (OBJECT_ID(N'dbo.FK_RFC_Owner', 'F') IS NULL)
BEGIN
ALTER TABLE [dbo].[RFC] WITH NOCHECK
    ADD CONSTRAINT [FK_RFC_Owner]
    FOREIGN KEY ([OwnerID])
    REFERENCES [dbo].[Пользователи] ([IMObjID])

ALTER TABLE [dbo].[RFC] CHECK CONSTRAINT [FK_RFC_Owner]
END
GO

IF (OBJECT_ID(N'dbo.FK_RFC_Initiator', 'F') IS NULL)
BEGIN
ALTER TABLE [dbo].[RFC] WITH NOCHECK
    ADD CONSTRAINT [FK_RFC_Initiator]
    FOREIGN KEY ([InitiatorID])
    REFERENCES [dbo].[Пользователи] ([IMObjID])

ALTER TABLE [dbo].[RFC] CHECK CONSTRAINT [FK_RFC_Initiator]
END
GO