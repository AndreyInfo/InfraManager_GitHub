if 'IconName' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_NAME] = 'MassiveIncidentType' and [TABLE_SCHEMA] = 'dbo')
begin
	
	merge [dbo].[ObjectIcon] as t
	using (select [IMObjID], [IconName] from [dbo].[MassiveIncidentType] where len([IconName]) > 0) as s
		on s.[IMObjID] = t.[ObjectID] and 824 = t.[ObjectClassID]
	when not matched then insert ([ObjectID], [ObjectClassID], [Name]) values (s.[IMObjID], 824, s.[IconName]);
end

if 'IconName' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_NAME] = 'MassiveIncidentType' and [TABLE_SCHEMA] = 'dbo')
begin
	alter table [dbo].[MassiveIncidentType] drop column [IconName]
end