create table if not exists im.form_values (
    id bigint constraint pk_form_values primary key generated always as identity,
    form_builder_form_id uuid not null,
	constraint fk_form_values_form_builder_form
	foreign key (form_builder_form_id) references form_builder_form(id)
);

create table if not exists im.values (
	id bigint constraint pk_values primary key generated always as identity,
	form_values_id bigint not null,
	constraint fk_values_form_values
	foreign key (form_values_id) references form_values(id) on delete cascade,
    form_builder_form_tabs_fields_id uuid not null,
	constraint fk_values_form_builder_form_tabs_fields
	foreign key (form_builder_form_tabs_fields_id) references form_builder_form_tabs_fields(id),
    value character varying(4000) not null
);

alter table im.problem
add column if not exists form_values_id bigint null;

alter table im.call
add column if not exists form_values_id bigint null;

alter table im.rfc
add column if not exists form_values_id bigint null;

alter table im.work_order
add column if not exists form_values_id bigint null;

do $$
begin
	if not exists (select 1 from pg_constraint where conname = 'fk_problem_form_values') then
		alter table im.problem
		add constraint fk_problem_form_values
		foreign key(form_values_id) references form_values(id);
	end if;
	if not exists (select 1 from pg_constraint where conname = 'fk_call_form_values') then
		alter table im.call
		add constraint fk_call_form_values
		foreign key(form_values_id) references form_values(id);
	end if;
	if not exists (select 1 from pg_constraint where conname = 'fk_rfc_form_values') then
		alter table im.rfc
		add constraint fk_rfc_form_values
		foreign key(form_values_id) references form_values(id);
	end if;
	if not exists (select 1 from pg_constraint where conname = 'fk_work_order_form_values') then
		alter table im.work_order
		add constraint fk_work_order_form_values
		foreign key(form_values_id) references form_values(id);
	end if;
end;
$$

