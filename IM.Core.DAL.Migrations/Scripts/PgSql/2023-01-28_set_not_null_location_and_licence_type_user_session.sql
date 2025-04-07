DO $$
BEGIN
  IF EXISTS(SELECT *
    FROM information_schema.columns
    WHERE table_name='user_session' and column_name='location')
  THEN
      alter table user_session alter column location set not null;
  END IF;
  
  IF EXISTS(SELECT *
    FROM information_schema.columns
    WHERE table_name='user_session' and column_name='licence_type')
  THEN
	  alter table user_session alter column licence_type set not null;
  END IF;
END $$;