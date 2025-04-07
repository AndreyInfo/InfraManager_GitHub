CREATE SEQUENCE IF NOT EXISTS adapter_int_id_seq;

SELECT SETVAL('adapter_int_id_seq', (
  SELECT max(int_id) FROM adapter)
);

ALTER TABLE adapter
ALTER COLUMN int_id
SET DEFAULT nextval('adapter_int_id_seq'::regclass);

ALTER SEQUENCE adapter_int_id_seq
OWNED BY adapter.int_id;