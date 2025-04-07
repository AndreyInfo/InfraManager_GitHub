define(['knockout', 'jquery', 'ajax', 'formControl', 'sweetAlert'], function (ko, $, ajaxLib, fc) {
    var module = {
        ViewModel: function (kbArticle, $region, maxAttachmentSize) {
            var self = this;
            self.$region = $region;
            self.kbArticle = kbArticle;
            self.maxAttachmentSize = maxAttachmentSize;
            //
            self.frm = null;
            //
            self.createMailBody = function () {
                var header = 'font-size: 12px;font-style: italic;line-height: 14px;margin-bottom:20px;';
                var imBigText = 'font-family: Arial,Helvetica,sans-serif;font-weight: normal;font-size: 22px;';
                var kbaCaption = 'height: 25px;margin-right: 20px;overflow: hidden;';
                var kbaFieldName = 'line-height: normal;margin-top: 20px;margin-bottom: 10px;';
                var kbaHtml = 'margin-top: 5px;margin-left: 0;white-space: normal;font-size: 12px;color: #333;';
                var marginTop = 'margin-top: 20px;';
                var footer = 'font-size: 12px;font-style: italic;line-height: 14px;';
                //
                var html =
                    [
                        "<div style=\"" + header + "\" data-bind=\"restext: 'KBArticleFormCaption'\"></div>",

                        "<div style=\"" + kbaCaption + imBigText + "\" data-bind=\"text: Caption\"></div>",

                        "<div style=\"" + kbaFieldName + imBigText + "\" data-bind=\"restext: 'Description'\"></div>",
                        "<div style=\"" + kbaHtml + "\" data-bind=\"html: HTMLDescription\"></div>",

                        "<div style=\"" + kbaFieldName + imBigText + "\" data-bind=\"restext: 'Solution'\"></div>",
                        "<div style=\"" + kbaHtml + "\" data-bind=\"html: HTMLSolution\"></div>",

                        "<div style=\"" + kbaFieldName + imBigText + "\" data-bind=\"restext: 'Workaround'\"></div>",
                        "<div style=\"" + kbaHtml + "\" data-bind=\"html: HTMLAltSolution\"></div>",

                        "<div style=\"" + marginTop + footer + "\" data-bind=\"text: getTextResource('Author') + ' ' + AuthorFullName\"></div>",
                        "<div style=\"" + footer + "\" data-bind=\"text: getTextResource('PublicationDate') + ' ' + DateCreation\"></div>",
                        "<div style=\"" + footer + "\" data-bind=\"text: getTextResource('KBADataLastEdit') + ' ' + DateModified\"></div>",
                        "<div style=\"" + footer + "\" data-bind=\"text: getTextResource('KBASection') + ': ' + Section()\"></div>",
                        "<div style=\"" + footer + "\" data-bind=\"text: getTextResource('TypeOfArticle') + ' ' + TypeName\"></div>"

                    ].join("\n");
                //
                return html;
            };
            //
            self.htmlMailBobyControlD = $.Deferred();
            //
            self.htmlMailBobyControl = null;
            self.InitializeMailBoby = function () {
                var htmlElement = $region.find('.mailBody');
                if (self.htmlMailBobyControl == null)
                    require(['htmlControl'], function (htmlLib) {
                        self.htmlMailBobyControl = new htmlLib.control(htmlElement, false, false, true);//targetDiv, extendedMode, readOnly, noScrolling
                        //
                        self.htmlMailBobyControlD.resolve();
                        self.htmlMailBobyControl.OnHTMLChanged = function (htmlValue) {
                        };
                    });
                else
                    self.htmlMailBobyControl.Load(htmlElement);
            };
            //
            self.ToAddresses = ko.observable('');
            self.BccAddresses = ko.observable('');
            self.Subject = ko.observable(getTextResource('KBArticleFormCaption') + ': ' + self.kbArticle().Caption());
            //
            self.ajaxControl = new ajaxLib.control();
            self.SendEmail = function () {
                if (!self.ToAddresses()) {
                    swal(getTextResource('AddressMustBeSet'), '', 'info');
                    return;
                }
                //
                if (!self.Subject()) {
                    swal(getTextResource('MessageSubjectMustBeSet'), '', 'info');
                    return;
                }
                //
                var data = {
                    ToAddresses: self.ToAddresses(),
                    BccAddresses: self.BccAddresses(),
                    KBArticleID: self.kbArticle().ID,
                    Subject: self.Subject(),
                    HtmlBody: self.GetEmailBodyHtml(),
                    Files: self.attachmentsControl == null ? null : self.attachmentsControl.GetData(),
                };
                //
                self.ajaxControl.Ajax($region,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: data,
                        url: '/sdApi/SendEmail'
                    },
                    function (newVal) {
                        if (newVal.Result === 0) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('MailSendSuccessful'), '', 'info');
                                self.frm.Close();
                            });
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), newVal.Message, 'error');
                            });
                        }
                    });
            };
            //
            self.BindKbArticleToMailBody = function () {
                clearInterval(self.bindKbArticleToMailBodyPrivate);
                //
                self.bindKbArticleToMailBodyPrivate = setInterval(function () {//hack
                    var doc = self.htmlMailBobyControl.getDoc();
                    var el = doc.getElementsByClassName('htmlEditor-textArea')[0];
                    //
                    if (!el)
                        return;
                    //
                    if (ko.dataFor(el)) {
                        clearInterval(self.bindKbArticleToMailBodyPrivate);
                        hideSpinner($region[0]);
                        return;
                    }
                    //
                    ko.applyBindings(self.kbArticle, el);
                    self.htmlMailBobyControl.setFrameSize();
                }, 100);
            };
            //
            self.AfterRender = function () {
                showSpinner($region[0]);
                //
                self.InitializeMailBoby();
                //
                $.when(self.htmlMailBobyControlD).done(function () {
                    var html = self.createMailBody();
                    self.htmlMailBobyControl.SetHTML(html);
                    //
                    $.when(self.htmlMailBobyControl.frameD).done(function () {
                        //
                        self.BindKbArticleToMailBody();
                        //
                        self.LoadAttachmentsControl();
                    });
                });
            };

            //attachments
            {
                self.attachmentsControl = null;
                //
                self.LoadAttachmentsControl = function () {
                    if (!self.kbArticle())
                        return;
                    //
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsControl != null) {
                            if (self.attachmentsControl.ObjectID != self.kbArticle().ID)//previous object  
                                self.attachmentsControl.RemoveUploadedFiles();
                            else if (!self.attachmentsControl.IsAllFilesUploaded())//uploading
                            {
                                setTimeout(self.LoadAttachmentsControl, 1000);//try to reload after second
                                return;
                            }
                        }
                        if (self.attachmentsControl == null || self.attachmentsControl.IsLoaded() == false) {
                            var attachmentsElement = self.$region.find('.documentList');
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        }
                        self.attachmentsControl.ReadOnly(false);
                        self.attachmentsControl.RemoveFileAvailable(true);
                        self.attachmentsControl.Initialize(self.kbArticle().ID, true);//загружаем файлы статьи БЗ
                        self.attachmentsControl.MaxFileSize = self.maxAttachmentSize;
                    });
                };
            }

            {//toAddress, bccAddress
                self.toAddressList = ko.observableArray([]);
                self.updateToAddresses = function () {
                    self.ToAddresses('');
                    //
                    var str = '';
                    //
                    self.toAddressList().forEach(function (el) {
                        if (str.length !== 0)
                            str += ', ';
                        //
                        str += el.Email;
                    });
                    //
                    self.ToAddresses(str);
                };
                self.ClearToAddresses = function () {
                    self.toAddressList.removeAll();
                    self.updateToAddresses();
                };
                self.AddToAddressEmail = function () {
                    showSpinner();
                    var obj = self.kbArticle();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        $.when(userD).done(function (user) {
                            var options = {
                                ID: obj.ID,
                                objClassID: obj.ClassID,
                                fieldName: 'User',
                                fieldFriendlyName: getTextResource('SendMailToAddresses'),
                                oldValue: null,
                                object: ko.toJS(user),
                                searcherName: 'UserWithEmailSearcher',
                                searcherPlaceholder: getTextResource('EnterFIO'),
                                nosave: true,
                                onSave: function (objectInfo) {
                                    if (!objectInfo)
                                        return;
                                    //
                                    var exist = ko.utils.arrayFirst(self.toAddressList(), function (exItem) {
                                        return exItem.ID.toUpperCase() == objectInfo.ID.toUpperCase();
                                    });
                                    if (!exist) {
                                        var userInfo =
                                        {
                                            ID: objectInfo.UserInfo.ID,
                                            Email: objectInfo.UserInfo.Email
                                        };
                                        //
                                        self.toAddressList.push(userInfo);
                                        //
                                        self.updateToAddresses();
                                    }
                                    if (objectInfo && objectInfo.UserInfo && objectInfo.UserInfo.UserDeputyList)
                                        objectInfo.UserInfo.UserDeputyList.forEach(function (el) {
                                            var existDeputy = ko.utils.arrayFirst(self.toAddressList(), function (exItem) {
                                                return exItem.ID.toUpperCase() == el.ID.toUpperCase();
                                            });
                                            if (!existDeputy && el && el.Email && el.Email!='') {
                                                var userInfoDeputy =
                                                {
                                                    ID: el.ID,
                                                    Email: el.Email
                                                };
                                                //
                                                self.toAddressList.push(userInfoDeputy);
                                                //
                                                self.updateToAddresses();
                                            }
                                        });
                                }
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                        });
                    });
                };

                self.bccAddressList = ko.observableArray([]);
                self.updateBccAddresses = function () {
                    self.BccAddresses('');
                    //
                    var str = '';
                    //
                    self.bccAddressList().forEach(function (el) {
                        if (str.length !== 0)
                            str += ', ';
                        //
                        str += el.Email;
                    });
                    //
                    self.BccAddresses(str);
                };
                self.ClearBccAddresses = function () {
                    self.bccAddressList.removeAll();
                    self.updateBccAddresses();
                };
                self.AddBccAddressEmail = function () {
                    showSpinner();
                    var obj = self.kbArticle();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        $.when(userD).done(function (user) {
                            var options = {
                                ID: obj.ID,
                                objClassID: obj.ClassID,
                                fieldName: 'User',
                                fieldFriendlyName: getTextResource('SendMailHiddenCopy'),
                                oldValue: null,
                                object: ko.toJS(user),
                                searcherName: 'UserWithEmailSearcher',
                                searcherPlaceholder: getTextResource('EnterFIO'),
                                nosave: true,
                                onSave: function (objectInfo) {
                                    if (!objectInfo)
                                        return;
                                    //
                                    var exist = ko.utils.arrayFirst(self.bccAddressList(), function (exItem) {
                                        return exItem.ID.toUpperCase() == objectInfo.ID.toUpperCase();
                                    });
                                    if (!exist) {
                                        var userInfo =
                                        {
                                            ID: objectInfo.UserInfo.ID,
                                            Email: objectInfo.UserInfo.Email
                                        };
                                        //
                                        self.bccAddressList.push(userInfo);
                                        //
                                        self.updateBccAddresses();
                                    }
                                }
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                        });
                    });
                };
            }

            self.SizeChanged = function () {
                if (self.htmlMailBobyControl)
                    self.htmlMailBobyControl.setFrameSize();
            };

            self.GetEmailBodyHtml = function () {
                var doc = self.htmlMailBobyControl.getDoc();
                var el = doc.getElementsByClassName('htmlEditor-textArea')[0];
                //
                var retval = el.innerHTML;
                return retval;
            };
        },
        ShowDialog: function (kbArticle, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            var maxAttachmentSize = null;
            var ajax = new ajaxLib.control();
            //
            ajax.Ajax(null,
                {
                    dataType: "json",
                    method: 'POST',
                    url: '/sdApi/CheckMailServiceConnection'
                },
                function (newVal) {
                    if (newVal && newVal.IsConnected) {
                        maxAttachmentSize = newVal.MaxAttachmentSize;
                        //
                        $.when(userD).done(function (user) {
                            var frm = undefined;
                            //
                            var buttons = {};
                            buttons[getTextResource('SendMessage')] = function () {
                                vm.SendEmail();
                            };
                            var vm = null;
                            //
                            frm = new fc.control(
                                'region_kbaSendEmailForm',//form region prefix
                                'setting_kbaSendEmailForm',//location and size setting
                                getTextResource('Send_by_mail'),//caption
                                true,//isModal
                                true,//isDraggable
                                true,//isResizable
                                710, 520,//minSize
                                buttons,//form buttons
                                function () {
                                },//afterClose function
                                'data-bind="template: {name: \'../UI/Forms/KB/frmKBASendEmail\', afterRender: AfterRender}"'//attributes of form region
                            );
                            //
                            if (!frm.Initialized)
                                return;//form with that region and settingsName was open
                            //
                            frm.BeforeClose = function () {
                                hideSpinner();
                            };
                            //
                            var $region = $('#' + frm.GetRegionID());
                            vm = new module.ViewModel(kbArticle, $region, maxAttachmentSize);
                            vm.frm = frm;
                            //
                            frm.SizeChanged = function () {
                                var width = frm.GetInnerWidth();
                                var height = frm.GetInnerHeight();
                                //
                                vm.$region.css('width', width + 'px').css('height', height + 'px');
                                vm.SizeChanged();
                            };
                            //
                            ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                            $.when(frm.Show()).done(function () {
                                hideSpinner();
                            });
                        });
                    }
                    else {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('MailServiceUnAvailable'), '', 'info');
                            hideSpinner();
                            return;
                        });
                    }

                });
        }
    }
    return module;
});