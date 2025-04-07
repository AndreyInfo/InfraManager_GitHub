ALTER TABLE [dbo].[WorkOrderFinancePurchase] DROP CONSTRAINT [FK_WorkOrderFinancePurchase_WorkOrderID]

ALTER TABLE [dbo].[WorkOrderFinancePurchase]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderFinancePurchase_WorkOrderID] FOREIGN KEY([WorkOrderID])
REFERENCES [dbo].[WorkOrder] ([ID])
ON DELETE CASCADE