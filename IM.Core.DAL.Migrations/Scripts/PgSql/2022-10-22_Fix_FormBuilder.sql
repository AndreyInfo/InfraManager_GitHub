	drop table if exists im.workflow_field_options;
	drop table if exists im.workflow_activity_form_field;
	drop table if exists im.workflow_activity_form_tab;
	drop table if exists im.workflow_activity_form;
	
		create table IF NOT EXISTS form_builder_field_options
	(
	    id uuid not null,
		constant Text NULL,
		operation_id int not null,
		action_id int not null,
		parent_identifier Text not null,
		row_version timestamp null,
		workflow_activity_form_field_id uuid not null,
		
		primary key(ID),
		constraint fk_field_options_field foreign key (workflow_activity_form_field_id) references form_builder_form_tabs_fields(id) on delete cascade
	);