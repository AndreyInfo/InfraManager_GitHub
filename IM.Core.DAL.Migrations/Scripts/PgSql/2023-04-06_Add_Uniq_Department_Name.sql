create unique index if not exists ui_subdivision_organization_parent_subdivision_name2
    on im.department (name, organization_id, coalesce(cast(department_id as varchar(36)),''));