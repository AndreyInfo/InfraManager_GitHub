if not EXISTS (select 1 from dbo.Role where ID = '00000000-0000-0000-0000-000000000001')
	insert into dbo.Role (ID,Name,Note) 
		values ('00000000-0000-0000-0000-000000000001',
			    'Администратор системы',
				'Администратор системы имеет право на изменение параметров системы и распределение ролей и прав.')

update dbo.RoleOperation set RoleID = '00000000-0000-0000-0000-000000000001' where RoleID = '00000000-0000-0000-0000-000000000000'

update dbo.RoleLifeCycleStateOperation set RoleID = '00000000-0000-0000-0000-000000000001' where RoleID = '00000000-0000-0000-0000-000000000000'

update dbo.UserRole set RoleID = '00000000-0000-0000-0000-000000000001' where RoleID = '00000000-0000-0000-0000-000000000000'

delete from dbo.Role where id = '00000000-0000-0000-0000-000000000000'