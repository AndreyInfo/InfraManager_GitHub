-- Добавление характеристик для моделей CD/DVD привода
INSERT INTO CDAndDVDDrives (ID)
SELECT AdapterTypeID
FROM AdapterType
WHERE NOT EXISTS (
	SELECT 1
	FROM CDAndDVDDrives
	WHERE CDAndDVDDrives.ID = AdapterType.AdapterTypeID
	) AND AdapterType.ProductCatalogTypeID = '85cd226d-e0e5-4040-9095-34064559ee65';

-- Добавление характеристик для моделей жесткого диска
INSERT INTO Storage (ID)
SELECT AdapterTypeID
FROM AdapterType
WHERE NOT EXISTS (
	SELECT 1
	FROM Storage
	WHERE Storage.ID = AdapterType.AdapterTypeID
	) AND AdapterType.ProductCatalogTypeID = '078cc8e2-a6b0-4455-8e55-8b66a6e71bdf';

-- Добавление характеристик для моделей модуля оперативной памяти
INSERT INTO Memory (ID)
SELECT AdapterTypeID
FROM AdapterType
WHERE NOT EXISTS (
	SELECT 1
	FROM Memory
	WHERE Memory.ID = AdapterType.AdapterTypeID
	) AND AdapterType.ProductCatalogTypeID = '27a63736-881f-4cf4-983b-4ba7d80069f3';

-- Добавление характеристик для моделей материнской платы
INSERT INTO Motherboard (ID)
SELECT AdapterTypeID
FROM AdapterType
WHERE NOT EXISTS (
	SELECT 1
	FROM Motherboard
	WHERE Motherboard.ID = AdapterType.AdapterTypeID
	) AND AdapterType.ProductCatalogTypeID = '51f76f40-21a3-4249-bbfe-5802052ae43a';

-- Добавление характеристик для моделей процессора
INSERT INTO Processor (ID)
SELECT AdapterTypeID
FROM AdapterType
WHERE NOT EXISTS (
	SELECT 1
	FROM Processor
	WHERE Processor.ID = AdapterType.AdapterTypeID
	) AND AdapterType.ProductCatalogTypeID = '55d30962-3811-4acc-9944-5d9f2b1b37df';
	
-- Добавление характеристик для моделей сетевого адаптера
INSERT INTO NetworkAdapter (ID)
SELECT AdapterTypeID
FROM AdapterType
WHERE NOT EXISTS (
	SELECT 1
	FROM NetworkAdapter
	WHERE NetworkAdapter.ID = AdapterType.AdapterTypeID
	) AND AdapterType.ProductCatalogTypeID = '8723cab7-a5d1-4991-a919-78895b58ae84';

-- Добавление характеристик для моделей видеоадаптера
INSERT INTO VideoAdapter (ID)
SELECT AdapterTypeID
FROM AdapterType
WHERE NOT EXISTS (
	SELECT 1
	FROM VideoAdapter
	WHERE VideoAdapter.ID = AdapterType.AdapterTypeID
	) AND AdapterType.ProductCatalogTypeID = '8be5cfab-7fc6-4c85-8254-49c61fa0123e';
	
-- Добавление характеристик для моделей звуковой карты
INSERT INTO SoundCard (ID)
SELECT AdapterTypeID
FROM AdapterType
WHERE NOT EXISTS (
	SELECT 1
	FROM SoundCard
	WHERE SoundCard.ID = AdapterType.AdapterTypeID
	) AND AdapterType.ProductCatalogTypeID = 'abeb9c5c-c986-4478-95b7-722ca3b46951';
	