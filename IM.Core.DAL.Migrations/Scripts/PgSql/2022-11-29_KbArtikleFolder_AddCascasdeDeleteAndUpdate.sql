ALTER TABLE kb_article_folder DROP CONSTRAINT fk_kb_article_folder_kb_article_folder;

ALTER TABLE kb_article_folder 
ADD CONSTRAINT 
	fk_kb_article_folder_kb_article_folder FOREIGN KEY (parent_id)
    REFERENCES im.kb_article_folder (id) MATCH SIMPLE
    ON DELETE CASCADE;