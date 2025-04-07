CREATE SEQUENCE if not exists pk_connector_type_seq;
ALTER SEQUENCE pk_connector_type_seq OWNED BY connector_kinds.identificator;

ALTER TABLE connector_kinds
    ALTER COLUMN identificator SET DEFAULT nextval('pk_connector_type_seq');
select  setval('pk_connector_type_seq', mx.mx)
    FROM (SELECT MAX(identificator) + 1 AS mx FROM connector_kinds) mx;
