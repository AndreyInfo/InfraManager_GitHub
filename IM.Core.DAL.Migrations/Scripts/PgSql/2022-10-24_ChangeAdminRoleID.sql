insert into im.role (ID,Name,Note) values ('00000000-0000-0000-0000-000000000001', 'Администратор системы', 'Администратор системы имеет право на изменение параметров системы и распределение ролей и прав.')
ON Conflict DO Nothing;

update im.role_operation set role_id = '00000000-0000-0000-0000-000000000001' where role_id = '00000000-0000-0000-0000-000000000000';

update im.role_life_cycle_state_operation set role_id = '00000000-0000-0000-0000-000000000001' where role_id = '00000000-0000-0000-0000-000000000000';

update im.user_role set role_id = '00000000-0000-0000-0000-000000000001' where role_id = '00000000-0000-0000-0000-000000000000';

delete from im.role where id = '00000000-0000-0000-0000-000000000000';