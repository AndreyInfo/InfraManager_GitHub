DO $$
begin
create sequence if not exists massive_incident_work_order_id start 1 increment 1;

create table if not exists im.massive_incident_work_order (
	id bigint not null default(nextval('massive_incident_work_order_id')),
	massive_incident_id int not null,
	work_order_id uuid not null,
	constraint pk_massive_incident_work_order primary key (id),
	constraint uk_massive_incident_work_order unique(massive_incident_id, work_order_id),
	constraint fk_massive_incident_work_order_massive_incident foreign key (massive_incident_id) references im.massive_incident(id),
	constraint fk_massive_incident_work_order_work_order foreign key (work_order_id) references im.work_order(id)
);

alter table im.massive_incident ADD COLUMN IF NOT EXISTS executor_user_id int null;

if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_massive_incident_executor_user_id')
then
ALTER TABLE im.massive_incident
    ADD CONSTRAINT fk_massive_incident_executor_user_id FOREIGN KEY (executor_user_id) references im.users(identificator);
end if;

alter table im.massive_incident ADD COLUMN IF NOT EXISTS utc_date_accomplished timestamp(3) null;
alter table im.massive_incident ADD COLUMN IF NOT EXISTS utc_date_closed timestamp(3) null;
END
$$