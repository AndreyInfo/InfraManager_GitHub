DO $$
    BEGIN	
	
	create sequence if not exists job_title_id start 1;


if EXISTS (SELECT FROM information_schema.tables WHERE  table_schema = 'im' AND table_name   = 'positions') then

	if exists(select from im.positions where identificator > 0) then 
		perform setval('job_title_id', (select max(identificator) from im.positions) );
	end if;
	
	alter table im.positions alter column identificator set default nextval('job_title_id');
	
	alter table im.positions alter column name set not null;
	
	alter table im.positions alter column im_obj_id set not null;
	
	alter table im.positions drop constraint if exists uk_job_title_im_obj_id;
	 
	alter table im.positions add constraint uk_job_title_im_obj_id unique (im_obj_id);
	
	alter table im.positions drop constraint if exists uk_job_title_name;
	 
	alter table im.positions add constraint uk_job_title_name unique (name);
end if;
	END
$$