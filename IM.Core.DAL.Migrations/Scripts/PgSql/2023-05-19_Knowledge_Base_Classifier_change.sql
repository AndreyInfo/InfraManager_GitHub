DO $$
begin
	if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'uc_knowledge_base_classifier_name') then
		ALTER TABLE IF EXISTS im.kb_article_folder
			ADD CONSTRAINT "uc_knowledge_base_classifier_name" UNIQUE (name);
	end if;
	
	if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_knowledge_base_classifier_expert') then

            ALTER TABLE IF EXISTS im.kb_article_folder
                ADD COLUMN expert_id integer DEFAULT 1 not null;

            ALTER TABLE IF EXISTS im.kb_article_folder
                ADD COLUMN update_period integer DEFAULT 0 NOT NULL;

            ALTER TABLE im.kb_article_folder
    		    ADD CONSTRAINT fk_knowledge_base_classifier_expert FOREIGN KEY (expert_id) REFERENCES users (identificator);
	end if;
end
$$