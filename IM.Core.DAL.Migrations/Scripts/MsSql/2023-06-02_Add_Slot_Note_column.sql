if 'Note' not in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'Slot')
begin
	alter table [dbo].[Slot]
	add [Note] nvarchar(250) not null
	constraint DF_Slot_Note default '';
end
go