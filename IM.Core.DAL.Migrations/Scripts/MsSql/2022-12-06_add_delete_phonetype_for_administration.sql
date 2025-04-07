if not exists (select top 1 1 from RoleOperation
					where RoleID = '00000000-0000-0000-0000-000000000001'
							AND OperationID = 1206)
BEGIN
	INSERT RoleOperation 
	VALUES ('00000000-0000-0000-0000-000000000001', 1206);
END