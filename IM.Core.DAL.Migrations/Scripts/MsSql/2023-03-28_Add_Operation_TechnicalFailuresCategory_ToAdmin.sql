if not exists(select 1 from RoleOperation 
	where RoleID = '00000000-0000-0000-0000-000000000001'
			AND OperationID = 1122)
BEGIN
	insert into RoleOperation
		values ('00000000-0000-0000-0000-000000000001', 1122);
END
GO