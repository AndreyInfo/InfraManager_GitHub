CREATE TABLE if not exists im.port_adapter (
	id uuid NOT NULL default(gen_random_uuid()),
	object_id uuid NOT NULL,
	"number" int4 NOT NULL,
	jack_type_id int4 NOT NULL,
	technology_type_id int4 NOT NULL,
	port_address CHARACTER VARYING(250) NULL,
	note CHARACTER VARYING(250) NULL,
	CONSTRAINT pk_port_adapter PRIMARY KEY (id),
	constraint fk_port_adapter_jack_type foreign key (jack_type_id) references connector_kinds(identificator),
	constraint fk_port_adapter_technology_type foreign key (technology_type_id) references technology_kinds(identificator)
);