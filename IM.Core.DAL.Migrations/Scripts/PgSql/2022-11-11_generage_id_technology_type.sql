DO $$
    BEGIN 
        IF NOT EXISTS (SELECT 0 FROM pg_class where relname = 'pk_technology_type_id_seq' )
        THEN
           CREATE SEQUENCE  pk_technology_type_id_seq;
        END IF;

	ALTER SEQUENCE pk_technology_type_id_seq 
              OWNED BY technology_kinds.identificator;
        
	ALTER TABLE technology_kinds 
              ALTER COLUMN identificator SET DEFAULT nextval('pk_technology_type_id_seq');        
        
        
        PERFORM  setval('pk_technology_type_id_seq', mx.mx)
        FROM (SELECT MAX(identificator) + 1 AS mx FROM technology_kinds) mx;
    END 
$$
