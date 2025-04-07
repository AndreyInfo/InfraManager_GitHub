-- remove public scheme from internal queries

CREATE OR REPLACE FUNCTION im.func_dashboard_treeitem_is_visible(IN _class_id integer,IN _id uuid,IN _user_id uuid)
    RETURNS boolean
    LANGUAGE 'plpgsql'
    VOLATILE
    PARALLEL UNSAFE
    COST 100
    
AS $$
    
declare
begin
	if _user_id is null or _id is null or _class_id is null then
		return false;
	end if;
	--
	if _class_id = 364 then -- DE panel
		--Admin
		if exists(select * from user_role where user_id = _user_id and role_id = '00000000-0000-0000-0000-000000000001') then
			return true;
		end if;

		if exists (select * from access_permission ap  
			inner join organization_item_group o on o.id = ap.id
			where ap.object_id = _id and (ap.properties = true or ap.add = true or ap.delete = true or ap.update = true or ap.access_manage = true)
			and func_user_in_organization_item(o.organization_item_class_id, o.organization_item_id, _user_id) ) then
			return true;
		end if;
	end if;
	--
	if _class_id = 153 then -- folder
		if exists(select * from dashboard d where d.dashboard_folder_id = _id and d.dashboard_class_id = 364 and func_dashboard_treeitem_is_visible(364, d.ID, _user_id) ) then
			return true;
		end if;

		if exists(select * from dashboard_folder f where f.parent_dashboard_folder_id = _id and func_dashboard_treeitem_is_visible(153, f.ID, _user_id) ) then
			return true;
		end if;
	end if;
	--
	return false;
end;
$$;