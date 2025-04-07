INSERT INTO Setting VALUES (70, decode('\000\000\000', 'escape'))
ON CONFLICT (id) DO UPDATE SET value = decode('\000\000\000', 'escape')