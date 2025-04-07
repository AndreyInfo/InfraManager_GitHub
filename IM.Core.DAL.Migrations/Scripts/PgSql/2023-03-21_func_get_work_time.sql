CREATE or replace FUNCTION im.func_get_work_time(IN begin_date timestamp without time zone, IN till_date timestamp without time zone)
    RETURNS integer
    LANGUAGE 'plpgsql'
    
AS $$
declare retval_in_minutes integer;
declare work_start timestamp without time zone;
declare work_duration_in_minutes smallint;
declare current_datetime timestamp without time zone; --текущая дата
declare begin_work_time timestamp without time zone; --начало рабочего дня в текущую дату
declare end_work_time timestamp without time zone;  --конец рабочего дня в текущую дату
declare dinner_time_from timestamp without time zone; --начало перерыва на обед
declare dinner_time_till timestamp without time zone; --конец перерыва на обед
declare dinner_time_in_minutes integer; --Время обеда в минутах
declare current_day_work_time integer; -- Кол-во рабочих минут в текущем дне

begin
--только для отчетов (календарь из настроек параметров системы)
if begin_date is null or till_date is null or begin_date >= till_date then
    return 0;
end if;

retval_in_minutes = 0;
select time_start, time_span_in_minutes, dinner_time_start, dinner_time_end into work_start, work_duration_in_minutes, dinner_time_from, dinner_time_till from im.calendar_work_schedule_default;
current_datetime = begin_date;
dinner_time_in_minutes = 0;

while current_datetime <= till_date loop
	if(im.func_is_work_day(current_datetime) = true) then
	
		if(dinner_time_from is not null and dinner_time_till is not null) then
			if(current_datetime::date = begin_date::date and begin_date::time > dinner_time_till::time) then
				dinner_time_in_minutes = 0;
			elseif(current_datetime::date = begin_date::date and begin_date::time between dinner_time_from::time and dinner_time_till::time) then
				dinner_time_in_minutes = date_part('minute', dinner_time_till::time - begin_date::time) + date_part('hour', dinner_time_till::time - begin_date::time) * 60;
			elseif(current_datetime::date = till_date::date and till_date::time between dinner_time_from::time and dinner_time_till::time) then
				dinner_time_in_minutes = date_part('minute', till_date::time - dinner_time_from::time) + date_part('hour', till_date::time - dinner_time_from::time) * 60;
			elseif(current_datetime::date = till_date::date and till_date::time < dinner_time_from::time) then
				dinner_time_in_minutes = 0;
			else
				dinner_time_in_minutes = (date_part('hour', dinner_time_till) - date_part('hour', dinner_time_from)) * 60 + date_part('minute', dinner_time_till) - date_part('minute', dinner_time_from);
			end if;
		end if;
		
		if dinner_time_in_minutes < 0 then
			dinner_time_in_minutes = 0;
		end if;
		
		begin_work_time = current_datetime + (-date_part('hour', current_datetime) + date_part('hour', work_start)) * interval '1 hour' + (-date_part('minute', current_datetime) + date_part('minute', work_start)) * interval '1 minute';
		end_work_time = begin_work_time + work_duration_in_minutes * interval '1 min';
		
		if(date_part('year', current_datetime) = date_part('year', begin_date) and
		   date_part('month', current_datetime) = date_part('month', begin_date) and
		   date_part('day', current_datetime) = date_part('day', begin_date)) then
			if(begin_work_time >= begin_date) then
				begin_work_time = begin_work_time;
			elseif(end_work_time > begin_date) then
				begin_work_time = begin_date;
			else 
				end_work_time = begin_work_time;
			end if;
		end if;
		
		
		if(date_part('year', current_datetime) = date_part('year', till_date) and
		   date_part('month', current_datetime) = date_part('month', till_date) and
		   date_part('day', current_datetime) = date_part('day', till_date)) then
		   if (begin_work_time >= till_date) then
		   		end_work_time = begin_work_time;
		   elseif (end_work_time > till_date) then
		   		end_work_time = till_date;
		   else 
		   		end_work_time = end_work_time;
		   end if;
		end if;
		
		current_day_work_time = - dinner_time_in_minutes + date_part('minute', end_work_time - begin_work_time) + DATE_PART('hour', end_work_time - begin_work_time) * 60;
		if(current_day_work_time < 0) then
			current_day_work_time = 0;
		end if;
		
		retval_in_minutes = retval_in_minutes + current_day_work_time;
	end if;
	
	current_datetime = ((current_datetime - date_part('minute', current_datetime) * interval '1 minute') - date_part('hour', current_datetime) * interval '1 hour') + interval '1 day';
end loop;

return retval_in_minutes;
end;
$$;