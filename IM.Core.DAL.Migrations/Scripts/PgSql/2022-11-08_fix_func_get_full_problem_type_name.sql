create or replace function func_get_full_problem_type_name(_problemtypeid uuid) returns character varying
    language plpgsql
as
$$
declare
    _retval   varchar(2000);
	_Name     varchar(250);
	_parentID UUID;
begin
_retval = '';
	if _problemtypeid is null then
		return _retval;
end if;
	--
	--
select into _retval, _parentID name, parent_problem_type_id from problem_type where id = _problemtypeid;
while not(_parentID is null) loop
select into _Name, _parentID name, parent_problem_type_id from problem_type where id = _parentID;
_retval = _Name || ' \ ' || _retval;
end loop;
	--
	return _retval;
end;
$$;