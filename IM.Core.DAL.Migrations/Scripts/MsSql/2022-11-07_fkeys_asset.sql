IF (OBJECT_ID(N'dbo.FK_Asset_ServiceCenter', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Asset] WITH NOCHECK
        ADD CONSTRAINT [FK_Asset_ServiceCenter]
        FOREIGN KEY ([ServiceCenterID])
        REFERENCES [dbo].[Supplier] ([SupplierID])

    ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_ServiceCenter]
END
GO

IF (OBJECT_ID(N'dbo.FK_Asset_ServiceContract', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Asset] WITH NOCHECK
        ADD CONSTRAINT [FK_Asset_ServiceContract]
        FOREIGN KEY ([ServiceContractID])
        REFERENCES [dbo].[ServiceContract] ([ServiceContractID])

    ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_ServiceContract]
END
GO

IF (OBJECT_ID(N'dbo.FK_Asset_Supplier', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[Asset] WITH NOCHECK
        ADD CONSTRAINT [FK_Asset_Supplier]
        FOREIGN KEY ([SupplierID])
        REFERENCES [dbo].[Supplier] ([SupplierID])

    ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Supplier]
END
GO