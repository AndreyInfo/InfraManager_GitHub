
CREATE OR ALTER function [dbo].[func_GetFullUserName](@userID uniqueidentifier)
	returns nvarchar(350)
as
begin
	declare @lastName nvarchar(100)
	declare @firstName nvarchar(100)
	declare @patronymic nvarchar(100)
	declare @fullName nvarchar(350)
	declare @removed bit
	set @fullName = ''
	if not(@userID is null)
	begin
		select 	@lastName = isnull(u.[Фамилия],''),
			@firstName = isnull(u.[Имя],''),
			@patronymic = isnull(u.[Отчество],''),
			@removed = isnull(u.Removed, 0)
		from dbo.[Пользователи] u WITH(NOLOCK)
		where u.[IMObjID] = @userID
		if len(@lastName) != 0
		begin
			set @fullName = @lastName
			if len(@firstName) != 0
			begin
				set @fullName = @fullName + ' ' + @firstName
				if len(@patronymic) != 0
					set @fullName = @fullName + ' ' + @patronymic
			end
		end
		if @removed = 1
			set @fullName = '[УДАЛЕН] ' + @fullName
	end
	return @fullName
end