if not exists (select * from dbo.Class where ClassID = 823)
	insert into dbo.Class values(823, 'Массовый инцидент')

if not exists (select * from dbo.Class where ClassID = 824)
	insert into dbo.Class values(824, 'Тип массовых инцидентов')

if not exists (select * from dbo.Class where ClassID = 825)
	insert into dbo.Class values(825, 'Причина массовых инцидентов')