define(['knockout', 'jquery', 'ajax', 'formControl', 'sweetAlert'], function (ko, $, ajaxLib, fc) {
    var module = {
        ViewModel: function (kbArticle, $region, maxAttachmentSize) {
            var self = this;
            self.$region = $region;
            self.kbArticle = ko.observable(kbArticle);
            self.SendEmail = ko.observable();
            self.canNote = ko.computed(function () {
                var tmpObj = self.kbArticle().Obj; 
                var showButton = false;
                if (tmpObj.ClassID == 701)// 701 заявка
                    showButton = true;
                return self.kbArticle().CanNote ? showButton : false;
            });
            self.maxAttachmentSize = maxAttachmentSize;
            //
            self.frm = null;
            //
            self.IsMassegeRadio = ko.observable('0');
            //
            self.createMailBodyD = $.Deferred();
            //
            self.createMailBody = function () {
                var sub = self.kbArticle().Subject ? self.kbArticle().Subject : '';
                self.Subject(sub);
                //
                var html = [];
                var tmpObj = self.kbArticle().Obj; 
                if (tmpObj)
                    $.when(self.GetEmailTemplate(tmpObj)).done(function (result) { 
                        html = result;
                        self.createMailBodyD.resolve(html);
                    });
                else
                self.createMailBodyD.resolve(html);
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
            self.Subject = ko.observable('');
            //
            //
            self.GetEmailTemplate = function (obj) {
                var retD = $.Deferred();
                //
                var data =
                {
                    ID: obj.ID,
                    ClassID: obj.ClassID,
                    NotificationID: obj.NotificationID,
                };
                self.ajaxControl.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/api/EMailTemplate'
                    },
                    function (result) {
                        if (result) {
                            self.Subject(result.Subject);
                            retD.resolve(result.Body);
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), 'Ошибка получения шаблона', 'error');                                
                            });
                            retD.resolve('');
                        }
                    });
                //
                return retD.promise();
            };
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
                var tmpHtml = self.GetEmailBodyHtml();
                if (tmpHtml == '') {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('SDNoteIsEmptyCaption'), getTextResource('SDNoteIsEmpty'), 'warning');
                    });
                    //
                    return;
                }
                //
                var data = {
                    ToAddresses: self.ToAddresses(),
                    BccAddresses: self.BccAddresses(),
                    KBArticleID: self.kbArticle().Obj ? self.kbArticle().Obj.ID:null,
                    Subject: self.Subject(),
                    HtmlBody: self.GetEmailBodyHtml(),
                    Files: self.attachmentsControl == null ? null : self.attachmentsControl.GetData(),
                };
                //
                self.ajaxControl.Ajax($region,
                    {
                        dataType: 'json',
                        method: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(data),
                        url: '/sdApi/SendEMail'
                    },
                    function (newVal) {
                        if (newVal) {
                            self.AddNewNote();
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
            self.createMessageNote = function () {

                var TapeText= 'font-size: 12px; line-height: 14px;margin-bottom:6px;';
                //var FileText = 'вместе со следующими файлами:' + 
                //var test = self.attachmentsControl.GetData(); Array каждый элемент вызвать и зпаисать FileName через зпт
                var text =
                    [
                        "<div style=\"" + TapeText + "\">Следующим пользователям: " + self.ToAddresses() + " </div>",
                        "<div style=\"" + TapeText + "\">Отправлено почтовое сообщение</div>",
                        "<div style=\"" + TapeText + "\">\"" + self.Subject() + "\"</div>",
                        "<div style=\"" + TapeText + "\">"+self.GetEmailBodyHtml() + "</div>"
                    ].join("\n");
                //
                return text;
            };
            //
            self.AddNewNote = function () {//добавить новую заметку / сообщение               
                var isNote = self.IsMassegeRadio()!=="1";
                text = self.createMessageNote();
                //
                var tmpObj = self.kbArticle().Obj;
                //                
                if (!tmpObj.ID || !tmpObj.ClassID)
                    return;
                if (tmpObj.ClassID != 701 && tmpObj.ClassID != 702 && tmpObj.ClassID != 119 && tmpObj.ClassID != 703 && tmpObj.ClassID != 823)
                    return;
                let url = '';
                switch (tmpObj.ClassID) {
                    case 701:
                        url = 'calls';
                        break;
                    case 702:
                        url = 'Problems';
                        break;
                    case 119:
                        url = 'workorders';
                        break;
                    case 703:
                        url = 'ChangeRequests';
                        break;
                    case 823:
                        url = 'massIncidents';
                        break;
                }
                self.ajaxControl.Ajax($region,
                    {
                        dataType: "json",
                        method: 'POST',
                        contentType: 'application/json',
                        url: '/api/' + url + '/' + tmpObj.ID + '/notes',
                        data: JSON.stringify({
                            Message: text,
                            IsNote: isNote
                        })
                    },
                    function (model) {
                        if (model) {    
                            $(document).trigger('local_objectInserted', [117, model.ID, tmpObj.ID.toLowerCase()]);
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[SDForm.Tape.js, AddNewNote]', 'error');
                            });
                        }
                    });
            };
            //
            self.EmailUser = function () {
                if ((self.kbArticle().Email && self.kbArticle().ID) || (self.kbArticle().Obj.ClientEmail && self.kbArticle().Obj.ClientID)) {
                    var userInfo =
                    {
                        ID: self.kbArticle().ID ? self.kbArticle().ID : self.kbArticle().Obj.ClientID,
                        Email: self.kbArticle().Email ? self.kbArticle().Email : self.kbArticle().Obj.ClientEmail
                    };
                    self.toAddressList.push(userInfo);
                    //
                    self.updateToAddresses();
                }          
            }

            //
            self.AfterRender = function () {
                showSpinner($region[0]);
                //
                self.EmailUser(); 
                //
                self.InitializeMailBoby();
                //
                self.createMailBody();
                //
                $.when(self.htmlMailBobyControlD, self.createMailBodyD).done(function (a,t) {
                    var html = t;
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
                            self.attachmentsControl.OnChange = function (URL, onSuccess, results, isDeleting) {
                                if (onSuccess) {
                                    onSuccess();
                                }
                            }
                        }
                        self.attachmentsControl.ReadOnly(false);
                        self.attachmentsControl.RemoveFileAvailable(true);
                        var objId = self.kbArticle().Obj ? self.kbArticle().Obj.ID : null;
                        if (objId != null) {
                            self.attachmentsControl.Initialize(objId, true);//загружаем файлы 
                        }
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
                                    if (!objectInfo || !objectInfo.UserInfo)
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
                                            if (!existDeputy && el && el.Email && el.Email != '') {
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
                                    if (!objectInfo || !objectInfo.UserInfo)
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
        ShowDialog: function (Article, isSpinnerActive) {
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
                                'region_userSendEmailForm',//form region prefix
                                'setting_userSendEmailForm',//location and size setting
                                getTextResource('Send_by_mail'),//caption
                                true,//isModal
                                true,//isDraggable
                                true,//isResizable
                                710, 630,//minSize
                                buttons,//form buttons
                                function () {
                                },//afterClose function
                                'data-bind="template: {name: \'../UI/Forms/Email/frmUserSendEmail\', afterRender: AfterRender}"'//attributes of form region
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
                            vm = new module.ViewModel(Article, $region, maxAttachmentSize);
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