INSERT INTO setting
(id, value)
VALUES(133, decode('20','hex')) ON Conflict do nothing;