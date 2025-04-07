if not exists (select * from Setting where id = 129)
begin
	insert into Setting 
	values (129, 0x00, null);
end;

if not exists (select * from Setting where id = 130)
begin
	insert into Setting 
	values (130, 0x00, null);
end;

if not exists (select * from Setting where id = 131)
begin
	insert into Setting 
	values (131, 0x0000000000000000, null);
end;