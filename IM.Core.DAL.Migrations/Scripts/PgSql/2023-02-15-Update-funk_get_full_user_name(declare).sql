CREATE OR REPLACE FUNCTION im.func_get_full_user_name(IN _userid uuid)
    RETURNS character varying
    LANGUAGE 'plpgsql'
    VOLATILE
    PARALLEL UNSAFE
    COST 100
    
AS $$
declare _lastName varchar(100);
	_firstName varchar(100);
	_patronymic varchar(100);
	_fullName varchar(350);
	_removed boolean;
begin
	_fullName = '';
	if not(_userid is null) then
		select into	_lastName,_firstName,_patronymic,_removed
			COALESCE(u.surname,''),
			COALESCE(u.name,''),
			COALESCE(u.patronymic,''),
			COALESCE(u.removed, false)
		from users u
		where u.im_obj_id = _userid;
		if char_length(_lastName) != 0 then
			_fullName = _lastName;
			if char_length(_firstName) != 0 then
				_fullName = _fullName || ' ' || _firstName;
				if char_length(_patronymic) != 0 then
					_fullName = _fullName || ' '|| _patronymic;
				end if;
			end if;
		end if;
		if _removed then
			_fullName = '[УДАЛЕН] ' || _fullName;
		end if;
	end if;
	return _fullName;
end;
$$;