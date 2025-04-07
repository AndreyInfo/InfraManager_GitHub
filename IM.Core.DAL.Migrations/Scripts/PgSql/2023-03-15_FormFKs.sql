DO 
$$
begin

if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_problem_type_form') then
	ALTER TABLE IF EXISTS im.problem_type
		ADD CONSTRAINT fk_problem_type_form FOREIGN KEY (form_id)
		REFERENCES im.form_builder_form (id) MATCH SIMPLE
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
		NOT VALID;
end if;
    
if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_work_order_template_form') then	
	ALTER TABLE IF EXISTS im.work_order_template
		ADD CONSTRAINT fk_work_order_template_form FOREIGN KEY (form_id)
		REFERENCES im.form_builder_form (id) MATCH SIMPLE
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
		NOT VALID;
end if;

if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_rfc_form') then	
	ALTER TABLE IF EXISTS im.rfc
		ADD CONSTRAINT fk_rfc_form FOREIGN KEY (form_id)
		REFERENCES im.form_builder_form (id) MATCH SIMPLE
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
		NOT VALID;
end if;

if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_sla_form') then	
	ALTER TABLE IF EXISTS im.sla
		ADD CONSTRAINT fk_sla_form FOREIGN KEY (form_id)
		REFERENCES im.form_builder_form (id) MATCH SIMPLE
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
		NOT VALID;
end if;

if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_massive_incident_type_form') then	
	ALTER TABLE IF EXISTS im.massive_incident_type
		ADD CONSTRAINT fk_massive_incident_type_form FOREIGN KEY (form_id)
		REFERENCES im.form_builder_form (id) MATCH SIMPLE
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
		NOT VALID;
end if;
end;
$$;