DO $$
BEGIN
	if not exists (select 1 from im.product_catalog_template where id = 450) then
		insert into im.product_catalog_template(id, name, parent_id, class_id)
			values(450, 'Конфигурационная единица', null, 450);
	end if;

	if not exists (select 1 from im.product_catalog_category where id = '00000000-0000-0000-0000-000000000450') then
		insert into im.product_catalog_category(id, name, removed, icon_name)
			values ('00000000-0000-0000-0000-000000000450', 'Конфигурационные единицы', false, 'SvgOpenDirectory');
	end if;

	if not exists (select 1 from im.product_catalog_type where id = '00000000-0000-0000-0000-000000000450') then
		insert into im.product_catalog_type(id, name, removed, product_catalog_template_id, product_catalog_category_id, life_cycle_id, external_code, external_name, can_buy, icon_name)
			values('00000000-0000-0000-0000-000000000450', 'Конфигурационная единица', false, 450, '00000000-0000-0000-0000-000000000450', '00000000-0000-0000-0000-000000000017', '', '', true, 'SvgAdapter');
	end if;
END
$$;

