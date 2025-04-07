if not exists (select * from Setting where id = 133)
begin
	insert into Setting 
	values (133, 0x20, null);
end;
