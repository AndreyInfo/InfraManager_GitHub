CREATE SEQUENCE IF NOT EXISTS active_port_id_seq;

SELECT SETVAL('active_port_id_seq', (
  SELECT max(identificator) FROM active_port)
);

ALTER TABLE active_port
ALTER COLUMN identificator
SET DEFAULT nextval('active_port_id_seq'::regclass);

ALTER SEQUENCE active_port_id_seq
OWNED BY active_port.identificator;