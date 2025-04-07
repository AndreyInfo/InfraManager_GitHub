INSERT into operation
            values(1112, 903, 'FormBuilder.Export ', 'Экспорт', 'Операция дает возможность экспортировать объект Форма.'),
			  	  (1113, 903, 'FormBuilder.Import', 'Импорт', 'Операция дает возможность импортировать объект Форма.')
ON CONFLICT DO NOTHING;


INSERT into im.Class values(903, 'Формы')
ON CONFLICT DO NOTHING;


insert into im.role_operation values ('00000000-0000-0000-0000-000000000001',1112),
										 ('00000000-0000-0000-0000-000000000001',1113)

ON CONFLICT DO NOTHING