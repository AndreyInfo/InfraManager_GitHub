IF Exists(select 1 from dbo.Class where ClassID = 903)
	update dbo.Class set name = 'Форма' where ClassID = 903
	update dbo.Operation set Description = 'Операция позволяет опубликовывать объект Форма для его дальнейшего применения.' where ID = 1111