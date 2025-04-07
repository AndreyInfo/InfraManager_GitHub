define(['knockout', 'jquery'], function (ko, $) {
    var module = {
        StringToDate: function (dt) {
            if (dt == null || dt == undefined || dt == '')
                return null;
            //
            //dd.MM.yyyy hh:mm:ss
            var mainParts = dt.toString().split(' ');
            if (mainParts.length == 0)
                return null;
            //
            var dateParts = mainParts[0].split('.');//d.m.y
            if (dateParts.length != 3)
                return null;
            var d = parseInt(dateParts[0]);
            var m = parseInt(dateParts[1]);
            var y = parseInt(dateParts[2]);
            //
            var hh = 0;
            var mm = 0;
            var ss = 0;
            if (mainParts.length > 1) {
                var timeParts = mainParts[1].split(':');//hh.mm.ss
                if (timeParts.length != 3)
                    return null;
                hh = parseInt(timeParts[0]);
                mm = parseInt(timeParts[1]);
                ss = parseInt(timeParts[2]);
            }
            // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
            dt = new Date(y, m - 1, d, hh, mm, ss); //months are 0-based
            return dt;
        },
        StringIsDate: function (dt) {
            var dt = module.StringToDate(dt);
            //
            return (dt != 'Invalid Date' && !isNaN(dt) && dt != null) ? true : false;
        },
        Date2String: function (dt, onlyDate) {
            if (!dt)
                return '';
            if (onlyDate !== true)
                onlyDate = false;
            //
            var year = dt.getFullYear();
            var month = dt.getMonth() + 1; if (month < 10) month = '0' + month;
            var day = dt.getDate(); if (day < 10) day = '0' + day;
            var hour = dt.getHours(); if (hour < 10) hour = '0' + hour;
            var min = dt.getMinutes(); if (min < 10) min = '0' + min;
            //
            var retval = day + '.' + month + '.' + year + (onlyDate ? "" : (" " + hour + ":" + min));
            return retval;
        },
        GetMillisecondsSince1970: function (dt) {
            if (dt == null) {
                return null;
            } else {
                // NOTE: Приводим выбранную дату к текущему смещению, а не историческому.
                const adjustOffset = (new Date().getTimezoneOffset() - dt.getTimezoneOffset()) * 60000;
                return dt.getTime() + adjustOffset; //web to server: ms from 1.1.1970 in utc
            }
        },
        SerializeTimeRoundingByMinute: function (time) {
            return new Date(time - new Date(time).getSeconds() * 1000 - new Date(time).getMilliseconds());
        },
        control: function () {
            var self = this;
            self.$region = null;
            self.$isLoaded = $.Deferred();
            self.divID = 'dateTimeControl_' + ko.getNewID();
            self.Options = null;
            self.dtpControl = '';
            self.stringObservable = null;
            self.headerText = ko.observable('');
            self.classText = ko.observable('');
            //
            self.IsDisabled = ko.observable(false);
            //
            self.CalendarClick = function () {
                if (self.$isLoaded.state() == 'resolved')
                    self.dtpControl.datetimepicker('toggle');
            };
            //
            self.defaultConfig = {
                StringObservable: null,
                ValueDate: null,
                OnSelectDateFunc: null,
                OnSelectTimeFunc: null,
                HeaderText: '',
                ClassText: '',
                OnlyDate: false
            };
            //
            self.init = function ($region, settings) {
                self.$region = $region;
                var config = self.defaultConfig;
                $.extend(config, settings);
                //
                self.Options = config;
                self.stringObservable = self.Options.StringObservable;
                self.headerText(self.Options.HeaderText);
                self.classText(self.Options.ClassText);
                //
                self.$region.append('<div id="' + self.divID + '" style="position:relative" data-bind="template: {name: \'DateTimeControl/DateTimeControl\', afterRender: AfterRender}" ></div>');
                //
                try {
                    ko.applyBindings(self, document.getElementById(self.divID));
                }
                catch (err) {
                    if (document.getElementById(self.divID))
                        throw err;
                }
            };
            //
            self.destroy = function () {
                if (self.dtpControl)
                    self.dtpControl.datetimepicker('destroy');
            };
            //
            self.AfterRender = function () {
                showSpinner(self.$region[0]);
                //
                require(['dateTimePicker'], function () {
                    if (locale && locale.length > 0)
                        $.datetimepicker.setLocale(locale.substring(0, 2));
                    //
                    var allowTimes = []; for (var xh = 0; xh <= 23; xh++) for (var xm = 0; xm < 60; xm++) allowTimes.push(("0" + xh).slice(-2) + ':' + ("0" + xm).slice(-2));
                    var startDate = new Date();
                    //startDate.setMonth(startDate.getMonth());
                    var processedDate = '';
                    var controlIsOpen = false;

                    const selectedDateTrigger = function (selectDate, input) {
                        selectDateHandler(selectDate, input);
                    };

                    const optionsControl = {
                        startDate: self.Options.ValueDate == null ? startDate : self.Options.ValueDate,
                        closeOnDateSelect: self.Options.OnlyDate === true ? true : false,
                        timepicker: self.Options.OnlyDate === true ? false : true,
                        format: self.Options.OnlyDate ? 'd.m.Y' : 'd.m.Y H:i',
                        mask: self.Options.OnlyDate ? '39.19.9999' : '39.19.9999 29:59',
                        dayOfWeekStart: locale && locale.length > 0 && locale.substring(0, 2) == 'en' ? 0 : 1,
                        value: self.Options.ValueDate,
                        allowTimes: allowTimes,
                        id: `calendar_${self.divID}`,
                        onShow: function (_selectDate, $input) {
                            const inputHeight = $input.height();
                            const inputOffsetByTop = $input.offset().top;
                            const inputOffsetByBottom = $(document.documentElement).height() - inputOffsetByTop - inputHeight;

                            const calendarWrapper = $(`#calendar_${self.divID}`);
                            const calendarHeight = calendarWrapper.height();

                            if (inputOffsetByBottom < calendarHeight) {
                                // исключаем мигание календаря
                                calendarWrapper.css({
                                    opacity: 0
                                });

                                const topPosition = inputOffsetByTop - calendarHeight - inputHeight;
                                // на данном этапе, стили из плагина еще не установлены, поэтому меняем на следующем цикле eventLoop
                                setTimeout(function () {
                                    calendarWrapper.css({
                                        top: `${topPosition}px`,
                                        opacity: 1
                                    });
                                }, 0);
                            };
                            controlIsOpen = true;
                        },
                        onSelectDate: function (_selectDate, $input) {
                            if (self.Options.OnlyDate === false) {
                                setMinDate(_selectDate);
                            }
                        },
                        onClose: function (_selectDate, $input) {
                            if (controlIsOpen) {
                                controlIsOpen = false;
                                selectedDateTrigger(_selectDate, $input);
                            }
                        }
                    };

                    const setMinDate = function (_selectDate) {
                        if (self.Options.MinDate) {
                            self.dtpControl.datetimepicker('setOptions', { minDate: self.Options.MinDate });
                            if (!self.Options.OnlyDate) {
                                if (_selectDate.getFullYear() <= self.Options.MinDate.getFullYear() && _selectDate.getMonth() <= self.Options.MinDate.getMonth()) {
                                    if (_selectDate.getDate() == self.Options.MinDate.getDate()) {
                                        self.dtpControl.datetimepicker('setOptions', { minTime: self.Options.MinDate });
                                    } else if (_selectDate.getDate() < self.Options.MinDate.getDate()) {
                                        self.dtpControl.datetimepicker('setOptions', { minTime: 0 });
                                    } else {
                                        self.dtpControl.datetimepicker('setOptions', { minTime: false });
                                    }
                                } else {
                                    self.dtpControl.datetimepicker('setOptions', { minTime: false });
                                }
                            }
                        };
                    }

                    self.dtpControl = self.$region.find(':input').datetimepicker(optionsControl);

                    const selectDateHandler = function (selectDate, $input) {
                        var inputDate = ParseInputDate($input.val());
                        if (inputDate) {
                            selectDate.setFullYear(inputDate.getFullYear(), inputDate.getMonth(), inputDate.getDate());
                            selectDate.setHours(inputDate.getHours(), inputDate.getMinutes(), 0);
                        }
                        if (`${processedDate}` === `${selectDate}`) {
                            return;
                        }
                        processedDate = `${selectDate}`;

                        const isClearInputValue = selectDate == null;
                        if (isClearInputValue) {
                            if (!self.Options.OnClearMaskFunc) {
                                return;
                            };

                            self.Options.OnClearMaskFunc(selectDate, $input);
                            return;
                        };


                        const serializeSelectDate = module.SerializeTimeRoundingByMinute(selectDate);

                        if (self.Options.OnSelectDateFunc) {
                            const DEFAULT_INVALID_DATE_PICKER = new Date('1899-12-31T04:18:20.000Z');

                            // Проверяем, если в инпуте уже цифры, а так же не заполненные поля
                            const isNotValidValue = serializeSelectDate.getTime() === DEFAULT_INVALID_DATE_PICKER.getTime() || ~$input.val().search(/\d/) && $input.val().includes('_');
                            self.Options.OnSelectDateFunc(serializeSelectDate, $input, isNotValidValue);
                        };

                        function ParseInputDate(dateString) { // 20.06.2023 21:5
                            if (dateString) {
                                var regEx = /^(\d{1,2})\.(\d{1,2})\.(\d{4}) (\d{1,2}):(\d{1,2})$/;
                                var result = regEx.exec(dateString);
                                if (result != null && result.length == 6) {
                                    return new Date(result[3], result[2] - 1, result[1], result[4], result[5]);
                                }
                            }
                            return null;
                        }
                    };

                    setMinDate(optionsControl.startDate);

                    self.dtpControl.on('blur', function () {
                        selectDateHandler(self.dtpControl.datetimepicker('getValue'), self.dtpControl);
                    });

                    //
                    hideSpinner(self.$region[0]);
                    //
                    self.$isLoaded.resolve(self.dtpControl);
                    //
                    if (self.Options.FocusControl)//перефокусируем на нужный нам контрол (IE автоматически фокусирует dataTimeControl)
                        self.Options.FocusControl();
                });
            };
        }
    }
    return module;
});