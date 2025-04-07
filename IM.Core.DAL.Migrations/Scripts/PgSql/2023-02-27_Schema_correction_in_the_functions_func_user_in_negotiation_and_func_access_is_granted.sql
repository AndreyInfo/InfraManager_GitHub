CREATE OR REPLACE FUNCTION im.func_user_in_negotiation(IN _object_id uuid,IN _object_class_id integer,IN _user_id uuid)
    RETURNS boolean
    LANGUAGE 'plpgsql'
    VOLATILE
    PARALLEL UNSAFE
    COST 100
    
AS $$
 
declare
	_now timestamp(3);
begin

	_now := now() at time zone 'utc';

	if 303 not in (select ro.operation_id from im.user_role ur join im.role_operation ro on ur.role_id = ro.role_id where ur.user_id = _user_id) then
		return false;
	end if;

	if exists(
		select 1
		from im.negotiation_user nu
		join im.negotiation n on n.ID = nu.negotiation_id		
		where n.object_id = _object_id
			and n.object_class_id = _object_class_id
			and (
				nu.user_id = _user_id
					or _user_id in (select du.child_user_id from im.deputy_user du where du.parent_user_id = nu.user_id 
						and du.utc_data_deputy_with <= _now
						and du.utc_data_deputy_by >= _now))) then
		return true;
	end if;

	return false;

end;
$$;