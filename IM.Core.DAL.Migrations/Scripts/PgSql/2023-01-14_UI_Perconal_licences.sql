DO
$$
BEGIN
	if exists (select constraint_name 
                    from information_schema.table_constraints 
                    where table_name = 'user_personal_licence' and constraint_type = 'PRIMARY KEY' and constraint_name = 'pk_user_personal_session') 
	then 
	 	ALTER TABLE user_personal_licence DROP constraint pk_user_personal_session;
	end if;
	--
    if not exists (SELECT column_name
                          FROM information_schema.columns
                          WHERE table_name='user_personal_licence' 
                                and column_name='id')
    then 
        create sequence if not exists user_personal_licence_id start 1 increment 1;

        ALTER TABLE IF EXISTS im.user_personal_licence
            ADD COLUMN id int NOT NULL default(nextval('user_personal_licence_id'));
        --
		if not exists (select constraint_name 
                    from information_schema.table_constraints 
                    where table_name = 'user_personal_licence' and constraint_type = 'PRIMARY KEY' and constraint_name = 'pk_user_personal_licence') 
		then 
			alter table im.user_personal_licence add constraint pk_user_personal_licence primary key (id);
		end if;
		--
    end if;
    --
	CREATE UNIQUE INDEX 
    	if not exists ui_personal_licence_user_id on user_personal_licence(user_id);
END $$;