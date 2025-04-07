DO $$                  
    BEGIN 
		if exists(select 1 from im.class where class_id = 903) then
				update im.class set name = 'Форма' where class_id = 903;
				update im.operation set description = 'Операция позволяет опубликовывать объект Форма для его дальнейшего применения.' where id = 1111;
		end if;
	end
 $$ ;