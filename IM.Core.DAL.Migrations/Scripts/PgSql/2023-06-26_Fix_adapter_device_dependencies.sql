DO $$
BEGIN
	UPDATE im.adapter
	SET network_device_id = null
	WHERE network_device_id = 0;
	
	UPDATE im.adapter
	SET terminal_device_id = null
	WHERE terminal_device_id = 0;
	
	UPDATE im.adapter
	SET network_device_id = null, terminal_device_id = null
	WHERE network_device_id = 2900001 AND terminal_device_id = 2900001;
END
$$;