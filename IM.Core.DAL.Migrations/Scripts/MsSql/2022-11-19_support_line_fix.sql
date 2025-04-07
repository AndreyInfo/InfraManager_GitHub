if EXISTS (SELECT 1 FROM Setting where id = 70)
	BEGIN 
		update Setting SET Value= 0x00000005
		WHERE ID = 70
	END
ELSE 
	BEGIN 
		INSERT INTO Setting 
		VALUES(70, 0x00000005, null)
	END