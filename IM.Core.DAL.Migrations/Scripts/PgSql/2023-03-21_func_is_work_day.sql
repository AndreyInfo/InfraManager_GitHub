CREATE or replace FUNCTION im.func_is_work_day(IN _date timestamp without time zone)
    RETURNS boolean
    LANGUAGE 'plpgsql'
as $$
declare holiday_id uuid;
declare weekend_id uuid;
declare _day integer;
declare _month integer;
declare _dayOfWeek smallint;

begin
  if _date is null then
      return 0;
  end if;
  
  select calendar_holiday_id, calendar_weekend_id into holiday_id, weekend_id from im.calendar_work_schedule_default;
  
  if holiday_id is not null then
    _day = DATE_PART('day', _date);
    _month = DATE_PART('month', _date);
    
    if exists(select * from im.calendar_holiday_item where day = _day and month = _month and calendar_holiday_id = holiday_id) then
        return 0; --праздничный день
    end if;

  end if;
   
  if weekend_id is not null then
    _dayOfWeek = 1 + DATE_PART('day', _date - '1900-01-01'::timestamp )::integer % 7;
    if exists(select * from im.calendar_weekend where id = weekend_id and 
             ((sunday = true and _dayOfWeek = 7) or
             (monday = true and _dayOfWeek = 1) or
             (tuesday = true and _dayOfWeek = 2) or
             (wednesday = true and _dayOfWeek = 3) or
             (thursday = true and _dayOfWeek = 4) or
             (friday = true and _dayOfWeek = 5) or
             (saturday = true and _dayOfWeek = 6))) then
             return 0; --выходной день
      end if;
   end if;
    
   return 1; --рабочий
end;      
$$;