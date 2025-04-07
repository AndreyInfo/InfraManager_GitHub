ALTER TABLE IF EXISTS im.uicsv_setting
    ADD COLUMN if not exists removed boolean NOT NULL DEFAULT False;

ALTER TABLE IF EXISTS im.uiad_setting
    ADD COLUMN if not exists removed boolean NOT NULL DEFAULT False;

    
ALTER TABLE IF EXISTS im.ui_setting
    ADD COLUMN if not exists removed boolean NOT NULL DEFAULT False;