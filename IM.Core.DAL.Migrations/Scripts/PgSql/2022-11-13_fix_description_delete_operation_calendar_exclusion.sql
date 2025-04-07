INSERT INTO operation 
VALUES(820007, 901, 'CalendarExclusion.Delete','Удалить' ,'Операция дает возможность удалять объект Причина отклонения от графика.')
ON CONFLICT(id) DO UPDATE SET description = 'Операция дает возможность удалять объект Причина отклонения от графика.'