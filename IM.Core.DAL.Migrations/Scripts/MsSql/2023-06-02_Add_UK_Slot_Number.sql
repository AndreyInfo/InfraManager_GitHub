alter table [dbo].[Slot] 
	add constraint UK_Slot_Number unique (ObjectID, Number);
