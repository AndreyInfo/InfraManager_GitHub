if not exists (select 1 from ProductCatalogTemplate where ClassID = 450)
	insert into ProductCatalogTemplate(ID, Name, ParentID, ClassID)
		values(450, 'Конфигурационная единица', null, 450);

if not exists (select 1 from ProductCatalogCategory where ID = '00000000-0000-0000-0000-000000000450')
	insert into ProductCatalogCategory(ID, Name, Removed)
		values('00000000-0000-0000-0000-000000000450', 'Конфигурационные единицы', 0);

if not exists (select 1 from ProductCatalogType where Name = 'Конфигурационная единица' and ProductCatalogTemplateID = 450)
	insert into ProductCatalogType(ID, Name, Removed, ProductCatalogTemplateID, ProductCatalogCategoryID, LifeCycleID, ExternalCode, ExternalName, CanBuy)
		values('00000000-0000-0000-0000-000000000450', 'Конфигурационная единица', 0, 450, '00000000-0000-0000-0000-000000000450', '00000000-0000-0000-0000-000000000017', '', '', 1);