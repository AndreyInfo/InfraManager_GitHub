ALTER TABLE Service 
ADD  CONSTRAINT [DF_Service]  
DEFAULT NEWID() FOR ID;