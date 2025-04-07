SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[func_GetWorkTime] (@begin datetime, @end datetime)
RETURNS int
as
begin
	--только для отчетов (календарь из настроек параметров системы)
	if (@begin is null) or (@end is null) or (@begin >= @end)
		 return 0
	--
	declare @retvalInMinutes int
	set @retvalInMinutes = 0
	--
	declare @workStart datetime
	declare @workDurationInMinutes smallint
	declare @dinnerTimeFrom datetime
	declare @dinnerTimeTill datetime
	select @workStart = TimeStart, @workDurationInMinutes = TimeSpanInMinutes, @dinnerTimeFrom = DinnerTimeStart, @dinnerTimeTill = DinnerTimeEnd from dbo.CalendarWorkScheduleDefault WITH(NOLOCK)
	--
	declare @dt datetime --текущая дата
	declare @beginWorkTime datetime --начало рабочего дня в текущую дату
	declare @endWorkTime datetime --конец рабочего дня в текущую дату
	declare @currentDayWorkTime int
	set @dt = @begin
	--
	while @dt <= @end
	begin
		if dbo.func_IsWorkDay(@dt) = 1
		begin
			declare @dinnerTimeInMitunes int -- Время обеда в минутах
			set @dinnerTimeInMitunes = 0
			if(@dinnerTimeFrom is not null and @dinnerTimeTill is not null)
			begin 

				if(CONVERT(date, @dt) = CONVERT(date, @begin) and CONVERT(time, @begin) > CONVERT(time, @dinnerTimeTill))
				begin
					set @dinnerTimeInMitunes = 0
				end
				else if(CONVERT(date, @dt) = CONVERT(date, @begin) and CONVERT(time, @begin) between CONVERT(time, @dinnerTimeFrom) and CONVERT(time, @dinnerTimeTill))
				begin
					set @dinnerTimeInMitunes = DATEDIFF(minute, Convert(time, @begin), Convert(time, @dinnerTimeTill))
				end
				else if(CONVERT(date, @dt) = CONVERT(date, @end) and CONVERT(time, @end) between CONVERT(time, @dinnerTimeFrom) and CONVERT(time, @dinnerTimeTill))
				begin 
					set @dinnerTimeInMitunes = DATEDIFF(minute, Convert(time, @dinnerTimeFrom), Convert(time, @end))
				end
				else if(CONVERT(date, @dt) = CONVERT(date, @end) and CONVERT(time, @end) < CONVERT(time, @dinnerTimeFrom))
				begin 
					set @dinnerTimeInMitunes = 0
				end
				else
				begin
					set @DinnerTimeInMitunes = DATEDIFF(MINUTE, Convert(time, @dinnerTimeFrom), Convert(time, @dinnerTimeTill))
				end;

				if(@DinnerTimeInMitunes < 0)
				begin
					set @dinnerTimeInMitunes = 0
				end;
			end;

			set @beginWorkTime =
				DATEADD(minute,
					-DATEPART(minute, @dt)+DATEPART(minute, @workStart),
					DATEADD(hour,
						-DATEPART(hour, @dt)+DATEPART(hour, @workStart),
						@dt)
					)
			set @endWorkTime = DATEADD(minute, @workDurationInMinutes, @beginWorkTime)
			--
			if DATEPART(YEAR, @dt) = DATEPART(YEAR, @begin) and
				DATEPART(MONTH, @dt) = DATEPART(MONTH, @begin) and
				DATEPART(DAY, @dt) = DATEPART(DAY, @begin)
			begin
				--начальный день
				if @beginWorkTime >= @begin
				begin
					--начало работы после начала интервала
					--все отлично (учитываем весь день целиком)
					set @beginWorkTime = @beginWorkTime
				end
				else if @endWorkTime > @begin
				begin
					--конец работы после начала интервала
					set @beginWorkTime = @begin
				end
				else
				begin
					--конец работы до начала интервала
					--выкалываем
					set @endWorkTime = @beginWorkTime
				end
			end
			if DATEPART(YEAR, @dt) = DATEPART(YEAR, @end) and
				DATEPART(MONTH, @dt) = DATEPART(MONTH, @end) and
				DATEPART(DAY, @dt) = DATEPART(DAY, @end)
			begin
				--конечный день
				if @beginWorkTime >= @end
				begin
					--начало работы после конца интервала
					--выкалываем
					set @endWorkTime = @beginWorkTime
				end
				else if @endWorkTime > @end
				begin
					--конец работы после конца интервала
					set @endWorkTime = @end
				end
				else
				begin
					--конец работы до конца интервала
					--все отлично (учитываем весь день целиком)
					set @endWorkTime = @endWorkTime
				end
			end
			--
			set @currentDayWorkTime = DATEDIFF(minute, Convert(time, @beginWorkTime), Convert(time, @endWorkTime)) - @dinnerTimeInMitunes

			if(@currentDayWorkTime < 0)
				begin
					set @currentDayWorkTime = 0
				end

			set @retvalInMinutes = @retvalInMinutes + @currentDayWorkTime
		end
		--
		set @dt =
			DATEADD(day,
				1,
				DATEADD(minute,
					-DATEPART(minute, @dt),
					DATEADD(hour,
						-DATEPART(hour, @dt),
						@dt)
				)
			)
	end
	--
	return @retvalInMinutes;
end
