ALTER function [dbo].[func_DashboardTreeItemIsVisible] (@classID int, @id uniqueidentifier, @userID uniqueidentifier)
	returns bit
as
begin
	if @userID is null or @id is null or @classID is null
		return 0
	--
	if @classID = 364 -- DE panel
	begin
		--Admin
		if exists(select * from dbo.UserRole WITH(NOLOCK) where UserID = @userID and RoleID = '00000000-0000-0000-0000-000000000001')
			return 1
		if exists (select * from dbo.AccessPermission ap WITH(NOLOCK) 
			inner join dbo.OrganizationItemGroup o WITH(NOLOCK) on o.ID = ap.ID
			where ap.ObjectID = @id and (ap.Properties = 1 or ap.[Add] = 1 or ap.[Delete] = 1 or ap.[Update] = 1 or ap.AccessManage = 1)
			and dbo.func_UserInOrganizationItem(o.OrganizationItemClassID, o.OrganizationItemID, @userID) = 1)
			return 1
	end
	--
	if @classID = 153 -- folder
	begin
		if exists(select * from dbo.Dashboard d WITH(NOLOCK) where d.DashboardFolderID = @id and d.DashboardClassID = 364 and dbo.func_DashboardTreeItemIsVisible(364, d.ID, @userID) = 1)
			return 1
		if exists(select * from dbo.DashboardFolder f WITH(NOLOCK) where f.ParentDashboardFolderID = @id and dbo.func_DashboardTreeItemIsVisible(153, f.ID, @userID) = 1)
			return 1
	end
	--
	return 0
end
GO