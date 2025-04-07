if not exists (select 1 from ProductCatalogCategory where ID = '00000000-0000-0000-0000-000000000409')
	insert into ProductCatalogCategory(ID, Name, Removed)
		values('00000000-0000-0000-0000-000000000409', 'Узел сети', 0);
	
if exists (select 1 from ProductCatalogCategory where ID = '00000000-0000-0000-0000-000000000409')
	update ProductCatalogCategory
	set Name = 'Узел сети'
	where ID = '00000000-0000-0000-0000-000000000409';

if not exists (select 1 from ProductCatalogTemplate where ClassID = 409)
	insert into ProductCatalogTemplate(ID, Name, ParentID, ClassID)
		values(409, 'Узел сети', 450, 409);

if exists (select 1 from ProductCatalogTemplate where ClassID = 409)
	update ProductCatalogTemplate
	set ParentID=450
	where ClassID=409
