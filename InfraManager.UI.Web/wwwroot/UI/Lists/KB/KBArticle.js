define(['knockout', 'jquery', 'ajax', 'models/SDForms/SDForm.User', 'sweetAlert', '../Settings/OrgStructure.Attachments', 'comboBox'],
    function (ko, $, ajaxLib, userLib, swal, attachments) {
        var module = {
            ViewModel: function (canEdit, $region) {
                var self = this;
                //
                self.modes =
                {
                    nothing: 'nothing',
                    add: 'add',
                    properties: 'properties'
                };
                //
                self.showCreateButton = ko.observable(true);
                //
                self.mode = ko.observable(self.modes.nothing);
                //
                self.kbArticle = ko.observable(null);
                //
                self.problem = ko.observable(null);//для создания статьи по проблеме
                //
                self.ClassID = 137;//Статья базы знаний
                //
                self.Attachments = ko.observable(null);
                //
                self.ajaxControl_KBArticle = new ajaxLib.control();
                //
                self.IsEditMode = ko.observable(false);//по умолчанию открываем статью на чтение

                self.IsEditMode.subscribe(function (newValue) {
                    self.SetReadOnly(!newValue);
                    self.InitializeExpert(true);
                    //
                    self.updateTagHeight();
                });
                //
                self.AccessRestricted = ko.observable(false);
                //
                self.updateTagHeight = function () {
                    clearInterval(self.updateTagHeightPrivate);
                    //
                    self.updateTagHeightPrivate = setInterval(function () {
                        var el = $('.tags')[0];
                        //
                        if (!el || el.style.display === 'none')
                            return;
                        //
                        el.style.height = "1px";//hack
                        el.style.height = (10 + el.scrollHeight) + "px";
                        //
                        clearInterval(self.updateTagHeightPrivate);
                    }, 10);
                };
                //
                self.SetReadOnly = function (value) {
                    if (self.htmlDescriptionControl) {
                        self.htmlDescriptionControl.SetReadOnly(value);
                    }
                    //
                    if (self.htmlSolutionControl) {
                        self.htmlSolutionControl.SetReadOnly(value);
                    }
                    //
                    if (self.htmlWorkaroundControl) {
                        self.htmlWorkaroundControl.SetReadOnly(value);
                    }
                };
                //
                self.IsReadOnly = ko.observable(!canEdit);
                self.CanEdit = ko.observable(canEdit);
                //
                self.SendEmail = function () {
                    showSpinner();
                    require(['sdForms'], function (module) {
                        var fh = new module.formHelper(true);
                        //
                        fh.ShowKbaSendEmail(self.kbArticle);
                    });
                };
                //
                //self.AssetOperationControl = ko.observable(null);
                //self.LoadAssetOperationControl = function () {
                //    if (!self.kbArticle())
                //        return;
                //    //
                //    require(['assetOperations'], function (wfLib) {
                //        if (self.AssetOperationControl() == null) {
                //            self.AssetOperationControl(new wfLib.control(self.$region, self.kbArticle, self.Load));
                //        }
                //        self.AssetOperationControl().ReadOnly(self.IsReadOnly());
                //        self.AssetOperationControl().Initialize();
                //    });
                //};
                //
                self.createDocFileBody = function () {
                    var header = 'font-size: 12px;font-style: italic;line-height: 14px;margin-bottom:20px;';
                    var imBigText = 'font-family: Arial,Helvetica,sans-serif;font-weight: normal;font-size: 22px;';
                    var kbaCaption = 'height: 25px;margin-right: 20px;overflow: hidden;';
                    var kbaFieldName = 'line-height: normal;margin-top: 20px;margin-bottom: 10px;';
                    var kbaHtml = 'margin-top: 5px;margin-left: 0;white-space: normal;font-size: 12px;color: #333;';
                    var file = 'font-size: 12px;line-height: 14px;margin-top: 10px;';
                    var marginTop = 'margin-top: 40px;';
                    var marginFooter = 'margin-top: 10px;';
                    var footer = 'font-size: 12px;font-style: italic;line-height: 14px;';
                    //
                    var kba = self.kbArticle();
                    //
                    var html =
                        [
                            "<div style=\"" + header + "\">" + getTextResource('KBArticleFormCaption') + "</div>",

                            "<div style=\"" + kbaCaption + imBigText + "\">" + kba.Caption() + "</div>",

                            "<div style=\"" + kbaFieldName + imBigText + "\">" + getTextResource('Description') + "</div>",
                            "<div style=\"" + kbaHtml + "\">" + kba.HTMLDescription() + "</div>",

                            "<div style=\"" + kbaFieldName + imBigText + "\">" + getTextResource('Solution') + "</div>",
                            "<div style=\"" + kbaHtml + "\">" + kba.HTMLSolution() + "</div>",

                            "<div style=\"" + kbaFieldName + imBigText + "\">" + getTextResource('Workaround') + "</div>",
                            "<div style=\"" + kbaHtml + "\">" + kba.HTMLAltSolution() + "</div>"
                        ];
                    //
                    var attach = self.Attachments();
                    if (attach && attach.attachmentsControl && attach.attachmentsControl.Items && attach.attachmentsControl.Items()) {
                        html.push("<div style=\"" + kbaFieldName + imBigText + "\">" + getTextResource('OrgStructureFilesTitle') + "</div>");
                        //
                        var items = attach.attachmentsControl.Items();
                        //
                        items.forEach(function (el) {
                            html.push("<div style=\"" + file + "\">" + el.FullName() + "</div>");
                        });
                    }
                    //
                    html = html.concat(
                        [
                            "<div style=\"" + marginTop + footer + "\">" + getTextResource('Posted') + ' ' + kba.AuthorFullName + "</div>",
                            "<div style=\"" + marginFooter + footer + "\">" + getTextResource('PublicationDate') + ' ' + kba.DateCreation + "</div>",
                            "<div style=\"" + marginFooter + footer + "\">" + getTextResource('KBADataLastEdit') + ' ' + kba.DateModified + "</div>",
                            "<div style=\"" + marginFooter + footer + "\">" + getTextResource('KBASection') + ': ' + kba.Section() + "</div>",
                            "<div style=\"" + marginFooter + footer + "\">" + getTextResource('TypeOfArticle') + ' ' + kba.TypeName + "</div>",
                            "<div style=\"" + marginFooter + footer + "\">" + getTextResource('KBA_ReadArticles') + ' ' + kba.AccessName + "</div>"
                        ]);
                    //
                    return html.join("\n");
                };
                //
                self.ExportDocFile = function () {
                    var bodyHtml = self.createDocFileBody();
                    //
                    var preHtml =
                        "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'><head><meta charset='utf-8'><title>Export HTML To Doc</title></head><body>";
                    var postHtml = "</body></html>";
                    //
                    var html = preHtml + bodyHtml + postHtml;
                    //
                    var blob = new Blob(['\ufeff', html],
                        {
                            type: 'application/msword'
                        });
                    //
                    // Specify link url
                    var url = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(html);

                    // Specify file name
                    filename =
                        self.kbArticle() ? self.kbArticle().Name() + '.doc' : 'document.doc';
                    //
                    if (navigator.msSaveOrOpenBlob) {
                        navigator.msSaveOrOpenBlob(blob, filename);//IE
                    } else {//normal browser
                        var a = document.createElement("a");
                        a.style.visibility = 'hidden';
                        document.body.appendChild(a);//Firefox turdie
                        a.href = url;
                        a.download = filename;
                        a.click();
                        document.body.removeChild(a);
                    }
                };
                //
                self.printDialogClosed = function () {
                    var printDiv = document.getElementById("kbaPrint");
                    //
                    if (printDiv && printDiv.parentNode) {
                        printDiv.parentNode.removeChild(printDiv);
                    }
                };
                //
                self.browser_ie11 = getIEVersion() == 11 ? true : false;
                self.ShowPrintBtn = ko.observable(!self.browser_ie11);
                //
                //в ie печать не работает, потому что событие window.onafterprint не срабатывает (баг браузера)
                self.PrintKba = function () {
                    var div = document.getElementById("kbaPrint");
                    if (!div) {
                        var div = document.createElement("div");
                        div.id = 'kbaPrint';
                        document.body.appendChild(div);//Firefox turdie
                    }
                    //
                    var html = self.createDocFileBody();
                    div.innerHTML = html;
                    //
                    window.print();
                    //
                    if (div && div.parentNode) {
                        div.parentNode.removeChild(div);
                    }
                    //
                    return true;
                };
                //
                self.serverAddress = null;
                self.serverAddressD = $.Deferred();
                //
                self.doCopyLink = function () {
                    var el = document.createElement('textarea');
                    el.value = self.serverAddress + "?kbArticleNumber=" + self.kbArticle().Number();
                    if ($region && $region.length == 1)
                        $region[0].appendChild(el);
                    else
                        document.body.appendChild(el);
                    //
                    el.select();
                    document.execCommand('copy');
                    //
                    if ($region && $region.length == 1)
                        $region[0].removeChild(el);
                    else
                        document.removeChild(el);
                };
                //
                self.CopyLink = function () {
                    if (self.serverAddress === null) {
                        var ajaxControl = new ajaxLib.control();
                        //
                        ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                url: '/api/settings/server/address'
                            },
                            function (serverAddress) {
                                self.serverAddress = serverAddress;
                                self.doCopyLink();
                            });
                    }
                    else
                        self.doCopyLink();
                    //
                };
                //
                self.Load = function (id) {
                    var retD = $.Deferred();
                    //
                    if (!id) {
                        retD.resolve(false);
                        return retD;
                    }
                    //
                    var data = {
                        kbaId: id,
                        seeInvisible: canEdit ? true : false
                    };
                    //
                    self.ajaxControl_KBArticle.Ajax($region,
                        {
                            dataType: "json",
                            method: 'GET',
                            data: data,
                            url: '/api/kb/article'
                        },
                        function (kba) {
                            if (kba) {
                                require(['ui_lists/KB/KBA'], function ( kbaLib) {
                                    self.kbArticle(new kbaLib.KBArticle(kba, self));
                                    //
                                    self.kbArticle().TagsString.subscribe(function () {
                                        self.updateTagHeight();
                                    });
                                    //
                                    self.mode(self.modes.properties);
                                    //
                                    retD.resolve(true);
                                });
                            }
                            else {
                                swal(getTextResource('ErrorCaption'), 'Permision denied', 'error');
                                retD.resolve(false);
                            }
                        });
                    //
                    return retD.promise();
                };
                //
                self.CreateNewByProblem = function (problemID) {
                    var retD = $.Deferred();
                    //
                    self.showCreateButton(false);
                    self.CanEdit(true);
                    //
                    $.when(self.GetProblem(problemID)).done(function () {
                        $.when(self.CreateNew(null, null)).done(function () {
                            self.kbArticle().TypeID = '00000000-0000-0000-0000-000000000003';//Решение проблемы
                            self.kbArticle().Name(self.problem().Summary());
                            self.kbArticle().HTMLDescription(self.problem().Description());
                            self.kbArticle().HTMLSolution(self.problem().Solution());
                            self.kbArticle().HTMLAltSolution(self.problem().Fix());
                            //
                            retD.resolve();
                        });
                    });
                    //
                    return retD.promise();
                };
                //
                self.ajaxControl_load = new ajaxLib.control();
                self.GetProblem = function (problemID) {
                    var retD = $.Deferred();
                    if (problemID) {
                        self.ajaxControl_load.Ajax(self.$region,
                            {
                                method: 'GET',
                                url: '/api/problems/' + problemID
                            },
                            function (pInfo) {
                                var loadSuccessD = $.Deferred();
                                var processed = false;
                                //
                                if (pInfo) {
                                    if (pInfo.ID) {
                                        require(['models/SDForms/ProblemForm.Problem'], function (pLib) {
                                            self.problem(new pLib.Problem(self, pInfo));
                                            //
                                            processed = true;
                                            loadSuccessD.resolve(true);
                                        });
                                    }
                                    else loadSuccessD.resolve(false);
                                }
                                else loadSuccessD.resolve(false);
                                //
                                $.when(loadSuccessD).done(function (loadSuccess) {
                                    retD.resolve(loadSuccess);
                                    if (loadSuccess == false && processed == false) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[ProblemForm.js, Load]', 'error');
                                        });
                                    }
                                });
                            });
                    }
                    else retD.resolve(false);
                    //
                    return retD.promise();
                };
                //
                self.folderID = null;
                self.AddKbaCallback = null;
                self.CreateNew = function (folderID, callback) {
                    var retD = $.Deferred();
                    //
                    require(['ui_lists/KB/KBA'], function (kbaLib) {
                        self.kbArticle(new kbaLib.KBArticle({} , self));
                        //
                        self.kbArticle().TagsString.subscribe(function () {
                            self.updateTagHeight();
                        });
                        //
                        self.IsEditMode(true);
                        //
                        self.folderID = folderID;
                        self.AddKbaCallback = callback;
                        //
                        self.mode(self.modes.add);
                        //
                        $.when(self.htmlDescriptionControlD, self.htmlSolutionControlD, self.htmlWorkaroundControlD).done(function () {
                            self.SetReadOnly(false);
                        });
                        self.tmpKbaIDForAccessList(self.generateTmpId());
                        //
                        retD.resolve(true);
                    });
                    //
                    return retD.promise();
                };
                //
                self.AfterRender = function () {
                    self.InitializeDescription();
                    self.InitializeSolution();
                    self.InitializeWorkaround();
                    //
                    self.InitializeExpert();
                    //
                    self.UserID(self.kbArticle().AuthorID);
                    self.InitializeUser();
                    //
                   /* self.LoadAssetOperationControl();*/
                    self.GetTypeList();
                    self.GetStatusList();
                    self.GetAccessList();
                    var OnChange = function (URL, onSuccess, fileId, isDeleting) {
                        var ajaxConfig;
                        if (isDeleting) {
                            URL = URL + self.ClassID + "/" + self.Attachments().attachmentsControl.ObjectID + "/documents/" + fileId;
                            ajaxConfig =
                            {
                                url: window.location.origin + "/" + URL,
                                method: "Delete",
                                dataType: "json"
                            };
                        }
                        else {
                            URL = URL + self.ClassID + "/" + self.Attachments().attachmentsControl.ObjectID + "/documents";
                            ajaxConfig =
                            {
                                url: window.location.origin + "/" + URL,
                                method: "Post",
                                dataType: "json",
                                data: { 'docID': fileId }
                            };
                        }
                        self.ajaxControl = new ajaxLib.control();
                        self.ajaxControl.Ajax(null,
                            ajaxConfig,
                            null,
                            null,
                            function () {
                                if (onSuccess) onSuccess();
                            });
                    }
                    self.Attachments(new attachments.ViewModel(self.ClassID, self.kbArticle().ID, self.CanEdit, OnChange));
                    //
                    self.updateTagHeight();
                };
                //
                self.EditArticleClick = function () {
                    self.IsEditMode(true);
                };
                //
                self.ajaxControlSave = new ajaxLib.control();
                self.SaveEditClick = function () {
                    var retD = $.Deferred();
                    //
                    var RelatedArticlesId = [];
                    for (var i = 0; i < self.kbArticle().KBArticleDependencyList().length; i++)
                        RelatedArticlesId.push(self.kbArticle().KBArticleDependencyList()[i].KBArticleDependencyID);
                    
                    var data =
                    {
                        'ID': self.kbArticle().ID,
                        'AuthorID': self.kbArticle().AuthorID,
                        'Name': self.kbArticle().Name(),
                        'TagString': self.kbArticle().TagsString(),
                        'HtmlDescription': self.kbArticle().HTMLDescription(),
                        'HtmlSolution': self.kbArticle().HTMLSolution(),
                        'HtmlAltSolution': self.kbArticle().HTMLAltSolution(),
                        'TypeID': self.selectedType() ? self.selectedType().ID : null,
                        'StatusID': self.selectedStatus() ? self.selectedStatus().ID : null,
                        'FolderID': self.folderID,
                        'VisibleForClient': self.kbArticle().VisibleForClient(),
                        'ExpertID': self.kbArticle().ExpertID(),
                        'DateValidUntil': self.kbArticle().DateValidUntil(),
                        'AccessID': self.selectedAccess() ? self.selectedAccess().ID : null,
                        'TMPID': self.tmpKbaIDForAccessList() ? self.tmpKbaIDForAccessList() : "00000000-0000-0000-0000-000000000000",
                        'KBArticleDependencyList': RelatedArticlesId
                        //,'LifeCycleStateID': self.kbArticle().LifeCycleStateID ? self.kbArticle().LifeCycleStateID  : null,
                    };
                    //
                    if (self.mode() === self.modes.properties) {
                        self.ajaxControlSave.Ajax($region,
                            {
                                dataType: "json",
                                contentType: 'application/json',
                                method: 'PUT',
                                data: JSON.stringify(data),
                                url: '/api/kb/article'
                            },
                            function (newVal) {
                                if (newVal) {
                                    swal(getTextResource('KBAEditSuccessful'), '', 'info');
                                    
                                    require(['ui_lists/KB/KBA'], function (kbaLib) {
                                        self.kbArticle(new kbaLib.KBArticle(newVal, self));
                                        self.kbArticle().TagsString.subscribe(function () {
                                            self.updateTagHeight();
                                        });
                                    });

                                    self.IsEditMode(false);
                                    retD.resolve();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), '', 'error');
                                    });
                                }
                            });
                    } else if (self.mode() === self.modes.add) {
                        if (!self.kbArticle().Name()) {
                            swal(getTextResource('KBANameMustBeSet'), '', 'info');
                            return retD.resolve(false);
                        }
                        //
                        self.ajaxControlSave.Ajax($region,
                            {
                                dataType: "json",
                                contentType: 'application/json',
                                method: 'POST',
                                data: JSON.stringify(data),
                                url: '/api/KB/articles'
                            },
                            function (newVal) {
                                if (newVal) {
                                    swal(getTextResource('KBAAddSuccessful'), '', 'info');
                                    self.IsEditMode(false);
                                    if (self.AddKbaCallback)
                                        self.AddKbaCallback();
                                    retD.resolve(true);
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), '', 'error');
                                    });
                                }
                            });
                    }
                    //
                    return retD.promise();
                };
                //
                self.CancelClick = function () {
                    self.IsEditMode(false);
                };
                //
                {//user          
                    //
                    self.UserID = ko.observable(null);
                    self.UserLoaded = ko.observable(false);
                    //
                    /*self.EditUser = function () {
                        showSpinner();
                        require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                            var fh = new module.formHelper(false);
                            var options = {
                                fieldFriendlyName: getTextResource('Posted'),
                                oldValue: self.UserLoaded() ? { ID: self.User().ID(), ClassID: self.User().ClassID(), FullName: self.User().FullName() } : null,
                                object: ko.toJS(self.User()),
                                searcherName: 'WebUserSearcher',
                                searcherPlaceholder: getTextResource('EnterFIO'),
                                searcherParams: [],
                                nosave: true,
                                onSave: function (objectInfo) {
                                    self.UserLoaded(false);
                                    self.User(new userLib.EmptyUser(self, userLib.UserTypes.initiator, self.EditUser));
                                    if (!objectInfo)
                                        self.UserID(null);
                                    else
                                        self.UserID(objectInfo.ID);
                                    //
                                    self.InitializeUser();
                                }
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                        });
                    };*/
                    self.User = ko.observable(new userLib.EmptyUser(self, userLib.UserTypes.initiator/*, self.EditUser*/));
                    //
                    self.InitializeUser = function () {
                        require(['models/SDForms/SDForm.User'], function (userLib) {
                            if (self.UserLoaded() == false) {
                                if (self.UserID()) {
                                    var options = {
                                        UserID: self.UserID(),
                                        UserType: userLib.UserTypes.kbaAuthor,
                                        UserName: null,
                                        //EditAction: self.EditUser,
                                    };
                                    var user = new userLib.User(self, options);
                                    self.User(user);
                                    self.UserLoaded(true);
                                }
                            }
                        });
                    };
                }
                //
                {//kba type
                    self.selectedType = ko.observable(null);
                    self.typeComboItems = ko.observableArray([]);
                    self.getTypeComboItems = function () {
                        return {
                            data: self.typeComboItems(),
                            totalCount: self.typeComboItems().length
                        };
                    };
                    //
                    self.ajaxControlType = new ajaxLib.control();
                    self.GetTypeList = function () {
                        self.ajaxControlType.Ajax($region.find('.kba-type-combobox'),
                            {
                                dataType: "json",
                                method: 'GET',
                                url: '/api/kb/ArticleTypes'
                            },
                            function (result) {
                                if (result && self.typeComboItems().length === 0) {
                                    var selEl = null;
                                    result.forEach(function (el) {
                                        self.typeComboItems().push(el);
                                        //
                                        if (self.kbArticle().TypeID === el.ID)
                                            selEl = el;
                                    });
                                    //
                                    if (!selEl && self.typeComboItems().length !== 0)
                                        selEl = self.typeComboItems()[0];
                                    //
                                    self.typeComboItems.valueHasMutated();
                                    self.selectedType(selEl);
                                }
                            });
                    };
                }
                //
                {//kba status
                    self.selectedStatus = ko.observable(null);
                    self.statusComboItems = ko.observableArray([]);
                    self.getStatusComboItems = function () {
                        return {
                            data: self.statusComboItems(),
                            totalCount: self.statusComboItems().length
                        };
                    };
                    //
                    self.ajaxControlStatus = new ajaxLib.control();
                    self.GetStatusList = function () {
                        self.ajaxControlStatus.Ajax($region.find('.kba-status-combobox'),
                            {
                                dataType: "json",
                                method: 'GET',
                                url: '/api/kb/ArticleStatuses'
                            },
                            function (result) {
                                if (result && self.statusComboItems().length === 0) {
                                    var selEl = null;
                                    result.forEach(function (el) {
                                        self.statusComboItems().push(el);
                                        //
                                        if (self.kbArticle().StatusID === el.ID)
                                            selEl = el;
                                    });
                                    //
                                    if (!selEl && self.statusComboItems().length !== 0)
                                        selEl = self.statusComboItems()[0];
                                    //
                                    self.statusComboItems.valueHasMutated();
                                    self.selectedStatus(selEl);
                                }
                            });
                    };
                }
                //

                {//kbaAccess
                    self.tmpKbaIDForAccessList = ko.observable(null);
                    //скрипт генерации гуида
                    self.generateTmpId = function () {
                        let
                            d = new Date().getTime(),
                            d2 = (performance && performance.now && (performance.now() * 1000)) || 0;
                        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
                            let r = Math.random() * 16;
                            if (d > 0) {
                                r = (d + r) % 16 | 0;
                                d = Math.floor(d / 16);
                            } else {
                                r = (d2 + r) % 16 | 0;
                                d2 = Math.floor(d2 / 16);
                            }
                            return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
                        });
                    }
                    self.selectedAccess = ko.observable(null);
                    self.selectedAccess.subscribe(function (newValue) {
                        if (newValue.ID == '00000000-0000-0000-0000-000000000002') {
                            self.AccessRestricted(true);
                        }
                        else
                            self.AccessRestricted(false);
                    });
                    self.accessComboItems = ko.observableArray([]);
                    self.getAccessComboItems = function () {
                        return {
                            data: self.accessComboItems(),
                            totalCount: self.accessComboItems().length
                        };
                        /* $region.find('.kba-access-combobox').addEventListener('combobox', self.AccessRestricted(false));*/
                    };
                    //
                    self.ajaxControlAccess = new ajaxLib.control();
                    self.GetAccessList = function () {
                        self.ajaxControlAccess.Ajax($region.find('.kba-access-combobox'),
                            {
                                dataType: "json",
                                method: 'GET',
                                url: '/api/kb/ArticleAccess'
                            },
                            function (result) {
                                if (result && self.accessComboItems().length === 0) {
                                    var selEl = null;
                                    result.forEach(function (el) {
                                        self.accessComboItems().push(el);
                                        //
                                        if (self.kbArticle().AccessID === el.ID)
                                            selEl = el;
                                    });
                                    //
                                    if (!selEl && self.accessComboItems().length !== 0)
                                        selEl = self.accessComboItems()[0];
                                    //
                                    self.accessComboItems.valueHasMutated();
                                    self.selectedAccess(selEl);
                                }

                                if (self.kbArticle().AccessID == '00000000-0000-0000-0000-000000000002') {
                                    self.AccessRestricted(true);
                                }
                                else
                                    self.AccessRestricted(false);
                            });
                    };
                }

                self.EditKBAUserList = function () {
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(false);
                        fh.ShowKBAUserList(self.kbArticle(), self.tmpKbaIDForAccessList);
                    });
                };

                //description
                {
                    self.htmlDescriptionControlD = $.Deferred();
                    //
                    self.htmlDescriptionControl = null;
                    self.InitializeDescription = function () {
                        var htmlElement = $region.find('.KBADescription_');
                        if (self.htmlDescriptionControl == null)
                            require(['htmlControl'], function (htmlLib) {
                                self.htmlDescriptionControl = new htmlLib.control(htmlElement, false, true, true);//targetDiv, extendedMode, readOnly, noScrolling
                                self.htmlDescriptionControlD.resolve();
                                self.htmlDescriptionControl.SetHTML(self.kbArticle().HTMLDescription());
                                self.htmlDescriptionControl.OnHTMLChanged = function (htmlValue) {
                                    self.kbArticle().HTMLDescription(htmlValue);
                                };
                            });
                        else
                            self.htmlDescriptionControl.Load(htmlElement);
                    };
                    //
                    self.EditDescription = function () {
                        if (!self.IsEditMode())
                            return;
                        //
                        showSpinner();
                        require(['usualForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var options = {
                                ID: self.kbArticle().ID,
                                objClassID: self.ClassID,
                                fieldName: 'KBArticle.Description',
                                fieldFriendlyName: getTextResource('Description'),
                                oldValue: self.kbArticle().HTMLDescription(),
                                nosave: true,
                                onSave: function (newHTML) {
                                    self.kbArticle().HTMLDescription(newHTML);
                                    self.htmlDescriptionControl.SetHTML(newHTML);
                                },
                                readOnly: !self.CanEdit()
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                        });
                    };
                }
                //
                self.EditExpert = function () {
                    if (!self.IsEditMode())
                        return;
                    //
                    showSpinner();
                    var kba = self.kbArticle();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: kba.ID,
                            objClassID: kba.ClassID,
                            fieldName: 'KBArticle.Expert',
                            fieldFriendlyName: getTextResource('Kba_Expert'),
                            oldValue: kba.ExpertLoaded() ? { ID: kba.ExpertID(), ClassID: 9, FullName: kba.ExpertFullName() } : null,
                            object: ko.toJS(kba.Expert()),
                            searcherName: 'WebUserSearcherNoTOZ',
                            searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                            nosave: true,
                            onSave: function (objectInfo) {
                                kba.ExpertLoaded(false);
                                kba.Expert(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.IsEditMode() ? self.EditExpert : null, false, false));
                                //
                                kba.ExpertID(objectInfo ? objectInfo.ID : '');
                                kba.ExpertFullName(objectInfo ? objectInfo.FullName : '');
                                self.InitializeExpert();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                //
                self.InitializeExpert = function (hardReset) {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var kba = self.kbArticle();
                        if ((kba.ExpertLoaded() == false || hardReset) && kba.ExpertID()) {
                            var type = null;
                            type = userLib.UserTypes.utilizer;
                            //
                            var options = {
                                UserID: kba.ExpertID(),
                                UserType: type,
                                UserName: null,
                                EditAction: self.IsEditMode() ? self.EditExpert : null,
                                RemoveAction: null,
                                ShowTypeName: false
                            };
                            var user = new userLib.User(self, options);
                            kba.Expert(user);
                            kba.ExpertLoaded(true);
                        }
                    });
                };
                //
                self.ajaxControl_AddKBArticleDependency = new ajaxLib.control();
                self.OpenKBArticleDependency = function (kba, event) {
                    if (kba.KBArticleDependencyID == null)
                        return;
                    var wnd = window.open(window.location.protocol + '//' + window.location.host + location.pathname + '?kbArticleID=' + kba.KBArticleDependencyID);
                    if (wnd) //browser cancel it?  
                        return;
                }

                self.RemoveKBArticleDependency = function (kba) {
                    self.OldKBArticleDependencyID(kba.KBArticleDependencyID);
                    self.kbArticle().KBArticleDependencyList(self.kbArticle().KBArticleDependencyList().filter(function (t) { return t.KBArticleDependencyID != kba.KBArticleDependencyID }))
                    self.kbArticle().KBArticleDependencyList.valueHasMutated();
                    self.KBArticleDependencyIDChanged(true);
                };
                self.OldKBArticleDependencyID = ko.observable(null);
                self.KBArticleDependencyIDChanged = ko.observable(false);
                   
                self.AddKBArticleDependency = function () {
                        require(['usualForms'], function (module) {
                            var fh = new module.formHelper(false);
                            fh.ShowKBADependencyrList(self.kbArticle());
                        });
                    self.KBArticleDependencyIDChanged(true);
                };
                //Дата
                self.EditDateValidUntil = function () {
                    if (!self.CanEdit())
                        return;
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.kbArticle().ID,
                            objClassID: self.kbArticle().ClassID,
                            fieldName: 'kbArticle.DateValidUntil',
                            fieldFriendlyName: getTextResource('Kba_ValidUntil'),
                            oldValue: self.kbArticle().DateValidUntilDT(),
                            nosave: true,
                            onSave: function (newDate) {
                                self.kbArticle().DateValidUntil(parseDate(newDate));
                                self.kbArticle().DateValidUntilDT(new Date(parseInt(newDate)));
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };
                //
                self.DateValidUntilCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                    var retval = '';
                    //
                    if (!retval && self.kbArticle) {
                        var lo = self.kbArticle();
                        if (lo && lo.DateValidUntil)
                            retval = lo.DateValidUntil();
                    }
                    //
                    return retval;
                });

                //solution
                {
                    self.htmlSolutionControlD = $.Deferred();
                    //
                    self.htmlSolutionControl = null;
                    self.InitializeSolution = function () {
                        var htmlElement = $region.find('.KBASolution_');
                        if (self.htmlSolutionControl == null)
                            require(['htmlControl'], function (htmlLib) {
                                self.htmlSolutionControl = new htmlLib.control(htmlElement, false, true, true);//targetDiv, extendedMode, readOnly, noScrolling
                                self.htmlSolutionControl.SetHTML(self.kbArticle().HTMLSolution());
                                self.htmlSolutionControlD.resolve();
                                self.htmlSolutionControl.OnHTMLChanged = function (htmlValue) {
                                    self.kbArticle().HTMLSolution(htmlValue);
                                };
                            });
                        else
                            self.htmlSolutionControl.Load(htmlElement);
                    };
                    //
                    self.EditSolution = function () {
                        showSpinner();
                        require(['usualForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var options = {
                                ID: self.kbArticle().ID,
                                objClassID: self.ClassID,
                                fieldName: 'KBArticle.Solution',
                                fieldFriendlyName: getTextResource('Solution'),
                                oldValue: self.kbArticle().HTMLSolution(),
                                nosave: true,
                                onSave: function (newHTML) {
                                    self.kbArticle().HTMLSolution(newHTML);
                                    self.htmlSolutionControl.SetHTML(newHTML);
                                },
                                readOnly: !self.CanEdit()
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                        });
                    };
                }
                //
                //workaround
                {
                    self.htmlWorkaroundControlD = $.Deferred();
                    //
                    self.htmlWorkaroundControl = null;
                    self.InitializeWorkaround = function () {
                        var htmlElement = $region.find('.KBAWorkaround_');
                        if (self.htmlWorkaroundControl == null)
                            require(['htmlControl'], function (htmlLib) {
                                self.htmlWorkaroundControl = new htmlLib.control(htmlElement, false, true, true);//targetDiv, extendedMode, readOnly, noScrolling
                                self.htmlWorkaroundControl.SetHTML(self.kbArticle().HTMLAltSolution());
                                self.htmlWorkaroundControlD.resolve();
                                self.htmlWorkaroundControl.OnHTMLChanged = function (htmlValue) {
                                    self.kbArticle().HTMLAltSolution(htmlValue);
                                };
                            });
                        else
                            self.htmlWorkaroundControl.Load(htmlElement);
                    };
                    //
                    self.EditWorkaround = function () {
                        showSpinner();
                        require(['usualForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var options = {
                                ID: self.kbArticle().ID,
                                objClassID: self.ClassID,
                                fieldName: 'KBArticle.Workaround',
                                fieldFriendlyName: getTextResource('Workaround'),
                                oldValue: self.kbArticle().HTMLAltSolution(),
                                nosave: true,
                                onSave: function (newHTML) {
                                    self.kbArticle().HTMLAltSolution(newHTML);
                                    self.htmlWorkaroundControl.SetHTML(newHTML);
                                },
                                readOnly: !self.CanEdit()
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                        });
                    };
                }
            }
        };
        //
        return module;
    });

