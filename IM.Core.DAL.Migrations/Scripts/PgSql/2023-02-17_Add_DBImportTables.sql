
create table if not exists im.uidb_configurations
(
    id uuid primary key default gen_random_uuid(),
    name varchar(255) not null,
    note varchar(500) not null,
    organization_table_name varchar(50),
    subdivision_table_name varchar(50),
    user_table_name varchar(50),
    unique(name)
);

create table if not exists im.uidb_settings
(
    id uuid primary key references ui_setting(id),
    db_configuration_id uuid references uidb_configurations(id),
    database_name varchar(50)
);

create table if not exists im.uidb_fields
(
    id uuid primary key default  gen_random_uuid(),
    configuration_id uuid not null references uidb_configurations(id) on delete cascade on update cascade,
    field_id bigint not null,
    value varchar(1024),
    UNIQUE (configuration_id, field_id)
);

create table if not exists im.uidb_connection_strings
(
    id uuid primary key default gen_random_uuid(),
    uidb_settings_id uuid references uidb_settings(id),
    connection_string varchar(1024) not null,
    import_source_type int not null,
    unique(uidb_settings_id,connection_string, import_source_type)
);