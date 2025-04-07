DO $$
BEGIN
	if not exists (select 1 from im.product_catalog_category where id = '00000000-0000-0000-0000-000000000409') then
		insert into im.product_catalog_category(id, name, removed, icon_name)
			values('00000000-0000-0000-0000-000000000409', ' ', false, 'SvgOpenDirectory');
	end if;
	
	if exists (select 1 from im.product_catalog_category where id = '00000000-0000-0000-0000-000000000409') then
		update im.product_catalog_category
		set name = ' '
		where id = '00000000-0000-0000-0000-000000000409';
	end if;

	if not exists (select 1 from im.product_catalog_template where class_id = 409) then
		insert into im.product_catalog_template(id, name, parent_id, class_id)
			values(409, ' ', 450, 409);
	end if;

	if exists (select 1 from im.product_catalog_template where class_id = 409) then
		update im.product_catalog_template
		set parent_id=450
		where class_id=409;
	end if;
END
$$;
