UPDATE Adapter
SET NetworkDeviceID = null
WHERE NetworkDeviceID = 0;
	
UPDATE Adapter
SET TerminalDeviceID = null
WHERE TerminalDeviceID = 0;
	
UPDATE Adapter
SET NetworkDeviceID = null, TerminalDeviceID = null
WHERE NetworkDeviceID = 2900001 AND TerminalDeviceID = 2900001;
