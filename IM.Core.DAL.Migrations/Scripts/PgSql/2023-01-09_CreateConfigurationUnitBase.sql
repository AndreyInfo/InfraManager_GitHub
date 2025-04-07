DO $$
    BEGIN

    CREATE SEQUENCE if not exists df_configuration_unit_base_seq AS int
    START WITH 1
    MINVALUE 1
    INCREMENT BY 1;

    CREATE TABLE if not exists im.configuration_unit_base(
	        id uuid not null default(gen_random_uuid()),
            number int not null CONSTRAINT DF_CONFIGURATION_UNIT_BASE_NUMBER default nextval('df_configuration_unit_base_seq'),
            name varchar(250) not null,
            description varchar(250) null,
            note varchar(500) null,
            external_id varchar(250) null,
            tags varchar(250) null,
            created_by uuid null,
            date_received timestamp(3) null,
            product_catalog_type_id uuid not null,
            life_cycle_state_id uuid null,
            infrastructure_segment_id uuid null,
            criticality_id uuid null,
            date_changed timestamp(3) null,
            changed_by uuid null,
            date_last_inquired timestamp(3) null,
            date_annulated timestamp(3) null,
            organization_item_id uuid null,
            organization_item_class_id int null,
            owner_id uuid null,
            client_id uuid null,
            configuration_unit_scheme_id uuid null,
		    CONSTRAINT pk_configuration_unit_base PRIMARY KEY (id),
		    CONSTRAINT fk_configuration_unit_base_criticality FOREIGN KEY (criticality_id) REFERENCES im.criticality(id),
		    CONSTRAINT fk_configuration_unit_base_infrastructure_segment FOREIGN KEY (infrastructure_segment_id) REFERENCES im.infrastructure_segment(id),
		    CONSTRAINT fk_configuration_unit_base_product_catalog_type FOREIGN KEY (product_catalog_type_id) REFERENCES im.product_catalog_type(id),
		    CONSTRAINT fk_configuration_unit_base_life_cycle_state FOREIGN KEY (life_cycle_state_id) REFERENCES im.life_cycle_state(id)
		    );
    END
$$;