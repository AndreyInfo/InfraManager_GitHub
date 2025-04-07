DO
$$
BEGIN

	create table if not exists im.highlighting (
		id uuid not null default(gen_random_uuid()),
		name character varying(255) not null,
		sequence int not null,
		constraint pk_highlighting primary key (id),
		constraint uk_highlighting unique (name)
	);

	create table if not exists im.highlighting_condition (
		id uuid not null default(gen_random_uuid()),
		highlighting_id uuid not null,
		directory_parameter int null,
		enum_parameter smallint null,
		condition smallint not null,
		background_color character varying(50) not null,
		font_color character varying(50) not null,
		constraint pk_highlighting_condition primary key (id),
		constraint fk_highlighting_condition_highlighting foreign key (highlighting_id) references im.highlighting(id) on delete cascade
	);

	create table if not exists im.highlighting_condition_value (
		id uuid not null default(gen_random_uuid()),
		highlighting_condition_id uuid not null,
		priority_id uuid null,
		urgency_id uuid null,
		influence_id uuid null,
		sla_id uuid null,
		int_value1 int null,
		int_value2 int null,
		string_value character varying(255) null,
		constraint pk_highlighting_condition_value primary key (id),
		constraint fk_highlighting_condition_value_highlighting_condition foreign key (highlighting_condition_id) references im.highlighting_condition(id) on delete cascade,
		constraint fk_highlighting_condition_value_priority foreign key (priority_id) references im.priority(id) on delete cascade,
		constraint fk_highlighting_condition_value_urgency foreign key (urgency_id) references im.urgency(id) on delete cascade,
		constraint fk_highlighting_condition_value_influence foreign key (influence_id) references im.influence(id) on delete cascade,
		constraint fk_highlighting_condition_value_sla foreign key (sla_id) references im.sla(id) on delete cascade
	);


	INSERT INTO im.operation VALUES (1344, 190, 'Highlighting.Add', 'Создать', 'Операция дает возможность создавать новый объект Правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1345, 190, 'Highlighting.Update', 'Обновить', 'Операция дает возможность изменять объект Правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1346, 190, 'Highlighting.Delete', 'Удалить', 'Операция дает возможность удалять объект Правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1347, 190, 'Highlighting.Properties', 'Открыть свойства', 'Операция дает возможность просмотра объекта Правила выделения строк в списке.')
		ON Conflict DO Nothing;

	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1344) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1344)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1345) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1345)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1346) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1346)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1347) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1347)
	  ON Conflict DO Nothing;
	END IF;
	
	INSERT INTO im.operation VALUES (1348, 191, 'HighlightingCondition.Add', 'Создать', 'Операция дает возможность создавать новый объект Условие правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1349, 191, 'HighlightingCondition.Update', 'Обновить', 'Операция дает возможность изменять объект Условие правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1350, 191, 'HighlightingCondition.Delete', 'Удалить', 'Операция дает возможность удалять объект Условие правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1351, 191, 'HighlightingCondition.Properties', 'Открыть свойства', 'Операция дает возможность просмотра объекта Условие правила выделения строк в списке.')
		ON Conflict DO Nothing;
		

	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1348) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1348)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1349) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1349)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1350) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1350)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1351) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1351)
	  ON Conflict DO Nothing;
	END IF;
	
	INSERT INTO im.operation VALUES (1352, 192, 'HighlightingConditionValue.Add', 'Создать', 'Операция дает возможность создавать новый объект Значение условий правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1353, 192, 'HighlightingConditionValue.Update', 'Обновить', 'Операция дает возможность изменять объект Значение условий правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1354, 192, 'HighlightingConditionValue.Delete', 'Удалить', 'Операция дает возможность удалять объект Значение условий правила выделения строк в списке.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1355, 192, 'HighlightingConditionValue.Properties', 'Открыть свойства', 'Операция дает возможность просмотра объекта Значение условий правила выделения строк в списке.')
		ON Conflict DO Nothing;
		

	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1352) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1352)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1353) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1353)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1354) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1354)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1355) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1355)
	  ON Conflict DO Nothing;
	END IF;

END
$$;