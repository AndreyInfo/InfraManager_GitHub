define(['knockout', 'jquery', 'ajax'], function (ko, $, ajax) {
    var module = {
        Tab: function (vm) {
            var self = this;
            //
            self.Name = getTextResource('Contract_AttachmentListTab');
            self.Template = '../UI/Forms/Asset/Contracts/frmContract_attachmentsTab';
            self.IconCSS = 'attachmentsTab';
            //
            self.IsVisible = ko.observable(true);
            //
            {//variables
                self.attachmentsControl = null;
            }
            //
            {//events
                self.CanUpdate_handle = vm.CanUpdate.subscribe(function (newValue) {
                    if (self.attachmentsControl != null)
                        self.attachmentsControl.RemoveFileAvailable(newValue);
                    if (self.attachmentsControl != null)
                        self.attachmentsControl.ReadOnly(!newValue);
                });
            }
            //
            //when object changed
            self.init = function (obj) {
            };
            //
            //when tab selected
            self.load = function () {
                require(['fileControl'], function (fcLib) {
                    var attachmentsElement = $('#' + vm.frm.GetRegionID()).find('.documentList');
                    if (attachmentsElement.length == 0) {
                        setTimeout(self.load, 100);
                        return;
                    }
                    //
                    if (self.attachmentsControl != null) {
                        if (self.attachmentsControl.ObjectID != vm.object().ID())//previous object  
                            self.attachmentsControl.RemoveUploadedFiles();
                    }
                    if (self.attachmentsControl == null) {
                        self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                    }
                    self.attachmentsControl.ReadOnly(!vm.CanUpdate());
                    self.attachmentsControl.RemoveFileAvailable(vm.CanUpdate());
                    //
                    if (self.attachmentsControl.ObjectID != vm.object().ID())
                        self.attachmentsControl.Initialize(vm.object().ID());
                    else if (self.attachmentsControl.IsLoaded() == false)
                        self.attachmentsControl.Load(attachmentsElement)
                });
            };
            //
            //when tab unload
            self.dispose = function () {
                if (self.attachmentsControl != null && !self.attachmentsControl.IsAllFilesUploaded())
                    require(['sweetAlert'], function () {
                        swal({
                            title: getTextResource('UploadedFileNotFoundAtServerSide'),
                            text: getTextResource('FormClosingQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: true,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                        function (value) {
                            if (value == true) {
                                self.attachmentsControl.StopUpload();//
                                self.CanUpdate_handle.dispose();
                            }
                        });
                    });
            };
        }
    };
    return module;
});