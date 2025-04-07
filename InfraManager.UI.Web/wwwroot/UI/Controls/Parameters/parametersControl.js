define(['jquery', 'ajax', 'knockout', 'dateTimeControl'], function ($, ajaxLib, ko, dtLib) {
    var module = {
        control: function () {
            var self = this;
            //
            //получение наименования раздела (все до слеша)
            self.getGroupName = function (value) {
                var index = value.indexOf('\\');
                if (index == -1)
                    return value;
                else
                    return value.substring(0, index).trim();
            };
            //получение наименования подгруппы раздела (все после слеша)
            self.getSubgroupName = function (value) {
                var index = value.indexOf('\\');
                if (index == -1)
                    return '';
                else if (value.length <= index + 2)
                    return '';
                else
                    return value.substring(index + 2).trim();
            };
            //           
            //от идентификатора кого зависит, кто запрашивает/кому нужно
            self.getConcatValuesOfDependencyParameter = function (dependencyParameterIdentifier, parameter) {
                if (dependencyParameterIdentifier.length == 0)
                    return '';
                //
                var collect = function (type, editors) {
                    var retval = '';
                    switch (type) {
                        case 8: //ParameterType.Model
                            {
                                for (var i = 0; i < editors.length; i++) {
                                    if (editors[i].Value()) {//tuple
                                        var id = editors[i].Value().Item2;
                                        //                                       
                                        retval += (retval.length == 0 ? '' : ';') + id;
                                    }
                                }
                            }
                            break;
                        case 4: //ParameterType.EnumComboBox
                        case 5: //ParameterType.EnumRadioButton 
                        case 6: //ParameterType.User
                            {
                                for (var i = 0; i < editors.length; i++) {
                                    if (editors[i].Value()) {//guid
                                        var id = editors[i].Value();
                                        //                                       
                                        retval += (retval.length == 0 ? '' : ';') + id;
                                    }
                                }
                            }
                            break;
                        case 10: //ParameterType.Location
                            {
                                for (var i = 0; i < editors.length; i++) {
                                    if (editors[i].Value()) {//guid
                                        var id = editors[i].Value().Item2;
                                        //                                       
                                        retval += (retval.length == 0 ? '' : ';') + id;
                                    }
                                }
                            }
                            break;
                    }
                    return retval;
                };
                //
                //search in simple parameters
                for (var i = 0; i < self.ParameterList().length; i++) {
                    var p = self.ParameterList()[i];
                    if (p.Identifier == dependencyParameterIdentifier) {
                        var tmp = collect(p.Type, p.Editors());
                        return tmp;
                    };
                }
                //search in table parameters, if requestor in table
                if (parameter.ParentTableParameter != null) {
                    var tablePrameterEditors = parameter.ParentTableParameter.Editors();
                    for (var i = 0; i < tablePrameterEditors.length; i++) {
                        var rowParameters = tablePrameterEditors[i].RowParameterList();
                        for (var j = 0; j < rowParameters.length; j++) {
                            if (rowParameters[j] === parameter) {//cell found
                                for (var k = 0; k < rowParameters.length; k++) {//analys all parameters in this row, which index less than requestor index
                                    var p = rowParameters[k];
                                    if (p.Identifier == dependencyParameterIdentifier) {
                                        var tmp = collect(p.Type, p.Editors());
                                        return tmp;
                                    };
                                }
                                return '';
                            }
                        }
                    }
                }
                return '';
            };
            //
            self.getConcatVIDLocationOfDependencyParameter = function (dependencyParameterIdentifier, parameter) {
                if (dependencyParameterIdentifier.length == 0)
                    return '';
                //
                var collect = function (type, editors) {
                    var retval = '';
                    switch (type) {
                        case 10: //ParameterType.Location
                            {
                                for (var i = 0; i < editors.length; i++) {
                                    if (editors[i].Value()) {//guid
                                        var id = editors[i].Value().Item1;
                                        return id;
                                    }
                                }
                            }
                            break;
                    }
                };
                //
                //search in simple parameters
                for (var i = 0; i < self.ParameterList().length; i++) {
                    var p = self.ParameterList()[i];
                    if (p.Identifier == dependencyParameterIdentifier) {
                        var tmp = collect(p.Type, p.Editors());
                        return tmp;
                    };
                }
                //search in table parameters, if requestor in table
                if (parameter.ParentTableParameter != null) {
                    var tablePrameterEditors = parameter.ParentTableParameter.Editors();
                    for (var i = 0; i < tablePrameterEditors.length; i++) {
                        var rowParameters = tablePrameterEditors[i].RowParameterList();
                        for (var j = 0; j < rowParameters.length; j++) {
                            if (rowParameters[j] === parameter) {//cell found
                                for (var k = 0; k < rowParameters.length; k++) {//analys all parameters in this row, which index less than requestor index
                                    var p = rowParameters[k];
                                    if (p.Identifier == dependencyParameterIdentifier) {
                                        var tmp = collect(p.Type, p.Editors());
                                        return tmp;
                                    };
                                }
                                return '';
                            }
                        }
                    }
                }
                return '';
            };
            //
            var checkIndexOfEditorInTableParameter = function (changedParameter, p) {
                if (changedParameter.ParentTableParameter != null) {
                    var tablePrameterEditors = changedParameter.ParentTableParameter.Editors();
                    var indexOfValue_changedParameter = -1
                    var indexOfValue_p = -1;
                    for (var i = 0; i < tablePrameterEditors.length; i++) {
                        var rowParameters = tablePrameterEditors[i].RowParameterList();
                        for (var j = 0; j < rowParameters.length; j++) {
                            if (rowParameters[j] === changedParameter && indexOfValue_changedParameter == -1) //index of row found (it is "i")
                                indexOfValue_changedParameter = i;
                            else if (rowParameters[j] === p && indexOfValue_p == -1)//index of row found (it is "i")
                                indexOfValue_p = i;
                        }
                    }
                    //
                    if (indexOfValue_changedParameter == -1 || indexOfValue_p == -1 || indexOfValue_changedParameter != indexOfValue_p)
                        return false;
                }
                return true;
            };
            var checkValueForModel = function (changedEditor, p) {
                if (checkIndexOfEditorInTableParameter(changedEditor.Parameter, p) == false)
                    return;
                if (changedEditor.Parameter.Type == 8) {//model
                    if (p.Type == 9 && p.Filter && p.Filter.Model == 2 && p.Filter.ModelParameterIdentifier == changedEditor.Parameter.Identifier) //configurationObject + modelFromParam
                        p.ClearAllValues(self.ObjectID == null);
                    if (p.Type == 16 && p.Filter && p.Filter.ParameterModelIdFilter && p.Filter.ModelParameterIdentifier == changedEditor.Parameter.Identifier) //
                        p.ClearAllValues(self.ObjectID == null);
                }
            };
            var checkValueForEnum = function (changedEditor, p) {
                if (checkIndexOfEditorInTableParameter(changedEditor.Parameter, p) == false)
                    return;
                if (changedEditor.Parameter.Type == 4 || changedEditor.Parameter.Type == 5) {//EnumRadioButton or EnumComboBox
                    if ((p.Type == 4 || p.Type == 5) && p.Filter && p.Filter.Mode == 1 && p.Filter.ParentIDParameterIdentifier == changedEditor.Parameter.Identifier) //enum + modelFromParam
                        p.ClearAllValues(self.ObjectID == null);
                }
            };
            var checkValueForLocation = function (changedEditor, p) {
                if (checkIndexOfEditorInTableParameter(changedEditor.Parameter, p) == false)
                    return;
                if (changedEditor.Parameter.Type == 10) {//locatiom
                    if (p.Type == 8 && p.Filter && p.Filter.ParameterModelIdFilter && p.Filter.ModelParameterIdentifier == changedEditor.Parameter.Identifier) //model + locationFromParam
                        p.ClearAllValues(self.ObjectID == null);
                    if (p.Type == 16 && p.Filter && p.Filter.ParameterStorageFilter && p.Filter.StorageParameterIdentifier == changedEditor.Parameter.Identifier) 
                        p.ClearAllValues(self.ObjectID == null);
                }
            };
            var checkValueForUser = function (changedEditor, p) {
                if (checkIndexOfEditorInTableParameter(changedEditor.Parameter, p) == false)
                    return;
                if (changedEditor.Parameter.Type == 6) {//locatiom
                    if (p.Type == 16 && p.Filter && p.Filter.ParameterMOLFilter && p.Filter.MOLParameterIdentifier == changedEditor.Parameter.Identifier)
                        p.ClearAllValues(self.ObjectID == null);
                }
            };
            var checkAllValues = function (changedEditor) {
                for (var i = 0; i < self.ParameterList().length; i++) {
                    var p = self.ParameterList()[i];
                    //
                    if (p.Type != 13) {
                        checkValueForModel(changedEditor, p);
                        checkValueForEnum(changedEditor, p);
                        checkValueForLocation(changedEditor, p);
                        checkValueForUser(changedEditor, p);
                    } else {//table
                        var editors = p.Editors();
                        for (var j = 0; j < editors.length; j++) {
                            var rowParameters = editors[j].RowParameterList();
                            for (var k = 0; k < rowParameters.length; k++) {
                                var pCell = rowParameters[k];
                                checkValueForModel(changedEditor, pCell);
                                checkValueForEnum(changedEditor, pCell);
                                checkValueForLocation(changedEditor, pCell);
                                checkValueForUser(changedEditor, pCell);
                            }
                        }
                    }
                }
            };
            //представление редактора одиночного значения для параметра
            var createParameterValueEditor = function (parameter, val) {//DTL.Parameters.ParameterInfo
                var thisObj = this;
                thisObj.Parameter = parameter;
                //
                thisObj.IsLoaded = ko.observable(false);//after loaded values
                thisObj.IsBusy = ko.observable(false);//for file uploading (ready to get value)
                //
                thisObj.User = ko.observable(null);//need for SearchUser
                //
                thisObj.oldValue = val;
                thisObj.Value = ko.observable(val);
                thisObj.GetValue = function () {//get real value for server side
                    return parameter.Editor_GetValue(thisObj);
                };
                thisObj.GetValueForInit = function () {//get real value for client side, reinit
                    return parameter.Editor_GetValueForInit(thisObj);
                };
                thisObj.GetValueString = ko.computed(function () {//get string representaion of value
                    if (!thisObj.IsLoaded())
                        return '';
                    var value = thisObj.Value();//not need, for ko auto update
                    return parameter.Editor_GetValueString(thisObj, value);
                });
                thisObj.ValidationError = ko.computed(function () {//change value - error message
                    if (!thisObj.IsLoaded())
                        return '';
                    var value = thisObj.Value();//not need, for ko auto update
                    return parameter.Editor_GetValidationError(thisObj, value);
                });
                thisObj.IsInvalid = ko.computed(function () {
                    if (self.DisableValidation() == true)
                        return false;
                    return thisObj.ValidationError().length > 0;
                });
                thisObj.IsEmptyValue = function () {//for IsValueRequired
                    return parameter.Editor_IsEmptyValue(thisObj);
                };
                thisObj.IsValuesExists = function () {//for IsValueRequired
                    return parameter.Editor_IsValuesExists(thisObj);
                };
                thisObj.ClearValues = function (resetSelectedValue) {//for parameters dependency reaction
                    parameter.Editor_ClearValues(thisObj, resetSelectedValue);
                };
                thisObj.AfterRender = function (elements) { //ko control init                   
                    parameter.Editor_AfterRender(thisObj, elements);
                };
                thisObj.DestroyControl = function (allControls) {//some operations with controls
                    if (parameter.Editor_DestroyControl)
                        parameter.Editor_DestroyControl(thisObj, allControls);
                    //
                    if (thisObj.Parameter.Editors().length == 0 && thisObj.ValueHandler && allControls == true)
                        thisObj.ValueHandler.dispose();
                };
                //
                parameter.Editor_InitializeValue(thisObj);
                //
                if (!thisObj.ValueHandler)
                    thisObj.ValueHandler = thisObj.Value.subscribe(function (newValue) {
                        if (thisObj.oldValue == newValue)
                            return;
                        thisObj.oldValue = newValue;
                    });
            };
            //общиее представление параметра, от которого происходит "наследование"
            var createParameter = function (p) {//DTL.Parameters.ParameterInfo
                var thisObj = this;
                //
                thisObj.ParentTableParameter = null;
                //
                thisObj.ID = p.ID;

                thisObj.method = p.method;
                thisObj.urlSetField = p.urlSetField;
                thisObj.callback = p.callback;
                thisObj.GetData = p.GetData; 

                thisObj.DynamicOptions = p.DynamicOptions;
                thisObj.DynamicOperation = function (val, targetOptions) {
                    var flag = true;
                    switch (targetOptions.OperationID) {
                        case 0:
                            flag = val == targetOptions.Constant ? true : false;
                            thisObj.DynamicAction(flag, targetOptions.ActionID);
                            break;
                        case 1:
                            flag = val == targetOptions.Constant ? false : true;
                            thisObj.DynamicAction(flag,targetOptions.ActionID);
                            break;
                        case 2:
                            flag = val == null ? false : true;
                            thisObj.DynamicAction(flag,targetOptions.ActionID);
                            break;
                        case 3:
                            flag = val == null ? true : false;
                            thisObj.DynamicAction(flag,targetOptions.ActionID);
                            break;
                    }
                }
                thisObj.DynamicAction = function (flag, action) {
                    switch (action) {
                        case 0:
                            thisObj.Visibility(flag);
                            break;
                        case 1:
                            thisObj.Visibility(!flag);
                            break;
                        case 2:
                            thisObj.ReadOnly(flag);
                            break;
                        case 3:
                            thisObj.ReadOnly(!flag);
                            break;
                        case 4:
                            thisObj.IsValueRequired = flag;
                            break;
                        case 5:
                            thisObj.IsValueRequired = !flag;
                            break;
                    }
                }
                thisObj.ObjectID = p.ObjectID;//не обязательно идентификатор объекта. может быть и идентификатор прототипа (ЖЦ, типа)
                thisObj.IsValueRequired = p.IsValueRequired;
                thisObj.IsMultiple = p.IsMultiple;
                thisObj.Name = p.Name;
                thisObj.Identifier = p.Identifier;
                thisObj.GroupName = self.getGroupName(p.GroupName);//наименование вкладки
                thisObj.SubgroupName = self.getSubgroupName(p.GroupName);//наименование раздела на вкладке
                thisObj.Order = p.Order;
                thisObj.Type = p.Type;
                thisObj.WebVisibility = p.WebVisibility;
                thisObj.Visibility = ko.observable(p.WebVisibility);
                thisObj.ClientMode = self.ClientMode;
                thisObj.UseSearchControl = p.UseSearchControl;
                thisObj.ParameterValueIsReadOnly = p.ParameterValueIsReadOnly;
                thisObj.Filter = p.Filter;//object, see SD.BusinessLayer.Parameters.Filter
                //
                thisObj.InitialPackedValueList = p.JSValueList;//for Validate
                thisObj.Editors = ko.observableArray([]);//one value = one editor    
                
                //
                thisObj.ReadOnly = ko.observable(false);
                thisObj.IsLoadedAllEditors = ko.computed(function () {//after data loaded
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++)
                        if (editors[i].IsLoaded() == false)
                            return false;
                    //
                    return true;
                });
                thisObj.GetPackedValueList = ko.computed(function () {//get all values as string
                    var valueList = [];
                    //
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++) {
                        var value = editors[i].GetValue();
                        valueList.push(value);
                    }
                    //
                    return ko.toJSON(valueList);
                });
                thisObj.GetValueList = function () {
                    let valueList = [];
                    const editors = thisObj.Editors();
                    for (let i = 0; i < editors.length; i++) {
                        const value = editors[i].GetValue();
                        valueList.push(value);
                    }
                    
                    return valueList;
                };
                thisObj.GetPackedValueListForInit = ko.computed(function () {//get all values as string
                    var valueList = [];
                    //
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++) {
                        var value = editors[i].GetValueForInit();
                        valueList.push(value);
                    }
                    //
                    return ko.toJSON(valueList);
                });
                thisObj.IsBusyAnyEditor = ko.computed(function () {//for file uploading (ready to get value)
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++)
                        if (editors[i].IsBusy() == true)
                            return true;
                    //
                    return false;
                });
                thisObj.DestroyAllEditors = function (allControls) {//destroy all editors, for recreate editors
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++)
                        editors[i].DestroyControl(allControls);
                };
                thisObj.ClearAllValues = function (resetSelectValue) {//for parameters dependency reaction
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++)
                        editors[i].ClearValues(resetSelectValue);
                };
                thisObj.IsEmptyAllValues = ko.computed(function () {//for Validate
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++)
                        if (editors[i].IsEmptyValue() == false)
                            return false;
                    //
                    return true;
                });
                thisObj.IsValueExistsInAnyEditor = ko.computed(function () {//for Validate
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++)
                        if (editors[i].IsValuesExists() == true)
                            return true;
                    //
                    return false;
                });
                thisObj.GetValidationErrors = function () {//for Validate
                    var retval = '';
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++) {
                        var error = editors[i].ValidationError();
                        if (error != null && error.length > 0)
                            retval += (retval.length > 0 ? '; ' : '') + error;
                    }
                    //
                    return retval;
                };
                thisObj.GetRestoreValuesAction = function () {//for EditField form
                    var storedValues = thisObj.GetPackedValueListForInit();
                    var retval = function () {
                        thisObj.InitializeEditors(storedValues);
                    };
                    return retval;
                };
                thisObj.ReInitAllValues = function () {//for repaint values in template
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++)
                        editors[i].Value.valueHasMutated();
                    //
                    thisObj.Editors.valueHasMutated();
                };
                thisObj.AddNewEditor = function () {//for add new value
                    if (thisObj.IsMultiple == false)
                        return;
                    var valueEditor = new createParameterValueEditor(thisObj, null);
                    thisObj.Editors().push(valueEditor);
                    thisObj.Editors.valueHasMutated();
                };
                thisObj.RemoveEditor = function (editor) {//for remove exist editor
                    if (thisObj.IsMultiple == false)
                        return;
                    var index = thisObj.Editors().indexOf(editor);
                    if (index != -1 && thisObj.Editors().length > 1) {
                        thisObj.Editors().splice(index, 1);
                        thisObj.Editors.valueHasMutated();
                        //
                        editor.DestroyControl(true);
                    }
                    self.IsLoaded(false);
                    var restoreValues = thisObj.GetRestoreValuesAction();
                    restoreValues();
                    var editors = thisObj.Editors();
                    checkAllValues(editors[0]);
                    self.IsLoaded(true);
                };
                //
                //overrides
                thisObj.TemplateName = null;//ko template
                thisObj.CssClass = ko.observable('');//icon style
                //
                thisObj.CssClassFunc = ko.computed(function () {
                    return thisObj.CssClass() + ((thisObj.ReadOnly() == false && thisObj.IsLoadedAllEditors() == true && (thisObj.Type != 0 || thisObj.Type == 0 && thisObj.IsMultiple == true) && thisObj.Type != 13 && self.DisableValidation() == false) ? ' active' : '');
                });
                thisObj.OnParameterChanged = null;//when request changed                
                //
                //overrides for editors (чтобы не дублировать методы, их спицифичность определяется идивидуально и зависит от типа параметра)
                thisObj.Editor_GetValue = null;
                thisObj.Editor_GetValueForInit = function (editor) {
                    var retval = thisObj.Editor_GetValue(editor);
                    var parameterList = self.ParameterList();
                    parameterList.forEach(function (el) {
                        if(el.DynamicOptions != null)
                        el.DynamicOptions.forEach(function (dyEl) {
                            if (thisObj.Identifier == dyEl.ParentIdentifier) {
                                el.DynamicOperation(retval, dyEl);
                                el.ReInitAllValues();
                            }
                        });
                    })
                    thisObj.Editors.valueHasMutated();
                    return retval;
                };
                thisObj.Editor_GetValueString = null;
                thisObj.Editor_GetValidationError = null;
                thisObj.Editor_IsEmptyValue = null;
                thisObj.Editor_ClearValues = null;
                thisObj.Editor_AfterRender = null;
                thisObj.Editor_DestroyControl = null;
                thisObj.Editor_InitializeValue = null;
                //
                //параметры открытия редактора значений
                thisObj.GetEditorSettings = function (objClassID) {
                    var options = {
                        ID: self.ObjectID,
                        objClassID: objClassID,
                        fieldName: 'ParameterValue',
                        fieldFriendlyName: thisObj.Name,
                        urlSetField: thisObj.urlSetField,
                        callback: thisObj.callback,
                        GetData: p.GetData, 
                        method: thisObj.method,
                        oldValue: thisObj.ParentTableParameter ? thisObj.ParentTableParameter.GetPackedValueList() : thisObj.GetPackedValueList(),//значение на момент вызова редактора
                        restoreValueAction: thisObj.GetRestoreValuesAction(),//для возвращения значения параметра к первоначальному виду - до открытия редактора
                        template: thisObj.TemplateName,//шаблон редактрора параметра
                        model: thisObj,//модель параметра
                        tableModel: thisObj.ParentTableParameter,//модель родительского параметра-таблицы
                        onSave: function () {//реакция на изменение параметра
                            options.restoreValueAction = null;
                        },
                        onClose: function () {//реакция за закрытие редактора
                            thisObj.EditorOptions = null;
                            //
                            if (thisObj.Type == 4)//rb
                                thisObj.ReInitAllValues();//ko problem for radio
                            var restoreValues = thisObj.GetRestoreValuesAction();
                            restoreValues();
                        },
                        nosave: self.ObjectID == null ? true : false
                    };
                    return options;
                };
                //кешированные параметры
                thisObj.EditorOptions = null;
                //метод отображения редактора значений параметра
                thisObj.ShowEditor = function (objClassID) {
                    if (thisObj.ReadOnly()/* && thisObj.ParentTableParameter == null*/)
                        return;
                    //
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        thisObj.EditorOptions = thisObj.GetEditorSettings(objClassID);
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.parameterEdit, thisObj.EditorOptions);
                    });
                };
                //метод сохранения значений параметра
                thisObj.SaveValue = function ($parentRegion, objClassID, silent) {
                    if ($parentRegion) {
                        var target = $parentRegion[0];
                        showSpinner(target);
                    }
                    var options = this.EditorOptions == null ? thisObj.GetEditorSettings(objClassID) : thisObj.EditorOptions;
                    if (silent == true)
                        options.silent = true;
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(false);
                        require(['models/SDForms/EditField'], function (vm) {
                            var mod = new vm.ViewModel(fh.SDEditorTemplateModes.parameterEdit, options);
                            mod.TemplateLoadedD.resolve();//unblock save method
                            var saveD = mod.Save(true);//replace anyway
                            $.when(saveD).done(function () {
                                if (options.restoreValueAction != null)//nod saved
                                    options.restoreValueAction();
                                options.oldValue = thisObj.GetPackedValueList();
                                //
                                if ($parentRegion)
                                    hideSpinner(target);
                            });
                        });
                    });
                };
               
                //метод сохранения текущих значений на сервер (для галочки, для файла)
                thisObj.SilentSaveValue = function ($parentRegion, objClassID) {
                    if (thisObj.EditorOptions != null)
                        return;
                    thisObj.SaveValue($parentRegion, objClassID, true);
                };
                //метод инициализации редакторов
                thisObj.InitializeEditors = function (jsValueList) {
                    var editors = thisObj.Editors();
                    //
                    var oldList = [];
                    for (var i = 0; i < editors.length; i++)
                        oldList.push(editors[i]);
                    editors.splice(0, editors.length);
                    //
                    for (var i = 0; i < oldList.length; i++)
                        oldList[i].DestroyControl(true);
                    //
                    if (jsValueList) {
                        var valueList = JSON.parse(jsValueList);
                        if (thisObj.Type == 13 && valueList.length == 1 && valueList[0] == null)//no value (empty table)
                            return;
                        for (var i = 0; i < valueList.length; i++) {
                            var valueEditor = new createParameterValueEditor(thisObj, valueList[i]);
                            editors.push(valueEditor);
                            if (thisObj.IsMultiple == false)
                                break;
                        }
                    }
                    thisObj.Editors.valueHasMutated();
                };
                thisObj.Height = ko.observable(45);
                //
                //init paramter by type
                switch (thisObj.Type) {//InfraManager.SD.BusinessLayer.Parameters.ParameterType
                    case -1://Separator subgroup
                        self.initAsSeparator(thisObj);
                        break;
                    case 0://Boolean
                        self.initAsBooleanParam(thisObj);
                        break;
                    case 1://Number
                        self.initAsNumberParam(thisObj);
                        break;
                    case 2://String
                        self.initAsStringParam(thisObj);
                        break;
                    case 3://DateTime
                        self.initAsDateTimeParam(thisObj);
                        break;
                    case 4://EnumRadioButton
                        self.initAsEnumRadioButtonParam(thisObj);
                        break;
                    case 5://EnumComboBox
                        self.initAsEnumComboBoxParam(thisObj);
                        break;
                    case 6://User
                        self.initAsUserParam(thisObj);
                        break;
                    case 7://Subdivision
                        self.initAsSubdivisionParam(thisObj);
                        break;
                    case 8://Model
                        self.initAsModelParam(thisObj);
                        break;
                    case 9://ConfigurationObject
                        self.initAsConfigurationObjectParam(thisObj);
                        break;
                    case 10://Location
                        self.initAsLocationParam(thisObj);
                        break;
                    case 11://File
                        self.initAsFileParam(thisObj);
                        break;
                    case 12://Position
                        self.initAsPositionParam(thisObj);
                        break;
                    case 13://Table
                        self.initAsTableParam(thisObj);
                        break;
                    case 14://StorageLocation
                        self.initAsStorageLocationParam(thisObj);
                        break;
                    case 15://DataEntity
                        self.initAsDataEntityParam(thisObj);
                        break;
                    case 16://Consignment
                        self.initAsConsignmentParam(thisObj);
                        break;
                }
                //
                thisObj.InitializeEditors(p.JSValueList);
            };
            //
            //метод открытия карточки объекта
            self.ShowObjectForm = function (id, classID) {
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    if (classID == 165)
                        fh.ShowDataEntityObjectForm(id);
                    else if (classID == 5 || classID == 6 || classID == 33 || classID == 34)
                        fh.ShowAssetForm(id, classID);
                    else if (classID == 409 || classID == 410 || classID == 411 || classID == 412 ||
                        classID == 413 || classID == 414 || classID == 415 || classID == 419)
                        fh.ShowConfigurationUnitForm(id);
                    else
                        hideSpinner();
                });
            };
            //
            self.initAsSeparator = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('');
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/Separator';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return false;
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return true;
                };
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                };
                thisObj.Editor_GetValue = function (editor) {
                    return null;
                };
                thisObj.Editor_GetValueString = function (editor) {
                    return '';
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                };
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.IsLoaded(false);
                };
            };
            self.initAsBooleanParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramBool');
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/BooleanParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    var val = editor.Value();
                    if (val == null && thisObj.IsValueRequired)
                        return getTextResource('ParameterMustBeSet');
                    //
                    return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return editor.Value() == null;
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return true;
                };
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                };
                thisObj.Editor_GetValue = function (editor) {//bool?
                    return editor.Value();
                };
                thisObj.Editor_GetValueString = function (editor) {
                    if (editor.Value() == true)
                        return getTextResource('True');
                    else if (editor.Value() == false)
                        return getTextResource('False');
                    else
                        return '';
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                };
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.IsLoaded(true);
                };
            };
            self.initAsNumberParam = function (p) {
                var thisObj = p;
                //
                var minValue = thisObj.Filter.MinValue;
                var maxValue = thisObj.Filter.MaxValue;
                var isInteger = thisObj.Filter.IsInteger;
                //
                thisObj.CssClass('paramNumber');
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/NumberParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    var val = editor.Value();
                    if (typeof val === 'string' || val instanceof String)
                        val = val.replace(',', '.').split(' ').join('');
                    //
                    if (isNaN(parseFloat(val)) || !isFinite(val))
                        return getTextResource('ParameterNumber_ValueIsNotNumber');
                    val = parseFloat(val);
                    //
                    if (val < minValue)
                        return getTextResource('ParameterNumber_ValueIsLessThanMin');
                    if (val > maxValue)
                        return getTextResource('ParameterNumber_ValueIsMoteThanMax');
                    if (isInteger && val != parseInt(val))
                        return getTextResource('ParameterNumber_ValueIsNotAInteger');
                    //
                    return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    var val = editor.Value();
                    if (typeof val === 'string' || val instanceof String)
                        val = val.replace(',', '.').split(' ').join('');
                    return (val == null || val.length == 0) ? true : isNaN(parseFloat(val)) || !isFinite(val);
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return true;
                };
                thisObj.Editor_ClearValues = function (resetSelectedValue) {
                    if (resetSelectedValue && minValue > 0 && maxValue < 0)
                        editor.Value(0);
                };
                thisObj.Editor_GetValue = function (editor) {//decimal
                    var val = editor.Value();
                    if (typeof val === 'string' || val instanceof String)
                        val = val.replace(',', '.').split(' ').join('');
                    return !isNaN(parseFloat(val)) && isFinite(val) ? parseFloat(val) : 0;
                };
                thisObj.Editor_GetValueString = function (editor) {
                    var val = editor.Value();
                    if (val == null)
                        return '';
                    else
                        return editor.GetValue();
                };
                thisObj.Editor_DestroyControl = function (editor, allControls) {
                    if (editor.handlers) {
                        if (allControls == false) {
                            if (editor.handlers.length > 0) {
                                var handler = editor.handlers[editor.handlers.length - 1];
                                handler.dispose();
                                editor.handlers.splice(editor.handlers.length - 1, 1);
                            }
                        }
                        else {
                            for (var i = 0; i < editor.handlers.length; i++)
                                editor.handlers[i].dispose();
                            editor.handlers.splice(0, editor.handlers.length);
                        }
                    }
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                    var $parent = $(elements[0].parentElement);
                    var $input = $parent.find(':input');
                    showSpinner($parent[0]);
                    require(['jqueryStepper'], function () {
                        $input.stepper({
                            type: isInteger ? 'int' : 'float',
                            floatPrecission: isInteger ? 0 : 2,
                            wheelStep: 1,
                            arrowStep: isInteger ? 1 : 0.01,
                            limit: [minValue, maxValue],
                            onStep: function (val, up) {
                                editor.Value(val);
                            }
                        });
                        $parent.find('.stepper-btn-up').css({ display: thisObj.ReadOnly() == true ? 'none' : 'block' });
                        $parent.find('.stepper-btn-dwn').css({ display: thisObj.ReadOnly() == true ? 'none' : 'block' });
                        hideSpinner($parent[0]);
                        //
                        var handler = null;
                        handler = thisObj.ReadOnly.subscribe(function (newValue) {
                            if ($.contains(window.document, $parent[0])) {
                                $parent.find('.stepper-btn-up').css({ display: newValue == true ? 'none' : 'block' });
                                $parent.find('.stepper-btn-dwn').css({ display: newValue == true ? 'none' : 'block' });
                            }
                            else {
                                handler.dispose();
                                var index = editor.handlers.indexOf(handler);
                                if (index != -1)
                                    editor.handlers.splice(index, 1);
                            }
                        });
                        editor.handlers.push(handler);
                    });
                };
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.handlers = [];
                    editor.ClearValues(true);
                    if (editor.Value()) {
                        var realValue = editor.Value();
                        if (typeof realValue === 'string' || realValue instanceof String)
                            realValue = realValue.replace(',', '.').split(' ').join('');
                        if (isInteger)
                            realValue = parseInt(realValue);
                        else if (realValue - Math.floor(realValue) == 0 && typeof realValue === 'number') {//целое число
                            realValue = realValue.toFixed(2);//JSON bugs
                            editor.oldValue = realValue;
                        }
                        editor.Value(realValue);
                    }
                    //
                    editor.IsLoaded(true);
                };
            };
            self.initAsDateTimeParam = function (p) {
                var thisObj = p;
                //
                var isMoreThanNow = thisObj.Filter.IsMoreThenNow;
                //
                thisObj.CssClass('paramDate');
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/DateTimeParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    var val = editor.GetValue();
                    if (val && isMoreThanNow && val.valueOf() < (new Date()).valueOf())
                        return getTextResource('ParameterDateTime_DateMustBeMoreThanNow');
                    else if (!val && thisObj.IsValueRequired)
                        return getTextResource('ParameterMustBeSet');
                    else
                        return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return editor.GetValue() == null;
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return true;
                };
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                };
                thisObj.Editor_GetValue = function (editor) {//dateTime?
                    var val = editor.ValueDateTime();
                    return dtLib.GetMillisecondsSince1970(val);
                };
                thisObj.Editor_GetValueForInit = function (editor) {
                    var val = editor.ValueDateTime();
                    if (val == null || isNaN(val))
                        return val;
                    else
                        return val.toISOString();//web to client
                };
                thisObj.Editor_GetValueString = function (editor) {
                    return editor.Value();
                };
                thisObj.Editor_DestroyControl = function (editor, allControls) {
                    if (editor.handlers) {
                        if (allControls == false) {
                            if (editor.handlers.length > 0) {
                                var handler = editor.handlers[editor.handlers.length - 1];
                                handler.dispose();
                                editor.handlers.splice(editor.handlers.length - 1, 1);
                            }
                        }
                        else {
                            for (var i = 0; i < editor.handlers.length; i++)
                                editor.handlers[i].dispose();
                            editor.handlers.splice(0, editor.handlers.length);
                        }
                    }
                    if (editor.controls) {
                        if (allControls == false) {
                            if (editor.controls.length > 0) {
                                var ctrl = editor.controls[editor.controls.length - 1];
                                ctrl.datetimepicker('destroy');
                                editor.controls.splice(editor.controls.length - 1, 1);
                            }
                        }
                        else {
                            for (var i = 0; i < editor.controls.length; i++)
                                editor.controls[i].datetimepicker('destroy');
                            editor.controls.splice(0, editor.controls.length);
                        }
                    }
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                    var $parent = $(elements[0].parentElement);
                    showSpinner($parent[0]);
                    require(['dateTimePicker'], function () {
                        if (locale && locale.length > 0)
                            $.datetimepicker.setLocale(locale.substring(0, 2));
                        var allowTimes = []; for (var xh = 0; xh <= 23; xh++) for (var xm = 0; xm < 60; xm++) allowTimes.push(("0" + xh).slice(-2) + ':' + ("0" + xm).slice(-2));
                        var control = $parent.find(':input').datetimepicker({
                            startDate: editor.ValueDateTime() == null ? new Date() : editor.ValueDateTime(),
                            closeOnDateSelect: false,
                            format: 'd.m.Y H:i',
                            mask: '39.19.9999 29:59',
                            allowTimes: allowTimes,
                            dayOfWeekStart: locale && locale.length > 0 && locale.substring(0, 2) == 'en' ? 0 : 1,
                            value: editor.ValueDateTime(),
                            validateOnBlur: true,
                            onSelectDate: function (current_time, $input) {
                                editor.ValueDateTime(current_time);
                                editor.Value(dtLib.Date2String(current_time));
                            },
                            onSelectTime: function (current_time, $input) {
                                editor.ValueDateTime(current_time);
                                editor.Value(dtLib.Date2String(current_time));
                            }
                        });
                        hideSpinner($parent[0]);
                        editor.controls.push(control);
                        //
                        var handler = null;
                        handler = editor.Value.subscribe(function (newValue) {
                            if ($.contains(window.document, $parent[0])) {
                                var ctrl = editor.controls.length == 0 ? null : editor.controls[editor.controls.length - 1];//last control: typing
                                if (ctrl == null)
                                    return;
                                var dt = ctrl.length > 0 ? ctrl.datetimepicker('getValue') : null;
                                //
                                if (!newValue || newValue.length == 0)
                                    editor.ValueDateTime(null);//clear field => reset value
                                else if (dtLib.Date2String(dt) != newValue) {
                                    editor.ValueDateTime(null);//value incorrect => reset value
                                    editor.Value('');
                                }
                                else
                                    editor.ValueDateTime(dt);
                            }
                            else {
                                handler.dispose();
                                var index = editor.handlers.indexOf(handler);
                                if (index != -1)
                                    editor.handlers.splice(index, 1);
                                //
                                index = editor.controls.indexOf(control);
                                if (index != -1)
                                    editor.controls.splice(index, 1);
                                control.datetimepicker('destroy');
                            }
                        });
                        editor.handlers.push(handler);
                    });
                };
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.handlers = [];
                    editor.controls = [];
                    editor.ValueDateTime = ko.observable(dtLib.StringIsDate(editor.Value()) || isDate(editor.Value()) ? new Date(getUtcDate(editor.Value())) : null);//always dateTime, auto convert serverUtcDateString to jsLocalTime
                    editor.Value(dtLib.Date2String(editor.ValueDateTime()));//always local string
                    //
                    editor.IsLoaded(true);
                };
            };
            self.initAsStringParam = function (p) {
                var thisObj = p;
                //
                var minLength = thisObj.Filter.MinLength;
                var maxLength = thisObj.Filter.MaxLength;
                //
                thisObj.CssClass('paramText');
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/StringParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    var val = editor.Value();
                    if ((!val || val == '') && thisObj.IsValueRequired)
                        return getTextResource('ParameterMustBeSet');
                    else if (val && val.length < minLength)
                        return getTextResource('ParameterString_StringLenMustBeMoreThan').replace('{0}', minLength);
                    else if (val && val.length > maxLength)
                        return getTextResource('ParameterString_StringLenMustBeLessThan').replace('{0}', maxLength);
                    //
                    return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return editor.Value() == ''
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return true;
                };
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                    if (resetSelectedValue)
                        editor.Value('');
                };
                thisObj.Editor_GetValue = function (editor) {//string
                    return editor.Value();
                };
                thisObj.Editor_GetValueString = function (editor) {
                    return editor.Value();
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                    var $parent = $(elements[0].parentElement);
                    $parent.find(':input').attr({ maxlength: maxLength });
                    setTimeout(function () { $parent.find(':input').focus();}, 40);
                };
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.IsLoaded(true);
                };
            };
            self.initAsEnumRadioButtonParam = function (p) {
                var thisObj = p;
                //
                var enumID = thisObj.Filter.EnumID;
                var mode = thisObj.Filter.Mode;
                var parentID = thisObj.Filter.ParentID;
                var parentIDParameterIdentifier = thisObj.Filter.ParentIDParameterIdentifier;
                //
                thisObj.CssClass('paramRadioButton');
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/EnumRadioButtonParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    if (editor.IsValuesExists() && thisObj.IsValueRequired && !editor.Value())
                        return getTextResource('ParameterMustBeSet');
                    else
                        return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return !editor.Value()
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return editor.EnumValueList().length > 0;
                };
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                    if (resetSelectedValue)
                        editor.Value(null);
                    //
                    thisObj.LoadValues(editor);
                };
                thisObj.Editor_GetValue = function (editor) {//Guid?
                    var val = editor.Value();
                    return val;
                };
                thisObj.Editor_GetValueString = function (editor) {
                    var val = editor.Value();
                    if (val != null)
                        for (var i = 0; i < editor.EnumValueList().length; i++) {
                            var tmp = editor.EnumValueList()[i];
                            if (tmp.ID == val)
                                return tmp.Name;
                        }
                    return '';
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                };
                //
                thisObj.LoadValues = function (editor) {
                    editor.IsLoaded(false);
                    editor.EnumValueList([]);
                    //
                    var allParametersLoaded = self.IsLoaded();
                    var parametersValueIDs = self.getConcatValuesOfDependencyParameter(parentIDParameterIdentifier, thisObj);//идентификатор моделей из связного параметра через ;
                    var params = [thisObj.Type, enumID, mode, parentID, parentIDParameterIdentifier, parametersValueIDs];
                    var ajaxControl = new ajaxLib.control();
                    ajaxControl.Ajax(null,
                        {
                            url: '/sdApi/getParameterValueList',
                            data: {
                                Params: ko.toJSON(params),//for post null params
                            },
                            method: 'POST'
                        },
                        function (valueList) {
                            if (valueList) {
                                var objectFound = false;
                                for (var i = 0; i < valueList.length; i++) {
                                    editor.EnumValueList().push(valueList[i]);
                                    if (editor.Value() && !objectFound && valueList[i].ID == editor.Value())
                                        objectFound = true;
                                }
                                //
                                if (objectFound == false && editor.Value()) {
                                    if (self.ObjectID == null) {
                                        editor.ClearValues(true);
                                        return;
                                    } else if (allParametersLoaded == false) {
                                        if (self.IsLoaded() == true)
                                            thisObj.LoadValues(editor);
                                        else {
                                            var handle = null;
                                            handle = self.IsLoaded.subscribe(function (newValue) {
                                                if (newValue == true) {
                                                    thisObj.LoadValues(editor);
                                                    handle.dispose();
                                                }
                                            });
                                        }
                                        return;
                                    }
                                }
                                //
                                editor.EnumValueList.valueHasMutated();
                                editor.Value.valueHasMutated();
                                editor.IsLoaded(true);
                            }
                        });
                };
                thisObj.indexOfEditor = 0;
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.EnumValueList = ko.observableArray([]);
                    //
                    var indexOfEditor = thisObj.indexOfEditor++;
                    editor.UniqueValueIdentifier = thisObj.Identifier + '_' + indexOfEditor.toString();
                    if (editor.Parameter.ParentTableParameter != null && editor.Parameter.UniqueIndex)//if in table
                        editor.UniqueValueIdentifier += editor.Parameter.UniqueIndex.toString();
                    //
                    thisObj.LoadValues(editor);
                };
            };
            //
            self.initAsObjectSearcherParam = function (p, getSearcherParametersAction, selectObjectInfoAction, setSelectedItemAction, getObjectFullNameParamsAction) {
                var thisObj = p;
                //
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/ObjectSearcherParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    if (editor.IsValuesExists() && thisObj.IsValueRequired && editor.Value() == null)
                        return getTextResource('ParameterMustBeSet');
                    else
                        return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return !editor.Value()
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return true;
                };
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                    var params = getSearcherParametersAction();
                    for (var i = 0; i < editor.controls.length; i++)
                        editor.controls[i].SetSearchParameters(params);
                    //
                    if (resetSelectedValue) {
                        editor.Value(null);
                        editor.ValueString('');
                        //
                        for (var i = 0; i < editor.controls.length; i++)
                            editor.controls[i].SetSelectedItem();
                    }
                };
                thisObj.Editor_GetValue = function (editor) {
                    var val = editor.Value();
                    if (val == null)
                        return val;
                    //
                    if (typeof val != 'object')
                        return val;//Guid
                    //
                    return '{' + val.Item1 + ',' + val.Item2 + '}';//tupple<int, guid>
                };
                thisObj.Editor_GetValueForInit = function (editor) {
                    return editor.Value();
                };
                thisObj.Editor_GetValueString = function (editor) {
                    return editor.ValueString();
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                    var $parent = $(elements[0].parentElement);
                    editor.IsLoaded(false);
                    showSpinner($parent[0]);
                    require(['usualForms'], function (fhModule) {
                        $.when(editor.loadD).done(function () {
                            var fh = new fhModule.formHelper();
                            var d = fh.SetTextSearcherToField(
                                $($parent).find(':input'),
                                'FuncSearcher',
                                null,
                                getSearcherParametersAction(),
                                function (objectInfo) {
                                    selectObjectInfoAction(editor, objectInfo);
                                    checkAllValues(editor);
                                },
                                function () {//reset
                                    editor.Value(null);
                                    editor.ValueString('');
                                    checkAllValues(editor);
                                },
                                function (selectedItem) {//close
                                    if (!selectedItem) {
                                        editor.Value(null);
                                        editor.ValueString('');
                                        checkAllValues(editor);
                                    }
                                });
                            $.when(d).done(function (ctrl) {
                                var handler = thisObj.ReadOnly.subscribe(function (newValue) {
                                    if ($.contains(window.document, $parent[0])) {
                                        ctrl.ReadOnly(newValue);
                                    }
                                    else {
                                        handler.dispose();
                                        var index = editor.handlers.indexOf(handler);
                                        if (index != -1)
                                            editor.handlers.splice(index, 1);
                                        //
                                        index = editor.controls.indexOf(control);
                                        if (index != -1)
                                            editor.controls.splice(index, 1);
                                        ctrl.Remove();
                                    }
                                });
                                editor.handlers.push(handler);
                                editor.controls.push(ctrl);
                                //
                                ctrl.ReadOnly(thisObj.ReadOnly());
                                if (editor.Value())
                                    setSelectedItemAction(editor, ctrl);
                                else
                                    ctrl.SetSelectedItem();
                                //
                                editor.IsLoaded(true);
                                hideSpinner($parent[0]);
                            });
                        });
                    });
                };
                thisObj.Editor_DestroyControl = function (editor, allControls) {
                    if (editor.handlers) {
                        if (allControls == false) {
                            if (editor.handlers.length > 0) {
                                var handler = editor.handlers[editor.handlers.length - 1];
                                handler.dispose();
                                editor.handlers.splice(editor.handlers.length - 1, 1);
                            }
                        }
                        else {
                            for (var i = 0; i < editor.handlers.length; i++)
                                editor.handlers[i].dispose();
                            editor.handlers.splice(0, editor.handlers.length);
                        }
                    }
                    if (editor.controls) {
                        if (allControls == false) {
                            if (editor.controls.length > 0) {
                                var ctrl = editor.controls[editor.controls.length - 1];
                                ctrl.Remove();
                                editor.controls.splice(editor.controls.length - 1, 1);
                            }
                        }
                        else {
                            for (var i = 0; i < editor.controls.length; i++)
                                editor.controls[i].Remove();
                            editor.controls.splice(0, editor.controls.length);
                        }
                    }
                };
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.handlers = [];
                    editor.controls = [];
                    editor.loadD = $.Deferred();
                    editor.ValueString = ko.observable('');
                    //                
                    if (editor.Value()) {//value set
                        {
                            var ajaxControl = new ajaxLib.control();
                            var params = getObjectFullNameParamsAction(editor);
                            var d = ajaxControl.Ajax(null,
                                {
                                    url: '/searchApi/getObjectFullName?' + $.param(params),
                                    method: 'GET'
                                },
                                function (objectFullName) {
                                    editor.ValueString(objectFullName.result);
                                    editor.loadD.resolve();
                                    editor.IsLoaded(true);//if AfterRender absent
                                },
                                function () {//object not found => not exists
                                    editor.ClearValues(true);
                                    editor.loadD.resolve();
                                    editor.IsLoaded(true);//if AfterRender absent
                                });
                        }
                    }
                    else {
                        editor.loadD.resolve();
                        editor.IsLoaded(true);//if AfterRender absent
                    }
                };
            };
            //
            self.initAsObjectSearcherParamWithUserInfo = function (p, getSearcherParametersAction, selectObjectInfoAction, setSelectedItemAction, getObjectFullNameParamsAction) {
                var thisObj = p;
                //
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/ObjectSearcherParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    if (editor.IsValuesExists() && thisObj.IsValueRequired && editor.Value() == null)
                        return getTextResource('ParameterMustBeSet');
                    else
                        return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return !editor.Value()
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return true;
                };
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                    var params = getSearcherParametersAction();
                    for (var i = 0; i < editor.controls.length; i++)
                        editor.controls[i].SetSearchParameters(params);
                    //
                    if (resetSelectedValue) {
                        editor.Value(null);
                        editor.ValueString('');
                        //
                        for (var i = 0; i < editor.controls.length; i++)
                            editor.controls[i].SetSelectedItem();
                    }
                };
                thisObj.Editor_GetValue = function (editor) {
                    var val = editor.Value();
                    if (val == null)
                        return val;
                    //
                    if (typeof val != 'object')
                        return val;//Guid
                    //
                    return '{' + val.Item1 + ',' + val.Item2 + '}';//tupple<int, guid>
                };
                thisObj.Editor_GetValueForInit = function (editor) {
                    return editor.Value();
                };
                thisObj.Editor_GetValueString = function (editor) {
                    return editor.ValueString();
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                    var $parent = $(elements[0].parentElement);
                    editor.IsLoaded(false);
                    showSpinner($parent[0]);
                    require(['usualForms'], function (fhModule) {
                        $.when(editor.loadD).done(function () {
                            var fh = new fhModule.formHelper();
                            var d = fh.SetTextSearcherToField(
                                $($parent).find(':input'),
                                'FuncSearcher',
                                null,
                                getSearcherParametersAction(),
                                function (objectInfo) {
                                    selectObjectInfoAction(editor, objectInfo);
                                    self.CheckUserInfo(editor, objectInfo.ID);
                                },
                                function () {//reset
                                    editor.Value(null);
                                    editor.ValueString('');
                                    self.CheckUserInfo(editor, null);
                                },
                                function (selectedItem) {//close
                                    if (!selectedItem) {
                                        editor.Value(null);
                                        editor.ValueString('');
                                        self.CheckUserInfo(editor, null);
                                    }
                                });
                            $.when(d).done(function (ctrl) {
                                var handler = thisObj.ReadOnly.subscribe(function (newValue) {
                                    if ($.contains(window.document, $parent[0])) {
                                        ctrl.ReadOnly(newValue);
                                    }
                                    else {
                                        handler.dispose();
                                        var index = editor.handlers.indexOf(handler);
                                        if (index != -1)
                                            editor.handlers.splice(index, 1);
                                        //
                                        index = editor.controls.indexOf(control);
                                        if (index != -1)
                                            editor.controls.splice(index, 1);
                                        ctrl.Remove();
                                    }
                                });
                                editor.handlers.push(handler);
                                editor.controls.push(ctrl);
                                //
                                ctrl.ReadOnly(thisObj.ReadOnly());
                                if (editor.Value()) {
                                    setSelectedItemAction(editor, ctrl);
                                    self.CheckUserInfo(editor, editor.Value());
                                }
                                else
                                    ctrl.SetSelectedItem();
                                //
                                editor.IsLoaded(true);
                                hideSpinner($parent[0]);
                            });
                        });
                    });
                };
                thisObj.Editor_DestroyControl = function (editor, allControls) {
                    if (editor.handlers) {
                        if (allControls == false) {
                            if (editor.handlers.length > 0) {
                                var handler = editor.handlers[editor.handlers.length - 1];
                                handler.dispose();
                                editor.handlers.splice(editor.handlers.length - 1, 1);
                            }
                        }
                        else {
                            for (var i = 0; i < editor.handlers.length; i++)
                                editor.handlers[i].dispose();
                            editor.handlers.splice(0, editor.handlers.length);
                        }
                    }
                    if (editor.controls) {
                        if (allControls == false) {
                            if (editor.controls.length > 0) {
                                var ctrl = editor.controls[editor.controls.length - 1];
                                ctrl.Remove();
                                editor.controls.splice(editor.controls.length - 1, 1);
                            }
                        }
                        else {
                            for (var i = 0; i < editor.controls.length; i++)
                                editor.controls[i].Remove();
                            editor.controls.splice(0, editor.controls.length);
                        }
                    }
                };
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.handlers = [];
                    editor.controls = [];
                    editor.loadD = $.Deferred();
                    editor.ValueString = ko.observable('');
                    //                
                    if (editor.Value()) {//value set
                        {
                            var ajaxControl = new ajaxLib.control();
                            var params = getObjectFullNameParamsAction(editor);
                            var d = ajaxControl.Ajax(null,
                                {
                                    url: '/searchApi/getObjectFullName?' + $.param(params),
                                    method: 'GET'
                                },
                                function (objectFullName) {
                                    editor.ValueString(objectFullName.result);
                                    editor.loadD.resolve();
                                    editor.IsLoaded(true);//if AfterRender absent
                                },
                                function () {//object not found => not exists
                                    editor.ClearValues(true);
                                    editor.loadD.resolve();
                                    editor.IsLoaded(true);//if AfterRender absent
                                });
                        }
                    }
                    else {
                        editor.loadD.resolve();
                        editor.IsLoaded(true);//if AfterRender absent
                    }
                };
            };
            //
            self.initAsComboBox = function (p, getValueParametersAction, selectObjectInfoAction) {
                var thisObj = p;
                //               
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/EnumComboBoxParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    if (editor.IsValuesExists() && thisObj.IsValueRequired && !editor.Value())
                        return getTextResource('ParameterMustBeSet');
                    else
                        return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return !editor.Value()
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return editor.ValueExists();
                };
                thisObj.ReloadValues = ko.observable(false);
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                    if (!thisObj.ReloadValues()) {
                        var restoreValues = thisObj.GetRestoreValuesAction();
                        restoreValues();
                        thisObj.ReloadValues(true);
                    }
                };
                thisObj.Editor_GetValue = function (editor) {
                    var val = editor.Value();
                    if (val == null)
                        return val;
                    //
                    if (typeof val != 'object')
                        return val;//Guid
                    //
                    return '{' + val.Item1 + ',' + val.Item2 + '}';//tupple<int, guid>
                };
                thisObj.Editor_GetValueForInit = function (editor) {
                    return editor.Value();
                };
                thisObj.Editor_GetValueString = function (editor) {
                    var val = editor.ComboBoxValue();
                    if (val == null)
                        return '';
                    else
                        return val.Name;
                };
                thisObj.Editor_DestroyControl = function (editor, allControls) {
                    if (editor.handler) {
                        editor.handler.dispose();
                        editor.handler = null;
                    }
                };
                thisObj.Editor_AfterRender = function (editor, elements) {
                    var $parent = $(elements[0].parentElement);
                    //                    
                    if (!editor.IsLoaded()) {
                        showSpinner($parent[0]);
                        $.when(editor.ValueListD).done(function () {
                            hideSpinner($parent[0]);
                        });
                    }
                    //
                    thisObj.Editor_DestroyControl(editor, true);
                    editor.handler = editor.ComboBoxValue.subscribe(function (newValue) {
                        if (newValue) {
                            selectObjectInfoAction(editor, newValue);
                            checkAllValues(editor);
                        }
                        else
                            editor.Value(null);
                    });
                };
                thisObj.Editor_CheckHendler = function (editor) {
                    thisObj.Editor_DestroyControl(editor, true);
                    editor.handler = editor.ComboBoxValue.subscribe(function (newValue) {
                        if (newValue)
                        {
                            selectObjectInfoAction(editor, newValue);
                            checkAllValues(editor);
                        }
                        else
                            editor.Value(null);
                    });
                };
                thisObj.LoadValues = function (editor) {
                    editor.ValueExists(false);
                    editor.IsLoaded(false);
                    editor.ValueList.splice(0, editor.ValueList.length - 1);
                    //
                    var allParametersLoaded = self.IsLoaded();
                    var ajaxControl = new ajaxLib.control();
                    var params = getValueParametersAction();
                    ajaxControl.Ajax(null,
                        {
                            url: '/sdApi/getParameterValueList',
                            data: {
                                Params: ko.toJSON(params),//for post null params
                            },
                            method: 'POST'
                        },
                        function (valueList) {
                            if (valueList && valueList.length > 0)
                                editor.ValueExists(true);
                            //
                            var val = editor.Value();
                            if (val) {//value set                            
                                var objectID = null;
                                //
                                if (typeof val != 'object')
                                    objectID = val;//Guid
                                else
                                    objectID = val.Item2;//tupple<int, guid>
                                //
                                var objectFound = false;
                                if (valueList)
                                    for (var i = 0; i < valueList.length; i++)
                                        if (valueList[i].ID == objectID) {
                                            thisObj.Editor_CheckHendler(editor);
                                            editor.ComboBoxValue(valueList[i]);
                                            objectFound = true;
                                            break;
                                        }
                                //
                                if (objectFound == false && self.ObjectID == null && editor.IsLoaded()) {
                                    thisObj.ReloadValues(false);
                                    editor.ClearValues(true);
                                    return;
                                }
                                else if (objectFound == false && allParametersLoaded == false) {
                                    if (self.IsLoaded() == true)
                                        thisObj.LoadValues(editor);
                                    else {
                                        var handle = null;
                                        handle = self.IsLoaded.subscribe(function (newValue) {
                                            if (newValue == true) {
                                                thisObj.LoadValues(editor);
                                                handle.dispose();
                                            }
                                        });
                                    }
                                    return;
                                }
                            }
                            editor.ValueList = valueList ? valueList : [];
                            editor.ValueListD.resolve();
                            editor.IsLoaded(true);
                            thisObj.ReloadValues(false);
                        });
                };
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.ValueListD = $.Deferred();
                    editor.ValueExists = ko.observable(false);
                    editor.ValueList = [];
                    editor.getComboBoxValueList = function (options) {
                        $.when(editor.ValueListD).done(function () {
                            options.callback({
                                data: editor.ValueList, total: editor.ValueList.length
                            });
                        });
                    };
                    editor.ComboBoxValue = ko.observable(null);
                    //
                    thisObj.LoadValues(editor);
                };
            };
            //
            self.initAsEnumComboBoxParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramComboBox');
                //
                var enumID = thisObj.Filter.EnumID;
                var mode = thisObj.Filter.Mode;
                var parentID = thisObj.Filter.ParentID;
                var parentIDParameterIdentifier = thisObj.Filter.ParentIDParameterIdentifier;
                //
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {
                            var parametersValueIDs = self.getConcatValuesOfDependencyParameter(parentIDParameterIdentifier, thisObj);//идентификатор моделей из связного параметра через ;
                            return ['parameter', thisObj.Type, enumID, mode, parentID, parentIDParameterIdentifier, parametersValueIDs]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value(objectInfo.ID);
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value(), 169, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value(),
                                objectClassID: 169//IMSystem.Global.OBJ_ParameterEnum
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            var parametersValueIDs = self.getConcatValuesOfDependencyParameter(parentIDParameterIdentifier, thisObj);//идентификатор моделей из связного параметра через ;
                            return [thisObj.Type, enumID, mode, parentID, parentIDParameterIdentifier, parametersValueIDs]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value(parameterValue.ID);
                        });
            };
            self.ajaxControl_loadUserInfo = new ajaxLib.control();
            self.CheckUserInfo = function (editor, ID) {
                if (ID == null) {
                    editor.User(null);
                    editor.User.valueHasMutated();
                    return;
                }
                
                self.ajaxControl_loadUserInfo.Ajax(null,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/api/users/' + ID
                    },
                    function (response) {
                        if (response) {
                            editor.User(response);
                            editor.User.valueHasMutated();
                        }
                        else
                            editor.User = ko.observable(null);
                    });
            }
            self.initAsUserParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramUser');
                var type = thisObj.Filter.Type;
                var subdivisionID = thisObj.Filter.SubdivisionID;
                var onlyMaterialResponsible = thisObj.Filter.OnlyMaterialResponsible;
                //
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParamWithUserInfo(
                        thisObj,
                        function () {
                            return ['parameter', thisObj.Type, type, subdivisionID, onlyMaterialResponsible, self.ClientID()]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value(objectInfo.ID);
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value(), 9, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value(),
                                objectClassID: 9//IMSystem.Global.OBJ_USER
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            return [thisObj.Type, type, subdivisionID, onlyMaterialResponsible, self.ClientID()]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value(parameterValue.ID);
                        });
                //
                thisObj.OnParameterChanged = function (param) {
                    if (param == 'client')
                        thisObj.ClearAllValues(false);
                };
            };
            self.initAsSubdivisionParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramSubdivision');
                var type = thisObj.Filter.Type;
                var objectID = thisObj.Filter.ObjectID;
                //
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {
                            return ['parameter', thisObj.Type, type, objectID, self.ClientID()]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value(objectInfo.ID);
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value(), 102, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value(),
                                objectClassID: 102//IMSystem.Global.OBJ_DIVISION
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            return [thisObj.Type, type, objectID, self.ClientID()]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value(parameterValue.ID);
                        });
                //
                thisObj.OnParameterChanged = function (param) {
                    if (param == 'client')
                        thisObj.ClearAllValues(false);
                };
            };
            self.initAsPositionParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramPosition');
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {
                            return ['parameter', thisObj.Type]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value(objectInfo.ID);
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value(), 90, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value(),
                                objectClassID: 90//IMSystem.Global.OBJ_POSITION
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            return [thisObj.Type]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value(parameterValue.ID);
                        });
            };
            self.initAsStorageLocationParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramStorageLocation');
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {
                            return ['parameter', thisObj.Type]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value(objectInfo.ID);
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value(), 397, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value(),
                                objectClassID: 397//IMSystem.Global.OBJ_StorageLocation
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            return [thisObj.Type]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value(parameterValue.ID);
                        });
            };
            self.initAsDataEntityParam = function (p) {
                var thisObj = p;
                //
                var typeID = thisObj.Filter.TypeID;
                var lifeCycleStateNameList = thisObj.Filter.LifeCycleStateNameList;
                //
                thisObj.IsCheck = ko.observable(false);
                thisObj.CssClass('paramDataEntity');
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {
                            return ['parameter', thisObj.Type, typeID, lifeCycleStateNameList]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value(objectInfo.ID);
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value(), 165, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value(),
                                objectClassID: 165//IMSystem.Global.OBJ_DataEntity
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            return [thisObj.Type, typeID, lifeCycleStateNameList]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value(parameterValue.ID);
                            thisObj.IsCheck(parameterValue!= null?(parameterValue.ID == null ? false : true):false);
                        });
                thisObj.OpenForm = function (p) {
                    var obj = p.ComboBoxValue();
                    if (obj == null)
                        return;
                    self.ShowObjectForm(obj.ID,obj.ClassID);
                };
            };
            //
            self.initAsConsignmentParam = function (p) {
                var thisObj = p;
                //
                var molFilter = thisObj.Filter.ParameterMOLFilter;
                var storFilter = thisObj.Filter.ParameterStorageFilter;
                var modelFilter = thisObj.Filter.ParameterModelIdFilter;

                var MOLIDParameterIdentifier = thisObj.Filter.MOLParameterIdentifier;
                var storageIDParameterIdentifier = thisObj.Filter.StorageParameterIdentifier;
                var modelIDParameterIdentifier = thisObj.Filter.ModelParameterIdentifier;
                //
                thisObj.IsCheck = ko.observable(false);
                thisObj.CssClass('paramConsignment');
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {
                            var parametersValueMOLIDs = self.getConcatValuesOfDependencyParameter(MOLIDParameterIdentifier, thisObj);
                            var parametersValueStorIDs = self.getConcatValuesOfDependencyParameter(storageIDParameterIdentifier, thisObj);
                            var parametersValueModelIDs = self.getConcatValuesOfDependencyParameter(modelIDParameterIdentifier, thisObj);
                            return ['parameter', thisObj.Type, molFilter, storFilter, modelFilter, parametersValueMOLIDs, parametersValueStorIDs, parametersValueModelIDs]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value(objectInfo.ID);
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value(), 120, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value(),
                                objectClassID: 120//IMSystem.Global.OBJ_MATERIAL
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            var parametersValueMOLIDs = self.getConcatValuesOfDependencyParameter(MOLIDParameterIdentifier, thisObj);
                            var parametersValueStorIDs = self.getConcatValuesOfDependencyParameter(storageIDParameterIdentifier, thisObj);
                            var parametersValueModelIDs = self.getConcatValuesOfDependencyParameter(modelIDParameterIdentifier, thisObj);
                            return [thisObj.Type,molFilter, storFilter, modelFilter, parametersValueMOLIDs, parametersValueStorIDs, parametersValueModelIDs]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value(parameterValue.ID);
                            thisObj.IsCheck(parameterValue != null ? (parameterValue.ID == null ? false : true) : false);
                        });
                thisObj.OpenForm = function (p) {
                    var obj = p.ComboBoxValue();
                    if (obj == null)
                        return;
                    self.ShowObjectForm(obj.ID, obj.ClassID);
                };
            };
            //
            self.initAsModelParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramModel');
                var type = thisObj.Filter.Type;
                var typeID = thisObj.Filter.TypeID;
                var commercialSoftwareModelsOnly = thisObj.Filter.CommercialSoftwareModelsOnly;
                var modelParameterIdentifier = thisObj.Filter.ModelParameterIdentifier;
                var modelParameterAccept = thisObj.Filter.ParameterModelIdFilter;
                //
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {
                            var parametersModelIDs = self.getConcatValuesOfDependencyParameter(modelParameterIdentifier, thisObj);
                            var parametersLocationIDs = self.getConcatVIDLocationOfDependencyParameter(modelParameterIdentifier, thisObj);
                            return ['parameter', thisObj.Type, type, typeID, commercialSoftwareModelsOnly, modelParameterAccept, modelParameterIdentifier, parametersModelIDs, parametersLocationIDs]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value({
                                Item1: objectInfo.ClassID, Item2: objectInfo.ID
                            });
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value().Item2, editor.Value().Item1, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value().Item2,
                                objectClassID: editor.Value().Item1
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            var parametersModelIDs = self.getConcatValuesOfDependencyParameter(modelParameterIdentifier, thisObj);
                            var parametersLocationIDs = self.getConcatVIDLocationOfDependencyParameter(modelParameterIdentifier, thisObj);
                            return [thisObj.Type, type, typeID, commercialSoftwareModelsOnly, modelParameterAccept, modelParameterIdentifier, parametersModelIDs, parametersLocationIDs]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value({
                                Item1: parameterValue.ClassID, Item2: parameterValue.ID
                            });
                        });
            };
            self.initAsLocationParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramLocation');
                var startLocation = thisObj.Filter.StartLocation;
                var startLocationClassID = thisObj.Filter.StartLocationClassID;
                var startLocationID = thisObj.Filter.StartLocationID;
                var hasOnStorage = thisObj.Filter.HasOnStorage;
                var hasMaterials = thisObj.Filter.HasMaterials;
                var type = thisObj.Filter.Type;
                //
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {
                            return ['parameter', thisObj.Type, startLocation, startLocationClassID, startLocationID, hasOnStorage, hasMaterials, type, self.ClientID()]
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value({
                                Item1: objectInfo.ClassID, Item2: objectInfo.ID
                            });
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value().Item2, editor.Value().Item1, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value().Item2,
                                objectClassID: editor.Value().Item1
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {
                            return [thisObj.Type, startLocation, startLocationClassID, startLocationID, hasOnStorage, hasMaterials, type, self.ClientID()]
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value({
                                Item1: parameterValue.ClassID, Item2: parameterValue.ID
                            });
                        });
                //
                thisObj.OnParameterChanged = function (param) {
                    if (param == 'client')
                        thisObj.ClearAllValues(false);
                };
            };
            self.initAsConfigurationObjectParam = function (p) {
                var thisObj = p;
                //
                thisObj.IsCheck = ko.observable(false);
                thisObj.CssClass('paramConfigurationItem');
                var startLocation = thisObj.Filter.StartLocation;
                var startLocationClassID = thisObj.Filter.StartLocationClassID;
                var startLocationID = thisObj.Filter.StartLocationID;
                var onlyFromStorage = thisObj.Filter.OnlyFromStorage;
                var model = thisObj.Filter.Model;
                var modelID = thisObj.Filter.ModelID;
                var searchCode = thisObj.Filter.SearchCode;
                var searchInventaryNumber = thisObj.Filter.SearchInventaryNumber;
                var searchModel = thisObj.Filter.SearchModel;
                var searchSerialNumber = thisObj.Filter.SearchSerialNumber;
                var searchType = thisObj.Filter.SearchType;
                //
                var modelParameterIdentifier = thisObj.Filter.ModelParameterIdentifier;
                //var type = thisObj.Filter.Type;
                var template = thisObj.Filter.ProductCatalogTemplate;
                var isRoot = thisObj.Filter.IsRoot;
                //                
                if (thisObj.UseSearchControl == true)
                    self.initAsObjectSearcherParam(
                        thisObj,
                        function () {//getSearcherParametersAction
                            var parametersModelIDs = self.getConcatValuesOfDependencyParameter(modelParameterIdentifier, thisObj);//идентификатор моделей из связного параметра через ;
                            return ['parameter', thisObj.Type, startLocation, startLocationClassID, startLocationID, onlyFromStorage, model, modelID, modelParameterIdentifier, template, isRoot, self.ClientID(), parametersModelIDs, searchType, searchModel, searchInventaryNumber, searchCode, searchSerialNumber];
                        },
                        function (editor, objectInfo) {//selectedObjectInfoAction
                            editor.Value({
                                Item1: objectInfo.ClassID, Item2: objectInfo.ID
                            });
                            editor.ValueString(objectInfo.FullName);
                        },
                        function (editor, ctrl) {//setSelectedItemAction
                            ctrl.SetSelectedItem(editor.Value().Item2, editor.Value().Item1, editor.ValueString(), '');
                        },
                        function (editor) {//getObjectFullNameParamsAction
                            return {
                                objectID: editor.Value().Item2,
                                objectClassID: editor.Value().Item1
                            }
                        });
                else
                    self.initAsComboBox(
                        thisObj,
                        function () {//getSearcherParametersAction
                            var parametersModelIDs = self.getConcatValuesOfDependencyParameter(modelParameterIdentifier, thisObj);//идентификатор моделей из связного параметра через ;
                            return [thisObj.Type, startLocation, startLocationClassID, startLocationID, onlyFromStorage, model, modelID, modelParameterIdentifier, template, isRoot, self.ClientID(), parametersModelIDs, searchType, searchModel,searchInventaryNumber,searchCode, searchSerialNumber ];
                        },
                        function (editor, parameterValue) {//selectedObjectInfoAction
                            editor.Value({
                                Item1: parameterValue.ClassID, Item2: parameterValue.ID
                            });
                            thisObj.IsCheck(parameterValue != null ? (parameterValue.ID == null ? false : true) : false);
                        });
                //
                thisObj.OnParameterChanged = function (param) {
                    if (param == 'client')
                        thisObj.ClearAllValues(false);
                };
                thisObj.OpenForm = function (p) {
                    var obj = p.ComboBoxValue();
                    if (obj == null)
                        return;
                    self.ShowObjectForm(obj.ID, obj.ClassID);
                };
            };
            self.initAsTableParam = function (p) {
                var thisObj = p;
                //
                thisObj.ClientMode = self.ClientMode;
                thisObj.IsInversion = thisObj.Filter.IsInversion;
                thisObj.LockRowsCount = thisObj.Filter.LockRowsCount;
                thisObj.ParameterTemplateList = thisObj.Filter.Parameters;//ParamterTemplateInfo[]
                {//width of columns / resize
                    thisObj.WidthOfColumnList = thisObj.Filter.WidthOfColumn;//int[] init by filter
                    //
                    for (var i = 0; i < thisObj.ParameterTemplateList.length; i++) {
                        let _columnTemplate = thisObj.ParameterTemplateList[i];
                        if (thisObj.IsInversion)
                            thisObj.ParameterTemplateList[i].Height = ko.observable(thisObj.Height());
                        //
                        _columnTemplate.Width = ko.observable((thisObj.WidthOfColumnList.length > i) ? thisObj.WidthOfColumnList[i] : null);//real value
                        _columnTemplate.WidthCSS = ko.computed(function () {//value for css
                            if (_columnTemplate.Width() == null)
                                return 'auto';
                            else
                                return _columnTemplate.Width() + 'px';
                        });
                        _columnTemplate.ResizeThumbVisible = ko.observable(false);
                    }
                    //
                    thisObj.EnableResizeThumb = function (m, e) {
                        m.ResizeThumbVisible(true);
                    };
                    thisObj.DisableResizeThumb = function (m, e) {
                        m.ResizeThumbVisible(false);
                    };
                    //
                    thisObj.moveThumbData = ko.observable(null);
                    thisObj.moveThumbData.subscribe(function (newValue) {
                        if (newValue) {
                            var columnTemplate = newValue.columnTemplate;
                            if (thisObj.ParameterTemplateList.indexOf(columnTemplate) == thisObj.ParameterTemplateList.length - 1) {
                                columnTemplate.Width(columnTemplate.Width() == null ? 200 : columnTemplate.Width() + 200);
                            }
                        }
                    });
                    thisObj.cancelThumbResize = function () {
                        if (thisObj.moveThumbData() != null) {
                            var columnTemplate = thisObj.moveThumbData().columnTemplate;
                            columnTemplate.ResizeThumbVisible(false);
                            thisObj.moveThumbData(null);
                            //
                            clearTimeout(thisObj.timeoutToSaveWidths);
                            thisObj.timeoutToSaveWidths = setTimeout(thisObj.saveWidths, 2000);
                        }
                    }
                    thisObj.ThumbResizeCatch = function (columnTemplate, e) {
                        if (e.button == 0) {
                            thisObj.moveThumbData({ columnTemplate: columnTemplate, startX: e.screenX, startWidth: columnTemplate.Width() == null ? 100 : columnTemplate.Width() });
                            thisObj.moveThumbData().columnTemplate.ResizeThumbVisible(true);
                        }
                        else
                            thisObj.cancelThumbResize();
                    };
                    thisObj.mouseMoveHandler = function (e) {
                        if (thisObj.moveThumbData() != null) {
                            var dx = e.screenX - thisObj.moveThumbData().startX;
                            thisObj.moveThumbData().columnTemplate.Width(Math.max(thisObj.moveThumbData().startWidth + dx, 50));
                            thisObj.moveThumbData().columnTemplate.ResizeThumbVisible(true);
                        }
                    };
                    thisObj.mouseUpHandler = function (e) {
                        thisObj.cancelThumbResize();
                    };
                    //
                    $(document).bind('mousemove', thisObj.mouseMoveHandler);
                    $(document).bind('mouseup', thisObj.mouseUpHandler);
                    //
                    thisObj.timeoutToSaveWidths = null;
                    thisObj.saveWidths = function () {
                        clearTimeout(thisObj.timeoutToSaveWidths)
                        thisObj.timeoutToSaveWidths = null;
                        //
                        var widths = [];
                        for (var i = 0; i < thisObj.ParameterTemplateList.length; i++) {
                            var columnTemplate = thisObj.ParameterTemplateList[i];
                            var w = columnTemplate.Width() == null ? 100 : columnTemplate.Width();
                            widths.push(w);
                        }
                        thisObj.WidthOfColumnList = widths;
                        //
                        if (self.ObjectID != null) {
                            var ajaxControl = new ajaxLib.control();
                            ajaxControl.Ajax(null,
                                {
                                    method: 'POST',
                                    url: '/sdApi/SaveTableParameterWidths',
                                    dataType: 'json',
                                    data: {
                                        'ID': thisObj.ID,
                                        'ObjectID': self.ObjectID,
                                        'Widths': widths
                                    }
                                });
                        }
                    };
                };
                thisObj.WidthCSS = ko.computed(function () {
                    var retval = 0;
                    for (var i = 0; i < thisObj.ParameterTemplateList.length; i++) {
                        var columnTemplate = thisObj.ParameterTemplateList[i];
                        retval += columnTemplate.Width() == null ? 100 : columnTemplate.Width();
                    }
                    return retval + 'px';
                });
                //
                thisObj.CssClass('paramTable');
                {
                    thisObj.CssTableClass = ko.observable('');
                    $.when(userD).done(function (user) {
                        var val = '';
                        if (user.ListView_GridLines == true) val += ' _gridLines';
                        if (user.ListView_CompactMode === true) val += ' _compact';
                        if (user.ListView_Multicolor === true) val += ' _multiColor';
                        thisObj.CssTableClass(val);
                    });
                }
                thisObj.TemplateName = null;
                //
                thisObj.CheckAll = ko.observable(false);
                thisObj.CheckAll.subscribe(function (newValue) {
                    for (var i = 0; i < thisObj.Editors().length; i++)
                        thisObj.Editors()[i].Checked(newValue);
                });
                thisObj.CkeckedEditors = ko.computed(function () {
                    var retval = [];
                    for (var i = 0; i < thisObj.Editors().length; i++) {
                        var editor = thisObj.Editors()[i];
                        if (editor.Checked() == true)
                            retval.push(editor);
                    }
                    return retval;
                });
                //
                thisObj._readOnly = ko.observable(false);
                thisObj.ReadOnly = ko.pureComputed({
                    read: function () {
                        return thisObj._readOnly();
                    },
                    write: function (readOnly) {
                        thisObj._readOnly(readOnly);
                        for (var j = 0; j < thisObj.Editors().length; j++) {
                            var editor = thisObj.Editors()[j];
                            for (var i = 0; i < editor.RowParameterList().length; i++) {
                                var columnParamterTemplate = editor.RowParameterList()[i];
                                columnParamterTemplate.ReadOnly(readOnly || columnParamterTemplate.ParameterValueIsReadOnly());
                            }
                        }
                    },
                    owner: thisObj
                });
                //
                thisObj.indexOfValue = 0;
                thisObj.CreateColumnParameterTemplate = function (parameterTemplate) {
                    parameterTemplate.GroupName = '';//hack                    
                    //
                    var columnParamterTemplate = new createParameter(parameterTemplate);//init editors by default
                    columnParamterTemplate.UniqueIndex = thisObj.indexOfValue++;
                    columnParamterTemplate.ParentTableParameter = thisObj;//table parameter
                    columnParamterTemplate.ParameterTemplate = parameterTemplate;//original prototype (not js)
                    columnParamterTemplate.ParameterValueIsReadOnly = ko.observable(parameterTemplate.ParameterValueIsReadOnly);
                    columnParamterTemplate.ParameterValueIsReadOnly.subscribe(function (newValue) {
                        for (var i = 0; i < columnParamterTemplate.Editors().length; i++)
                            columnParamterTemplate.ReadOnly(newValue || self.ReadOnly());
                    });
                    columnParamterTemplate.ParameterValueIsReadOnly.valueHasMutated();
                    columnParamterTemplate.ValidationError = ko.computed(function () {
                        var retval = '';
                        var editors = columnParamterTemplate.Editors();
                        for (var i = 0; i < editors.length; i++) {
                            var error = editors[i].ValidationError();
                            if (error != null && error.length > 0)
                                retval += (retval.length > 0 ? '; ' : '') + error;
                        }
                        //
                        return retval;
                    });
                    return columnParamterTemplate;
                }
                //
                thisObj.Editor_GetValidationError = null;
                thisObj.Editor_IsEmptyValue = null;
                //
                thisObj.IsEmptyAllValues.dispose();
                thisObj.IsEmptyAllValues = ko.computed(function () {
                    var editors = thisObj.Editors();
                    return editors.length == 0;//no values
                });
                //
                thisObj.HasValidationErrors = function () {
                    var editors = thisObj.Editors();
                    //
                    for (var i = 0; i < editors.length; i++) {
                        var error = editors[i].ValidationError();
                        if (error)
                            return true;
                    }
                    //
                    return false;
                };
                //
                thisObj.Editor_IsValuesExists = null;
                thisObj.IsValueExistsInAnyEditor.dispose();
                thisObj.IsValueExistsInAnyEditor = ko.computed(function () {
                    if (thisObj.ParameterTemplateList.length == 0)
                        return false;
                    if (thisObj.LockRowsCount == true || thisObj.ParameterValueIsReadOnly == true)
                        return thisObj.Editors().length > 0;
                    return true;
                });
                //
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                    for (var i = 0; i < editor.RowParameterList().length; i++) {//not RowParameterList
                        var columnParamterTemplate = editor.RowParameterList()[i];
                        columnParamterTemplate.ClearAllValues(resetSelectedValue);
                    }
                };
                thisObj.Editor_GetValue = function (editor) {
                    return editor.Value();
                };
                thisObj.Editor_GetValueString = null;
                //
                thisObj.Editor_AfterRender = null;
                //
                thisObj.Editor_InitializeValue = function (editor) {//row editor                    
                    editor.Checked = ko.observable(false);
                    editor.RowParameterList = ko.observableArray([]);
                    //
                    //override properties and functions
                    editor.IsLoaded = ko.computed(function () {
                        for (var i = 0; i < editor.RowParameterList().length; i++) {
                            var columnParamterTemplate = editor.RowParameterList()[i];
                            for (var j = 0; j < columnParamterTemplate.Editors().length; j++) {
                                var editor1 = columnParamterTemplate.Editors()[j];
                                if (editor1.IsLoaded() == false)
                                    return false;
                            }
                        }
                        return true;
                    });
                    editor.IsBusy = ko.computed(function () {
                        for (var i = 0; i < editor.RowParameterList().length; i++) {
                            var columnParamterTemplate = editor.RowParameterList()[i];
                            if (columnParamterTemplate.IsBusyAnyEditor())
                                return true;
                        }
                        return false;
                    });
                    //
                    editor.ValidationError.dispose();
                    editor.ValidationError = ko.computed(function () {
                        if (!editor.IsLoaded())
                            return '';
                        //
                        var retval = '';
                        for (var i = 0; i < editor.RowParameterList().length; i++) {
                            var columnParamterTemplate = editor.RowParameterList()[i];
                            var error = columnParamterTemplate.GetValidationErrors()
                            if (error != null && error.length > 0) {
                                if (retval.length != 0)
                                    retval += ', ';
                                retval += columnParamterTemplate.Name;
                            }
                        }
                        return retval;
                    });
                    //
                    editor.IsEmptyValue = undefined;//main method of parameter is overrided
                    editor.IsValuesExists = undefined;//main method of parameter is overrided
                    editor.GetValueString.dispose();
                    editor.GetValueString = undefined;
                    editor.AfterRender = undefined;
                    //
                    editor.Value = ko.pureComputed({
                        read: function () {
                            var valueList = [];
                                for (var i = 0; i < thisObj.ParameterTemplateList.length; i++) {//not RowParameterList
                                    if (editor.RowParameterList().length > i) {
                                        var columnParamterTemplate = editor.RowParameterList()[i];
                                        //
                                        var jsValueList = columnParamterTemplate.GetPackedValueList();//json
                                        var readOnly = columnParamterTemplate.ParameterValueIsReadOnly();
                                        valueList.push({
                                            Item1: jsValueList,
                                            Item2: readOnly
                                        });
                                    }
                                    else
                                        valueList.push({
                                            Item1: null,
                                            Item2: false
                                        });
                                }
                            var retval = ko.toJSON(valueList);
                            thisObj.ReHeight();
                            return retval;
                        },
                        write: function (rowPackedValueList) {
                            var rowValueList = rowPackedValueList ? JSON.parse(rowPackedValueList) : null;
                            //
                            //формируем параметры по столбцам (параметры вложены в редактор строки таблицы = значение таблицы)
                            editor.RowParameterList([]);
                                if (rowValueList != null) {
                                    for (var j = 0; j < thisObj.ParameterTemplateList.length; j++) {
                                        var columnParamterTemplate = thisObj.CreateColumnParameterTemplate(thisObj.ParameterTemplateList[j]);
                                        //
                                        var jsValueList = rowValueList ? rowValueList[j].Item1 : null;
                                        var readOnly = rowValueList ? rowValueList[j].Item2 : false;
                                        //
                                        columnParamterTemplate.ParameterValueIsReadOnly(readOnly);
                                        editor.RowParameterList().push(columnParamterTemplate);
                                        columnParamterTemplate.InitializeEditors(jsValueList);//init editors by real value
                                    }
                                    editor.RowParameterList.valueHasMutated();
                            }
                        },
                        owner: thisObj
                    });
                    editor.Value(editor.oldValue);//first value initialization
                    //
                    editor.DestroyControl = function (allControls) {
                        if (thisObj.Editor_DestroyControl)
                            thisObj.Editor_DestroyControl(editor, allControls);
                        //
                        if (thisObj.Editors().length == 0 && thisObj.ValueHandler && allControls == true)
                            thisObj.ValueHandler.dispose();
                    };                    
                };
                 //для инвертированной таблицы
                thisObj.ReHeight = function () {
                    if (thisObj.IsInversion) {
                        for (var col = 0; col < thisObj.Editors().length; col++) {
                            var el = thisObj.Editors()[col];
                            for (var row = 0; row < el.RowParameterList().length; row++) {
                                var arrayHight = $.map(thisObj.Editors(),function (a) { return a.RowParameterList()[row].Height()});
                                var maxHeight = Math.max.apply(Math, arrayHight);
                                var rowHeight = 25 + (20 * el.RowParameterList()[row].Editors().length);
                                if (el.RowParameterList()[row].Type == 11) {
                                    rowHeight = rowHeight * 1.6;
                                }
                                var newHeight = rowHeight > maxHeight ? rowHeight : maxHeight;
                                thisObj.Editors().forEach(function (ell) {
                                    ell.RowParameterList()[row].Height(newHeight);
                                })
                                thisObj.ParameterTemplateList[row].Height(newHeight);
                            }
                        }
                    }
                }
                if (thisObj.IsInversion) {
                    let tmpReadOnly = thisObj.ReadOnly();
                    thisObj.headerBeingResized;
                    thisObj.OnMouseDown = function (m, e) {
                        if (thisObj.IsInversion) {
                            thisObj.ReadOnly(true);
                            thisObj.headerBeingResized = e.target.parentElement.parentElement;
                            window.addEventListener('mousemove', onMouseMove);
                            window.addEventListener('mouseup', onMouseUp);
                        }
                    }
                    const onMouseMove = function (e) {
                        requestAnimationFrame(function () {
                            if (!thisObj.headerBeingResized) {
                                return;
                            }
                            const min = 30;
                            let width = e.movementX + thisObj.headerBeingResized.offsetWidth;
                            width = Math.max(min, width) + 'px';
                            thisObj.headerBeingResized.style.width = width;
                            thisObj.headerBeingResized.style.maxWidth = width;
                            var elements = doArray(thisObj.headerBeingResized.getElementsByTagName("td"));
                            elements.forEach(function (el) {
                                el.style.maxWidth = width;
                            })
                            elements = doArray(thisObj.headerBeingResized.getElementsByTagName("th"));
                            elements.forEach(function (el) {
                                el.style.maxWidth = width;
                            })
                        })
                    };

                    const doArray = function (htmlArr) {
                        var arr = [];
                        for (var x = 0; x < htmlArr.length; x++)
                            arr.push(htmlArr[x]);
                        return arr;
                    }

                    const onMouseUp = function() {
                        window.removeEventListener('mousemove', onMouseMove);
                        window.removeEventListener('mouseup', onMouseUp);
                        thisObj.headerBeingResized = null;
                        setTimeout(setdefaultReadOnly, 50);
                    };

                    let setdefaultReadOnly = function () {
                        thisObj.ReadOnly(tmpReadOnly);
                    }
                }
                thisObj.Editor_DestroyControl = function (editor, allControls) {
                    for (var i = 0; i < editor.RowParameterList().length; i++) {
                        var columnParamterTemplate = editor.RowParameterList()[i];
                        columnParamterTemplate.DestroyAllEditors(allControls);
                    }
                };
                thisObj.DestroyAllEditors = function (allControls) {//destroy all editors, for recreate editors
                    var editors = thisObj.Editors();
                    for (var i = 0; i < editors.length; i++)
                        editors[i].DestroyControl(allControls);
                    //
                    clearTimeout(thisObj.timeoutToSaveWidths);
                    thisObj.timeoutToSaveWidths = null;
                    //
                    //problem with client call registration
                    //$(document).unbind('mousemove', thisObj.mouseMoveHandler);
                    //$(document).unbind('mouseup', thisObj.mouseUpHandler);
                };
                thisObj.AddNewEditor = function () {//for add new value
                    var valueEditor = new createParameterValueEditor(thisObj, null);
                    //
                    valueEditor.RowParameterList([]);
                    for (var j = 0; j < thisObj.ParameterTemplateList.length; j++) {
                        var columnParamterTemplate = thisObj.CreateColumnParameterTemplate(thisObj.ParameterTemplateList[j]);
                        valueEditor.RowParameterList().push(columnParamterTemplate);
                        columnParamterTemplate.InitializeEditors(thisObj.ParameterTemplateList[j].JSValueList);//init editors by real value
                    }
                    valueEditor.RowParameterList.valueHasMutated();
                    //
                    thisObj.Editors().push(valueEditor);
                    thisObj.Editors.valueHasMutated();
                };
                thisObj.RemoveEditor = function (editor) {//for remove exist editor          
                    var index = thisObj.Editors().indexOf(editor);
                    if (index != -1) {
                        thisObj.Editors().splice(index, 1);
                        thisObj.Editors.valueHasMutated();
                        //
                        editor.DestroyControl(true);
                    }
                };
                thisObj.RemoveEditorClick = function () {
                    var editors = thisObj.CkeckedEditors();
                    for (var i = 0; i < editors.length; i++) {
                        if (thisObj.IsValueRequired == true && thisObj.Editors().length == 1)
                            return;
                        thisObj.RemoveEditor(editors[i]);
                    }
                };
            };
            //
            self.initAsFileParam = function (p) {
                var thisObj = p;
                //
                thisObj.CssClass('paramFile');
                thisObj.TemplateName = '../UI/Controls/Parameters/Templates/FileParameter';
                //
                thisObj.Editor_GetValidationError = function (editor) {
                    if (thisObj.IsValueRequired && !editor.Value())
                        return getTextResource('ParameterMustBeSet');
                    else
                        return '';
                };
                thisObj.Editor_IsEmptyValue = function (editor) {
                    return !editor.Value();
                };
                thisObj.Editor_IsValuesExists = function (editor) {
                    return true;
                };
                thisObj.Editor_ClearValues = function (editor, resetSelectedValue) {
                    if (resetSelectedValue) {
                        thisObj.StopProgress(editor);
                        //
                        var oldDocumentID = editor.Value();
                        editor.Value(null);
                        editor.ValueString('');
                        //
                        //новое требование - не удалять файл из репозитария
                        //if (oldDocumentID != null)
                        //    thisObj.RemoveDocumentFromServer(editor, oldDocumentID);
                    }
                };
                thisObj.Editor_GetValue = function (editor) {
                    return editor.Value();
                };//Guid?
                thisObj.Editor_GetValueString = function (editor) {
                    return editor.ValueString();
                };
                thisObj.Editor_AfterRender = function (editor, elements) {//COPIED FROM FILECONTROL.JS
                    var $parent = $(elements[0].parentElement);
                    $($parent).find('.fileInput').change(function () {
                        var files = $parent.find('.fileInput')[0].files;
                        if (!thisObj.ReadOnly() && editor.IsLoaded())
                            thisObj.UploadFiles(editor, files);
                        //
                        $($parent).find('.fileInput').val('');//clear uploaded info FormData
                    });
                };
                //
                //
                thisObj.UploadClick = function (vm, e) {//COPIED FROM FILECONTROL.JS
                    if (window.FormData == undefined || !e.target) {
                        alert("This browser doesn't support HTML5 file uploads!");
                        return;
                    }
                    $(e.target).parent().find('.fileInput').trigger('click');//invoke open file dialog in browser                    
                };
                thisObj.DownloadClick = function (vm, e) {//COPIED FROM FILECONTROL.JS
                    require(['fileControl'], function (fcLib) {
                        var fc = new fcLib.control(null, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        thisObj.DownloadFile(vm, fc, e.target, e.ctrlKey);
                    });
                    return true;
                };
                thisObj.RemoveClick = function (vm) {//vm is editor
                    vm.ClearValues(true);
                };
                //
                //thisObj.RemoveDocumentFromServer = function (editor, id) {
                //    editor.IsBusy(true);
                //    //
                //    var ajaxControl = new ajaxLib.control();
                //    var params = {
                //        documentID: id,
                //        objectID: self.ObjectID
                //    };
                //    ajaxControl.Ajax(null,
                //    {
                //        url: '/fileApi/removeDocumentWithoutReferences?' + $.param(params),
                //        method: 'POST'
                //    },
                //    function (result) {
                //        if (result == true) {//removed
                //            thisObj.SaveValue(null, null);
                //            editor.IsBusy(false);
                //        }
                //    });
                //};
                thisObj.DownloadFile = function (editor, fc, target, controlPressed) {//COPIED FROM FILECONTROL.JS
                    if (editor.Ajax() != null || editor.Value() == null)
                        return;//uploading, not implemented
                    //                
                    var ajaxControl = new ajaxLib.control();
                    ajaxControl.Ajax(target == null ? null : $(target)[0],
                        {
                            method: 'POST',
                            url: '/fileApi/downloadFile',
                            dataType: 'json',
                            data: {
                                'ID': editor.Value(),
                                'ObjectID': self.ObjectID,
                                'FileName': editor.ValueString(),
                                'FilePostfix': 'notMetter'
                            }
                        },
                        function (data) {//file prepared for download
                            var url = data.url;
                            if (url != null) {
                                if (controlPressed == true) {
                                    var re = /(?:\.([^.]+))?$/;
                                    var ext = re.exec(editor.ValueString())[1];
                                    var mediaType = null;
                                    //
                                    if (ext && ext.length > 0) {
                                        ext = ext.toLowerCase();
                                        if (fc.MediaType[ext])
                                            mediaType = fc.MediaType[ext];
                                    }
                                    //
                                    if (mediaType) {//open file from server
                                        var wnd = window.open();
                                        if (wnd) {//browser cancel it?                                    
                                            wnd.document.write('<object id="target" type="' + mediaType + '"></object><script>document.getElementById("target").setAttribute("data", "' + url + '");</script>');
                                            return;
                                        }
                                    }
                                }
                                window.location.href = url;//download file from server
                            }
                            else //файл не найден 
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('DocumentFileNotFound') + '\n[parametersControl.js, DownloadFile]', 'error');
                                });
                        });
                };
                thisObj.StopProgress = function (editor) {//COPIED FROM FILECONTROL.JS
                    var ajax = editor.Ajax();
                    if (ajax != null)
                        ajax.abort();
                    editor.Ajax(null);
                    //
                    editor.ProgressValue(0);
                };
                thisObj.UploadFiles = function (editor, files) {//COPIED FROM FILECONTROL.JS
                    for (var i = 0; i < files.length; i++) {
                        if (files[i].size == 0 && getIEVersion() != -1) {
                            alert('File "' + files[i].name + '" will be ignored, because have zero-size.');
                            continue;
                        }
                        var data = new FormData();
                        var fileInfo = files[i];
                        data.append("file", fileInfo);
                        //
                        var filePostfix = '_' + (new Date()).getTime();//have unique name at any time
                        var param = {
                            'filePostfix': filePostfix,
                            'objectID': '00000000-0000-0000-0000-000000000000'//for only add in repositary
                        }
                        extendAjaxData(param);
                        var xhrUpload = $.ajax({
                            type: "POST",
                            url: '/fileApi/uploadFile?' + $.param(param),
                            cache: false,
                            contentType: false,
                            processData: false,
                            data: data,
                            dataType: 'json',
                            xhr: function () {
                                var xhr = $.ajaxSettings.xhr(); // получаем объект XMLHttpRequest
                                xhr.upload.addEventListener('progress', function (evt) { // добавляем обработчик события progress (onprogress) передачи данных
                                    if (evt.lengthComputable) { // если известно количество байт
                                        var receivePercentComplete = Math.ceil(evt.loaded / evt.total * 100);
                                        //
                                        var oldPercentReceived = editor.ProgressValue() * 2;//2 части - чтение и запись
                                        editor.ProgressValue(receivePercentComplete / 2);
                                        //
                                        if (oldPercentReceived != receivePercentComplete && receivePercentComplete == 100) {//для прогресса получения данных
                                            var checkUploadProgress = undefined;
                                            checkUploadProgress = function () {
                                                var param = {
                                                    'filePostfix': filePostfix
                                                }
                                                var data = {
                                                };
                                                extendAjaxData(data);
                                                $.ajax({
                                                    type: "GET",
                                                    url: '/fileApi/getUploadPercentage?' + $.param(param),
                                                    dataType: 'json',
                                                    data: data,
                                                    success: function (writePercentComplete) {
                                                        if (writePercentComplete != null && writePercentComplete >= 0 && writePercentComplete <= 100)
                                                            editor.ProgressValue(50 + writePercentComplete / 2);
                                                        //
                                                        if (editor.Ajax() != null)
                                                            setTimeout(checkUploadProgress, 200);
                                                    }
                                                });
                                            };
                                            checkUploadProgress();
                                        }
                                    }
                                }, false);
                                return xhr;
                            },
                            success: function (result) {
                                thisObj.StopProgress(editor);
                                if (result && result.id) {
                                    editor.Value(result.id);
                                    editor.ValueString(fileInfo.name);
                                }
                            },
                            error: function (result) {
                                thisObj.StopProgress(editor);
                            }
                        });
                        //
                        editor.Ajax(xhrUpload);
                    }
                };
                //        
                thisObj.Editor_InitializeValue = function (editor) {
                    editor.ValueString = ko.observable('');
                    editor.Ajax = ko.observable(null);//COPIED FROM FILECONTROL.JS
                    editor.Ajax.subscribe(function (newValue) {
                        editor.IsBusy(newValue != null ? true : false);
                    });
                    editor.ProgressValue = ko.observable(0);//COPIED FROM FILECONTROL.JS
                    editor.ProgressVisible = ko.computed(function () {//COPIED FROM FILECONTROL.JS
                        return (editor.Ajax() != null);
                    });
                    //
                    if (editor.Value()) {//value set
                        {
                            var ajaxControl = new ajaxLib.control();
                            var params = {
                                objectID: editor.Value(),
                                objectClassID: 110 // OBJ_DOCUMENT
                            };
                            ajaxControl.Ajax(null,
                                {
                                    url: '/searchApi/getObjectFullName?' + $.param(params),
                                    method: 'GET'
                                },
                                function (objectFullName) {
                                    editor.ValueString(objectFullName.result);
                                    editor.IsLoaded(true);
                                },
                                function () {//object not found => not exists
                                    editor.Value(null);
                                    editor.ValueString('');
                                    editor.IsLoaded(true);
                                });
                        }
                    }
                    else
                        editor.IsLoaded(true);
                };
            };
            //
            //
            //для разделения параметров по группам
            var createParametersGroup = function (groupName, parameterArray) {
                var thisObj = this;
                //
                thisObj.GroupName = groupName;
                thisObj.ParameterList = parameterArray;
                thisObj.IsValid = ko.computed(function () {
                    for (var i = 0; i < thisObj.ParameterList.length; i++)
                        if (thisObj.ParameterList[i].GetValidationErrors().length > 0 &&
                            (self.ObjectID == null || self.ObjectID != null && thisObj.ParameterList[i].Type != 3))//creationMode xor not dateTime
                            return false;
                    //
                    return true;
                });
            };
            //
            //идентификатор объекта, чьи параметры загружены (или null, если объект создается)
            self.ObjectID = null;
            self.ObjectClassID = null;
            //клиент заявки (для фильтрации значений параметров - подствляется в параметры фильтра при получении возможных значений)
            self.ClientID = ko.observable(null);
            self.ClientID.subscribe(function (newValue) {
                $.each(self.ParameterList(), function (index, parameter) {
                    if (parameter.OnParameterChanged)
                        parameter.OnParameterChanged('client');
                });
            });
            //параметры отображаются на клиентской или инженерной форме
            self.ClientMode = ko.observable(true);
            //
            //контрол готов к использоваению (загружены все параметры)
            self.IsLoaded = ko.observable(false);
            //весь список параметров
            self.ParameterList = ko.observableArray([]);
            //отсортированные и разбитые по группам поля
            self.ParameterListByGroup = ko.computed(function () {
                var retval = [];
                var groups = {
                };
                //
                var defaultGroupName = getTextResource('ParametersDefaultGroupName');
                var parameterList = self.ParameterList();
                parameterList.sort(function (x, y) {
                    return x.Order - y.Order;
                });
                $.each(parameterList, function (index, parameter) {
                    if (!parameter.GroupName) {
                        if (!groups[defaultGroupName])
                            groups[defaultGroupName] = [];
                        groups[defaultGroupName].push(parameter);
                    }
                    else {
                        if (!groups[parameter.GroupName])
                            groups[parameter.GroupName] = [];
                        groups[parameter.GroupName].push(parameter);
                    }
                });
                for (var groupName in groups) {
                    var params = groups[groupName];
                    //
                    //вставляем сепараторы-подгруппы, которые нужны только для визуального разграничения
                    var subgroupName = '';
                    for (var i = 0; i < params.length; i++) {
                        var p = params[i];
                        if (p.SubgroupName != subgroupName) {
                            subgroupName = p.SubgroupName;
                            var semiParameter = self.CreateParameterSeparator(i, p.GroupName, p.SubgroupName);
                            var separator = new createParameter(semiParameter);
                            params.splice(i, 0, separator);//insert command
                            i++;
                        }
                    }
                    //
                    var parametersGroup = new createParametersGroup(groupName, params);
                    retval.push(parametersGroup);
                }
                //
                return retval;
            });
            //
            //безопасное очищение всех параметров
            self.ClearParameterList = function () {
                self.DestroyControls();
                //
                self.IsLoaded(false);
                self.ObjectID = null;
                self.ObjectClassID = null;
                self.ClientMode(true);
                self.ParameterList().splice(0, self.ParameterList().length);
            };
            //используется для формирования перечня параметров /(запрос на сервер)
            self.ajaxControl_load = new ajaxLib.control();
            //
            //загрузка существующих парметров и пользовательских полей 
            self.Initialize = function (objectClassID, objectID, objectModel) {
                self.ClearParameterList();
                //
                self.ObjectID = objectID;
                self.ObjectClassID = objectClassID;
                if (objectID == null) {
                    self.ParameterList.valueHasMutated();
                    self.IsLoaded(true);
                    return;
                }
                //
                var param = {
                    'objectClassID': objectClassID,
                    'objectID': objectID
                };
                self.ajaxControl_load.Ajax(null,
                    {
                        url: '/sdApi/getParameters?' + $.param(param),
                        method: 'GET',
                        dataType: "json"
                    },
                    function (response) {
                        if (response)
                            $.when(userD).done(function (user) {
                                $.each(response, function (index, parameterInfo) {
                                    if (!objectModel && parameterInfo.WebVisibility == false)//параметры, у которых стоит показывать в веб относится только к клиентам
                                        return;
                                    var p = new createParameter(parameterInfo);
                                    p.ReadOnly(self.ReadOnly() || p.ParameterValueIsReadOnly);// && user.HasAdminRole == false);
                                    self.ParameterList().push(p);
                                });
                                //
                                if (objectModel) {
                                    var userFields = self.CreateParamaterListForUserFields(objectModel);
                                    $.each(userFields, function (index, parameterInfo) {
                                        var p = new createParameter(parameterInfo);
                                        p.ReadOnly(self.ReadOnly() || p.ParameterValueIsReadOnly);// && user.HasAdminRole == false);
                                        self.ParameterList().push(p);
                                    });
                                }
                                //
                                self.ParameterList.valueHasMutated();
                                self.IsLoaded(true);
                            });
                    });
            };
            //загрузка существующих пользовательских полей (используется в AssetFields.js)
            self.InitializeUserFields = function (objectClassID, objectID, objectModel) {
                self.ClearParameterList();
                //
                self.ObjectID = objectID;
                self.ObjectClassID = objectClassID;
                if (objectID == null) {
                    self.ParameterList.valueHasMutated();
                    self.IsLoaded(true);
                    return;
                }
                //
                $.when(userD).done(function (user) {
                    if (objectModel) {
                        const userFields = self.CreateParamaterListForUserFields(objectModel);
                        $.each(userFields, function (index, parameterInfo) {
                            const parameter = new createParameter(parameterInfo);
                            parameter.ReadOnly(self.ReadOnly() || parameter.ParameterValueIsReadOnly);// && user.HasAdminRole == false);
                            self.ParameterList().push(parameter);
                        });
                    }
                    //
                    self.ParameterList.valueHasMutated();
                    self.IsLoaded(true);
                });
            };
            //загрузка существующих параметров или создание (с сохранением в БД) и пользовательских полей
            self.InitializeOrCreate = function (objectClassID, objectID, objectModel, recalculateParameters) {
                self.ClearParameterList();
                //
                self.ObjectID = objectID;
                self.ObjectClassID = objectClassID;
                //
                self.ClientMode(!objectModel);
                //
                if (objectID == null) {
                    self.ParameterList.valueHasMutated();
                    self.IsLoaded(true);
                    return;
                }
                //
                const param = {
                    'objectClassID': objectClassID,
                    'objectID': objectID,
                    'recalculateParameters': recalculateParameters
                };
                self.ajaxControl_load.Ajax(null,
                    {
                        url: '/sdApi/getOrCreateParameters?' + $.param(param),
                        method: 'GET',
                        dataType: "json"
                    },
                    function (response) {
                        if (response)
                            $.when(userD).done(function (user) {
                                $.each(response, function (index, parameterInfo) {
                                    if (!objectModel && parameterInfo.WebVisibility === false)//параметры, у которых стоит показывать в веб относится только к клиентам
                                        return;
                                    const p = new createParameter(parameterInfo);
                                    if (p.urlSetField && objectID) p.urlSetField += objectID;
                                    p.ReadOnly(self.ReadOnly() || p.ParameterValueIsReadOnly);// && user.HasAdminRole == false);
                                    self.ParameterList().push(p);
                                });
                                //
                                if (objectModel) {
                                    const userFields = self.CreateParamaterListForUserFields(objectModel);
                                    $.each(userFields, function (index, parameterInfo) {
                                        const p = new createParameter(parameterInfo);
                                        if (p.urlSetField && objectID) p.urlSetField += objectID;
                                        p.ReadOnly(self.ReadOnly() || p.ParameterValueIsReadOnly);// && user.HasAdminRole == false);
                                        self.ParameterList().push(p);
                                    });
                                }
                                //
                                self.ParameterList.valueHasMutated();
                                self.IsLoaded(true);
                            });
                    });
            };
            //создание нового списка параметров (по элементу/услуге)
            self.Create = function (objectClassID, templateObjectID, clientMode, userFieldType) {
                self.ClearParameterList();
                //
                self.ClientMode(!!clientMode);
                //
                if (templateObjectID == null) {
                    self.ParameterList.valueHasMutated();
                    //
                    if (userFieldType != null)
                        self.CreateParamaterListForUserFieldsForObject(userFieldType);
                    else
                        self.IsLoaded(true);
                    return;
                }
                //
                var param = {
                    'objectClassID': objectClassID,
                    'templateObjectID': templateObjectID
                };
                self.ajaxControl_load.Ajax(null,
                    {
                        url: '/sdApi/getDefaultParameters?' + $.param(param),
                        method: 'GET',
                        dataType: "json"
                    },
                    function (response) {
                        if (response)
                            $.when(userD).done(function (user) {
                                $.each(response, function (index, parameterInfo) {
                                    if (clientMode == true && parameterInfo.WebVisibility == false)//параметры, у которых стоит показывать в веб относится только к клиентам
                                        return;
                                    var p = new createParameter(parameterInfo);
                                    p.ReadOnly(self.ReadOnly() || p.ParameterValueIsReadOnly);// && user.HasAdminRole == false);
                                    self.ParameterList().push(p);
                                });
                                //
                                if (userFieldType != null)
                                    self.CreateParamaterListForUserFieldsForObject(userFieldType);
                                //
                                self.ParameterList.valueHasMutated();
                                if (userFieldType == null)
                                    self.IsLoaded(true);
                            });
                    });
            };
            //Получение копии параметров (при создании по аналогии)
            self.GetCopy = function (classID, objectID, clientMode, objectModel, userFieldType) {
                self.ClearParameterList();
                //
                self.ClientMode(clientMode ? true : false);
                //
                var param = {
                    'objectClassID': classID,
                    'objectID': objectID
                };
                self.ajaxControl_load.Ajax(null,
                    {
                        url: '/sdApi/getParametersCopy?' + $.param(param),
                        method: 'GET',
                        dataType: "json"
                    },
                    function (response) {
                        if (response) {
                            $.each(response, function (index, parameterInfo) {
                                if (clientMode == true && parameterInfo.WebVisibility == false)//параметры, у которых стоит показывать в веб относится только к клиентам
                                    return;
                                var p = new createParameter(parameterInfo);
                                p.ReadOnly(self.ReadOnly());
                                self.ParameterList().push(p);
                            });
                            //
                            self.ParameterList.valueHasMutated();
                            //
                            if (objectModel != null) {
                                var userFields = self.CreateParamaterListForUserFields(objectModel);
                                $.each(userFields, function (index, parameterInfo) {
                                    var p = new createParameter(parameterInfo);
                                    p.ReadOnly(self.ReadOnly());
                                    self.ParameterList().push(p);
                                });
                                self.ParameterList.valueHasMutated();
                                self.IsLoaded(true);
                            }
                            else if (userFieldType) {
                                self.CreateParamaterListForUserFieldsForObject(userFieldType);//set isLoaded inside
                                return;
                            }
                            else
                                self.IsLoaded(true);
                        }
                    });
            };
            //
            //эмулирует параметр - разделитель, метаинформация (подгруппы групп параметров)
            self.CreateParameterSeparator = function (index, groupName, subgroupName) {
                var p = {
                    ID: null,
                    IsValueRequired: false,
                    IsMultiple: false,
                    Name: subgroupName,
                    Identifier: null,
                    GroupName: groupName,
                    SubgroupName: '',
                    Order: index,
                    Type: -1,//separator
                    WebVisibility: true,
                    ParameterValueIsReadOnly: false,
                    Filter: null,
                    JSValueList: null
                };
                return p;
            };
            self.SetParametersForUserFieldRequest = function (callback, urlSetField) {
                self.callback = callback;
                self.urlSetField = urlSetField;
            };
            //
            //эмулирует параметр для пользовательских полей заявки / задания / проблемы, метаинформация
            self.CreateParameterForUserField = function (index, name, value) {
                var p = {
                    ID: '00000000-0000-0000-0000-00000000000' + index.toString(),
                    IsValueRequired: false,
                    Name: name,
                    Identifier: 'object.UserField' + index.toString(),
                    urlSetField: self.urlSetField,
                    callback: self.callback,
                    GetData: (function (newValue) {
                        var res = {};
                        res["UserField" + index.toString()] = JSON.parse(newValue)[0];
                        return JSON.stringify(res);
                    }),
                    method: 'put',
                    GroupName: getTextResource('UserFieldsGroupName'),
                    SubgroupName: '',
                    Order: 1000 + index,
                    Type: 2,//string       
                    WebVisibility: true,
                    Filter: {
                        MinLength: 0, MaxLength: 250
                    },
                    JSValueList: ko.toJSON([value]),
                    ParameterValueIsReadOnly: false
                };
                return p;
            };
            //создает список метапараметров ~ пользовательских полей - 5 шт
            self.CreateParamaterListForUserFields = function (objectModel) {
                let retval = [];
                const userFieldsObj = objectModel.UserFieldNamesDictionary || objectModel;
                if (userFieldsObj['UserField1Name'])
                    retval.push(self.CreateParameterForUserField(1, objectModel.UserField1Name, objectModel.UserField1()));
                if (userFieldsObj['UserField2Name'])
                    retval.push(self.CreateParameterForUserField(2, objectModel.UserField2Name, objectModel.UserField2()));
                if (userFieldsObj['UserField3Name'])
                    retval.push(self.CreateParameterForUserField(3, objectModel.UserField3Name, objectModel.UserField3()));
                if (userFieldsObj['UserField4Name'])
                    retval.push(self.CreateParameterForUserField(4, objectModel.UserField4Name, objectModel.UserField4()));
                if (userFieldsObj['UserField5Name'])
                    retval.push(self.CreateParameterForUserField(5, objectModel.UserField5Name, objectModel.UserField5()));
                return retval;
            };
            //создает список параметров ~ пользовательских полей - 5 шт
            self.CreateParamaterListForUserFieldsForObject = function (userFieldType) {
                var param = {
                    'userFieldType': userFieldType
                };
                self.ajaxControl_load.Ajax(null,
                    {
                        url: '/api/userfields?' + $.param(param),
                        method: 'GET',
                        dataType: "json"
                    },
                    function (userFields) {
                        var emptyFunc = function () {
                            return '';
                        }                        

                        var objectModel = {
                            UserField1Name: null,
                            UserField1: emptyFunc,
                            UserField2Name: null,
                            UserField2: emptyFunc,
                            UserField3Name: null,
                            UserField3: emptyFunc,
                            UserField4Name: null,
                            UserField4: emptyFunc,
                            UserField5Name: null,
                            UserField5: emptyFunc
                        };
                        for (var idx = 0; idx < userFields.length; idx++) {
                            objectModel['UserField' + userFields[idx].Number + 'Name'] = userFields[idx].Text;
                        }

                        //
                        $.when(userD).done(function (user) {
                            var userFields = self.CreateParamaterListForUserFields(objectModel);
                            $.each(userFields, function (index, parameterInfo) {
                                var p = new createParameter(parameterInfo);
                                p.ReadOnly(self.ReadOnly() || p.ParameterValueIsReadOnly);// && user.HasAdminRole == false);
                                self.ParameterList().push(p);
                            });
                            self.ParameterList.valueHasMutated();
                            self.IsLoaded(true);
                        });
                    });
            };
            //
            //проверка значений параметров
            self.Validate = function () {
                if (!self.IsLoaded()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ParametersNotLoaded'));
                    });
                    return false;
                }
                //
                var retval = true;
                var notSetParameterList = '';
                var errorParameterList = '';
                for (var i = 0; i < self.ParameterList().length; i++) {
                    var p = self.ParameterList()[i];
                    if (p.Type == 11 && p.IsBusyAnyEditor() == true) {//file uploading
                        if (errorParameterList.length != 0)
                            errorParameterList += ', ';
                        errorParameterList += p.Name + ' (' + (p.GroupName ? p.GroupName : getTextResource('ParametersDefaultGroupName')) + ') - ' + getTextResource('UploadedFileNotFoundAtServerSide');
                        retval = false;
                    }
                    else if (p.IsValueRequired && p.IsEmptyAllValues()) {//должно быть задано и пустое
                        if (p.IsValueExistsInAnyEditor() && p.ParameterValueIsReadOnly == false) {//возможные значения выбора есть и не только чтение
                            if (notSetParameterList.length != 0)
                                notSetParameterList += ', ';
                            notSetParameterList += p.Name + ' (' + (p.GroupName ? p.GroupName : getTextResource('ParametersDefaultGroupName')) + ')';
                            retval = false;
                        }
                    }
                    else if (p.Type == 13/*table*/ && p.IsValueRequired && p.HasValidationErrors())
                    {
                        notSetParameterList += p.Name + ' (' + (p.GroupName ? p.GroupName : getTextResource('ParametersDefaultGroupName')) + ')';
                        retval = false;
                    }
                    else if (p.InitialPackedValueList != p.GetPackedValueList()) {//значение параметра менялось пользователем
                        var error = p.GetValidationErrors();//invoke ko func
                        if (error != '') {
                            if (errorParameterList.length != 0)
                                errorParameterList += ', ';
                            errorParameterList += p.Name + ' (' + (p.GroupName ? p.GroupName : getTextResource('ParametersDefaultGroupName')) + ')';
                            if (p.Type == 13)//table
                                errorParameterList += ' [ ' + error + ' ]';
                            retval = false;
                        }
                    }
                }
                //
                if (!retval) {
                    var msg = '';
                    if (notSetParameterList.length > 0)
                        msg += getTextResource('ParametersMustBeSet') + ':\r\n' + notSetParameterList;
                    if (errorParameterList.length > 0) {
                        if (msg.length > 0)
                            msg += '\r\n';
                        msg += getTextResource('ParametersValueIncorrect') + ':\r\n' + errorParameterList;
                    }
                    if (msg.length > 0)
                        require(['sweetAlert'], function () {
                            swal(msg);
                        });
                }
                return retval;
            };
            self.DisableValidation = ko.observable(false);//выключить подсветку параметров
            //
            self.FillUserFields = function (obj) {
                self.ParameterList().forEach(param => {
                    const nameProp = `UserField${param.Order - 1000}`;
                    const value = param.GetValueList()[0];
                    if (!!value)
                        obj[nameProp] = value;
                });
            };
            //получение списка значений (при регистрации объектов)
            self.GetParameterValueList = function () {
                var retval = [];
                //
                var list = self.ParameterList();
                for (var i = 0; i < list.length; i++) {
                    var p = list[i];
                    if (p.IsLoadedAllEditors())
                        retval.push({
                            ID: p.ID,
                            ObjectID: p.ObjectID,
                            Identifier: p.Identifier,
                            JSValueList: p.GetPackedValueList(),
                            Widths: p.WidthOfColumnList ? p.WidthOfColumnList : null
                        });
                }
                //
                return retval;
            };
            //
            //очистка загруженных значений
            self.ClearValues = function () {
                for (var i = 0; i < self.ParameterList().length; i++) {
                    var p = self.ParameterList()[i];
                    p.ClearAllValues(false);
                }
            };
            //
            //только чтение
            self.ReadOnly = ko.observable(false);
            self.ReadOnly.subscribe(function (newValue) {
                $.when(userD).done(function (user) {
                    for (var i = 0; i < self.ParameterList().length; i++) {
                        var p = self.ParameterList()[i];
                        p.ReadOnly(self.ReadOnly() || p.ParameterValueIsReadOnly);// && user.HasAdminRole == false);
                    }
                });
            });
            //
            //принудительное уничтожение контролов параметров
            self.DestroyControls = function () {
                for (var i = 0; i < self.ParameterList().length; i++) {
                    var p = self.ParameterList()[i];
                    p.DestroyAllEditors(true);
                }
            };
        }
    }
    return module;
});