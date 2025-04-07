DO $$
begin

alter table im.mass_incident drop constraint if exists fk_mass_incident_sla_id;
alter table im.mass_incident drop column if exists sla_id;
if 'ola_id' not in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident') then
	alter table im.mass_incident add column ola_id int null;
end if;
alter table im.mass_incident drop constraint if exists fk_mass_incident_ola;
alter table im.mass_incident add constraint fk_mass_incident_ola foreign key (ola_id) references im.operational_level_agreement(id);

end
$$