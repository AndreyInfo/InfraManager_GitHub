if not exists (select 1 from RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001'
												 and OperationID = 820005)
BEGIN
	INSERT INTO RoleOperation
	VALUES ('00000000-0000-0000-0000-000000000001', 820005)
END 


if not exists (select 1 from RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001'
												 and OperationID = 820006)
BEGIN
	INSERT INTO RoleOperation
	VALUES ('00000000-0000-0000-0000-000000000001', 820006)
END 



if not exists (select 1 from RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001'
												 and OperationID = 820007)
BEGIN
	INSERT INTO RoleOperation
	VALUES ('00000000-0000-0000-0000-000000000001', 820007)
END 

if not exists (select 1 from RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001'
												 and OperationID = 820008)
BEGIN
	INSERT INTO RoleOperation
	VALUES ('00000000-0000-0000-0000-000000000001', 820008)
END 