if not exists (select 1 from class where ClassID = 901)
	insert into class
		values (901, 'Причина отклонения от графика')