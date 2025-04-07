define(['knockout', 'jquery', 'ajax', 'selectControl'],
    function (ko, $, ajaxLib, scLib) {
        var module = {
            ViewModel: function (Organization, Subdivision, IsEditModeActive) {
                var self = this;
                {//variables
                    self.Organization = Organization;
                    self.Subdivision = Subdivision;
                    self.IsEditModeActive = IsEditModeActive;
                }
                //
                //when object changed
                self.init = function (obj) {
                };
                //
                //when tab selected
                self.AfterRender = function () {
                    if (self.IsEditModeActive()) {
                        self.InitializeOrganizationSelector();
                        self.InitializeSubdivisionSelector();
                    }
                };
                self.ajaxControl_loadOrganizations = new ajaxLib.control();
                self.organizationSelector = null;
                self.InitializeOrganizationSelector = function () {
                    var retD = $.Deferred();
                    var deffered = $.Deferred();
                    var $regionOrganization = $(".orgstructure-location").find('.orgstructure-organizationSelector');
                    //
                    if (!self.organizationSelector) {
                        self.organizationSelector = new scLib.control('../UI/Lists/Settings/OrgStructure.SelectControl');
                        self.organizationSelector.init($regionOrganization,
                            {
                                AlwaysShowTitle: false,
                                IsSelectMultiple: false,
                                AllowDeselect: false,
                                DisplaySelectionAsSearchText: true,
                                OnSelect: self.OnOrganizationSelected
                            }, deffered.promise());
                    } else {
                        self.organizationSelector.ClearItemsList();
                        $.when(deffered).done(function (values) {
                            self.organizationSelector.AddItemsToControl(values);
                        });
                    }
                    // 
                    self.ajaxControl_loadOrganizations.Ajax($regionOrganization,
                        {
                            dataType: "json",
                            method: 'GET',
                            url: '/assetApi/GetAllOrganizationList'
                        },
                        function (newData) {
                            if (newData != null && newData.Result === 0 && newData.Data) {
                                var retval = [];
                                //
                                newData.Data.forEach(function (el) {
                                    retval.push({
                                        ID: el.ID,
                                        ClassID: el.ClassID,
                                        Name: el.Name,
                                        Checked: (self.Organization() && self.Organization().ID) == el.ID
                                    });
                                });
                                //
                                deffered.resolve(retval);
                            }
                            else deffered.resolve();
                            //
                            $.when(self.organizationSelector.$initializeCompleted).done(function () {
                                retD.resolve();
                            });
                        });
                    //
                    return retD.promise();
                };
                self.OnOrganizationSelected = function (organization) {
                    if (!organization || organization.Checked === false) {
                        self.Organization(null);
                        self.Subdivision(null);
                    } else {
                        self.Organization(organization);
                        self.Subdivision(null);
                    }

                    //
                    $.when(self.InitializeSubdivisionSelector()).done(function () {

                    });
                };
                //
                self.ajaxControl_loadSubdivisions = new ajaxLib.control();
                self.subdivisionSelector = null;
                self.InitializeSubdivisionSelector = function () {
                    var retD = $.Deferred();
                    var deffered = $.Deferred();
                    var $regionSubdivision = $(".orgstructure-location").find('.orgstructure-subdivisionSelector');
                    //
                    if (!self.subdivisionSelector) {
                        self.subdivisionSelector = new scLib.control('../UI/Lists/Settings/OrgStructure.SelectControl');
                        self.subdivisionSelector.init($regionSubdivision,
                            {
                                AlwaysShowTitle: false,
                                IsSelectMultiple: false,
                                AllowDeselect: false,
                                DisplaySelectionAsSearchText: true,
                                OnSelect: self.OnSubdivisionSelected
                            }, deffered.promise());
                    }
                    else {
                        self.subdivisionSelector.ClearItemsList();
                        $.when(deffered).done(function (values) {
                            self.subdivisionSelector.AddItemsToControl(values);
                        });
                    }
                    //
                    var objectID = self.Organization() && self.Organization().ID;
                    var objectClassID = self.Organization() && self.Organization().ClassID;
                    var param = {
                        ObjectID: !!objectID ? objectID : -1,
                        ObjectClassID: !!objectClassID ? objectClassID : -1,
                    };
                    self.ajaxControl_loadSubdivisions.Ajax($regionSubdivision,
                        {
                            dataType: "json",
                            method: 'GET',
                            url: '/assetApi/GetSubdivisionListByOrganization?' + $.param(param)
                        },
                        function (newData) {
                            if (newData != null && newData.Result === 0 && newData.Data) {
                                var retval = [];
                                //
                                retval.push({
                                    ID: null,
                                    ClassID: null,
                                    Name: '-',
                                    Checked: self.Subdivision() == null
                                });
                                //
                                newData.Data.forEach(function (el) {
                                    retval.push({
                                        ID: el.ID,
                                        ClassID: el.ClassID,
                                        Name: getParentSubdivisionName(el) + el.Name,
                                        Checked: (self.Subdivision() && self.Subdivision().ID) == el.ID
                                    });
                                });
                                //

                                deffered.resolve(retval.sort(compareSubdivisions));
                            }
                            else deffered.resolve();
                            //
                            $.when(self.subdivisionSelector.$initializeCompleted).done(function () {
                                retD.resolve();
                            });
                        });
                    //
                    return retD.promise();
                };
                getParentSubdivisionName = function (subdivision) {
                    if (!subdivision || !subdivision.ParentSubdivision) {
                        return '';
                    }

                    return getParentSubdivisionName(subdivision.ParentSubdivision) + subdivision.ParentSubdivision.Name + " / ";
                }
                compareSubdivisions = function (a, b) {
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
                self.SubdivisionFullName = ko.computed(function () {
                    var subdivisionName = self.Subdivision() && self.Subdivision().Name;
                    return getParentSubdivisionName(self.Subdivision()) + (!!subdivisionName ? subdivisionName : '-');
                });
                //
                 self.OnSubdivisionSelected = function (subdivision) {
                    if (!subdivision || subdivision.Checked === false) {
                        self.Subdivision(null);
                        //self.ImplementFilter();
                        return;
                    }
                    //
                    self.Subdivision(subdivision);
                    //
                };
                //
                //when tab unload
                self.dispose = function () {

                };
            }
        };
        return module;
    });