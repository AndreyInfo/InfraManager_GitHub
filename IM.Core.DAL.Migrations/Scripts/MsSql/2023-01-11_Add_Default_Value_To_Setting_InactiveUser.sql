if not exists(select * from dbo.Setting where id = 126)
	insert into dbo.Setting (id, Value)  values(126,cast(30 as binary(4)))

if not exists(select * from dbo.Setting where id = 127)
	insert into dbo.Setting (id, Value)  values(127,cast(60 as binary(4)))
	
if not exists(select * from dbo.Setting where id = 128)
	insert into dbo.Setting (id, Value)  values(128,cast(60 as binary(4)))