define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu',],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            ViewModel: function (classID, id, IsEditable, OnChange) {
                var self = this;
                {//variables
                    self.attachmentsControl = null;
                    self.attachmentsControlInitialized = ko.observable(false);
                    self.IsEditable = IsEditable;

                    self.IsEditable.subscribe(function (newValue) {
                        if (self.attachmentsControlInitialized()) {
                            self.attachmentsControl.ReadOnly(!newValue);
                        }
                    });
                }
                //when object changed
                self.init = function (obj) {
                };
                //
                //when tab selected
                self.AfterRender = function () {
                    require(['fileControl'], function (fcLib) {
                        var attachmentsElement = $(".orgstructure-attachments").find('.documentList');
                        if (attachmentsElement.length == 0) {
                            setTimeout(self.AfterRender, 100);
                            return;
                        }
                        //
                        if (self.attachmentsControl == null) {
                            self.attachmentsControl = new fcLib.control(
                                attachmentsElement,
                                '.orgstructure-attachments',
                                '.orgstructure-addAttachmentBtn',
                                '../UI/Lists/Settings/OrgStructure.Attachments.FileControl');
                            self.attachmentsControlInitialized(true);
                            if (OnChange) self.attachmentsControl.OnChange = OnChange;
                        }
                        self.attachmentsControl.ReadOnly(!self.IsEditable());
                        self.attachmentsControl.RemoveFileAvailable(true);
                        if (id != null) {
                            self.attachmentsControl.Initialize(id);
                        }
                        //
                        if (self.attachmentsControl.IsLoaded() == false)
                            self.attachmentsControl.Load(attachmentsElement)
                    });
                };
                self.CanRemoveAll = ko.computed(function () {
                    if (!self.attachmentsControlInitialized()) return false;
                    return !self.attachmentsControl.ReadOnly() && self.attachmentsControl.RemoveFileAvailable() && self.attachmentsControl.Items().length > 0;
                });
                self.CanDownloadAll = ko.computed(function () {
                    if (!self.attachmentsControlInitialized()) return false;
                    return self.attachmentsControl.Items().length > 0;
                });
                self.DownloadArchive = function () {
                    if (!self.attachmentsControlInitialized()) return false;
                    return self.attachmentsControl.DownloadArchive();
                };
                self.RemoveAll = function () {
                    if (!self.attachmentsControlInitialized()) return false;
                    return self.attachmentsControl.RemoveAll();
                };
                //
                //when tab validating
                self.validate = function () {
                    vm.object().files(self.attachmentsControl != null ? self.attachmentsControl.GetData() : []);
                    //
                    if (self.attachmentsControl != null) {
                        if (!self.attachmentsControl.IsAllFilesUploaded()) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('WaitFilesUploading'));
                            });
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                        return true;
                };
                //
                //when tab unload
                self.dispose = function () {
                    if (self.attachmentsControl != null)
                        self.attachmentsControl.RemoveUploadedFiles();
                };
            }
        };
        return module;
    });