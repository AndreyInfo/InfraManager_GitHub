define(['knockout', 'jquery', 'ajax', 'ui_lists/KB/KBArticle.Short', 'formControl', 'ui_controls/ContextMenu/ko.ContextMenu', 'sweetAlert'],
    function (ko, $, ajaxLib, kbsLib, fc) {
        var module = {
            ViewModel: function (kba) {
                var self = this;
                //
                self.searchResults = {
                    nothingFound: 'nothingFound',
                    emptyFolder: 'emptyFolder',
                    allOK: 'allOK'
                };
                self.showContentParam = {
                    Default: 'Default',
                    KBAList: 'KBAList',
                    KBArticle: 'KBArticle',
                };
                self.showContenHints = {
                    breadcrumbs: 'breadcrumbs',
                    defaultText: 'defaultText',
                    searchResult: 'searchResult'
                };
                self.showPartialForms = {
                    default: 'default',
                    email: 'email',
                    export: 'export'
                }
                //
                self.searchResult = ko.observable(self.searchResults.emptyFolder);
                self.showContent = ko.observable(self.showContentParam.KBAList);
                self.showContenHint = ko.observable(self.showContenHints.defaultText);
                self.showPartialForm = ko.observable(self.showPartialForms.email);
                //
                self.minWidth = 250;
                self.maxWidth = (navigator.userAgent.indexOf("Edg") != -1) ? 375 : 400;

                self.leftPanelWidth = ko.observable(400);
                self.ResizeFunction = function (newWidth) {
                    if (newWidth && newWidth >= self.minWidth && newWidth <= self.maxWidth) {
                        self.leftPanelWidth(newWidth);
                        self.splitterDistance(newWidth);
                    }
                };
                //
                self.KBAFolderSelectedName = ko.observable(null);
                self.KBAFolderListFromParent = ko.observableArray([]);
                self.KBAFolderCurrentkRootName = ko.observable(null);
                self.ShowKbaForm = ko.observable(false);
                //
                {//breadcrumb
                    self.breadcrumbSection = ko.observableArray([]);
                    self.breadcrumbSectionList = ko.computed(function () {
                        return self.breadcrumbSection;
                    });
                    self.KBABreadcrumbSelectedSection = function (section) {

                        if (self.currentFolder() != null)
                            self.currentFolder().Selected(false);

                        var breadcrumbs = [];
                        for (var i = 0; i < self.breadcrumbSection().length; i++) {
                            breadcrumbs.push(self.breadcrumbSection()[i]);
                            if (section.ID == self.breadcrumbSection()[i].ID)
                                break;
                        }

                        self.breadcrumbSection.removeAll();
                        self.breadcrumbSection(breadcrumbs);

                        if (section != null && section.ID != null) {
                            $.when(userD).done(function (user) {

                                var folder_param = {
                                    parentID: section.ID,
                                    seeInvisible: user.HasRoles
                                };
                                var article_param = {
                                    folderID: section.ID,
                                    seeInvisible: user.HasRoles
                                };

                                var article_url = "/api/kb/Articles?" + $.param(article_param);
                                $.when(self.getKBHierarchyFolder(folder_param)).done(function (sections) {
                                    ShowKBArticles(article_url, section.FullName, sections);
                                });

                                self.SelectFolder(null, section.ID);

                            });
                        }
                    };
                }
                //
                self.SelectFolder = function (list, folderID) {
                    if (list === null)
                        list = self.kbaFolderList();
                    //
                    list.forEach(function (folder) {
                        if (folder.ID.toUpperCase() === folderID.toUpperCase()) {
                            folder.Selected(true);
                            self.currentFolder(folder);
                            return;
                        }
                        if (folder.innerList() && folder.innerList().length != 0)
                            self.SelectFolder(folder.innerList(), folderID);
                    });
                };
                //
                //self.showContextMenu = function (sender, e) {
                //    var contextMenuViewModel = self.contextMenu();
                //    e.preventDefault();
                //    contextMenuViewModel.show(e);
                //    return true;
                //};
                //
                {//ko.contextMenu
                    {//granted operations
                        self.grantedOperations = [];
                        //
                        self.operationIsGranted = function (operationID) {
                            for (var i = 0; i < self.grantedOperations.length; i++)
                                if (self.grantedOperations[i] === operationID)
                                    return true;
                            return false;
                        };
                        //
                        $.when(userD).done(function (user) {
                            self.grantedOperations = user.GrantedOperations;
                        });
                    }
                    //                   
                    //
                    {//splitter
                        self.minSplitterWidth = 300;
                        self.maxSplitterWidth = 500;
                        self.splitterDistance = ko.observable(300);
                        self.resizeSplitter = function (newWidth) {
                            if (newWidth && newWidth >= self.minSplitterWidth && newWidth <= self.maxSplitterWidth) {
                                self.splitterDistance(newWidth);
                            }
                        };
                    }
                }
                //
                self.HierarchySections = ko.observableArray([]);
                self.HierarchyRootSection = ko.observableArray([]);
                //
                self.ajaxControl_kba = new ajaxLib.control();
                //
                self.kbaList = ko.observableArray([]);
                self.kbaFolderList = ko.observableArray([]);
                self.popularTagsList = ko.observableArray([]);

                self.clientMode = ko.observable(true);
                self.ShowCenterMessage = ko.observable(true);
                //
                self.currentFolder = ko.observable(null);
                self.currentFolder.subscribe(function (newValue) {
                    self.ShowCenterMessage(false);
                    self.showContenHint(self.showContenHints.breadcrumbs);
                    if (newValue != null && newValue.ID != null) {
                        $.when(userD).done(function (user) {

                            var param = {
                                folderID: newValue.ID,
                                seeInvisible: user.HasRoles
                            };
                            var url = '/api/kb/Articles?' + $.param(param);

                            ShowKBArticles(url, self.KBAFolderSelectedName, null);

                        });
                    }
                });
                //
                self.searchPhrase = ko.observable(null);
                self.searchPhrase.subscribe(function (newValue) {
                    if (newValue && newValue.length > 0)
                        self.InitSearch(newValue);
                    else {
                        self.searchResult(self.searchResults.allOK);
                        self.showContent(self.showContentParam.KBAList);
                    }
                });
                self.SearchKeyPressed = function (data, event) {
                    if (event.keyCode == 13)
                        self.ButtonSearchClick();
                    else return true;
                };
                self.syncTimeout = null;
                self.syncD = null;
                self.InitSearch = function (phrase) {
                    var d = self.syncD;
                    if (d == null || d.state() == 'resolved') {
                        d = $.Deferred();
                        self.syncD = d;
                    }
                    //
                    if (self.syncTimeout)
                        clearTimeout(self.syncTimeout);
                    self.syncTimeout = setTimeout(function () {
                        if (d == self.syncD && phrase == self.searchPhrase()) {
                            $.when(self.Search(phrase)).done(function () {
                                d.resolve();
                            });
                        }
                    },
                        500);
                    //
                    return d.promise();
                };
                //
                self.ButtonSearchClick = function () {
                    var text = self.searchPhrase();
                    if (text)
                        self.Search(text);
                    else {
                        require(['sweetAlert'],
                            function () {
                                swal(getTextResource('SearchEmpty'), '', 'warning');
                            });
                    }
                };
                //

                self.kbaView = ko.observable(null);
                //
                self.CreateKbArticle = function () {
                    if (self.showContenHint() != "searchResult")
                        self.showContenHint(self.showContenHints.breadcrumbs);
                    //
                    self.showContent(self.showContentParam.KBArticle);
                    //
                    require(['ui_lists/KB/KBArticle'], function (lib) {
                        var canEdit = self.operationIsGranted(490);//OPERATION_KBArticle_Add
                        var mod = new lib.ViewModel(canEdit, $('#knowlegdeBase'));
                        //
                        var folderID = self.currentFolder() ? self.currentFolder().ID : null;
                        //
                        $.when(mod.CreateNew(folderID, self.backToList)).done(function () {
                            self.kbaView(mod);
                        });
                    });
                };
                //
                self.SelectKBArticle = function (kbArticle, event) {
                    if (event.currentTarget.className == "b-base-content__item-list")
                        event.currentTarget.className = "b-base-content__item-list-click";
                    else 
                        event.currentTarget.className = "b-base-content__item-list";
                    kbArticle.Selected(!kbArticle.Selected());
                    self.UpdateAddButton();
                };

                self.UpdateAddButton = function () {
                    var hasClicked = false;
                    var elem = document.getElementsByClassName('btnVisibility');
                    if (elem.length > 0) {
                        self.kbaList().forEach(function (el) {
                            if (el.Selected()) {
                                hasClicked = true;
                            }
                        });
                        if (hasClicked)
                            elem[0].style.visibility = 'visible';
                        else
                            elem[0].style.visibility = 'hidden';
                    }
                }

                self.AddKBAReference = function () {
                    for (var i = 0; i < self.kbaList().length; i++) {
                        self.kbaList().forEach(function (el) {
                            if (el.Selected() && (kba.KBArticleDependencyList().findIndex(i => i.KBArticleDependencyID === el.ID) == -1) && kba.ID !=  el.ID) {
                                var elem = {
                                    KBArticleDependencyID: el.ID,
                                    KBArticleDependencyNumber: el.Number,
                                    KBArticleDependencyName: el.Name
                                };
                                kba.KBArticleDependencyList().push(elem);
                            }
                        });                        
                    }
                    kba.KBArticleDependencyList.valueHasMutated();
                };
                //
                self.Search = function (phrase) {
                    $.when(userD).done(function (user) {
                        var param = {
                            text: encodeURIComponent(phrase),
                            seeInvisible: user.HasRoles
                        };
                        self.ajaxControl_kba.Ajax($('.itemsRegion'),
                            {
                                url: '/api/kb/Search?' + $.param(param),
                                method: 'GET'
                            },
                            function (response) {

                                if (response) {

                                    document.getElementsByClassName('searchResultKbaPage')[0].textContent =
                                        getTextResource('KBA_Search_Results') + '(' + ((response == null) ? 0 : response.length) + ')';

                                    self.ShowCenterMessage(false);
                                    self.showContenHint(self.showContenHints.searchResult);
                                    self.searchResult(self.searchResults.allOK);
                                    self.showContent(self.showContentParam.KBAList);

                                    if (response.length != null && response.length > 0) {
                                        if (self.currentFolder() != null)
                                            self.currentFolder().Selected(false);
                                        self.kbaList.removeAll();
                                        response.forEach(function (el) {
                                            el.Section = el.Section.split('\\').join('>');
                                            self.kbaList().push(new kbsLib.KBArticle(el));
                                        });
                                        self.kbaList.sort(function (left, right) {
                                            return left.Number == right.Number
                                                ? 0
                                                : (left.Number < right.Number ? -1 : 1);
                                        });
                                        self.kbaList.valueHasMutated();

                                    } else {
                                        self.searchResult(self.searchResults.nothingFound);
                                        if (self.currentFolder() != null)
                                            self.currentFolder().Selected(false);
                                    }
                                } else {

                                    self.ShowCenterMessage(false);
                                    self.showContenHint(self.showContenHints.searchResult);
                                    self.searchResult(self.searchResults.nothingFound);
                                    self.showContent(self.showContentParam.Default);

                                    document.getElementsByClassName('searchResultKbaPage')[0].textContent =
                                        getTextResource('KBA_Search_Results') + '(0)';

                                    self.kbaList.removeAll();
                                    self.kbaList.valueHasMutated();
                                }
                            });
                    });
                };
                // Draw left panel
                self.ajaxControl_kbafolder = new ajaxLib.control();
                self.Load = function () {
                    var returnD = $.Deferred();
                    $.when(userD).done(function (user) {
                        //
                        self.ShowCenterMessage(true);
                        self.kbaList.removeAll();
                        self.kbaFolderList.removeAll();
                        self.popularTagsList.removeAll();
                        self.searchPhrase('');
                        self.clientMode(!user.HasRoles);
                        //
                        var ajaxControl = new ajaxLib.control();
                        var param = {
                            parentID: '',
                            seeInvisible: user.HasRoles
                        };
                        self.ajaxControl_kbafolder.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                url: '/api/kb/Folders?' + $.param(param)
                            },
                            function (folderList) {
                                var numeric = 1; // Нумерация раздела
                                if (folderList && folderList.length > 0) {

                                    folderList.sort(function (left, right) {
                                        return left.Name.toLowerCase() == right.Name.toLowerCase()
                                            ? 0
                                            : (left.Name.toLowerCase() < right.Name.toLowerCase() ? -1 : 1);
                                    });


                                    folderList.forEach(function (el) {
                                        self.kbaFolderList().push(new module.KBFolder(el.ID,
                                            el.ParentID,
                                            ('0' + numeric).slice(-2) + '. ' + el.Name,
                                            el.HasChilds,
                                            el.Note,
                                            el.FullName,
                                            true,
                                            null,
                                            self));
                                        numeric++;
                                    });
                                    self.kbaFolderList.valueHasMutated();
                                    self.resizeWindow();
                                }
                                //
                                returnD.resolve();
                            });
                    });
                    //
                    return returnD.promise();
                };
                //
                self.resizeWindow = function () {
                    var height = getPageContentHeight() - 100; //отступ для красоты
                    //
                    $(".itemsRegion").height(height - 30 + "px");
                    //$(".b-content-dictionaries-container-KBA").height(height  + "px");
                    
                    //                
                    $('.b-blueMenu').css("height", 'auto'); //border = 1px
                 };
                //
                self.AfterRender = function () {
                    self.resizeWindow();
                    $(window).resize(function () {
                        self.resizeWindow();
                    });
                    //                
                    $('.b-base-content__header-input').focus();
                };
                //
                self.backToList = function () {
                    self.showContent(self.showContentParam.KBAList);
                    self.showContenHint(self.showContenHints.breadcrumbs);
                    //
                    self.currentFolder.valueHasMutated();
                };
                //
                //#region GetFolders

                self.getKBHierarchyFolder = function (param) {

                    return $.getJSON("/api/kb/FolderHierarchy?" + $.param(param),
                        function (folderList) {

                            self.KBAFolderListFromParent = [];

                            //#region sort
                            // Sort by HasChilds
                            folderList.sort(function (left, right) {
                                return (left.HasChilds === right.HasChilds) ? 0 : left.HasChilds ? -1 : 1;
                            });

                            // Sort by Name
                            folderList.sort(function (left, right) {

                                return left.Name.toLowerCase() == right.Name.toLowerCase()
                                    ? 0
                                    : !(left.HasChilds === right.HasChilds)
                                        ? 0
                                        : (left.Name.toLowerCase() < right.Name.toLowerCase() ? -1 : 1);
                            });
                            //#endregion

                            //#region Numeric sections
                            var counts = {};
                            folderList.forEach(function (folder) {
                                var length = (counts[folder.ParentID] || 0) + 1;
                                counts[folder.ParentID] = length;

                                folder.Name = ('0' + length).slice(-2) +
                                    '. ' +
                                    folder.Name;
                            });
                            //#endregion

                            if (folderList.length)
                                self.KBAFolderListFromParent = folderList;
                        }
                    );
                }

                //#endregion

                //#region GET ARTICLES

                function ShowKBArticles(url, folderFullName, sections) {

                    self.ajaxControl_kba.Ajax($('.itemsRegion'),
                        {
                            url: url,
                            method: 'GET'
                        },
                        function (response) {
                            if (response) {
                                if (sections) {
                                    sections = self.KBAFolderListFromParent;
                                }
                                self.kbaList.removeAll();

                                // Sorting Articles
                                response.sort(function (left, right) {
                                    return left.Number == right.Number
                                        ? 0
                                        : (left.Number < right.Number ? -1 : 1);
                                });

                                // Push articles that belong to the selected section (Root articles)
                                response.forEach(function (article) {
                                    if (article.ID != kba.ID) {                                       
                                        if (kba.KBArticleDependencyList().length != 0) {
                                            for (var i = 0; i < kba.KBArticleDependencyList().length; i++) {
                                                if (kba.KBArticleDependencyList()[i].KBArticleDependencyID === article.ID) {
                                                    article.Icon = true;
                                                }
                                            }
                                        }
                                        self.kbaList().push(new kbsLib.KBArticle(article));
                                    }
                                });

                                // Push articles with section name
                                if (sections) {
                                    sections.forEach(function (section) {
                                        response.forEach(function (article) {
                                            if (article.Section == section.FullName) {
                                                article.Section =
                                                    getTextResource('Section') + section.Name;
                                                self.kbaList().push(new kbsLib.KBArticle(article));
                                            }
                                        });
                                    });
                                }

                                self.kbaList.valueHasMutated();

                                // Hide List articles and open the article
                                if (self.kbaList().length > 0) {
                                    self.searchResult(self.searchResults.allOK);
                                    self.showContent(self.showContentParam.KBAList);
                                } else {
                                    self.showContent(self.showContentParam.Default);
                                    self.searchResult(self.searchResults.emptyFolder);
                                }
                            }
                        });
                }

                //#endregion


            },
            KBFolder: function (id, parentId, name, hasChilds, note, fullName, hasNumber, parentObject, self) {
                var kself = this;


                kself.Name = ko.observable(name);

                kself.ParentID = parentId;
                kself.ID = id;

                kself.HasChilds = hasChilds;
                kself.Note = note;
                kself.FullName = fullName;
                kself.Deep = 0;
                kself.Parent = parentObject;

                kself.Loaded = false;
                if (parentObject != null)
                    kself.Deep = parentObject.Deep + 1;
                //
                kself.Expanded = ko.observable(false);
                kself.Selected = ko.observable(false);

                kself.innerList = ko.observableArray([]);

                kself.hasNumber = hasNumber;

                //
                kself.CanExpand = ko.computed(function () {
                    if (!kself.Expanded() && kself.HasChilds)
                        return true;
                    return false;
                });
                //
                kself.ajaxControl = new ajaxLib.control();

                kself.CollapseExpandFolder = function (folder) {
                    if (!folder)
                        return;
                    //
                    folder.Expanded(!folder.Expanded());
                };

                kself.ClickFolder = function (item, event) {


                    $.when(userD).done(function (user) {



                        // Take the root.
                        if (kself.Parent != null)
                            self.KBAFolderCurrentkRootName = kself.FullName.slice(0, kself.FullName.indexOf(" \\"));
                        else
                            self.KBAFolderCurrentkRootName = kself.FullName;

                        self.KBAFolderSelectedName = kself.FullName;

                        //#region show folder options

                        if (user.HasRoles) {
                            var kbfFormMenu = document.getElementsByClassName("circleMenu");

                            // Update Visible Option
                            if (kself.folderVisibleOption() == true) {
                                kself.folderVisibleOption(false);
                            }

                            for (var i = 0; i < kbfFormMenu.length; i++) {
                                if (kbfFormMenu[i].style.display !== "none") {
                                    kbfFormMenu[i].style.display = "none";
                                    // Update visible option
                                    if (kself.folderVisibleOption())
                                        kself.folderVisibleOption(false);
                                }
                            }

                            kself.folderVisibleOption(true);
                        }

                        //#endregion

                        var param = {
                            parentID: kself.ID,
                            seeInvisible: user.HasRoles
                        };
                        $.when(self.getKBHierarchyFolder(param)).done(function (folderList) {
                            kself.innerList.removeAll();

                            //#region push inner list
                            if (folderList && folderList.length > 0) {

                                folderList.forEach(function (el) {

                                    if (el.ParentID == kself.ID) {
                                        kself.innerList().push(new module.KBFolder(
                                            el.ID,
                                            el.ParentID,
                                            el.Name,
                                            el.HasChilds,
                                            el.Note,
                                            el.FullName,
                                            true,
                                            kself,
                                            self));
                                    }
                                });
                            }
                            //#endregion

                            if (self.KBAFolderListFromParent.length == 0)
                                self.KBAFolderListFromParent.push(kself);

                            if (kself.Loaded)
                                kself.innerList.valueHasMutated();

                            kself.Loaded = false;

                            //#region broadcrumbs

                            // Initializing the hierarchy of partitions only once after the selected root
                            if (kself.Parent == null &&
                                self.HierarchyRootSection().map(function (folder) {
                                    return folder.ID;
                                }).indexOf(kself.ID) ===
                                -1) {

                                self.HierarchySections = [
                                    {
                                        ID: kself.ID,
                                        ParentID: null,
                                        Name: kself.Name(),
                                        FullName: kself.FullName
                                    } // The root, create root for top-level node(s)
                                ];

                                folderList.forEach(function (section) {
                                    self.HierarchySections.push({
                                        ID: section.ID,
                                        ParentID: section.ParentID,
                                        Name: section.Name,
                                        FullName: section.FullName
                                    });
                                });

                                self.HierarchySections.forEach(function (node) {
                                    // No parentId means top level
                                    if (!node.ParentID) return self.HierarchyRootSection.push(node);

                                    // Insert node as child of parent in sections array
                                    var parentIndex = -1;
                                    self.HierarchySections.some(function (el, i) {
                                        if (el.ID == node.ParentID) {
                                            parentIndex = i;
                                            return true;
                                        }
                                    });

                                    if (!self.HierarchySections[parentIndex].children) {
                                        return self.HierarchySections[parentIndex].children = [node];
                                    }

                                    self.HierarchySections[parentIndex].children.push(node);
                                });

                            }

                            // Clear all breadcrumbs list
                            self.breadcrumbSection.removeAll();

                            // The parent section
                            var parentId = kself.ParentID;

                            // The selected section
                            var curent_section = {};

                            function treeBreadcrumb(section_hierarchy, id) {

                                if (section_hierarchy != null && section_hierarchy.ID == id) {

                                    curent_section = {
                                        "ID": section_hierarchy.ID,
                                        "ParentID": section_hierarchy.ParentID,
                                        "Name": section_hierarchy.Name,
                                        "FullName": section_hierarchy.FullName
                                    };

                                    return section_hierarchy;
                                } else if (section_hierarchy != null && section_hierarchy.children != null) {
                                    var i;
                                    var result = null;

                                    for (i = 0; result == null && i < section_hierarchy.children.length; i++) {

                                        // Take the root.
                                        if (section_hierarchy.ParentID != null)
                                            root = section_hierarchy.FullName.slice(0,
                                                section_hierarchy.FullName.indexOf(" \\"));
                                        else
                                            root = section_hierarchy.FullName;

                                        result = treeBreadcrumb(section_hierarchy.children[i], id);

                                        if (section_hierarchy.ID == parentId) {
                                            {
                                                self.breadcrumbSection.unshift({
                                                    "ID": section_hierarchy.ID,
                                                    "ParentID": section_hierarchy.ParentID,
                                                    "Name": section_hierarchy.Name,
                                                    "FullName": section_hierarchy.FullName
                                                });

                                                parentId = section_hierarchy.ParentID;
                                            }
                                        }
                                    }

                                    return result;
                                }

                                return null;
                            }

                            // Take the root hierarchy section list
                            var section_hierarchy = self.HierarchyRootSection().filter(function (x) {
                                return x.FullName === self.KBAFolderCurrentkRootName
                            })[0];

                            // Breadcrumbs
                            treeBreadcrumb(section_hierarchy, kself.ID);

                            if (curent_section && curent_section.Name)
                                self.breadcrumbSection.push(curent_section);

                            //#endregion

                        });

                        self.searchPhrase('');
                        if (self.currentFolder() != null)
                            self.currentFolder().Selected(false);
                        self.currentFolder(kself);
                        kself.Selected(true);
                        if (kself.HasChilds) {
                            if (kself.Expanded()) {
                                kself.innerList.removeAll();
                                kself.innerList.valueHasMutated();
                                kself.Expanded(false);
                            } else {
                                var retD = kself.Load($(event.target));
                                $.when(retD).done(function () {
                                    kself.Expanded(true);
                                });
                            }
                        }
                        else
                            kself.CollapseExpandFolder(item);
                    });
                };
                //
                kself.folderVisibleOption = ko.observable(false);
                //
                kself.Load = function ($target) {
                    kself.Loaded = true;
                };



                //#region Kbf Option
                kself.isNotDeleted = ko.observable(true);
                kself.isShowMenu = ko.observable(false);
                kself.ShowFolderEditOption = function () {
                    $.when(userD).done(function (user) {

                        var name = kself.Name();

                        var sectionNumber = "";
                        var sectionName = "";

                        if (kself.hasNumber) {

                            sectionNumber = name.substring(0, name.indexOf('. ') + 2);

                            sectionName = name.substring(name.indexOf('. ') + 2, name.length);
                        } else {
                            sectionName = name;
                        }

                        var sectionNote = kself.Note;

                        var ctrl = undefined;

                        var buttons = {};

                        ctrl = new fc.control('KBFolderEdit',
                            'KBFolderEdit',
                            getTextResource('Edit_KBFolder_Name'),
                            true,
                            true,
                            true,
                            500,
                            350,
                            buttons,
                            null,
                            'data-bind="template: {name: \'../UI/Lists/KB/KBFolderEdit\'}"');

                        buttons[getTextResource('FilterActionSave')] = function () {
                            if ($('.kbaFormInput').val() != '') {

                                showSpinner();

                                var data = {
                                    'ID': kself.ID,
                                    'Name': $('.kbaFormInput').val(),
                                    'Note': $('.kbfAddChildNote').val(),
                                    'SeeInvisible': user.HasRoles
                                };

                                kself.ajaxControl.Ajax(null,
                                    {
                                        dataType: "json",
                                        method: 'PUT',
                                        data: data,
                                        url: '/api/kb/folder/' + kself.ID
                                    },
                                    function (response) {

                                        if (response.success) {
                                            kself.Name(sectionNumber + $('.kbaFormInput').val());
                                            ctrl.Close();
                                        } else {
                                            require(['sweetAlert'],
                                                function () {
                                                    swal(getTextResource('ErrorCaption'),
                                                        getTextResource('KBFEditNameError'),
                                                        'error');
                                                });
                                        }
                                    },
                                    null,
                                    function () {
                                        hideSpinner();
                                    });
                            }
                        };

                        buttons[getTextResource('FilterActionCancel')] = function () { ctrl.Close(); };

                        ctrl.Show();
                        ctrl.ExtendSize(500, 350);

                        require(['ui_lists/KB/KBFolderEdit'],
                            function (vm) {
                                var mod = new vm.ViewModel();

                                mod.Load(sectionName, sectionNote);

                                ko.applyBindings(mod, document.getElementById(ctrl.GetRegionID()));
                            });
                    });
                };
                kself.ShowFolderAddChildOption = function () {
                    $.when(userD).done(function (user) {

                        var ctrl = undefined;

                        var buttons = {};

                        buttons = {
                            "add": {
                                text: getTextResource('Add'),
                                'id': 'addFolder',
                                click: function () {

                                    if ($('.kbaFormInput').val() != '') {

                                        showSpinner();

                                        var data = {
                                            'ParentID': kself.ID,
                                            'Name': $('.kbaFormInput').val(),
                                            'Note': $('.kbfAddChildNote').val(),
                                            'SeeInvisible': user.HasRoles
                                        };

                                        kself.ajaxControl.Ajax(null,
                                            {
                                                dataType: "json",
                                                method: 'POST',
                                                data: data,
                                                url: '/api/kb/Folders'
                                            },
                                            function (response) {
                                                if (response.success) {
                                                    kself.HasChilds = true;
                                                    kself.innerList().push(new module.KBFolder(
                                                        response.section.ID,
                                                        response.section.ParentID,
                                                        response.section.Name,
                                                        false, // HasChilds
                                                        response.section.Note,
                                                        kself.FullName + ' // ' + response.section.Name,
                                                        false,
                                                        kself, // Parent
                                                        self));
                                                    kself.innerList.valueHasMutated();
                                                }
                                                else {
                                                    require(['sweetAlert'],
                                                        function () {
                                                            swal(getTextResource('ErrorCaption'),
                                                                getTextResource('KBFEditNameError'),
                                                                'error');
                                                        });
                                                }

                                                ctrl.Close();
                                            },
                                            null,
                                            function () {
                                                hideSpinner();
                                            });
                                    }
                                }
                            },
                            "cancel": {
                                text: getTextResource('FilterActionCancel'),
                                'class': 'GrayUIButton',
                                click: function () { ctrl.Close(); }
                            }
                        };

                        ctrl = new fc.control('KBFolderAddChild',
                            'KBFolderAddChild',
                            getTextResource('AddingChildSections'),
                            true,
                            true,
                            true,
                            500,
                            350,
                            buttons,
                            null,
                            'data-bind="template: {name: \'../UI/Lists/KB/KBFolderAddChild\'}"');

                        var ctllD = ctrl.Show();
                        ctrl.ExtendSize(500, 350);

                        require(['ui_lists/KB/KBFolderAddChild'],
                            function (vm) {
                                var mod = new vm.ViewModel();

                                var name = kself.Name();

                                var sectionName = "";

                                if (name.indexOf('. ') >= 0) {

                                    sectionName = name.substring(name.indexOf('. ') + 2, name.length);
                                } else {
                                    sectionName = name;
                                }

                                mod.Load(sectionName);

                                ko.applyBindings(mod, document.getElementById(ctrl.GetRegionID()));
                            });
                    });

                };
                kself.DeleteFolder = function () {
                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: getTextResource('Removing') + ': ' + kself.FullName,
                            text: getTextResource('ConfirmRemoveQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: true,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                            function (value) {
                                swal.close();
                                //
                                if (value == true) {
                                    $.when(userD).done(function (user) {
                                        showSpinner();

                                        var param = {
                                            'ID': kself.ID,
                                            'SeeInvisible': user.HasRoles
                                        };
                                        kself.ajaxControl.Ajax(null,
                                            {
                                                dataType: "json",
                                                method: 'GET',
                                                url: '/sdApi/deleteKBFolder?' + $.param(param)
                                            },
                                            function (response) {
                                                var showNewAlert = setInterval(function () {//hack
                                                    var alertIsClosed = $('.sweet-alert.hideSweetAlert')[0];
                                                    if (!alertIsClosed)
                                                        return;
                                                    //
                                                    clearInterval(showNewAlert);
                                                    //
                                                    if (response.Result === 0) {
                                                        kself.isNotDeleted(false);
                                                        swal(getTextResource('KBSDeleted'), '', '');
                                                    } else {
                                                        swal(getTextResource('ErrorCaption'), response.Message, 'info');
                                                    }
                                                }, 100);
                                            });
                                        //
                                        hideSpinner();
                                    });
                                }
                            });
                    });
                };
                //#endregion
            }
        };

        return module;
    });