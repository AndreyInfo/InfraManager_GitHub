if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_WorkOrderType_Name' and object_id = OBJECT_ID('WorkOrderType')))
        BEGIN
            CREATE UNIQUE INDEX UI_WorkOrderType_Name on WorkOrderType (Name) where [Removed] = 0;
        end;
	

    if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_WorkOrderPriority_Name' and object_id = OBJECT_ID('WorkOrderPriority')))
        BEGIN
            CREATE UNIQUE INDEX UI_WorkOrderPriority_Name on WorkOrderPriority (Name) where [Removed] = 0;
        end;

		
    if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_RFCType_Name' and object_id = OBJECT_ID('RFCType')))
        BEGIN
            CREATE UNIQUE INDEX UI_RFCType_Name on RFCType (Name) where [Removed] = 0;
        end;