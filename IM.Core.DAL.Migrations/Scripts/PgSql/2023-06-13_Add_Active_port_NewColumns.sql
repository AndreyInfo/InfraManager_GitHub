alter table active_port add column if not exists name varchar(250) null;
alter table active_port add column if not exists port_module uuid null;
alter table active_port add column if not exists jack_type_id int null;