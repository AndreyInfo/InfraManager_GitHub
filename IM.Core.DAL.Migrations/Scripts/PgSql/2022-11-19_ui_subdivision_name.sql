CREATE UNIQUE INDEX if not exists ui_subdivision_organization_parent_subdivision_name on im.department(name, organization_id, department_id);
