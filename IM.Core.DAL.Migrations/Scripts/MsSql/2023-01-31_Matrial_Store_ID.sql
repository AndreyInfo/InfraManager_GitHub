if exists(select 1 from Material where StorageLocationID is null)
begin
	declare @nullStoreID uniqueidentifier
	set @nullStoreID = isnull((select top 1 ID from StorageLocation order by ID), '00000000-0000-0000-0000-000000000000')

	update Material set StorageLocationID = @nullStoreID where StorageLocationID is null

	alter table Material alter column StorageLocationID uniqueidentifier not null
end
