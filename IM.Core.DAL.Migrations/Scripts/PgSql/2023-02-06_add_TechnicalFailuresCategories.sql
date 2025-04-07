DO $$
begin 

create sequence if not exists technical_failures_category_id start 1 increment 1;

if not EXISTS (SELECT FROM information_schema.tables WHERE  table_schema = 'im' AND table_name  = 'technical_failures_category' or table_name = 'technical_failure_category') then

create table im.technical_failures_category (
	id int not null default(nextval('technical_failures_category_id')),
	name character varying(100) not null,
	constraint pk_technical_failures_category_id primary key (id),
    constraint uk_technical_failures_category_name unique(name));

end if;

INSERT into im.Class values(907, 'Категории технических сбоев')
ON CONFLICT DO NOTHING;


insert into im.operation (id, class_id, name, operation_name, description)
	values (1119, 907, 'TechnicalFailuresCategory.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Категория технических сбоев через форму свойств, но не позволяет изменять.')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (1120, 907, 'TechnicalFailuresCategory.Add', 'Создать', 'Операция дает возможность создавать новый объект Категория технических сбоев, но не дает возможности просмотра и изменения объекта Категория технических сбоев.')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (1122, 907, 'TechnicalFailuresCategory.Update', 'Редактировать', 'Операция дает возможность изменять параметры Категории технических сбоев и сохранять внесенные изменения.')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (1121, 907, 'TechnicalFailuresCategory.Delete', 'Удалить', 'Операция дает возможность удалять объект Категория технических сбоев.')
	on conflict (id) do nothing;


insert into im.role_operation (operation_id, role_id)
	select t.id, '00000000-0000-0000-0000-000000000001'
	from im.operation t
	left join im.role_operation x on x.operation_id = t.id and x.role_id = '00000000-0000-0000-0000-000000000001'
	where (t.id between 1119 and 1121) and (x.operation_id is null);
end
$$
