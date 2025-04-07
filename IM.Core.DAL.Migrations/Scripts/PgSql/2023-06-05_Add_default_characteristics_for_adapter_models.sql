-- Добавление характеристик для моделей CD/DVD привода
INSERT INTO cd_and_dvd_drives (id)
SELECT adapter_type_id
FROM adapter_type
WHERE NOT EXISTS (
	SELECT 1
	FROM cd_and_dvd_drives
	WHERE cd_and_dvd_drives.id = adapter_type.adapter_type_id
	) AND adapter_type.product_catalog_type_id = '85cd226d-e0e5-4040-9095-34064559ee65';

-- Добавление характеристик для моделей жесткого диска
INSERT INTO storage (id)
SELECT adapter_type_id
FROM adapter_type
WHERE NOT EXISTS (
	SELECT 1
	FROM storage
	WHERE storage.id = adapter_type.adapter_type_id
	) AND adapter_type.product_catalog_type_id = '078cc8e2-a6b0-4455-8e55-8b66a6e71bdf';

-- Добавление характеристик для моделей модуля оперативной памяти
INSERT INTO memory (id)
SELECT adapter_type_id
FROM adapter_type
WHERE NOT EXISTS (
	SELECT 1
	FROM memory
	WHERE memory.id = adapter_type.adapter_type_id
	) AND adapter_type.product_catalog_type_id = '27a63736-881f-4cf4-983b-4ba7d80069f3';

-- Добавление характеристик для моделей материнской платы
INSERT INTO motherboard (id)
SELECT adapter_type_id
FROM adapter_type
WHERE NOT EXISTS (
	SELECT 1
	FROM motherboard
	WHERE motherboard.id = adapter_type.adapter_type_id
	) AND adapter_type.product_catalog_type_id = '51f76f40-21a3-4249-bbfe-5802052ae43a';

-- Добавление характеристик для моделей процессора
INSERT INTO processor (id)
SELECT adapter_type_id
FROM adapter_type
WHERE NOT EXISTS (
	SELECT 1
	FROM processor
	WHERE processor.id = adapter_type.adapter_type_id
	) AND adapter_type.product_catalog_type_id = '55d30962-3811-4acc-9944-5d9f2b1b37df';
	
-- Добавление характеристик для моделей сетевого адаптера
INSERT INTO network_adapter (id)
SELECT adapter_type_id
FROM adapter_type
WHERE NOT EXISTS (
	SELECT 1
	FROM network_adapter
	WHERE network_adapter.id = adapter_type.adapter_type_id
	) AND adapter_type.product_catalog_type_id = '8723cab7-a5d1-4991-a919-78895b58ae84';

-- Добавление характеристик для моделей видеоадаптера
INSERT INTO video_adapter (id)
SELECT adapter_type_id
FROM adapter_type
WHERE NOT EXISTS (
	SELECT 1
	FROM video_adapter
	WHERE video_adapter.id = adapter_type.adapter_type_id
	) AND adapter_type.product_catalog_type_id = '8be5cfab-7fc6-4c85-8254-49c61fa0123e';
	
-- Добавление характеристик для моделей звуковой карты
INSERT INTO sound_card (id)
SELECT adapter_type_id
FROM adapter_type
WHERE NOT EXISTS (
	SELECT 1
	FROM sound_card
	WHERE sound_card.id = adapter_type.adapter_type_id
	) AND adapter_type.product_catalog_type_id = 'abeb9c5c-c986-4478-95b7-722ca3b46951';
	