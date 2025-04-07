if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Service_Attendance_Name_Into_Service' and object_id = OBJECT_ID('ServiceAttendance')))
BEGIN
	CREATE UNIQUE INDEX UI_Service_Attendance_Name_Into_Service on ServiceAttendance (Name, ServiceID);
end;