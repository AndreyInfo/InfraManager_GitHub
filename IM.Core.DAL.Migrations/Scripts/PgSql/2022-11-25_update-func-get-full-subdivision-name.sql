CREATE OR REPLACE FUNCTION im.func_get_full_subdivision_name(
	_subdivisionid uuid)
    RETURNS character varying
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $$
declare
	_fullSubdivisionName varchar(2000);
	_parentSubdivisionID UUID;
	_subdivisionName varchar(255);
begin
	_fullSubdivisionName = '';
	if _subdivisionid is NULL then
		return _fullSubdivisionName;
	end if;
	--
	select into  _fullSubdivisionName, _parentSubdivisionID  name, department_id
	from department 
	where identificator = _subdivisionid;
	while not(_parentSubdivisionID is null) loop
	
		select into  _subdivisionName, _parentSubdivisionID  name, department_id
		from department
		where identificator = _parentSubdivisionID;
		_fullSubdivisionName = _subdivisionName || ' \ ' || _fullSubdivisionName;
	end loop;
	return _fullSubdivisionName;
end;
$$;