define(['knockout', 'jquery', 'ajax', 'formControl',
    './SoftwareLicence',
    './frmSoftwareLicenceAdd_generalTab',
    './frmSoftwareLicenceAdd_attachmentsTab',
    'usualForms', 'dateTimeControl', 'jqueryStepper'],
    function (ko, $, ajax, formControl,
        m_objects,
        tab_general,
        tab_attachments,
        fhModule, dtLib) {
        var module = {
            ViewModel: function ($region) {
                var self = this;

                self.region = $region;
                self.asset = ko.observable(null);
                //Объект лицензии
                self.object = ko.observable(null);
                self.object_handle = self.object.subscribe(function (newObject) {
                    if (newObject == null || !newObject.ID()) {
                        return;
                    }
                    self.tabList().forEach(function (tab) {
                        tab.init(newObject);
                    });
                    if (self.tabActive()) {
                        self.tabActive().load();//reload active tab
                    }
                });

                //editors
                {
                    //редактирование названия
                    self.editName = function () {
                        showSpinner();
                        var object = self.object();

                        require(['usualForms'], function (fhModule) {
                            var fh = new fhModule.formHelper(true);
                            var options = {
                                ID: object.ID(),
                                objClassID: object.ClassID,
                                fieldName: 'Name',
                                fieldFriendlyName: getTextResource('SoftwareLicence_Description'),
                                oldValue: object.Name(),
                                allowNull: true,
                                maxLength: 500,
                                onSave: function (newText) {
                                    object.Name(newText);
                                },
                                nosave: true
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                        });
                    };

                    self.EditInventoryNumber = function () {
                        showSpinner();
                        var object = self.object();
                        require(['usualForms'], function (fhModule) {
                            var fh = new fhModule.formHelper(true);
                            var options = {
                                ID: object.ID(),
                                objClassID: object.ClassID(),
                                fieldName: 'InventoryNumber',
                                fieldFriendlyName: getTextResource('Repair_InventoryNumber'),
                                oldValue: object.InventoryNumber(),
                                allowNull: true,
                                maxLength: 50,
                                ReplaceAnyway: true,
                                onSave: function (newText) {
                                    object.InventoryNumber(newText);
                                },
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                        });
                    };
                }

                //
                self.validate = function () {
                    if (!self.object().Name() || self.object().Name().trim().length == 0) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('SoftwareLicenceAdd_SoftwareLicenceNameShouldBeSet'));
                        });
                        return false;
                    }

                    if (!self.object().SoftwareLicenceModelID()) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('SoftwareLicenceAdd_SoftwareLicenceModelShouldBeSet'));
                        });
                        return false;
                    }

                    if (self.object().BeginDate() && self.object().EndDate()) {
                        var dateStart = dtLib.GetMillisecondsSince1970(self.object().BeginDateDT());
                        var dateEnd = dtLib.GetMillisecondsSince1970(self.object().EndDateDT());
                        if (dateStart > dateEnd) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SoftwareLicenceAdd_CheckDate'));
                            });
                            return false;
                        }
                    }

                    for (var i = 0; i < self.tabList().length; i++)
                        if (self.tabList()[i].validate() == false) {
                            return false;
                        }

                    return true;
                };

                self.raiseObjectModified = function () {
                    var object = self.object();
                };

                //tabs
                {
                    let arrayTabs =
                        [
                            new tab_general.Tab(self),      //0
                            new tab_attachments.Tab(self),  //1
                        ];

                    self.tabList = ko.observableArray(arrayTabs);
                    //
                    self.tabActive = ko.observable(null);
                    self.tabActive_handle = self.tabActive.subscribe(function (selectedTab) {
                        if (selectedTab.hasOwnProperty('load')) {
                            selectedTab.load();
                        }
                    });
                    //
                    self.selectTabClick = function (tab) {
                        self.tabActive(tab);
                    };
                }
                //
                self.dispose = function () {
                    self.object().ajaxControl.Abort();
                    //
                    self.object_handle.dispose();
                    self.tabActive_handle.dispose();
                    self.tabList().forEach(function (tab) {
                        if (tab.hasOwnProperty('dispose')) {
                            tab.dispose();
                        }
                    });
                };
                //rendering
                {
                    self.sizeChanged = function () {
                        var width = self.frm.GetInnerWidth();
                        var height = self.frm.GetInnerHeight();
                        $('#' + self.frm.GetRegionID()).find('.frmSoftwareLicence').css('width', width + 'px').css('height', height + 'px');
                        //
                        var $content = $('#' + self.frm.GetRegionID()).find('.content');
                        $content.find('.tabActive').css('height',
                            Math.max(
                                $content.innerHeight() -
                                $content.find('.identificationRow').outerHeight(true) -
                                parseInt($content.find('.tabActive').css('margin-top')) - 2 * parseInt($content.find('.tabActive').css('margin-bottom')),
                                0) + 'px');
                    };
                    self.afterRender = function () {
                        self.frm.SizeChanged();
                    };
                }
                //initialization (пустой объект, выделяем первый таб)
                {
                    //Объект лицензии
                    self.object(new m_objects.softwareLicence());//fill object
                    self.selectTabClick(self.tabList()[0]);//init general tab

                    self.IsReadOnly = ko.observable(false);
                    self.CanEdit = ko.observable(true);

                    self.reload = function (id, classID) {

                        var retD = $.Deferred();
                        var refreshObject = new m_objects.softwareLicence();
                        self.object(refreshObject);

                        return retD.promise();
                    };

                    self.initGeneralTab = function () {
                        self.selectTabClick(self.tabList()[0]);
                    }
                }
            },

            ShowDialog: function (mode, isSpinnerActive) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var frm = undefined;
                var bindElement = null;

                var buttons = [];
                var bSaveClose = {
                    text: getTextResource('SoftwareLicenceAddButtonSaveClose'),
                    click: function () {
                        if (!vm.validate())
                            return;
                        $.when(vm.object().Add(false)).done(function (result) {
                            if (result) {
                                forceClose = true;
                                frm.Close();
                            }
                        });
                    }
                }

                var bSaveOpenForm = {
                    text: getTextResource('SoftwareLicenceAddButtonSaveOpenForm'),
                    click: function () {
                        if (!vm.validate())
                            return;
                        $.when(vm.object().Add(true)).done(function (result) {
                            if (result) {
                                forceClose = true;
                                frm.Close();
                            }
                        });
                    }
                }

                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () {
                        require(['sweetAlert'], function () {
                            swal({
                                title: getTextResource('FormClosingQuestion'),
                                showCancelButton: true,
                                closeOnConfirm: true,
                                closeOnCancel: true,
                                confirmButtonText: getTextResource('ButtonOK'),
                                cancelButtonText: getTextResource('ButtonCancel')
                            },
                                function (value) {
                                    if (value == true) {
                                        forceClose = true;
                                        setTimeout(function () {
                                            frm.Close();
                                        }, 300);//TODO? close event of swal
                                    }
                                });
                        });
                    }
                }

                buttons.push(bSaveClose);
                buttons.push(bSaveOpenForm);
                buttons.push(bCancel);

                frm = new formControl.control(
                    'region_frmSoftwareLicenceAdd_',//form region prefix
                    'setting_frmSoftwareLicenceAdd_',//location and size setting
                    getTextResource('SoftwareLicenceAddTitle'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    1000, 760,//minSize
                    buttons,//form buttons
                    function () {
                        ko.cleanNode(bindElement);
                        vm.dispose();
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicenceAdd\', afterRender: afterRender}"'//attributes of form region
                );
                if (!frm.Initialized) {//form with that region and settingsName was open
                    hideSpinner();
                    return;
                }
                var $region = $('#' + frm.GetRegionID());
                var vm = new module.ViewModel($region);
                vm.frm = frm;
                var oldSizeChanged = frm.SizeChanged;
                frm.SizeChanged = function () {
                    oldSizeChanged();
                    vm.sizeChanged();
                };
                //
                frm.ExtendSize(1000, 760);//normal size
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);

                //инициализация списков на основной вкладке
                vm.initGeneralTab();

                vm.object().CanEdit(true);
                vm.CanEdit(true);

                $.when(frm.Show()).done(function (frmD) {
                    hideSpinner();
                });
            }
        };
        return module;
    });