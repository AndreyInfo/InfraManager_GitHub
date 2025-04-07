alter table [dbo].[SlotTemplate] 
	add constraint UK_SlotTemplate_Number unique (ObjectID, Number);
