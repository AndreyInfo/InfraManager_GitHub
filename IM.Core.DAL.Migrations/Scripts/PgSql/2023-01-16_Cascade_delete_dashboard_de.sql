ALTER TABLE IF EXISTS im.dashboard_de DROP CONSTRAINT IF EXISTS fk_dashboard_de_dashboard;

ALTER TABLE dashboard_de
ADD CONSTRAINT fk_dashboard_de_dashboard
FOREIGN KEY (dashboard_id)
REFERENCES dashboard(id)
ON DELETE CASCADE ON UPDATE NO ACTION; 
