define(['knockout', 'jquery', 'ttControl', 'ajax', 'jqueryMouseWheel'], function (ko, $, tclib, ajaxLib) {
    var module = {
        ViewModel: function () {
            var self = this;
            self.ajaxControl = new ajaxLib.control();
            //
            {//current view of list               
                self.viewName = ko.observable();
                self.viewName.subscribe(function (newValue) {
                    $.when(userD).done(function (user) {
                        if (user.ViewNameSD == newValue)
                            return;
                        //            
                        if (self.tableModel) {
                            if (self.tableModel.resetCount)
                                self.tableModel.resetCount(true);
                            self.tableModel.clearAllInfos();
                        }
                         
                        showSpinner($('#regionListMode')[0]);
                        //
                        self.ajaxControl.Ajax(null,
                            {
                                contentType: "application/json",
                                url: '/api/UserSettings',
                                method: 'POST',
                                data: JSON.stringify({ ViewNameSD: newValue }),
                                dataType: "text"
                            },
                            function () {
                                hideSpinner($('#regionListMode')[0]);
                                    user.ViewNameSD = newValue;
                                    //
                                    if (self.tableModel && self.filtersModel)
                                        $.when(self.filtersModel.Load()).done(function () {
                                            self.tableModel.reload();
                                        });
                                
                            });
                    });
                });
                $.when(userD).done(function (user) {
                    user.SDViewNameSubscribe = self.viewName;
                })
            }
            //
            {//css of buttons
                self.MyWorkplaceClass = ko.computed(function () {
                    return self.viewName() === 'CommonForTable' ? 'b-content-table__myworkplace_active' : 'b-content-table__myworkplace';
                });
                self.CallClass = ko.computed(function () {
                    //return self.viewName() === 'CallForTable' ? 'b-content-table__call_active' : (self.viewName() === 'LiteCallForTable' ? 'b-content-table__litecall_active' : 'b-content-table__call');
                    return self.viewName() === 'CallForTable' ? 'b-content-table__call_active' : 'b-content-table__call';
                });
                self.WorkOrderClass = ko.computed(function () {
                    return self.viewName() === 'WorkOrderForTable' ? 'b-content-table__workorder_active' : 'b-content-table__workorder';
                });
                self.ProblemClass = ko.computed(function () {
                    return self.viewName() === 'ProblemForTable' ? 'b-content-table__problem_active' : 'b-content-table__problem';
                });
                self.NegotiationClass = ko.computed(function () {
                    return self.viewName() === 'NegotiationForTable' ? 'b-content-table__negotiation_active' : 'b-content-table__negotiation';
                });
                self.MyCallClass = ko.computed(function () {
                    return self.viewName() === 'ClientCallForTable' ? 'b-content-table__mycalls_active' : 'b-content-table__mycalls';
                });
                self.CustomControlClass = ko.computed(function () {
                    return self.viewName() === 'CustomControlForTable' ? 'b-content-table__customcontrol_active' : 'b-content-table__customcontrol';
                });
                self.RFCClass = ko.computed(function () {
                    return self.viewName() === 'RFCForTable' ? 'b-content-table__rfc_active' : 'b-content-table__rfc';
                });
                self.MassIncidentClass = ko.computed(function () {
                    return self.viewName() === 'AllMassIncidentsList' ? 'b-content-table__mass-incident_active' : 'b-content-table__mass-incident';
                });
            }
            //
            self.tableModel = null; //модель таблицы, которой управляем
            self.ready = ko.observable(false);
            self.filtersModel = null; //модель управления фильтрами
            //
            {//click on buttons
                self.ShowCalls = function () {
                   self.viewName('CallForTable');
                    //if (self.viewName() != 'CallForTable')
                    //    self.viewName('CallForTable');
                    //else
                    //    self.viewName('LiteCallForTable');
                };
                self.ShowMassIncidents = function () {
                    self.viewName('AllMassIncidentsList');
                };
                self.ShowProblems = function () {
                    self.viewName('ProblemForTable');
                };
                self.ShowWorkOrders = function () {
                    self.viewName('WorkOrderForTable');
                };
                self.ShowRFCs = function () {
                    self.viewName('RFCForTable');
                };
                self.ShowMyWorkplace = function () {
                    self.viewName('CommonForTable');
                };
                self.ShowCustomControl = function () {
                    self.viewName('CustomControlForTable');
                };
                self.ShowNegotiations = function () {
                    self.viewName('NegotiationForTable');
                };
                self.ShowMyCalls = function () {
                    self.viewName('ClientCallForTable');
                };
            }
            //
            {//scrolling buttons
                self.ScrollButtonsVisible = ko.observable(true);
                self.OnScrollDownClick = function () {
                    var $scrollContainer = $('.b-content-table__left');
                    //
                    if ($scrollContainer.length > 0) {
                        var oldvalue = $scrollContainer.scrollTop();
                        if ((oldvalue + $scrollContainer.height()) < $scrollContainer[0].scrollHeight) {
                            var newvalue = oldvalue + 400;
                            $scrollContainer.animate({ scrollTop: newvalue }, 800);
                        }
                    }
                };
                self.OnScrollUpClick = function () {
                    var $scrollContainer = $('.b-content-table__left');
                    //
                    if ($scrollContainer.length > 0) {
                        var oldvalue = $scrollContainer.scrollTop();
                        if (oldvalue > 0) {
                            var newvalue = oldvalue - 400 >= 0 ? oldvalue - 400 : 0;
                            $scrollContainer.animate({ scrollTop: newvalue }, 800);
                        }
                    }
                };
                self.InitScroll = function () {
                    var $scrollcontainer = $('.b-content-table__left');
                    $scrollcontainer.mousewheel(function (event, delta) {
                        this.scrollTop -= (delta * 30);
                        event.preventDefault();
                    });
                };
                self.UpdateScrollButtonsVisibility = function () {
                    var $region = $('.b-content-table__left');
                    if ($region.length > 0) {
                        if ($region[0].scrollHeight > $region.height())
                            self.ScrollButtonsVisible(true);
                        else self.ScrollButtonsVisible(false);
                    }
                    else {
                        self.ScrollButtonsVisible(true);
                        setTimeout(self.UpdateScrollButtonsVisibility, 200);//try again later
                    };
                };
            }
            //
            //filter buttons
            self.AfterRender = function () {
                $.when(userD).done(function (user) {
                    var negotiationD = $.Deferred();
                    //
                    $('#listModeMyCalls').css('display', 'block');
                    $('#listModeCustomControl').css('display', 'block');
                    $('#listModeMyCallsText').css('display', 'block');
                    $('#listModeCustomControlText').css('display', 'block');
                    //
                    if (user.HasRoles) {
                        var myWorkplaceD = $.Deferred();
                        $.when(
                            operationIsGrantedD(373),//OPERATION_SD_General_Owner
                            operationIsGrantedD(358),//OPERATION_SD_General_Executor
                            operationIsGrantedD(357),//OPERATION_SD_General_Administrator                            
                            operationIsGrantedD(597),//OPERATION_Call_ShowCallsForITSubdivisionInWeb
                            operationIsGrantedD(706),//OPERATION_Call_ShowAllCallsForITSubdivisionInWeb
                            operationIsGrantedD(599),//OPERATION_WorkOrder_ShowWorkOrdersForITSubdivisionInWeb
                            operationIsGrantedD(707),//OPERATION_WorkOrder_ShowAllWorkOrdersForITSubdivisionInWeb
                            operationIsGrantedD(598)//OPERATION_Problem_ShowProblemsForITSubdivisionInWeb
                            ).done(function (owner, executor, administrator, callsForITSubdivision, allCallsForITSubdivision, workOrdersForITSubdivision, allWorkOrdersForITSubdivision, problemsForITSubdivision) {
                                if (owner == true || executor == true || administrator == true ||
                                    callsForITSubdivision == true || allCallsForITSubdivision == true ||
                                    workOrdersForITSubdivision == true || allWorkOrdersForITSubdivision == true ||
                                    problemsForITSubdivision == true) {
                                    $('#listModeMyWorkplace').css('display', 'block');
                                    $('#listModeMyWorkplaceText').css('display', 'block');
                                    myWorkplaceD.resolve(true);
                                }
                                else {
                                    $('#listModeMyWorkplace').remove();
                                    $('#listModeMyWorkplaceText').remove();
                                    myWorkplaceD.resolve(false);
                                }
                                self.UpdateScrollButtonsVisibility();
                            });
                        //
                        var callsD = $.Deferred();
                        $.when(operationIsGrantedD(647)).done(function (result) {
                            if (result == true) {
                                $('#listModeCalls').css('display', 'block');//OPERATION_SD_General_Calls_View = 647
                                $('#listModeCallsText').css('display', 'block');
                                callsD.resolve(true);
                            }
                            else {
                                $('#listModeCalls').remove();
                                $('#listModeCallsText').remove();
                                callsD.resolve(false);
                            }
                            self.UpdateScrollButtonsVisibility();
                        });
                        //
                        var massIncidentsD = $.Deferred();
                        $.when(operationIsGrantedD(981)).done(function (result) {
                            if (result == true) {
                                $('#listModeMassIncidents').css('display', 'block');//OPERATION_SD_General_Calls_View = 647
                                $('#listModeMassIncidentsText').css('display', 'block');
                                massIncidentsD.resolve(true);
                            }
                            else {
                                $('#listModeMassIncidents').remove();
                                $('#listModeMassIncidentsText').remove();
                                massIncidentsD.resolve(false);
                            }
                            self.UpdateScrollButtonsVisibility();
                        });
                        //
                        var workOrdersD = $.Deferred();
                        $.when(operationIsGrantedD(649)).done(function (result) {
                            if (result == true) {
                                $('#listModeWorkOrders').css('display', 'block');//OPERATION_SD_General_WorkOrders_View = 649
                                $('#listModeWorkOrdersText').css('display', 'block');
                                workOrdersD.resolve(true);
                            }
                            else {
                                $('#listModeWorkOrders').remove();
                                $('#listModeWorkOrdersText').remove();
                                workOrdersD.resolve(false);
                            }
                            self.UpdateScrollButtonsVisibility();
                        });
                        //
                        var problemsD = $.Deferred();
                        $.when(operationIsGrantedD(648)).done(function (result) {
                            if (result == true) {
                                $('#listModeProblems').css('display', 'block');//OPERATION_SD_General_Problems_View = 648
                                $('#listModeProblemsText').css('display', 'block');
                                problemsD.resolve(true);
                            }
                            else {
                                $('#listModeProblems').remove();
                                $('#listModeProblemsText').remove();
                                problemsD.resolve(false);
                            }
                            self.UpdateScrollButtonsVisibility();
                        });
                        var rfcD = $.Deferred();
                        $.when(operationIsGrantedD(707)).done(function (result) {
                            if (result == true) {
                                $('#listModeRFCs').css('display', 'block');//OPERATION_SD_General_RFCs_View = 707
                                $('#listModeRFCText').css('display', 'block');
                                rfcD.resolve(true);
                            }
                            else {
                                $('#listModeRFCs').remove();
                                $('#listModeRFCText').remove();
                                rfcD.resolve(false);
                            }
                            self.UpdateScrollButtonsVisibility();
                        });
                        //
                        $.when(myWorkplaceD, callsD, massIncidentsD, workOrdersD, problemsD, rfcD).done(function (myWResult, callsResult, massIncidentsResult, woResult, pbResult, pbRfc) {
                            if (myWResult === false && callsResult === false && !massIncidentsResult && woResult === false && pbResult === false && pbRfc===false)
                                $('.b-content-table__workplaceIconGroup.b-content-table__workplaceIcon_space').removeClass('b-content-table__workplaceIcon_space');
                        });
                        //
                        $.when(operationIsGrantedD(303)).done(function (result) {
                            negotiationD.resolve({ canVote: result, userID: user.UserID });//OPERATION_SD_General_VotingUser = 303
                        });
                    }
                    else {
                        $('#listModeMyWorkplace').remove();
                        $('#listModeCalls').remove();
                        $('#listModeWorkOrders').remove();
                        $('#listModeProblems').remove();
                        $('#listModeMyWorkplaceText').remove();
                        $('#listModeCallsText').remove();
                        $('#listModeWorkOrdersText').remove();
                        $('#listModeProblemsText').remove();
                        negotiationD.resolve({ canVote: false, userID: user.UserID });
                        //
                        $('.b-content-table__workplaceIconGroup.b-content-table__workplaceIcon_space').removeClass('b-content-table__workplaceIcon_space');
                    }
                    self.UpdateScrollButtonsVisibility();
                    self.InitScroll();
                    $(window).resize(self.UpdateScrollButtonsVisibility);
                    //
                    $.when(negotiationD).done(function (result) {
                        var unlockNegotiationButton = function () {
                            $('#listModeNegotiations').css('display', 'block');
                            $('#listModeNegotiationsText').css('display', 'block');
                        };
                        if (result.canVote) {//есть операция
                            unlockNegotiationButton();
                            self.UpdateScrollButtonsVisibility();
                        }
                        else {//а есть согласования, где я участник?
                            var negotiation_ajaxControl = new ajaxLib.control();
                            negotiation_ajaxControl.Ajax(null,
                                {
                                    url: '/api/negotiations?userID=' + result.userID + '&orderByProperty=IMObjID&take=1&ascending=false',
                                },
                                function (negotiations) {
                                    var exists = negotiations.length > 0;
                                    if (exists == true)
                                        unlockNegotiationButton();
                                    else {
                                        $('#listModeNegotiations').remove();
                                        $('#listModeNegotiationsText').remove();
                                    }
                                    self.UpdateScrollButtonsVisibility();
                                });
                        }
                    });
                    //
                    self.viewName(user.ViewNameSD);
                });
            };
        }
    }
    return module;
});