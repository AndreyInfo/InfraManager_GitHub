IF NOT EXISTS (SELECT 1 FROM [dbo].SoftwareLicenceScheme WHERE ID = '00000000-0000-0000-0000-000000000001')
BEGIN
    INSERT INTO [dbo].SoftwareLicenceScheme ([ID], [Name], [Description], [SchemeType], [LicensingObjectType], [IsLinkLicenseToObject], [IsLicenseAllHosts], [IsLinkLicenseToUser], [IsAllowInstallOnVM], [IsLicenceByAccess], [IncreaseForVM], [IsDeleted], [CreatedDate], [UpdatedDate], [IsCanHaveSubLicence], [CompatibilityTypeID]) VALUES
        ('00000000-0000-0000-0000-000000000001', '-', 'Отсутствие лицензии', 1, 4, 0, 0, 0, 0, 0, 0, 0, '20230221 13:00:00', '20230221 13:00:00', 0, NULL)
END;