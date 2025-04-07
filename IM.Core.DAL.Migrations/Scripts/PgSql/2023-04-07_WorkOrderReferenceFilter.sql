delete from im.web_filter_using where filter_id in (select id from im.web_filters where name = '_ALL_' and view_name = 'ReferencedWorkOrderList');
delete from im.web_filters where name = '_ALL_' and view_name = 'ReferencedWorkOrderList';
insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_ALL_', true, 'ReferencedWorkOrderList', true);