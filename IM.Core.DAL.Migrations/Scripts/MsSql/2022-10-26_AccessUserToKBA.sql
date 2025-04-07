/****** Object:  UserDefinedFunction [dbo].[func_AccessUserToKBA]    Script Date: 25.10.2022 11:21:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if object_id('[dbo].[func_AccessUserToKBA]') is null
	exec('CREATE function [dbo].[func_AccessUserToKBA] (@idAccess uniqueidentifier, @kbaID uniqueidentifier, @Object uniqueidentifier, @classID int, @WithQueue bit, @CurUserID uniqueidentifier) returns bit as begin return 0; end')
go

ALTER function [dbo].[func_AccessUserToKBA] (@idAccess uniqueidentifier, @kbaID uniqueidentifier, @Object uniqueidentifier, @classID int, @WithQueue bit, @CurUserID uniqueidentifier)
	returns bit
as
begin
	declare @SubdivisionID uniqueidentifier;
	--Admin
	if exists(select * from dbo.UserRole with(nolock) where UserID = @CurUserID and RoleID = '00000000-0000-0000-0000-000000000001')
		return 1
	--Видно всем
	else if @idAccess = '00000000-0000-0000-0000-000000000000'
		return 1
	--Проверка ИТ сотрудников
	else if @idAccess = '00000000-0000-0000-0000-000000000001'
		begin
		if exists(select * from dbo.UserRole with(nolock) where UserID = @CurUserID)
			return 1
		else 
			begin
				select @SubdivisionID = [ИД подразделения] from dbo.Пользователи where IMObjID = @CurUserID
				IF EXISTS
				(			
					SELECT * FROM [dbo].[GetParentsSubdivision](@SubdivisionID) where IT=1
				)
					return 1
				return 0;
			end
		end
	else if @idAccess = '00000000-0000-0000-0000-000000000002'		
		BEGIN
			if (@classID = 9)
				begin
					if(@Object = @CurUserID)
						return 1;
				end
			else if (@classID = 722)
				begin
					if EXISTS(select * from dbo.QueueUser where UserID = @CurUserID AND QueueID = @Object)
						return 1;
				end
			else if (@classID = 101)
				begin
					if EXISTS(
						select*from dbo.Пользователи p
						left join dbo.Подразделение о ON p.[ИД подразделения] = о.Идентификатор
						where p.IMObjID = @CurUserID and о.[ИД организации] = @Object)
						return 1;
				end
			else if (@classID = 102)
				begin
					select @SubdivisionID = [ИД подразделения] from dbo.Пользователи where IMObjID = @CurUserID
					if(@WithQueue = 0 AND @SubdivisionID = @Object)
						return 1;
					else if(@WithQueue = 1)
						IF EXISTS(SELECT * FROM [dbo].[GetParentsSubdivision](@SubdivisionID) where Идентификатор = @Object)
							return 1;
				end
			return 0;			
		END		
	--
	return 1 --haven't access settings
end
