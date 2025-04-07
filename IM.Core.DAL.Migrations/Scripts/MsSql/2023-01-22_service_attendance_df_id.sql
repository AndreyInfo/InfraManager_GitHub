ALTER TABLE ServiceAttendance 
ADD  CONSTRAINT [DF_ServiceAttendance]  
DEFAULT NEWID() FOR ID;