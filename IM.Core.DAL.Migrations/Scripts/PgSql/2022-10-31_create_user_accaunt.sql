CREATE SEQUENCE IF NOT EXISTS tag_id START 1 INCREMENT 1; 

CREATE TABLE IF NOT EXISTS im.tag (
	id INT NOT NULL DEFAULT(nextval ('tag_id')),
	name VARCHAR(50) NOT NULL,
	CONSTRAINT pk_tag_id PRIMARY KEY (id)	
);

CREATE UNIQUE INDEX IF NOT EXISTS unique_index_tag_name ON im.tag (name);

CREATE SEQUENCE IF NOT EXISTS user_account_id START 1 INCREMENT 1; 

CREATE TABLE IF NOT EXISTS im.user_account (
	id INT NOT NULL DEFAULT(nextval ('user_account_id')),
	name VARCHAR(50) NOT NULL,
	type INT NOT NULL,
	login VARCHAR(50) NULL,
	password VARCHAR(500) NULL,
	ssh_passphrase VARCHAR(500) NULL,
	ssh_private_key VARCHAR(500) NULL,
	authentication_protocol INT NOT NULL,
	authentication_key VARCHAR(500) NULL,
	privacy_protocol INT NOT NULL,
	privacy_key VARCHAR(500) NULL,	
	create_date TIMESTAMP NOT NULL,
	removed BOOLEAN NOT NULL,	
	removed_date TIMESTAMP NULL,	
	CONSTRAINT pk_user_account_id PRIMARY KEY (id)	
);

CREATE INDEX IF NOT EXISTS index_user_account_name ON im.user_account (name);	

CREATE TABLE IF NOT EXISTS im.user_account_tag (
	user_account_id INT NOT NULL,
	tag_id INT NOT NULL,
	CONSTRAINT pk_user_account_tag PRIMARY KEY (user_account_id, tag_id ),
	CONSTRAINT fk_user_account_tag_user_account_id FOREIGN KEY (user_account_id) REFERENCES im.user_account (id),
	CONSTRAINT fk_user_account_tag_tag_id FOREIGN KEY (tag_id) REFERENCES im.tag (id)
);
