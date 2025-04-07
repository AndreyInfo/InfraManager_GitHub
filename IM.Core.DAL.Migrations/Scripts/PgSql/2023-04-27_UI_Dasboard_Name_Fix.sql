DROP INDEX IF EXISTS ui_dashboard_name_by_folder_id;

CREATE UNIQUE INDEX
  IF NOT EXISTS ui_dashboard_name_by_folder_id
  ON im.dashboard(name, dashboard_folder_id);