if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_RuleName_SLAID' and object_id = OBJECT_ID('Rule')))
	BEGIN
		CREATE UNIQUE INDEX UI_RuleName_SLAID on [Rule] (Name, [SLAID])
		where [OperationalLevelAgreementID] is null;
	end;
  
if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_RuleName_OLAID' and object_id = OBJECT_ID('Rule')))
	BEGIN
		CREATE UNIQUE INDEX UI_RuleName_OLAID on [Rule] (Name, [OperationalLevelAgreementID])
		where [SLAID] is null;
	end;