update im.call_aggregate
set 
	work_order_count = (select count(1) from im.work_order_reference x where x.object_id = call_id and x.class_id = 701),
	queue_name = (select q.name from im.queue q join im.call c on c.queue_id = q.id and c.id = call_id )