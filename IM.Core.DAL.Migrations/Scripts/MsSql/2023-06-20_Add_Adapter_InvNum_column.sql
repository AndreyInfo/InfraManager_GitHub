if 'InventoryNumber' not in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'Adapter')
begin
	alter table [dbo].[Adapter]
	add [InventoryNumber] nvarchar(50) null;
end
go