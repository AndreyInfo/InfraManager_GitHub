if NOT EXISTS (SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					WHERE C.TABLE_NAME = 'TechnologyCompatibility'
						AND C.CONSTRAINT_SCHEMA = 'dbo' 
						AND C.CONSTRAINT_NAME = 'PK_TechnologyCompatibility')
BEGIN 
	ALTER TABLE TechnologyCompatibility
		ADD CONSTRAINT PK_TechnologyCompatibility PRIMARY KEY CLUSTERED (TechID1, TechID2);
END 

IF (OBJECT_ID(N'dbo.FK_From_TechnologyCompatibility', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].TechnologyCompatibility WITH NOCHECK
        ADD CONSTRAINT [FK_From_TechnologyCompatibility]
        FOREIGN KEY (TechID1)
        REFERENCES [dbo].[Виды технологий] ([Идентификатор])
		ON DELETE CASCADE;

    ALTER TABLE [dbo].TechnologyCompatibility CHECK CONSTRAINT FK_From_TechnologyCompatibility;
END
GO

IF (OBJECT_ID(N'dbo.FK_To_TechnologyCompatibility', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].TechnologyCompatibility WITH NOCHECK
        ADD CONSTRAINT [FK_To_TechnologyCompatibility]
        FOREIGN KEY (TechID2)
        REFERENCES [dbo].[Виды технологий] ([Идентификатор]);

    ALTER TABLE [dbo].TechnologyCompatibility CHECK CONSTRAINT FK_To_TechnologyCompatibility;
END
GO