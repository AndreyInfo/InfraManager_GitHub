CREATE SEQUENCE IF NOT EXISTS peripheral_int_id_seq;

SELECT SETVAL('peripheral_int_id_seq', (
  SELECT max(int_id) FROM peripheral)
);

ALTER TABLE peripheral
ALTER COLUMN int_id
SET DEFAULT nextval('peripheral_int_id_seq'::regclass);

ALTER SEQUENCE peripheral_int_id_seq
OWNED BY peripheral.int_id;