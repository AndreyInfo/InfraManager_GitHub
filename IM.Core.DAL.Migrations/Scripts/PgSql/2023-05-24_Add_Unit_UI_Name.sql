DO $$
BEGIN

	DELETE FROM unit
		WHERE name IN (
			SELECT name
			FROM unit
			GROUP BY name
			HAVING COUNT(*) > 1
		);

	CREATE UNIQUE INDEX
		if not exists ui_name_unit
		on unit(name);
END $$
