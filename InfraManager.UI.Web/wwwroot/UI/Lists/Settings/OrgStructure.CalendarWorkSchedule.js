define(['knockout', 'jquery', 'ajax', 'selectControl'],
    function (ko, $, ajaxLib, scLib) {
        var module = {
            ViewModel: function (Schedule, IsEditable) {
                var self = this;
                {//variables
                    self.Schedule = Schedule;
                    self.IsEditable = IsEditable;
                }
                //
                self.ScheduleName = ko.computed(function () {
                    var scheduleName = self.Schedule() && self.Schedule().Name;
                    return !!scheduleName ? scheduleName : '-';
                });
                //
                //when object changed
                self.init = function (obj) {
                };
                //
                //when tab selected
                self.AfterRender = function () {
                    if (self.IsEditable()) {
                        self.InitializeScheduleSelector();
                    }
                };
                self.ajaxControl_loadSchedules = new ajaxLib.control();
                self.scheduleSelector = null;
                self.InitializeScheduleSelector = function () {
                    var retD = $.Deferred();
                    var deffered = $.Deferred();
                    var $regionSchedule = $(".orgstructure-schedule").find('.orgstructure-scheduleSelector');
                    //
                    if (!self.scheduleSelector) {
                        self.scheduleSelector = new scLib.control('../UI/Lists/Settings/OrgStructure.SelectControl');
                        self.scheduleSelector.init($regionSchedule,
                            {
                                AlwaysShowTitle: false,
                                IsSelectMultiple: false,
                                AllowDeselect: false,
                                DisplaySelectionAsSearchText: true,
                                OnSelect: self.OnSchedueSelected
                            }, deffered.promise());
                    } else {
                        self.scheduleSelector.ClearItemsList();
                        $.when(deffered).done(function (values) {
                            self.scheduleSelector.AddItemsToControl(values);
                        });
                    }
                    // 
                    self.ajaxControl_loadSchedules.Ajax($regionSchedule,
                        {
                            dataType: "json",
                            method: 'GET',
                            url: '/assetApi/GetCalendarWorkScheduleList'
                        },
                        function (newData) {
                            if (newData != null && newData.Result === 0 && newData.Data) {
                                var retval = [];
                                //
                                retval.push({
                                    ID: null,
                                    ClassID: null,
                                    Name: '-',
                                    Checked: self.Schedule() == null
                                });
                                //
                                newData.Data.forEach(function (el) {
                                    retval.push({
                                        ID: el.ID,
                                        Name: el.Name,
                                        Checked: (self.Schedule() && self.Schedule().ID) == el.ID
                                    });
                                });
                                //
                                deffered.resolve(retval.sort(compareSchedules));
                            }
                            else deffered.resolve();
                            //
                            $.when(self.scheduleSelector.$initializeCompleted).done(function () {
                                retD.resolve();
                            });
                        });
                    //
                    return retD.promise();
                };
                //
                compareSchedules = function (a, b) {
                    const nameA = a.Name.toUpperCase();
                    const nameB = b.Name.toUpperCase();

                    let comparison = 0;
                    if (nameA == '-') {
                        comparison = -1;
                    } else if (nameB == '-') {
                        comparison = 1;
                    } else if (nameA > nameB) {
                        comparison = 1;
                    } else if (nameA < nameB) {
                        comparison = -1;
                    }
                    return comparison;
                };
                //
                self.OnSchedueSelected = function (schedule) {
                    if (!schedule || schedule.Checked === false) {
                        self.Schedule(null);
                    } else {
                        self.Schedule(schedule);
                    }
                };
                //
                //when tab unload
                self.dispose = function () {

                };
            }
        };
        return module;
    });