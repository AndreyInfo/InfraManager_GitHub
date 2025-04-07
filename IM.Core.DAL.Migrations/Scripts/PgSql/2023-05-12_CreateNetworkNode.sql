DO $$
    BEGIN

    CREATE TABLE if not exists im.network_node(
	        id uuid not null default(gen_random_uuid()),
            ip_address varchar(15) not null,
	        ip_mask varchar(15) not null,
            network_device_id int null,
            terminal_device_id int null,
            device_application_id uuid null,
		    CONSTRAINT pk_network_node PRIMARY KEY (id),
		    CONSTRAINT fk_network_node_configuration_unit_base_id FOREIGN KEY (id)
            REFERENCES im.configuration_unit_base (id) MATCH SIMPLE
            ON DELETE CASCADE,
            CONSTRAINT fk_network_node_network_device FOREIGN KEY (network_device_id)
            REFERENCES im.active_equipment (identificator),
            CONSTRAINT fk_network_node_terminal_device FOREIGN KEY (terminal_device_id)
            REFERENCES im.terminal_equipment (identificator),
            CONSTRAINT fk_network_node_device_application FOREIGN KEY (device_application_id)
            REFERENCES im.device_application (id)
		    );
    END
$$;
