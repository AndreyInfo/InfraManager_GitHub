if (NOT EXISTS(select * from sys.default_constraints where name = 'DF_PriorityID'))
begin 
ALTER TABLE dbo.Priority ADD CONSTRAINT DF_PriorityID DEFAULT newid() FOR ID;
end