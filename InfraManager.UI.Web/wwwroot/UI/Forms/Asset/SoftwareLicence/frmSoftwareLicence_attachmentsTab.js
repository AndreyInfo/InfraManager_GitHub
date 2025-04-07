define(['knockout', 'jquery', 'ajax'], function (ko, $, ajax) {
    var module = {
        Tab: function (vm) {
            var self = this;
            //
            self.Name = getTextResource('Contract_AttachmentListTab');
            self.Template = '../UI/Forms/Asset/Contracts/frmContractRegistration_attachmentsTab';
            self.IconCSS = 'attachmentsTab';
            //
            {//variables
                self.attachmentsControl = null;
            }
            //
            //when object changed
            self.Initialize = function (obj) {
            };
            //
            self.IsVisible = ko.observable(true);
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
                    if (self.attachmentsControl == null) {
                        self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                    }
                    self.attachmentsControl.ReadOnly(false);
                    self.attachmentsControl.RemoveFileAvailable(true);
                    //
                    if (self.attachmentsControl.ObjectID != vm.object().ID()) {
                        self.attachmentsControl.Initialize(vm.object().ID());
                    } else if (self.attachmentsControl.IsLoaded() == false) {
                        self.attachmentsControl.Load(attachmentsElement);
                    }
                });
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