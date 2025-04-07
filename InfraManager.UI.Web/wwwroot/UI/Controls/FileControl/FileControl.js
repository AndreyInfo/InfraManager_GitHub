define(['jquery', 'ajax', 'knockout', 'ttControl', 'sweetAlert'], function ($, ajaxLib, ko, tclib) {
    var module = {
        //объект jQuery, в который добавляется контрол; css родительского контрола; css кнопки, которая вызывает диалог и шаблон, если нужно его изменить
        //onefile - загрзка одного файла
        //withoutReference - загрузка файла без референса на объект (objectID нужен). Необходимо в случае множественной загрузки на форме
        control: function (obj, parentDivCss, addBtnCss, template, onefile, withoutReference, maxFiles) {
            template = template || '../UI/Controls/FileControl/FileControl';
            //
            var self = this;
            //
            self.OneFile = ko.observable(onefile == true ? onefile : false);
            self.MaxFiles = maxFiles;
            self.IsDownloaded = ko.observable(true);
            //
            self.WithoutReference = ko.observable(withoutReference == true ? withoutReference : false);
            //
            self.ObjectID = null;//идентификатор объекта, чьи документы смотрим / привязываем            
            //
            self.Items = ko.observableArray([]);//элементы-документы-прикрепления
            self.CurrentUserID = ko.observable(null);
            //
            self.MaxFileSize = 30*1024*1024;//максимальный размер файла 30мб
            //
            self.Template = template;
            //
            self.HumanSize = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
            self.FileType = {
                'application': ['exe', 'bat', 'com'],
                'audio': ['wav', 'mp3', 'ogg', 'flac', 'wma'],
                'video': ['avi', 'mpg', 'mkv', 'mp4', 'flv', 'wmv'],
                'text': ['doc', 'docx', 'xls', 'xlsx', 'txt', 'ini', 'rtf', 'pdf'],
                'image': ['png', 'jpg', 'jpeg', 'gif', 'tif', 'bmp', 'psd', 'ico']
            };
            self.MediaType = {
                'gif': 'image/gif',
                'jpg': 'image/jpeg',
                'jpeg': 'image/jpeg',
                'png': 'image/png',
                //'ico': 'image/vnd.microsoft.icon', not supported
                'tif': 'image/tiff',
                //'bmp': 'image/vnd.wap.wbmp', not supported
                //
                //TreeSettings
                //TODO: change next to click by a tag
                //'mp3': 'audio/mpeg',
                //'ogg': 'audio/ogg',
                //'flac': 'audio/ogg',
                //'wma': 'audio/x-ms-wma',
                //'wav': 'audio/vnd.wave',
                ////                     
                //'html': 'text/html',
                //'txt': 'text/plain',
                //'.ini': 'text/plain',
                //'xml': 'text/xml',
                ////
                //'avi': 'video/mpeg',
                //'mpg': 'video/mpeg',
                //'mp4': 'video/mp4',
                //'flv': 'video/x-flv',
                //'wmv': 'video/x-ms-wmv',
                ////     '
                //'pdf': 'application/pdf',
                //'xls': 'application/vnd.ms-excel',
                //'xlsx': 'application/vnd.ms-excel',
                //'doc': 'application/msword',
                //'docx': 'application/msword'
            };
            //
            self.getSizeString = function (size) {
                if (size == 0)
                    return '0';
                //
                var i = Math.floor(Math.log(size) / Math.log(1024));
                return (size / Math.pow(1024, i)).toFixed(2) * 1 + ' ' + self.HumanSize[i];
            }
            //
            var createItemByFile = function (fileInfo) {
                var retval = new self.CreateItem(null, self.ObjectID, fileInfo.name, fileInfo.size, new Date(), self.CurrentUserID(), fileInfo.type);
                return retval;
            };
            var createItemByDocument = function (documentInfo) {
                var ext = documentInfo.Extension; 
                var type = [];
                if (ext && ext.length > 0) {
                    ext = ext.toLowerCase();
                    //                
                    for (var p in self.FileType) {
                        var extArray = self.FileType[p];
                        if (extArray.indexOf(ext) != -1) {
                            type.push(p);
                            break;
                        }
                    }
                }
                //
                return new self.CreateItem(documentInfo.ID, documentInfo.ObjectID, documentInfo.FullName || documentInfo.Name, documentInfo.Size, documentInfo.UtcDateCreated, documentInfo.AuthorID, type);
            };
            self.DownloadAllClick = function () {
                self.DownloadArchive();
            };
            self.CreateItem = function (id, objectID, name, size, dateCreated, authorID, type) {//document.ID, referenceObject.ID, document.Name, document.SizeInBytes, document.DateCreated, document.AuthorID, extension type
                var thisObj = this;
                //
                thisObj.ID = id;
                thisObj.ObjectID = objectID;
                thisObj.FilePostfix = '_' + (new Date()).getTime();//have unique name at any time
                //
                thisObj.Name = name;
                thisObj.Size = size;
                thisObj.DateCreated = parseDate(dateCreated);
                thisObj.AuthorID = authorID;
                thisObj.Type = type;
                //
                thisObj.ProgressValue = ko.observable(0);
                thisObj.Ajax = ko.observable(null);
                thisObj.ProgressVisible = ko.computed(function () {
                    return (thisObj.Ajax() != null);
                });
                //
                thisObj.ImageSource = ko.computed(function () {
                    var imageName = 'ext_unknown.png';//default extension
                    if (thisObj.Type.indexOf('application') != -1)
                        imageName = 'ext_application.png';
                    else if (thisObj.Type.indexOf('audio') != -1)
                        imageName = 'ext_audio.png';
                    else if (thisObj.Type.indexOf('image') != -1)
                        imageName = 'ext_image.png';
                    else if (thisObj.Type.indexOf('text') != -1)
                        imageName = 'ext_text.png';
                    else if (thisObj.Type.indexOf('video') != -1)
                        imageName = 'ext_video.png';
                    //
                    return '/UI/Controls/FileControl/FileExtensions/' + imageName;
                });
                //
                thisObj.IsCurrentUserAuthor = ko.computed(function () {
                    return self.CurrentUserID() == thisObj.AuthorID;
                });
                thisObj.SizeString = ko.computed(function () {
                    return self.getSizeString(thisObj.Size);
                });
                //
                //
                thisObj.FullName = ko.computed(function () {
                    return thisObj.Name + ", " + thisObj.DateCreated + ", " + thisObj.SizeString();
                });
                //
                thisObj.StopProgress = function () {
                    var ajax = thisObj.Ajax();
                    if (ajax != null && ajax.readyState != 4) //ReadyState != done
                        ajax.abort();
                    thisObj.Ajax(null);
                    //
                    thisObj.ProgressValue(0);
                };
                //
                thisObj.DownloadClick = function (m, e) {
                    self.DownloadFile(thisObj, e.ctrlKey);
                };
                //
                thisObj.StopUploadAndRemove = function (onlyNotUploaded, notOnChange) {
                    if (thisObj.Ajax() != null && thisObj.Ajax().readyState != 4) //ReadyState != done
                    {//uploading
                        thisObj.StopProgress();
                        //
                        self.RemoveItem(thisObj);
                    }
                    else if (thisObj.ObjectID == null || thisObj.ObjectID != null && onlyNotUploaded != true) {//uploaded
                        //Remove all references
                        var afterChange = function () {
                            var d = self.RemoveUploadedFile(thisObj);
                            $.when(d).done(function (result) {
                                if (result == true) {//файл удален или не найден
                                    self.RemoveItem(thisObj);

                                }
                                else//файл найден + не удалось удалить файл
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('FailedToRemoveDocumentFromServer') + '\n[fileControl.js, RemoveClick]', 'error');
                                    });
                            });
                        };
                        if (notOnChange) {
                            afterChange();
                        } else {
                            if (thisObj.ObjectID != null && thisObj.ID != null) {
                                onSuccess = function () { $(document).trigger('local_objectDeleted', [110, thisObj.ID, thisObj.ObjectID]); afterChange(); } //OBJ_DOCUMENT удаление прикрепления в списке
                                //Unparallel
                                self.OnChange("api/DocumentReferences/", onSuccess, thisObj.ID, true);
                            } else {
                                self.RemoveItem(thisObj);
                            }
                        }
                    }
                    thisObj.ProgressValue(0);
                }
                thisObj.RemoveClick = function () {
                    if (self.ReadOnly() == true || self.RemoveFileAvailable() != true)
                        return;
                    thisObj.StopUploadAndRemove();
                };
                //
                thisObj.ToFileInfo = function () {
                    var fileInfo = {
                        'ID': thisObj.ID,
                        'ObjectID': thisObj.ObjectID,
                        'FileName': thisObj.Name,
                        'FilePostfix': thisObj.FilePostfix
                    };
                    return fileInfo;
                };
            };
            //
            self.AddItem = function (item) {
                self.Items().push(item);
                self.Items.valueHasMutated();
            };
            //
            self.RemoveItem = function (item) {
                var index = self.Items().indexOf(item);
                if (index == -1)
                    return;
                self.Items().splice(index, 1);
                self.Items.valueHasMutated();
            };
            //
            self.divID = 'fileControl_' + ko.getNewID();//main control div.ID
            self.getDIV = function () {
                var retval = $('#' + self.divID);
                return retval;
            };
            self.getAjaxDIV = function () {//for spinner position
                var div = self.getDIV();
                if (self.Items().length == 0) {
                    var divForm = div.closest(parentDivCss);//parentForm
                    var divAddButton = divForm.find(addBtnCss);
                    if (divAddButton.length == 1)
                        return divAddButton;
                }
                //
                return div;
            };
            //
            self.ajaxControl_init = new ajaxLib.control();
            self.ajaxControl_save = new ajaxLib.control();
            //
            self.LoadD = $.Deferred();
            self.Load = function (divObj) {
                obj = divObj;
                obj.append('<div id="' + self.divID + '" style="position:relative" data-bind="template: {name: \'' + self.Template + '\', afterRender: AfterRender}" ></div>');
                //
                try {
                    ko.applyBindings(self, document.getElementById(self.divID));
                }
                catch (err) {
                    if (document.getElementById(self.divID))
                        throw err;
                }
            };
            self.IsLoaded = function () {
                var div = document.getElementById(self.divID);
                if (div)
                    return true;
                else
                    return false;
            };
            //
            self.AvailableAdd = ko.observable(true);
            self.AddFileClick = function () {
                if (self.ReadOnly() == true || self.AvailableAdd() == false)
                    return;
                if (window.FormData == undefined) {
                    alert("This browser doesn't support HTML5 file uploads!");
                    return;
                }
                //
                self.getDIV().find('.fileInput').trigger('click');//invoke open file dialog in browser
            };
            //
            self.RemoveUploadedFile = function (item) {
                var retval = $.Deferred();
                //
                var ajaxControl = new ajaxLib.control();
                var fileInfo = item.ToFileInfo();
                ajaxControl.Ajax(self.getDIV(),
                    {
                        type: "DELETE",
                        url: '/api/object/' + self.ObjectID + '/documents' + (fileInfo.ID ? '/' + fileInfo.ID : ''),
                        dataType: 'json',
                    },
                    function (result) {
                        retval.resolve(result);
                    },
                    function (response) {
                        retval.resolve(response.status === 200);
                    });
                //
                return retval;
            };
            //
            self.AvailableDownload = ko.observable(true);
            self.DownloadFile = function (item, controlPressed) {
                if (self.AvailableDownload() == false)
                    return;
                if (item.Ajax() != null && item.Ajax().readyState != 4) //ReadyState != done
                    return;//uploading, not implemented
                //                
                var ajaxControl = new ajaxLib.control();
                var fileInfo = item.ToFileInfo();
                ajaxControl.Ajax(self.getDIV(),
                    {
                        method: 'GET',
                        url: '/api/files/' + fileInfo.ID + '/url'
                    },
                    function (data) {//file prepared for download
                        var url = data.url;
                        if (url != null) {
                            var re = /(?:\.([^.]+))?$/;
                            var ext = re.exec(item.Name)[1];
                            var mediaType = null;
                            //
                            if (ext && ext.length > 0) {
                                ext = ext.toLowerCase();
                                if (self.MediaType[ext])
                                    mediaType = self.MediaType[ext];
                            }
                            //
                            if (mediaType && controlPressed == false) {//open file from server
                                var wnd = window.open();
                                if (wnd) {//browser cancel it?                                    
                                    wnd.document.write('<object id="target" type="' + mediaType + '"></object><script>document.getElementById("target").setAttribute("data", "' + url + '");</script>');
                                    return;
                                }
                            }
                            //
                            window.location.href = url;//download file from server
                        }
                        else //файл не найден - удалить из списка?
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('DocumentFileNotFound') + '\n[fileControl.js, DownloadFile]', 'error');
                            });
                    });
            };
            self.DownloadAllFiles = function () {
                if (self.AvailableDownload() == false)
                    return;
                if (self.ObjectID == null) {
                    return;
                }
                //
                var ajaxControl = new ajaxLib.control();
                var objectID = self.ObjectID;
                //
                ajaxControl.Ajax(self.getDIV(),
                    {
                        method: 'GET',
                        url: '/api/object/' + objectID + '/attachments',
                        dataType: 'json'
                    }, function (data) {
                        //download archive from server
                        window.location.href = data.url;
                    });
            };
            //
            self.DownloadArchive = function () {
                if (self.AvailableDownload() == false)
                    return;
                if (self.ObjectID == null) {
                    var FileIds = new Array();
                    for (var i = 0; i < self.Items().length; i++) {
                        var id = self.Items()[i].ID;
                        FileIds.push(id);
                    }
                    var data = {
                        'docIDs': FileIds
                    };
                    //
                    var ajaxControl = new ajaxLib.control();
                    ajaxControl.Ajax(self.getDIV(),
                        {
                            url: '/api/data/downloadfilesarchive',
                            method: 'POST',
                            data: data
                        }, function (result) {
                            //download archive from server
                            window.location.href = result.url;
                        });
                } else {
                    //
                    var ajaxControl = new ajaxLib.control();
                    var objectID = self.ObjectID;
                    //
                    ajaxControl.Ajax(self.getDIV(),
                        {
                            method: 'GET',
                            url: '/api/object/' + objectID + '/check-attachments',
                            dataType: 'json'
                        }, function (data) {
                            //download archive from server
                            window.location.href = data.url;
                        });
                }
            };
            //
            self.RemoveAll = function () {
                for (var i = 0; i < self.Items().length; i++) {
                    var item = self.Items()[i];
                    item.StopUploadAndRemove(false);
                }
            };
            //
            self.UploadFiles = function (files) {
                self.IsDownloaded(false);
                var hasError = false;
                var error = getTextResource('FilesNotAttached').replace('\"{0}\"', self.getSizeString(self.MaxFileSize)) + ": ";
                for (var i = 0; i < files.length; i++) {
                    if (self.MaxFileSize && files[i].size > self.MaxFileSize) {
                        if (hasError)
                            error += ', ';
                        //
                        error += files[i].name;
                        //
                        hasError = true;
                    }
                }
                
                if (hasError)
                    swal(error, '', 'info');
                //
                var requests = [];
                var results = []; 
                var itemsToUpload = []; 
                for (var i = 0; i < files.length; i++) {
                    if (files[i].size == 0 && getIEVersion() != -1) {
                        alert('File "' + files[i].name + '" will be ignored, because have zero-size.');
                        continue;
                    }
                    if (self.MaxFileSize && files[i].size > self.MaxFileSize) {
                        continue;
                    }
                    var item = createItemByFile(files[i]);
                    //
                    var data = new FormData();
                    data.append("file", files[i]);
                    //
                    var createSuccess = function (objs, results) {
                        var cloneObjs = objs;
                        if (self.ObjectID != null && results != null) {
                            self.IsDownloaded(true);
                            for (var i = 0; i < files.length; i++) {
                                if (results[i] && results[i].id) {
                                    cloneObjs[i].ID = results[i].id;
                                    cloneObjs[i].ObjectID = self.ObjectID;
                                }
                            }
                            onSuccess = function () {
                                for (var i = 0; i < files.length; i++) {
                                    $(document).trigger('local_objectInserted', [110, results[i].id, self.ObjectID]);
                                }
                                for (var j = 0; j < cloneObjs.length; j++) {
                                    cloneObjs[j].StopProgress();
                                }
                            } //OBJ_DOCUMENT добавление прикрепления в списке
                            self.OnChange("api/DocumentReferences/", onSuccess, results);
                        } else {
                            //При создании заявки
                            for (var j = 0; j < cloneObjs.length; j++) {
                                cloneObjs[j].StopProgress();
                            }
                            self.OnChange(null, null, results);
                        }
                    }
                    var createError = function (obj) {
                        var cloneObj = obj;
                        return function (xhr, status, error) {
                            cloneObj.RemoveClick();
                            require(['sweetAlert'], function () {
                                var message;
                                if (xhr.status == 413) message = getTextResource('Error413');
                                else if (xhr.responseJSON.Message) message = xhr.responseJSON.Message;
                                swal(getTextResource('ErrorCaption'), message, 'error');
                            });
                        };
                    }
                    self.AddItem(item);
                    var param = {
                        'filePostfix': item.FilePostfix,
                        'objectID': self.WithoutReference() ? '00000000-0000-0000-0000-000000000000' : self.ObjectID,
                    }

                    function getCallBack(index) {
                        return function (result) {
                            results.push(result)
                            if (index != undefined && self.Items()[index])
                                self.Items()[index].ID = result;
                        };
                    };
                    var callBack = getCallBack(self.Items().indexOf(item));

                    extendAjaxData(param);
                    var xhrUpload = $.ajax({
                        type: "POST",
                        url: '/api/Files?' + $.param(param),
                        cache: false,
                        contentType: false,
                        processData: false,
                        data: data,
                        dataType: 'json',
                        xhr: function () {
                            var cloneItem = item;
                            //
                            var xhr = $.ajaxSettings.xhr(); // получаем объект XMLHttpRequest
                            xhr.upload.addEventListener('progress', function (evt) { // добавляем обработчик события progress (onprogress) передачи данных
                                if (evt.lengthComputable) { // если известно количество байт
                                    var receivePercentComplete = Math.ceil(evt.loaded / evt.total * 100);
                                    cloneItem.ProgressValue(receivePercentComplete);
                                }
                            }, false);
                            return xhr;
                        },
                        success: callBack,
                        error: createError(item)
                    });
                    //
                    itemsToUpload.push(item);
                    requests.push(xhrUpload);
                    item.Ajax(xhrUpload);
                }
                $.when.apply($, requests).then(function () {
                    if (createSuccess) createSuccess(itemsToUpload, results);
                });
            };
            //
            self.AfterRender = function () {
                var div = self.getDIV();
                var divForm = div.closest(parentDivCss);//parentForm
                //
                var divAddButton = divForm.find(addBtnCss);
                if (divAddButton.length > 0) {
                    if (window.FormData == undefined)  //upload not supported   
                        divAddButton.removeClass('active');
                    else {
                        var ttcontrol = new tclib.control();
                        ttcontrol.init(divAddButton, { text: getTextResource('Attachments'), side: 'top' });
                        //
                        divAddButton.on('click', function () {
                            if (self.MaxFiles && self.Items().length >= self.MaxFiles)
                                return
                            self.AddFileClick();
                        });
                    }
                }
                //
                var inputElem = div.find('.fileInput');
                div.find('.fileInput').change(function () {
                    var length = $(inputElem).length;
                    var files = $(inputElem)[length - 1].files;
                    if (onefile && files.length > 1) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('OnyOneFile'), 'error');
                        });
                        $(inputElem).val('');//clear uploaded info FormData
                        return;
                    }
                    if (onefile && self.IsDownloaded())
                        self.RemoveUploadedFilesOneControl();
                    if (self.ReadOnly() == false && self.AvailableAdd() == true)
                        self.UploadFiles(files);
                    //
                    $(inputElem).val('');//clear uploaded info FormData
                });
                //
                //     
                divForm.unbind('dragover').unbind('dragenter').unbind('drop');//previous control initialization
                //
                divForm.on('dragover', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    //
                    try {
                        if (e.originalEvent.dataTransfer)
                            e.originalEvent.dataTransfer.dropEffect = 'copy';
                    }
                    catch (e) { }

                });
                divForm.on('dragenter', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    //
                    try {
                        if (e.originalEvent.dataTransfer)
                            e.originalEvent.dataTransfer.effectAllowed = 'copy';
                    }
                    catch (e) { }//IE problem
                });
                divForm.on('drop', function (e) {
                    if (e.originalEvent.dataTransfer && e.originalEvent.dataTransfer.files && e.originalEvent.dataTransfer.files.length) {
                        e.preventDefault();
                        e.stopPropagation();
                        //
                        if (self.ReadOnly() == false && self.AvailableAdd() == true)
                            self.UploadFiles(e.originalEvent.dataTransfer.files);
                    }
                });
                //
                self.LoadD.resolve();
            };
            //          
            //
            //режим только чтение
            self.ReadOnly = ko.observable(false);
            self.RemoveFileAvailable = ko.observable(true);//возможность удаления прикреплений
            //
            //если все закаченное на сервер более не нужно
            self.RemoveUploadedFiles = function (removeBinding) {
                for (var i = 0; i < self.Items().length; i++) {
                    var item = self.Items()[i];
                    //
                    item.StopUploadAndRemove(true);
                }
                //
                if (removeBinding != false) {
                    var div = self.getDIV();
                    if (div.length == 1) {
                        ko.cleanNode(div[0]);
                        div.remove();
                    }
                }
            };
            self.RemoveUploadedFilesOneControl = function () {
                for (var i = 0; i < self.Items().length; i++) {
                    var item = self.Items()[i];
                    //
                    item.StopUploadAndRemove(false,true);
                }
            };
            //
            //загрузить документы из репозитория по объекту с идентификатором objectID,
            //withoutReference  - при добавлении и удалении файлов не менять документы у объекта с идентификатором objectID
            self.Initialize = function (objectID, withoutReference) {
                $.when(self.LoadD).done(function () {
                    if (self.ObjectID == null)
                        self.RemoveUploadedFiles(false);
                    //
                    self.ObjectID = objectID;
                    //                
                    var savedReadOnlyValue = self.ReadOnly();
                    self.ReadOnly(true);
                    //
                    self.ajaxControl_init.Ajax(self.getAjaxDIV(),
                        {
                            url: '/api/object/' + objectID + '/documents',
                            method: 'GET',
                            dataType: "json"
                        },
                        function (response) {
                            if (response) {
                                self.Items.removeAll();
                                //
                                var hasError = false;
                                var error = getTextResource('FilesNotAttached').replace('\"{0}\"', self.getSizeString(self.MaxFileSize)) + ": ";
                                $.each(response, function (index, documentInfo) {
                                    if (self.MaxFileSize && documentInfo.Size > self.MaxFileSize) {
                                        if (hasError)
                                            error += ', ';
                                        //
                                        error += documentInfo.FullName; // TODO: FullName is obsolete
                                        //
                                        hasError = true;
                                    }
                                });
                                //
                                if (hasError)
                                    swal(error, '', 'info');
                                //
                                $.each(response, function (index, documentInfo) {
                                    if (documentInfo) {
                                        //
                                        if (!self.MaxFileSize || documentInfo.Size <= self.MaxFileSize) {
                                            var item = createItemByDocument(documentInfo);
                                            self.Items().push(item);
                                        }
                                    }
                                });
                                //
                                self.Items.valueHasMutated();
                                self.ReadOnly(savedReadOnlyValue);
                            }
                        });
                });
            };
            //
            //загрузить документ из репозитория по ID документа
            //withoutReference - при добавлении и удалении файлов не менять документы у объекта с идентификатором objectID
            self.InitializeOneSelectFileControl = function (objectID, ID, withoutReference) {
                $.when(self.LoadD).done(function () {
                    if (self.ObjectID == null)
                        self.RemoveUploadedFiles(false);
                    //
                    self.ObjectID = objectID;
                    //
                    if (ID == null || ID == '' || ID == '00000000-0000-0000-0000-000000000000') {
                        self.Items.removeAll();
                        self.Items.valueHasMutated();
                        return;
                    }
                    //                
                    var savedReadOnlyValue = self.ReadOnly();
                    self.ReadOnly(true);
                    //
                    self.ajaxControl_init.Ajax(self.getAjaxDIV(),
                        {
                            url: '/api/document/' + ID,
                            method: 'GET',
                            dataType: "json"
                        },
                        function (response) {
                            if (response) {
                                self.Items.removeAll();
                                ////
                                //if (withoutReference) {
                                //    self.ObjectID = null;
                                //}
                                //
                                var hasError = false;
                                var error = getTextResource('FilesNotAttached').replace('\"{0}\"', self.getSizeString(self.MaxFileSize)) + ": ";
                                $.each(response, function (index, documentInfo) {
                                    if (self.MaxFileSize && documentInfo.Size > self.MaxFileSize) {
                                        if (hasError)
                                            error += ', ';
                                        //
                                        error += documentInfo.Name;
                                        //
                                        hasError = true;
                                    }
                                });
                                //
                                if (hasError)
                                    swal(error, '', 'info');
                                //
                                if (response) {
                                    //if (withoutReference) {
                                    //    response.ObjectID = null;
                                    //}
                                    //
                                    if (!self.MaxFileSize || response.Size <= self.MaxFileSize) {
                                        var item = createItemByDocument(response);
                                        self.Items().push(item);
                                    }
                                }
                                //
                                self.Items.valueHasMutated();
                                self.ReadOnly(savedReadOnlyValue);
                            }
                        });
                });
            };
            //принудительная остановка загрузки файлов
            self.StopUpload = function () {
                self.Items().forEach(function (item) {
                    item.StopProgress();
                });
            };
            //
            //получения объекта-данных о добавляемых файлах (уже закаченных на сервер)
            self.GetData = function () {
                var fileInfoList = [];
                for (var i = 0; i < self.Items().length; i++)
                    fileInfoList.push(self.Items()[i].ToFileInfo());
                //
                return fileInfoList;
            }
            //
            //сохранение прикреплений к объекту с идентификатором objectID (нужно дождаться завершения операции) + НЕ обновляет исходные данные (об идентификтораях сохраненных документов)
            self.SaveUploadedFiles = function (objectID) {
                var retval = $.Deferred();
                //
                var data = {
                    'ObjectID': objectID,//newObjectID
                    'Files': self.GetData()
                };
                //
                var savedReadOnlyValue = self.ReadOnly();
                self.ReadOnly(true);
                self.ajaxControl_save.Ajax(self.getAjaxDIV(),
                    {
                        url: '/fileApi/saveUploadedFiles',
                        method: 'POST',
                        data: data
                    },
                    function (response) {
                        if (response == false) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('AttachmentSaveError') + '\n[fileControl.js, SaveUploadedFiles]', 'error');
                            });
                            //
                            retval.resolve(false);
                        }
                        else {
                            self.ReadOnly(savedReadOnlyValue);
                            retval.resolve(true);
                        }
                    });
                //
                return retval;
            };
            //
            //проверяет все ли прогрессы загрузки уже завершены
            self.IsAllFilesUploaded = function () {
                for (var i = 0; i < self.Items().length; i++) {
                    var item = self.Items()[i];
                    //
                    if (item.Ajax() != null && item.Ajax().readyState != 4) //ReadyState != done
                        return false;
                }
                return true;
            };
            //
            //если пользователь что-то меняет - вызывается эта функция
            self.OnChange = function () { };
            //            
            if (obj)
                self.Load(obj);//fill div by ko template (file list not load)
            //для различия мои файлы и не мои
            $.when(userD).done(function (user) {
                self.CurrentUserID(user.UserID);
            });
        }
    }
    return module;
});