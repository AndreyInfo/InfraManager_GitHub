DELETE FROM [Виды разъемов] WHERE [MediumID] IS NULL;
ALTER TABLE [Виды разъемов] ALTER COLUMN [MediumID] int NOT NULL;