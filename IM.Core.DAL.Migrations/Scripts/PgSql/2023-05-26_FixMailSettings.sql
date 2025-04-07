UPDATE im.setting SET value = '{}' WHERE id = 124 AND value NOT LIKE '{%}';

UPDATE users SET workplace_id = 0 WHERE workplace_id IS NULL AND identificator NOT IN (0, 1);
