delete from [dbo].[MassIncidentAffectedService] where EXISTS(select 1 from [dbo].[MassIncident] m where m.[ID] = [MassIncidentID] and m.[ServiceID] = [ServiceID])
go