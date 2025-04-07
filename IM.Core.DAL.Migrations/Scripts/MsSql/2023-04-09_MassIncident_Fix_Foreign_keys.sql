IF EXISTS (SELECT 1
           FROM information_schema.referential_constraints t
           WHERE t.constraint_schema = 'dbo'
             AND t.constraint_name = 'FK_MassIncidentService_MassIncident'
             AND t.delete_rule <> 'CASCADE')
BEGIN
    ALTER TABLE [dbo].[MassIncidentAffectedService]
        DROP CONSTRAINT FK_MassIncidentService_MassIncident;

    ALTER TABLE [dbo].[MassIncidentAffectedService]
        ADD CONSTRAINT FK_MassIncidentService_MassIncident
            FOREIGN KEY ([MassIncidentID])
            REFERENCES [dbo].[MassIncident] ([ID])
            ON DELETE cascade;
END
GO
    
IF EXISTS (SELECT 1
           FROM information_schema.referential_constraints t
           WHERE t.constraint_schema = 'dbo'
             AND t.constraint_name = 'FK_MassIncidentService_Service'
             AND t.delete_rule <> 'CASCADE')
BEGIN
    ALTER TABLE [dbo].[MassIncidentAffectedService]
        DROP CONSTRAINT FK_MassIncidentService_Service;
    
    ALTER TABLE [dbo].[MassIncidentAffectedService]
        ADD CONSTRAINT FK_MassIncidentService_Service
            FOREIGN KEY ([ServiceID])
            REFERENCES [dbo].[Service] ([ID])
            ON DELETE cascade;
END
GO



IF EXISTS (SELECT 1
           FROM information_schema.referential_constraints t
           WHERE t.constraint_schema = 'dbo'
             AND t.constraint_name = 'FK_MassIncidentCall_Call'
             AND t.delete_rule <> 'CASCADE')
BEGIN
    ALTER TABLE [dbo].[MassIncidentCall]
        DROP CONSTRAINT FK_MassIncidentCall_Call;

    ALTER TABLE [dbo].[MassIncidentCall]
        ADD CONSTRAINT FK_MassIncidentCall_Call
            FOREIGN KEY ([CallID])
            REFERENCES [dbo].[Call] ([ID])
            ON DELETE cascade;
END
GO
    
IF EXISTS (SELECT 1
           FROM information_schema.referential_constraints t
           WHERE t.constraint_schema = 'dbo'
             AND t.constraint_name = 'FK_MassIncidentCall_MassiveIncident'
             AND t.delete_rule <> 'CASCADE')
BEGIN
    ALTER TABLE [dbo].[MassIncidentCall]
        DROP CONSTRAINT FK_MassIncidentCall_MassiveIncident;
    
    ALTER TABLE [dbo].[MassIncidentCall]
        ADD CONSTRAINT FK_MassIncidentCall_MassiveIncident
            FOREIGN KEY ([MassiveIncidentID])
            REFERENCES [dbo].[MassIncident] ([ID])
            ON DELETE cascade;
END
GO


IF EXISTS (SELECT 1
           FROM information_schema.referential_constraints t
           WHERE t.constraint_schema = 'dbo'
             AND t.constraint_name = 'FK_MassIncidentChangeRequest_ChangeRequest'
             AND t.delete_rule <> 'CASCADE')
BEGIN
    ALTER TABLE [dbo].[MassIncidentChangeRequest]
        DROP CONSTRAINT FK_MassIncidentChangeRequest_ChangeRequest;

    ALTER TABLE [dbo].[MassIncidentChangeRequest]
        ADD CONSTRAINT FK_MassIncidentChangeRequest_ChangeRequest
            FOREIGN KEY ([ChangeRequestID])
            REFERENCES [dbo].[RFC] ([ID])
            ON DELETE cascade;
END
GO
    
IF EXISTS (SELECT 1
           FROM information_schema.referential_constraints t
           WHERE t.constraint_schema = 'dbo'
             AND t.constraint_name = 'FK_MassIncidentChangeRequest_MassiveIncident'
             AND t.delete_rule <> 'CASCADE')
BEGIN
    ALTER TABLE [dbo].[MassIncidentChangeRequest]
        DROP CONSTRAINT FK_MassIncidentChangeRequest_MassiveIncident;
    
    ALTER TABLE [dbo].[MassIncidentChangeRequest]
        ADD CONSTRAINT FK_MassIncidentChangeRequest_MassiveIncident
            FOREIGN KEY ([MassiveIncidentID])
            REFERENCES [dbo].[MassIncident] ([ID])
            ON DELETE cascade;
END
GO
