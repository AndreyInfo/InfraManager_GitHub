if not exists(
        select top 1 0
        from sys.all_columns c
                 join sys.tables t on t.object_id = c.object_id
                 join sys.schemas s on s.schema_id = t.schema_id
                 join sys.default_constraints d on c.default_object_id = d.object_id
        where t.name = 'ProductCatalogCategory'
          and c.name = 'ID'
          and s.name = 'dbo')
begin
alter table ProductCatalogCategory
    add default NEWID() FOR ID;
end
GO
if not exists(
        select top 1 0
        from sys.all_columns c
                 join sys.tables t on t.object_id = c.object_id
                 join sys.schemas s on s.schema_id = t.schema_id
                 join sys.default_constraints d on c.default_object_id = d.object_id
        where t.name = 'ProductCatalogType'
          and c.name = 'id'
          and s.name = 'dbo')
begin
alter table ProductCatalogType
    add default NEWID() FOR ID;
end
GO
if (NOT EXISTS(SELECT TOP 1 0
               FROM sys.indexes
               WHERE name = 'PccNameParent'
                 AND object_id = OBJECT_ID('dbo."ProductCatalogCategory"')))
begin
if exists(select 1 from ProductCatalogCategory group by name, ParentProductCatalogCategoryID having count(*)>1)
begin
	declare @substList table (mainID uniqueidentifier)
	insert into @substList (mainID) select min(ID) from ProductCatalogCategory group by name, ParentProductCatalogCategoryID having count(*)>1
	update dup set ParentProductCatalogCategoryID = mainID
		from ProductCatalogCategory main join @substList sl on sl.mainID = main.ID
			join ProductCatalogCategory dup on dup.Name = main.Name 
				AND isnull(dup.ParentProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ParentProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID

	update ProductCatalogType set ProductCatalogCategoryID  = mainID
		from ProductCatalogCategory main join @substList sl on sl.mainID = main.ID
			join ProductCatalogCategory dup on dup.Name = main.Name 
				AND isnull(dup.ParentProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ParentProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ProductCatalogType on ProductCatalogCategoryID = dup.ID
	
	delete dup
		from ProductCatalogCategory main join @substList sl on sl.mainID = main.ID
			join ProductCatalogCategory dup on dup.Name = main.Name 
				AND isnull(dup.ParentProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ParentProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
end

create unique index PccNameParent on ProductCatalogCategory (name, ParentProductCatalogCategoryID);
end
GO
if (NOT EXISTS(SELECT TOP 1 0
               FROM sys.indexes
               WHERE name = 'PctNameParent'
                 AND object_id = OBJECT_ID('dbo."ProductCatalogType"')))
begin
if exists(select 1 from ProductCatalogType group by name, ProductCatalogCategoryID having count(*)>1)
begin
	declare @substList table (mainID uniqueidentifier)
	insert into @substList (mainID) select min(ID) from ProductCatalogType group by name, ProductCatalogCategoryID having count(*)>1

	update AdapterType set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join AdapterType on ProductCatalogTypeID = dup.ID

	update Cluster set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join Cluster on ProductCatalogTypeID = dup.ID

	update ConfigurationUnit set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ConfigurationUnit on ProductCatalogTypeID = dup.ID

	update DataEntity set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join DataEntity on ProductCatalogTypeID = dup.ID

	update DeviceApplication set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join DeviceApplication on ProductCatalogTypeID = dup.ID

	update MaterialModel set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join MaterialModel on ProductCatalogTypeID = dup.ID

	update PeripheralType set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join PeripheralType on ProductCatalogTypeID = dup.ID

	update ProductCatalogImportSettingTypes set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ProductCatalogImportSettingTypes on ProductCatalogTypeID = dup.ID

	update ServiceContract set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ServiceContract on ProductCatalogTypeID = dup.ID

	update ServiceContractAgreementLicence set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ServiceContractAgreementLicence on ProductCatalogTypeID = dup.ID

	update ServiceContractFeature set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ServiceContractFeature on ProductCatalogTypeID = dup.ID

	update ServiceContractLicence set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ServiceContractLicence on ProductCatalogTypeID = dup.ID

	update ServiceContractModel set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ServiceContractModel on ProductCatalogTypeID = dup.ID

	update ServiceContractTypeAgreement set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join ServiceContractTypeAgreement on ProductCatalogTypeID = dup.ID

	update SoftwareLicence set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join SoftwareLicence on ProductCatalogTypeID = dup.ID

	update SoftwareLicenceModel set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join SoftwareLicenceModel on ProductCatalogTypeID = dup.ID

	update [Типы активного оборудования] set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join [Типы активного оборудования] on ProductCatalogTypeID = dup.ID

	update [Типы оконечного оборудования] set ProductCatalogTypeID  = mainID
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
			join [Типы оконечного оборудования] on ProductCatalogTypeID = dup.ID

	delete dup
		from ProductCatalogType main join @substList sl on sl.mainID = main.ID
			join ProductCatalogType dup on dup.Name = main.Name 
				AND isnull(dup.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) = isnull(main.ProductCatalogCategoryID,cast(cast(0 as binary) as uniqueidentifier)) 
				and dup.ID!=main.ID
end
create unique index PctNameParent on ProductCatalogType (name, ProductCatalogCategoryID);
end
GO
IF NOT EXISTS(SELECT top 1 0
              FROM ProductCatalogCategory
              where name = N'Шкафы'
                and Removed = 0
                and ParentProductCatalogCategoryID is null)
begin
insert into ProductCatalogCategory (name, removed) values (N'Шкафы', 0);
end
GO
IF NOT EXISTS(SELECT top 1 0
              FROM ProductCatalogTemplate
              where name = N'Шкаф')
BEGIN
insert into ProductCatalogTemplate(id, name, ParentID, ClassID) values (421, N'Шкаф', null, 4);
end
GO
if NOT EXISTS(SELECT TOP 1 0
              FROM ProductCatalogType
              where name = N'Шкафы'
                and ProductCatalogCategoryID in
                    (select id
                     from ProductCatalogCategory
                     where ProductCatalogCategory.Name = N'Шкафы'
                       and ProductCatalogCategory.ParentProductCatalogCategoryID is null))
BEGIN
insert into ProductCatalogType(name, removed, ProductCatalogTemplateID, ProductCatalogCategoryID,
                               ExternalCode, ExternalName, CanBuy)
values (N'Шкафы', 0, 421, (SELECT id
                           FROM ProductCatalogCategory c
                           where c.name = N'Шкафы'
                             and c.ParentProductCatalogCategoryID is null
                             and c.removed = 0), '', '', 0)
end
GO
SELECT CASE WHEN NOT EXISTS(SELECT TOP 1 0 --нет product_catalog_type_id
                            FROM information_schema.columns
                            WHERE table_name = 'Типы шкафов'
                              and column_name = 'ProductCatalogTypeID') THEN 1 ELSE 0 END as NotExists
into #TMP_VAR
    IF (SELECT NotExists
    FROM #TMP_VAR) = 1
BEGIN
alter table [Типы шкафов]
    add ProductCatalogTypeID uniqueidentifier;
END
GO
IF (SELECT NotExists
    FROM #TMP_VAR) = 1
BEGIN
alter table [Типы шкафов]
    add constraint [fkТипы шкафовProductCatalogType]
    foreign key (ProductCatalogTypeID)
    references ProductCatalogType (ID)
    ON DELETE CASCADE;
END
GO
IF (SELECT NotExists
    FROM #TMP_VAR) = 1
BEGIN
update [Типы шкафов]
set [Типы шкафов].ProductCatalogTypeID=
    (select id
    from ProductCatalogType
    where ProductCatalogCategoryID = (select id
    from ProductCatalogCategory c
    where c.name = N'Шкафы'
    and c.ParentProductCatalogCategoryID is null
    and c.removed = 0))
where ProductCatalogTypeID is null;
END
GO
IF (SELECT NotExists
    FROM #TMP_VAR) = 1
BEGIN
alter table [Типы шкафов]
alter column ProductCatalogTypeID uniqueidentifier not null;
end
GO
if (NOT EXISTS(SELECT TOP 1 0
               FROM sys.indexes
               WHERE name = 'ux_name'
                 AND object_id = OBJECT_ID('dbo."Типы шкафов"')))
begin
create unique index ux_name on [Типы шкафов] ([Название], ProductCatalogTypeID);
end
GO
DROP TABLE #TMP_VAR
    GO
SELECT CASE WHEN EXISTS(SELECT TOP 1 0
                        FROM sys.objects
                        WHERE object_id = OBJECT_ID(N'[dbo].[seqТипы шкафов]')
                          AND type = 'SO') THEN 1 ELSE 0 END AS ExistsSequence into #TMP_VAR
    IF (SELECT ExistsSequence FROM #TMP_VAR) = 1
BEGIN
        DECLARE @script nvarchar(max)
SELECT @script = CONCAT('create sequence [seqТипы шкафов] START WITH ',
                        (SELECT max([Идентификатор]) + 1 FROM [Типы шкафов]));

exec sp_executesql @script
END
GO
IF (SELECT ExistsSequence FROM #TMP_VAR) = 1
BEGIN
alter table [Типы шкафов]
    add DEFAULT NEXT VALUE FOR [seqТипы шкафов] FOR [Идентификатор];
END
GO
if not exists(
        select top 1 0
        from sys.all_columns c
                 join sys.tables t on t.object_id = c.object_id
                 join sys.schemas s on s.schema_id = t.schema_id
                 join sys.default_constraints d on c.default_object_id = d.object_id
        where t.name = 'Типы шкафов'
          and c.name = 'ExternalID'
          and s.name = 'dbo')
BEGIN
ALTER TABLE [Типы шкафов]
    ADD ExternalID VARCHAR(250) NOT NULL DEFAULT N'Шкаф';
END
