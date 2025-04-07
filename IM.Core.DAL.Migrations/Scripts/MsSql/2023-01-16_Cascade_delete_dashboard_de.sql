if EXISTS (SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					WHERE C.TABLE_NAME = 'DashboardDE'
						AND C.CONSTRAINT_SCHEMA = 'dbo' 
						AND C.CONSTRAINT_NAME = 'FK_DashboardDE_Dashboard')
BEGIN 
ALTER TABLE [dbo].[DashboardDE]
drop CONSTRAINT FK_DashboardDE_Dashboard;
END

IF (OBJECT_ID(N'dbo.FK_DashboardDE_Dashboard', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[DashboardDE] WITH NOCHECK
        ADD CONSTRAINT [FK_DashboardDE_Dashboard]
        FOREIGN KEY ([DashboardID])
        REFERENCES [dbo].[Dashboard] ([ID])
		ON DELETE CASCADE ON UPDATE NO ACTION;
	
    ALTER TABLE [dbo].[DashboardDE] CHECK CONSTRAINT [FK_DashboardDE_Dashboard]
END