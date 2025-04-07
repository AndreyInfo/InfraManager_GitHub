CREATE or replace FUNCTION im.func_get_date(IN _utcdate timestamp without time zone)
    RETURNS timestamp without time zone
    LANGUAGE 'plpgsql'
    
AS $$
begin
	if _utcdate is null then
		return null;
	end if;
	
	-- convert utc date to local date (server time zone)
	return _utcdate + DATE_PART('hour', now() - timezone('utc', now())) * interval '1 hour';
end;        
$$;
