 IF NOT EXISTS(SELECT * FROM [dbo].[Типы активного оборудования] WHERE [Идентификатор] = 0)
  BEGIN
  INSERT INTO [dbo].[Типы активного оборудования] ([Идентификатор],[Название],[ИД производителя],[Количество портов],[Изображение],[Ширина],[Высота],[Размер по высоте],[Номер продукта],[OID],[Количество слотов],
													[ProductNumber],[ExternalID],[Code],[Note],[Removed],[IMObjID],[ComplementaryID],[Depth],[IsRackmount],[HypervisorModelID],[MaxLoad],[NominalLoad],[ColorPrint],
													[PhotoPrint],[DuplexMode],[PrintTechnology],[MaxPrintFormat],[PrintSpeedUnit],[RollNumber],[Speed],[ProductCatalogTypeID],[CanBuy])
											VALUES (0, null, null, null, null, 0.0, 0.0, null, null, null, 0, null, 'Default', null, null, 1, newid(), null, null, 0, null, null, null, null, null,
													null,null, null, null, null, null, '00000000-0000-0000-0000-000000000115', 0);
END

IF OBJECT_ID ('dbo.SnmpDeviceModels', 'U') IS NOT NULL
BEGIN
	ALTER TABLE [dbo].[SnmpDeviceModels]
	ADD [ModelID] [int] NOT NULL DEFAULT 0,
	CONSTRAINT [FK_SnmpDeviceModels_ActiveEquipmentTypes_ID] FOREIGN KEY([ModelID]) REFERENCES [dbo].[Типы активного оборудования]([Идентификатор]) ON DELETE CASCADE ON UPDATE CASCADE;
END
GO

----SnmpDeviceModel
if not exists(select * from dbo.Operation where ID = 1369) 
begin
	insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
		values (1369, 195, 'SnmpDeviceModel_Add', 'Добавить', 'Добавление объекта «Правило распознавания»');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1369)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) 
		values (1369, '00000000-0000-0000-0000-000000000001');
end

if not exists(select * from dbo.Operation where ID = 1370) 
begin
	insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
		values (1370, 195, 'SnmpDeviceModel_Update', 'Редактировать', 'Редактирование объекта «Правило распознавания»');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1370)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) 
		values (1370, '00000000-0000-0000-0000-000000000001');
end

if not exists(select * from dbo.Operation where ID = 1371) 
begin
	insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
		values (1371, 195, 'SnmpDeviceModel_Delete', 'Удалить', 'Удаление объекта «Правило распознавания»');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1371)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) 
		values (1371, '00000000-0000-0000-0000-000000000001');
end

if not exists(select * from dbo.Operation where ID = 1372) 
begin
	insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
		values (1372, 195, 'SnmpDeviceModel_Properties', 'Открыть свойства', 'Открыть свойства объекта «Правило распознавания»');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1372)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) 
		values (1372, '00000000-0000-0000-0000-000000000001');
end

--SnmpDeviceProfile
if not exists(select * from dbo.Operation where ID = 1373) 
begin
	insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
		values (1373, 196, 'SnmpDeviceProfile_Add', 'Добавить', 'Добавление профиля объекта «Правило распознавания»');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1373)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) 
		values (1373, '00000000-0000-0000-0000-000000000001');
end

if not exists(select * from dbo.Operation where ID = 1374) 
begin
	insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
		values (1374, 196, 'SnmpDeviceProfile_Update', 'Редактировать', 'Редактирование профиля объекта «Правило распознавания»');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1374)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) 
		values (1374, '00000000-0000-0000-0000-000000000001');
end

if not exists(select * from dbo.Operation where ID = 1375) 
begin
	insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
		values (1375, 196, 'SnmpDeviceProfile_Delete', 'Удалить', 'Удаление профиля объекта «Правило распознавания»');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1375)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) 
		values (1375, '00000000-0000-0000-0000-000000000001');
end

if not exists(select * from dbo.Operation where ID = 1376) 
begin
	insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
		values (1376, 196, 'SnmpDeviceProfile_Properties', 'Открыть свойства', 'Открыть свойства профиля объекта «Правило распознавания»');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1376)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) 
		values (1376, '00000000-0000-0000-0000-000000000001');
end