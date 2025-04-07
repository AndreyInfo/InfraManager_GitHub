define([
    'knockout',
    'jquery',
    'ajax',
    'dateTimeControl',
    'usualForms',
    'formControl'
],
function (ko, $, ajax, dtLib, fhModule, fc) {
    const module = {
        ViewModel: function (formUiId, settings) {
            const self = this;

            // Типы контролов с бекенда
            self.TypesFields = {
                Boolean: 'Boolean', // "Булево значение"
                Position: 'Position', // "Выберите должность"
                User: 'User', // "Выбор пользователя",
                Subdivision: 'Subdivision', // "Выбор подразделения",
                DateTime: 'DateTime', // Дата
                Password: 'Password', // Пароль
                TextArea: 'TextArea', // Текстовая область
                String: 'String', // Текстовое поле
                Number: 'Number', // Число,
                EnumComboBox: 'EnumComboBox', // Селект
                EnumRadioButton: 'EnumRadioButton', // Радио баттон
                Service: 'Service', // Сервис
                Table: 'Table',
                Header: 'Header', // Заголовок
                Separator: 'Separator', // Разделитель
                Group: 'Group' // Разделитель
            };

            // Типы динамического поведения
            self.DynamicType = {
                Show: 0, // Сделать видимым
                Hide: 1, // Спрятать
                Readonly: 2, // Разрешить изменения
                NotReadonly: 3, // Запретить изменения
                Required: 4, // Разрешить изменения
                NotRequired: 5 // Запретить изменения
            };

            // Тип формы
            self.TypesForm = {
                Create: 'Create',
                Completed: 'Completed'
            };

            // Текущий тип формы
            self.CurrentType = settings.typeForm;

            // Форма по типу юзера
            self.TypesUserForm = {
                User: 'User',
                Admin: 'Admin'
            };

            // Данные по форме шаблона
            self.FormData = ko.observable({});

            // Все табы с элементами
            self.FormTabs = ko.observableArray();

            // Заполненные данные, есть только в Completed форме
            self.FormValuesCompleted = null;
            
            self.FormValuesMap = new Map();

            // Состояние отрисовки
            self.RenderedD = $.Deferred();

            //
            self.LoadTabsElementD = $.Deferred();

            // Id формы на UI
            self.FormId = formUiId;

            // Объект зависимостей для запросов
            self.Dependences = ko.observable(null);

            self.AjaxControl = new ajax.control();

            self.IsInit = ko.observable(false);

            self.IsReadOnly = settings.isReadOnly || ko.observable(false);

            self.IsClientMode = settings.isClientMode || false;

            // Работа с сервером
            // TODO: накидать обработчкик ошибок
            {
                // получение шаблона
                const getTemplate = function (ajaxSettings, formValues, dependencies) {
                    self.AjaxControl.Ajax(null, ajaxSettings,
                        function (response) {
                            self.FormValuesCompleted = formValues;
                            if(self.FormValuesCompleted){
                                for (let obj of self.FormValuesCompleted) {
                                    self.FormValuesMap.set(obj.ID, obj);
                                }                                
                            }
                            self.FormData(response.Form);
                            self.Dependences(dependencies);

                            response.Elements.forEach(function (tab) {
                                const seriavizeTab = self.SerializeTabByUi(tab);
                                self.FormTabs().push(seriavizeTab);
                            });

                            self.FormTabs.valueHasMutated();

                            if (self.FormTabs().length) {
                                self.IsInit(true);
                                self.LoadTabsElementD.resolve();
                            }
                        });
                }

                // получение шаблона по идентификатору шаблона
                self.GetTemplateByID = function (templateID, values, dependencies) {
                    getTemplate(
                        {
                            url: `/api/FullForms/${templateID}`,
                            method: 'GET',
                        },
                        values, dependencies);
                }

                // получение шаблона по идентификатору объекта
                self.GetTemplate = function (classID, objectID, values, dependencies) {
                    getTemplate(
                        {
                            url: '/api/FullForms',
                            method: 'GET',
                            data: {
                                ClassID: classID,
                                ObjectID: objectID,
                            }
                        },
                        values, dependencies);
                }
            }

            // Helpers
            {
                self.CreateObjectModelSet = function (name, id) {
                    const result = {
                        Name: ko.observable(name),
                        ID: ko.observable(id)
                    };
                    result.Name.extend({ notify: 'always' });
                    return result;
                };

                self.SerializeTabsByServer = function () {
                    const result = {
                        FormId: self.FormData().ID,
                        Values: []
                    };

                    const createItemRequest = function (id, type, values, isReadOnly) {
                        return {
                            ID: id,
                            Type: type,
                            Data: values,
                            IsReadOnly: isReadOnly,
                        }
                    };

                    const serializeValues = function (values) {
                        return values.map(function (item) {
                            if (item.Value() === null) {
                                return ''
                            };

                            if (typeof item.Value() === 'object') {
                                return item.Value().ID();
                            } else {
                                return item.Value();
                            };
                        });
                    };


                    self.FormTabs().forEach(function (formTab) {
                        formTab.TabElements.forEach(function (tabElement) {
                            switch (tabElement.Type) {
                                case self.TypesFields.Group:
                                    tabElement.Grouped.forEach(function (itemGroup) {
                                        const values = serializeValues(itemGroup.KoUseValues());
                                        const itemRequest = createItemRequest(itemGroup.ID, itemGroup.Type, values);
                                        result.Values.push(itemRequest);
                                    });

                                    break;
                                case self.TypesFields.Table:
                                    const rowList = tabElement.RowList().map(function (row) {
                                        return row.RowItem;
                                    });

                                    const itemRequestTable = {
                                        ID: tabElement.ID,
                                        Type: tabElement.Type,
                                        Rows: []
                                    };

                                    rowList.forEach(function (row, rowIndex) {
                                        itemRequestTable.Rows[rowIndex] = {
                                            RowNumber: rowIndex,
                                            Columns: []
                                        };

                                        row.forEach(function (column) {
                                            const field = column.ChildField;
                                            const values = serializeValues(field.KoUseValues());
                                            const isReadOnly = self.GetReadOnly(field);
                                            const itemRequest = createItemRequest(field.ID, field.Type, values, isReadOnly);
                                            itemRequestTable.Rows[rowIndex].Columns.push(itemRequest);
                                        });

                                    });

                                    result.Values.push(itemRequestTable);
                                    break;

                                default:
                                    let isReadOnly = self.GetReadOnly(tabElement);
                                    const valuesItem = serializeValues(tabElement.KoUseValues());
                                    const itemRequest = createItemRequest(tabElement.ID, tabElement.Type, valuesItem, isReadOnly);
                                    result.Values.push(itemRequest);
                                    break;
                            }                            
                        });
                    });

                    return result;
                };

                self.GetReadOnly = function (field){               
                    if(self.FormValuesCompleted) {
                        let formValue = self.FormValuesMap.get(field.ID);
                        if(formValue) return formValue.IsReadOnly;
                    }                    
                    return field.IsReadOnly;
                }

                self.SerializeTabsByServerPut = function () {
                    return {
                        FormValuesData: self.SerializeTabsByServer()
                    };
                };

                self.ClearFieldValue = function (field) {
                    field.KoUseValues().forEach(function (value) {
                        if (typeof value.Value() === 'object') {
                            value.Value().Name('');
                            value.Value().ID(null);
                        } else {
                            value.Value('');
                        };
                    });
                };

                /** Создание объекта с настройками значения конкретного поля
                    * @param {number} index - индекс поля.
                    * @param {string} initialValue - изначальное значение.
                    * @param {string} fieldId - id поля из базы.
                    * @param {string} fieldIdentifier - id поля для идентефикации.
                */
                self.CreateItemValue = function (index, initialValue, fieldId, fieldIdentifier) {
                    const valueId = `${fieldId}-${index}`;

                    return {
                        Value: ko.observable(''),
                        InitialValue: initialValue,
                        ID: ko.observable(valueId),
                        Index: index
                    };
                };

                /** Установка значений по умолчанию для объекта с настройками
                    * @param {object} field - поле.
                */
                self.SetItemValueDefault = function (field) {
                    const itemValue = self.CreateItemValue(0, field.SpecialFields.Value, field.ID, field.Identifier);

                    // TODO: убрать условие для инпута
                    if (!field.SpecialFields.hasOwnProperty('ValueID') || field.Type === self.TypesFields.String) {
                        itemValue.Value(field.SpecialFields.Value);
                    } else {
                        itemValue.Value(self.CreateObjectModelSet(
                            field.SpecialFields.Value,
                            field.SpecialFields.ValueID
                        ));
                    };

                    field.KoUseValues.push(itemValue);
                };

                /** Установка заполненных значений для объекта с настройками
                    * @param {object} field - поле.
                    * @param {object} completedValue - объект заполненных данных.
                    * @param {number} index - индекс поля.
                */
                self.SetItemValueComplete = function (field, completedValue, index) {
                    const itemValue = self.CreateItemValue(index, completedValue.Value, field.ID, field.Identifier);

                    const isStringField = function () {
                        return field.Type === self.TypesFields.Boolean
                            || field.Type === self.TypesFields.DateTime
                            || field.Type === self.TypesFields.Number
                            || field.Type === self.TypesFields.Password
                            || field.Type === self.TypesFields.String
                            || field.Type === self.TypesFields.TextArea
                    };

                    if (isStringField()) {
                        itemValue.Value(completedValue.Value);
                    } else {
                        itemValue.Value(self.CreateObjectModelSet(
                            completedValue.Value,
                            completedValue.ValueID
                        ));
                    };

                    field.KoUseValues.push(itemValue);
                };

                self.CloneObject = function (object) {
                    return JSON.parse(JSON.stringify(object));
                };

                self.SerializeTabByUi = function (tab) {
                    const serializeFields = function (fieldList) {
                        fieldList = fieldList.sort((a, b) => a.Order - b.Order);
                        fieldList.forEach(function (field) {
                            field.IsPutByChange = ko.observable(true);

                            field.IsRender = ko.observable(false);

                            const setIsInvalidDefault = function (koValueObj, koValueComputed) {
                                koValueObj.IsInvalidSetCustom = ko.observable(false);

                                koValueObj.IsInvalid = ko.computed({
                                    read: function () {
                                        if (koValueObj.IsInvalidSetCustom()) {
                                            return koValueObj.IsInvalidSetCustom();
                                        };

                                        return field.KoUseDynamicRequired() && !koValueComputed() && field.KoUseDynamicShow();
                                    },
                                    write: function (value) {
                                        koValueObj.IsInvalidSetCustom(value)
                                    }
                                });
                            };

                            field.KoUseValues = ko.observableArray();

                            switch (self.CurrentType) {
                                case self.TypesForm.Create:
                                    self.SetItemValueDefault(field);
                                    field.KoUseValues.valueHasMutated();
                                    break;
                                case self.TypesForm.Completed:
                                    // Если нет готовых value заполняем шаблоном по умолчанию
                                    if (!self.FormValuesCompleted) {
                                        self.SetItemValueDefault(field);
                                        field.KoUseValues.valueHasMutated();
                                        break;
                                    };

                                    let completedValueByField = null;

                                    self.FormValuesCompleted.forEach(function (formValue) {
                                        if (formValue.hasOwnProperty('Rows') && formValue.Rows) {
                                            formValue.Rows.forEach(function (row) {
                                                const current = row.Columns.find(function (column) {
                                                    return field.ID === column.ID && field.hasOwnProperty('RowNumber') && field.RowNumber() === row.RowNumber;
                                                });

                                                if (!current) {
                                                    return;
                                                };

                                                completedValueByField = current.Data;
                                            });


                                        } else {
                                            const isCurrent = field.ID === formValue.ID;
                                            if (isCurrent) {
                                                completedValueByField = formValue.Data;
                                            };
                                        }
                                    });

                                    if (completedValueByField) {
                                        completedValueByField.forEach(function (completedValue, index) {
                                            self.SetItemValueComplete(field, completedValue, index);
                                        });
                                    } else {
                                        self.SetItemValueDefault(field);
                                    };

                                    field.KoUseValues.valueHasMutated();
                                    break;
                                default:
                                    throw new Error(getTextResource('NotSelectTypeForm'));
                            };

                            const validateSetValue = function () {
                                const validate = self.Validate();
                                if (!validate.valid) {
                                    validate.callBack();
                                };

                                return validate.valid;
                            };

                            const putValues = function () {
                                const form = $(`#${self.FormId}`);
                                const textFileds = form.find('input, textarea');
                                const wrappersFields = form.find('[data-wrapper-value]');

                                textFileds.attr('disabled', true);
                                wrappersFields.addClass('blocking');

                                settings.updateCallBack(self.SerializeTabsByServerPut()).always(function () {
                                    textFileds.removeAttr('disabled');
                                    wrappersFields.removeClass('blocking');
                                });
                            };

                            field.KoUseSetValue = function (value, valueIndex) {
                                const currentValue = field.KoUseValues()[valueIndex];
                                if (typeof value === 'object') {
                                    currentValue.Value().Name(value.Name());
                                    currentValue.Value().ID(value.ID());
                                } else {
                                    currentValue.Value(value);
                                };

                                if (self.CurrentType === self.TypesForm.Completed && field.IsPutByChange()) {
                                    const isValid = validateSetValue();
                                    if (!isValid) {
                                        return;
                                    };

                                    putValues();
                                };
                            };

                            field.KoUseClearValue = function (valueIndex) {
                                const currentValue = field.KoUseValues()[valueIndex];

                                if (typeof currentValue === 'object') {
                                    currentValue.Value().Name('');
                                    currentValue.Value().ID(null);
                                } else {
                                    currentValue.Value('');
                                };

                                if (self.CurrentType === self.TypesForm.Completed && field.IsPutByChange()) {
                                    putValues();
                                };
                            };

                            field.KoUseRemoveValue = function (value) {
                                field.KoUseValues(field.KoUseValues().filter(function (useValue) {
                                    return useValue !== value;
                                }));

                                if (self.CurrentType === self.TypesForm.Completed && field.IsPutByChange()) {
                                    putValues();
                                };
                            };

                            field.KoUseClone = function (item, setValidate = null) {
                                const prefixId = item.KoUseValues().length;
                                const mainValue = item.KoUseValues()[0];
                                const nodeId = `${item.ID}-${prefixId}`;

                                const cloneItem = self.CreateItemValue(prefixId, '', item.ID, field.Identifier);

                                if (mainValue.Value() !== null && typeof mainValue.Value() === 'object') {
                                    cloneItem.Value(self.CreateObjectModelSet(field.SpecialFields.Value || '', field.SpecialFields.ValueID || null));
                                    typeof setValidate === 'function'
                                        ? setValidate.call(this, cloneItem)
                                        : setIsInvalidDefault(cloneItem, cloneItem.Value().Name);
                                } else {
                                    cloneItem.Value(field.SpecialFields.Value || '');
                                    typeof setValidate === 'function'
                                        ? setValidate.call(this, cloneItem) :
                                        setIsInvalidDefault(cloneItem, cloneItem.Value);
                                };

                                item.KoUseValues.push(cloneItem);

                                return {
                                    nodeId,
                                    cloneItem
                                };
                            };

                            /**
                              * Проверка на совпадения значений зависимого поля и значений с сервера для динамического поведения.
                              * @param {Object} parentField - Зависимое поле.
                              * @param {Object} valueD - Объект из бекенда.
                             */
                            const getAllCoincidence = function (parentField, valueD) {
                                // Если пришли неправильные настройки из админки, возвращаем значение, для работы по умолчанию
                                if (!parentField) {
                                    return [false];
                                };

                                return parentField.KoUseValues().map(function (koValue) {
                                    switch (valueD.OperationID) {
                                        // Равно
                                        case 0:
                                            if (typeof koValue.Value() === 'object') {
                                                return valueD.Constant == koValue.Value().ID();
                                            } else {
                                                return koValue.hasOwnProperty('CompareValue')
                                                    ? valueD.Constant == koValue.CompareValue()
                                                    : valueD.Constant == koValue.Value();
                                            };
                                            break;
                                        // Не равно
                                        case 1:
                                            if (typeof koValue.Value() === 'object') {
                                                return valueD.Constant != koValue.Value().ID();
                                            } else {
                                                return koValue.hasOwnProperty('CompareValue')
                                                    ? valueD.Constant != koValue.CompareValue()
                                                    : valueD.Constant != koValue.Value();
                                            };
                                            break;
                                        // Заполнено
                                        case 2:
                                            if (typeof koValue.Value() === 'object') {
                                                return !!koValue.Value().Name();
                                            } else {
                                                return !!koValue.Value();
                                            };
                                        // Не заполнено
                                        case 3:
                                            if (typeof koValue.Value() === 'object') {
                                                return !(!!koValue.Value().Name());
                                            } else {
                                                return !(!!koValue.Value());
                                            };
                                        default:
                                            return true;
                                            break;
                                    };
                                });
                            };

                            field.KoUseVisibleForUser = function () {
                                return self.IsClientMode ? field.SpecialFields.DisplayForClient : true;
                            };

                            field.KoUseDynamicShow = ko.computed(function () {
                                const defaultValue = !field.SpecialFields.HideByDefault && field.KoUseVisibleForUser();

                                if (!field.Options.length) {
                                    return defaultValue;
                                };

                                const showOptions = field.Options.filter(function (option) {
                                    return option.ActionID === self.DynamicType.Show || option.ActionID === self.DynamicType.Hide;
                                });

                                if (!showOptions.length) {
                                    return defaultValue;
                                };

                                const result = showOptions.map(function (valueD) {
                                    const parentField = fieldList.find(function (ctrl) {
                                        return ctrl.Identifier === valueD.ParentIdentifier;
                                    });

                                    const isAllCoincidence = getAllCoincidence(parentField, valueD);

                                    switch (valueD.ActionID) {
                                        // Сделать видимым
                                        case self.DynamicType.Show:
                                            return isAllCoincidence.includes(true)
                                            break;
                                        // Спрятать
                                        case self.DynamicType.Hide:
                                            return !isAllCoincidence.includes(true)
                                            break;
                                        default:
                                            return defaultValue;
                                            break;
                                    };
                                });

                                return field.IsRender() ? result.includes(true) && field.KoUseVisibleForUser() : defaultValue;
                            });

                            field.KoUseDynamicReadonly = ko.computed(function () {
                                if (self.IsReadOnly()) {
                                    return self.IsReadOnly();
                                }

                                if (!field.Options.length) {
                                    return self.GetReadOnly(field);
                                }

                                const readonlyOptions = field.Options.filter(function (option) {
                                    return option.ActionID === self.DynamicType.Readonly || option.ActionID === self.DynamicType.NotReadonly;
                                });

                                if (!readonlyOptions.length) {
                                    return self.GetReadOnly(field);
                                }

                                const result = readonlyOptions.map(function (valueD) {
                                    const parentField = fieldList.find(function (ctrl) {
                                        return ctrl.Identifier === valueD.ParentIdentifier;
                                    });
                                    const isAllCoincidence = getAllCoincidence(parentField, valueD);

                                    switch (valueD.ActionID) {
                                        case self.DynamicType.Readonly:
                                            return !isAllCoincidence.includes(true);
                                            break;
                                        case self.DynamicType.NotReadonly:
                                            return isAllCoincidence.includes(true);
                                            break;
                                        default:
                                            return field.SpecialFields.IsReadOnly;
                                            break;
                                    };
                                });

                                return field.IsRender() ? result.includes(true) : field.SpecialFields.IsReadOnly;
                            });

                            field.KoUseDynamicRequired = ko.computed(function () {
                                if (!field.Options.length) {
                                    return field.SpecialFields.IsRequired;
                                };

                                const requiredOptions = field.Options.filter(function (option) {
                                    return option.ActionID === self.DynamicType.Required || option.ActionID === self.DynamicType.NotRequired;
                                });

                                if (!requiredOptions.length) {
                                    return field.SpecialFields.IsRequired;
                                };

                                const result = requiredOptions.map(function (valueD) {
                                    const parentField = fieldList.find(function (ctrl) {
                                        return ctrl.Identifier === valueD.ParentIdentifier;
                                    });

                                    const isAllCoincidence = getAllCoincidence(parentField, valueD);

                                    switch (valueD.ActionID) {
                                        case self.DynamicType.Required:
                                            return isAllCoincidence.includes(true)
                                            break;
                                        case self.DynamicType.NotRequired:
                                            return !isAllCoincidence.includes(true)
                                            break;
                                        default:
                                            return field.SpecialFields.IsRequired;
                                            break;
                                    };
                                });

                                return field.IsRender() ? result.includes(true) : field.SpecialFields.IsRequired;
                            });

                            field.KoUseLoadItem = function (_nodes, _item) { }

                            /**
                             * Вычисление подсказки при наведении на заголовок, если есть настройка
                             */
                            field.ShowTooltipByHoverTitle = function () {
                                return field.SpecialFields.ShowHint ? field.SpecialFields.Hint : '';
                            };

                            field.TooltipDelay = [150, 150];

                            /**
                             * Подписка на изменения enums.
                             * @param {Object} field - Поле.
                             */
                            const setEditEnums = function (field) {
                                field.KoUseValues().forEach(function (koValue) {
                                    koValue.Value.subscribe(function (nVal) {
                                        const updateValue = self.CreateObjectModelSet(nVal.Name(), nVal.ID())
                                        field.KoUseSetValue(updateValue, koValue.Index);
                                    });
                                });
                            };

                            /**
                              * Создание кастомного объекта валидации.
                              * @param {string} message - Собщение в попапе.
                              * @param {Function} callBack - Фукцния, которая выполнится после показа.
                             */
                            const createInvalidCustomObject = function (message, callBack) {
                                return {
                                    message,
                                    callBack
                                };
                            };

                            /**
                             * Отключение добавления новых значений при превышения кол-ва из настройки 
                             */
                            const isDisabledMultipleValues = function () {
                                return field.KoUseValues().length >= field.SpecialFields.MaxCountAdditionalFields;
                            };

                            // TODO: Отрефакторить switch, вынести общую логику в отдельные функции
                            switch (field.Type) {
                                case self.TypesFields.User:
                                    field.KoUseSelectList = ko.observableArray([]);
                                    field.KoUseSelectData = function () {
                                        return {
                                            data: field.KoUseSelectList(),
                                            totalCount: field.KoUseSelectList().length
                                        };
                                    };

                                    field.KoUseLoadItem = function (_nodes, item) {
                                        if (!field.SpecialFields.UseSearch) {
                                            setEditEnums(field);
                                            self.InitializeEnumUsers(field);
                                        } else {
                                            item.KoUseValues().forEach(function (koValue) {
                                                self.InitializeUserSearcher(koValue, field);
                                            });
                                        };

                                        field.IsRender(true);
                                    };

                                    field.KoUseValues().forEach(function (koValue) {
                                        koValue.IsInvalid = ko.computed(function () {
                                            // Перезаписываем, т.к. плагин комбобокса не корректно работает с Name, из-за чего computed перестает работать
                                            return field.KoUseDynamicRequired() && !koValue.Value().Name() && field.KoUseDynamicShow();
                                        });
                                    });

                                    field.KoUseSetValueCustom = function (user, userIndex) {
                                        const userValue = self.CreateObjectModelSet(user.FullName, user.ID);
                                        field.KoUseSetValue(userValue, userIndex);
                                    };

                                    field.KoUseCloneElementUser = function (item) {
                                        if (!field.SpecialFields.UseSearch) {
                                            // Перезаписываем, т.к. плагин комбобокса не корректно работает с Name, из-за чего computed перестает работать
                                            const callBackClone = function (item) {
                                                item.IsInvalid = ko.computed(function () {
                                                    return field.KoUseDynamicRequired() && !item.Value().Name() && field.KoUseDynamicShow();
                                                });
                                            };

                                            field.KoUseClone(item, callBackClone);

                                            if (!field.SpecialFields.UseSearch) {
                                                setEditEnums(field);
                                            };
                                        } else {
                                            field.KoUseClone(item);
                                            const lastValue = field.KoUseValues()[field.KoUseValues().length - 1];
                                            self.InitializeUserSearcher(lastValue, item);
                                        };
                                    };

                                    field.IsDisabledMultipleValues = isDisabledMultipleValues;
                                    break;

                                case self.TypesFields.DateTime:
                                    field.KoUseErrorToolttip = ko.observable('');

                                    field.KoUseLoadItem = function (_nodes, item) {
                                        item.KoUseValues().forEach(function (koValue) {
                                            const setDate = field.KoUseValues()[koValue.Index].Value();
                                            const setting = {
                                                ValueDate: setDate ? new Date(setDate) : null,
                                                OnlyDate: !field.SpecialFields.IsIncludeTime,
                                                MinDate: field.SpecialFields.IsMoreThenNow ? new Date() : false,
                                                OnSelectDateFunc: field.KoUseSetValueCustom,
                                                OnClearMaskFunc: field.KoUseClearValue
                                            };

                                            self.InitializeDatePicker(koValue, setting);
                                        });

                                        field.IsRender(true);
                                    };

                                    const getToday = function () {
                                        let today = new Date();
                                        if (field.SpecialFields.IsIncludeTime) {
                                            today = dtLib.SerializeTimeRoundingByMinute(today.getTime());
                                        } else {
                                            today.setHours(0, 0, 0, 0);
                                        };

                                        return today;
                                    };

                                    const createMessageInvalid = function () {
                                        const categoryName = field.CategoryName;
                                        return `${getTextResource('ParametersValueIncorrect')} :\r\n ${categoryName} (${getTextResource('ParametersDefaultGroupName')})`;
                                    };

                                    const invalidHandler = function (koValue) {
                                        const isBaseInvalid = field.KoUseDynamicRequired() && !koValue.Value() && field.KoUseDynamicShow();

                                        if (isBaseInvalid) {
                                            return isBaseInvalid;
                                        };

                                        if (self.CurrentType === self.TypesForm.Completed) {
                                            return false;
                                        };

                                        if (field.SpecialFields.IsMoreThenNow) {
                                            const isDateInvalid = new Date(koValue.Value()).getTime() < getToday().getTime();  

                                            if (isDateInvalid) {
                                                koValue.InvalidCustom = createInvalidCustomObject(createMessageInvalid());
                                                field.KoUseErrorToolttip(getTextResource('ParameterDateTime_DateMustBeMoreThanNow'));
                                            } else {
                                                koValue.InvalidCustom = null;
                                            };

                                            return isDateInvalid;
                                        };

                                        return isBaseInvalid;
                                    };

                                    field.KoUseValues().forEach(function (koValue) {
                                        const date = new Date(koValue.Value()).toJSON();
                                        koValue.Value(date || '');

                                        koValue.IsInvalid = ko.computed(function () {
                                            return invalidHandler(koValue);
                                        });
                                    });

                                    field.KoUseSetValueCustom = function (selectTime, $input, isNotValidValue) {
                                        field.KoUseErrorToolttip('');
                                        const date = new Date(selectTime).toJSON();

                                        const index = parseInt($input.closest('[data-custom-index]').attr('data-custom-index'));
                                        const currentValue = field.KoUseValues()[index];

                                        const parent = $input.closest('.dynamic-options__wrapper-item');
                                        const showError = function () {
                                            parent.addClass('date-picker_error');
                                        };

                                        const setInvalid = function () {
                                            currentValue.InvalidCustom = createInvalidCustomObject(createMessageInvalid(), showError);
                                            currentValue.InvalidCustom.callBack();
                                        };

                                        // обработка ручного ввода, если выбранна опция 'Не раньше, чем сейчас'
                                        if (field.SpecialFields.IsMoreThenNow && !isNotValidValue) {
                                            if (selectTime < getToday()) {
                                                field.KoUseErrorToolttip(getTextResource('ParameterDateTime_DateMustBeMoreThanNow'));
                                                showError();
                                                setInvalid();
                                                return;
                                            };
                                        };

                                        if (isNotValidValue) {
                                            setInvalid();
                                            return;
                                        };

                                        currentValue.InvalidCustom = null;
                                        parent.removeClass('date-picker_error');

                                        field.KoUseSetValue(date, index);
                                    };

                                    field.KoUseClearValue = function (_selectTime, $input, _isNotValidValue) {
                                        field.KoUseErrorToolttip('');

                                        const index = parseInt($input.closest('[data-custom-index]').attr('data-custom-index'));
                                        const currentValue = field.KoUseValues()[index];

                                        if (currentValue.InvalidCustom) {
                                            currentValue.InvalidCustom = null;
                                        };

                                        currentValue.Value('');

                                        if (!field.KoUseDynamicRequired()) {
                                            const parent = $input.closest('.dynamic-options__wrapper-item');
                                            parent.removeClass('date-picker_error');
                                        };
                                    };

                                    field.KoUseCloneElement = function (item) {
                                        const callBackClone = function (item) {
                                            item.IsInvalid = ko.computed(function () {
                                                return invalidHandler(item);
                                            });
                                        };

                                        field.KoUseClone(item, callBackClone);

                                        const value = item.KoUseValues()[item.KoUseValues().length - 1];
                                        const setting = {
                                            ValueDate: value.Value() ? new Date(value.Value()) : null,
                                            OnlyDate: !field.SpecialFields.IsIncludeTime,
                                            MinDate: field.SpecialFields.IsMoreThenNow ? new Date() : false,
                                            OnSelectDateFunc: field.KoUseSetValueCustom,
                                            OnClearMaskFunc: field.KoUseClearValue
                                        };

                                        self.InitializeDatePicker(value, setting);
                                    };

                                    field.IsDisabledMultipleValues = isDisabledMultipleValues;
                                    break;

                                case self.TypesFields.EnumRadioButton:
                                case self.TypesFields.EnumComboBox:
                                case self.TypesFields.Position:
                                case self.TypesFields.Subdivision:
                                case self.TypesFields.Service:
                                    field.KoUseSelectList = ko.observableArray([]);
                                    field.KoUseSelectData = function () {
                                        return {
                                            data: field.KoUseSelectList(),
                                            totalCount: field.KoUseSelectList().length
                                        };
                                    };

                                    field.KoUseLoadItem = function (_nodes, item) {
                                        setEditEnums(field);

                                        switch (field.Type) {
                                            case self.TypesFields.Position:
                                                self.InitializeSelectItem(field, '/api/JobTitles', 'IMObjID', function () {
                                                    self.SetSelectValues(item, field);
                                                });
                                                break;
                                            case self.TypesFields.Subdivision:
                                                const baseUrl = '/api/subdivisions';
                                                let url = `${baseUrl}?Ascending=true&OrderByProperty=Name`;
 
                                                if (field.SpecialFields.SubdivisionID) {
                                                    url = `${baseUrl}/${field.SpecialFields.SubdivisionID}/childer`;
                                                };

                                                if (field.SpecialFields.OrganizationID) {
                                                    url = `${url}&OrganizationID=${field.SpecialFields.OrganizationID}`;
                                                };

                                                if (self.IsBindingEmptyByUser(field)) {
                                                    self.SetSelectValues(item, field);
                                                    return;
                                                };

                                                const dependencesUser = self.GetDependencesUser();

                                                if (dependencesUser) {
                                                    switch (field.SpecialFields.AllowedValueID) {
                                                        // Передаем подразделение пользователя
                                                        case 1:
                                                            url = `${baseUrl}/${dependencesUser.SubdivisionID}/childer`;
                                                            break;
                                                        // Передаем организацию пользователя
                                                        case 2:
                                                            url = `${url}&OrganizationID=${dependencesUser.OrganizationID}`;
                                                            break;
                                                        default:
                                                    };
                                                };

                                                self.InitializeSelectItem(field, url, 'ID', function () {
                                                    self.SetSelectValues(item, field);
                                                });
                                                break;
                                            case self.TypesFields.Service:
                                                self.InitializeSelectItem(field, '/api/service/list', 'ID', function () {
                                                    self.SetSelectValues(item, field);
                                                });
                                                break;
                                            default:
                                                if (!field.SpecialFields.UseSearch) {
                                                    self.InitializeEnumValues(field);
                                                } else {
                                                    self.InitializeSelectItem(field, `/api/ParameterEnum/parameterEnumValues?parameterEnumId=${field.SpecialFields.Source}`, 'IMObjID', function () {
                                                        item.KoUseValues().forEach(function (koValue) {
                                                            self.InitializeSearcher(koValue, field);
                                                        });
                                                    });
                                                };
                                                break;
                                        };

                                        field.IsRender(true);
                                    };

                                    field.KoUseValues().forEach(function (koValue) {
                                        koValue.IsInvalid = ko.computed(function () {
                                            // Перезаписываем, т.к. плагин комбобокса не корректно работает с Name, из-за чего computed перестает работать
                                            return field.KoUseDynamicRequired() && !koValue.Value().Name() && field.KoUseDynamicShow();
                                        });
                                    });

                                    field.KoUseSetValueCustom = function (object, index) {
                                        const value = self.CreateObjectModelSet(object.FullName, object.ID);
                                        field.KoUseSetValue(value, index);
                                    };

                                    field.KoUseCloneElementEnum = function (item) {
                                        if (!field.SpecialFields.UseSearch) {
                                            // Перезаписываем, т.к. плагин комбобокса не корректно работает с Name, из-за чего computed перестает работать
                                            const callBackClone = function (item) {
                                                item.IsInvalid = ko.computed(function () {
                                                    return field.KoUseDynamicRequired() && !item.Value().Name() && field.KoUseDynamicShow();
                                                });
                                            };

                                            field.KoUseClone(item, callBackClone);

                                            if (!field.SpecialFields.UseSearch) {
                                                setEditEnums(field);
                                            };
                                        } else {
                                            field.KoUseClone(item);
                                            const lastValue = field.KoUseValues()[field.KoUseValues().length - 1];
                                            self.InitializeSearcher(lastValue, field);
                                        };
                                    };

                                    if (field.Type === self.TypesFields.EnumRadioButton) {
                                        field.KoUseChecked = function (indexValue, item) {
                                            const currentValue = field.KoUseValues()[indexValue];
                                            currentValue.Value().Name(item.Name());
                                            currentValue.Value().ID(item.ID());

                                            if (self.CurrentType === self.TypesForm.Completed && field.IsPutByChange()) {
                                                settings.updateCallBack(self.SerializeTabsByServerPut());
                                            };
                                        };
                                    };

                                    field.IsDisabledMultipleValues = isDisabledMultipleValues;
                                    break;

                                case self.TypesFields.Boolean:
                                    field.KoUseLoadItem = function (_nodes, _item) {
                                        field.IsRender(true);
                                    };

                                    field.KoUseValues().forEach(function (koValue) {
                                        // Нужно хранить исходное значение для нормальной сверки
                                        koValue.CompareValue = ko.observable('');
                                        koValue.CompareValue(koValue.Value());

                                        // Приобразовываем к числу, т.к. приходит строка "0" / "1"
                                        koValue.Value(Number(koValue.Value()));

                                        koValue.IsInvalid = function () { }
                                    });

                                    field.KoUseChecked = function (item) {
                                        const currentValue = field.KoUseValues()[item.Index];
                                        currentValue.Value(Number(item.Value()));

                                        // Нужно хранить исходное значение для нормальной сверки
                                        currentValue.CompareValue(Number(item.Value()).toString());

                                        if (self.CurrentType === self.TypesForm.Completed && field.IsPutByChange()) {
                                            settings.updateCallBack(self.SerializeTabsByServerPut());
                                        };
                                    };
                                    break;

                                case self.TypesFields.Password:
                                case self.TypesFields.TextArea:
                                case self.TypesFields.String:
                                    field.KoUseLoadItem = function (_nodes, _item) {
                                        field.IsRender(true);
                                    };

                                    field.KoUseValues().forEach(function (koValue) {
                                        setIsInvalidDefault(koValue, koValue.Value);
                                    });

                                    field.KoUseEditText = function (item, event) {
                                        const $target = $(event.target);
                                        if ($target.hasClass('ui-input_error')) {
                                            return;
                                        };

                                        const minValue = field.SpecialFields.MinValue;
                                        if (item.Value().length < minValue) {
                                            const categoryName = field.CategoryName;

                                            require(['sweetAlert'], function () {
                                                swal(`${getTextResource('ForField')} ${categoryName} ${getTextResource('MinLengthChar')} ${minValue}`);
                                            });

                                            item.IsInvalid(true);
                                            return;
                                        } else {
                                            item.IsInvalid(false);
                                        };

                                        if (self.CurrentType === self.TypesForm.Completed) {
                                            const initialValue = $target.attr('data-initial-value');
                                            if (initialValue === item.Value()) {
                                                return;
                                            };

                                            field.KoUseSetValue(item.Value(), item.Index);
                                        };
                                    };

                                    field.RemoveError = function (_item, event) {
                                        const $input = $(event.target);
                                        if ($input.val()) {
                                            $input.removeClass('ui-input_error');
                                        };
                                    };

                                    if (field.Type === self.TypesFields.Password) {
                                        field.KoUseTogglePassword = function (_item, event) {
                                            const $target = $(event.target);
                                            const input = $target.prev();
                                            const currentType = input.attr('type');
                                            const setType = currentType === 'password' ? 'text' : 'password';

                                            input.attr('type', setType);
                                        };
                                    };

                                    field.IsDisabledMultipleValues = isDisabledMultipleValues;
                                    break;

                                case self.TypesFields.Number:
                                    field.KoUseLoadItem = function (_nodes, item) {
                                        field.IsRender(true);

                                        item.KoUseValues().forEach((Value) => {
                                            const input = $(`#${Value.ID()}`);
                                            input.on('mouseup', function (e) {
                                                e.stopPropagation();
                                            });
                                        });
                                    };

                                    field.KoUseCloneNumber = function (item) {
                                        const clone = field.KoUseClone(item).cloneItem;
                                        clone.CompareValue = ko.observable('');
                                    };

                                    field.KoUseEditNumber = function (item, event) {
                                        let value = item.Value().replace(/[\,]/g, '.').replace(/[^\d\.]/g, '');

                                        if (value < field.SpecialFields.MinValue) {
                                            value = field.SpecialFields.MinValue;
                                        };

                                        if (value > field.SpecialFields.MaxValue) {
                                            value = field.SpecialFields.MaxValue;
                                        };

                                        const parseValue = parseFloat(value);
                                        if (Number.isNaN(parseValue)) {
                                            item.CompareValue('');
                                            item.Value('');
                                        } else {
                                            if (field.SpecialFields.IsInteger) {
                                                const floorValue = Math.floor(parseValue)
                                                item.CompareValue(floorValue);
                                                item.Value(floorValue.toString());
                                            } else {
                                                const roundedValue = Math.floor(parseValue * 100) / 100;
                                                item.CompareValue(roundedValue);
                                                item.Value(roundedValue.toFixed(2));
                                            };
                                        };

                                        if (self.CurrentType === self.TypesForm.Completed) {
                                            const $target = $(event.target);
                                            const initialValue = $target.attr('data-initial-value');
                                            if (initialValue === item.Value()) {
                                                return;
                                            };

                                            field.KoUseSetValue(item.Value(), item.Index);
                                        };
                                    };

                                    const isNotEmptyValue = function () {
                                        return field.SpecialFields.Value || field.SpecialFields.Value === 0;
                                    };

                                    field.KoUseValues().forEach(function (koValue) {
                                        setIsInvalidDefault(koValue, koValue.Value);

                                        let parseValue = parseFloat(koValue.Value());
                                        if (isNaN(parseValue)) {
                                            parseValue = 0;
                                        }

                                        if (!field.SpecialFields.IsInteger) {
                                            field.SpecialFields.Value = isNotEmptyValue()
                                                ? parseFloat(field.SpecialFields.Value).toFixed(2)
                                                : field.SpecialFields.Value;

                                            koValue.Value(parseValue.toFixed(2))
                                        } else {
                                            field.SpecialFields.Value = isNotEmptyValue()
                                                ? field.SpecialFields.Value.toString()
                                                : field.SpecialFields.Value;

                                            koValue.Value(parseValue.toString());
                                        };

                                        koValue.CompareValue = ko.observable('');
                                        koValue.CompareValue(parseValue);
                                    });

                                    field.KoUseTypeListener = function () {
                                        return self.CurrentType === self.TypesForm.Completed ? 'blur' : 'change';
                                    };

                                    field.IsDisabledMultipleValues = isDisabledMultipleValues;
                                    break;

                                case self.TypesFields.Table:
                                    const UI_SEPARATOR = '<br />';
                                    const URL_SETTINGS = `api/FullForms/${self.FormData().ID}/settings`;

                                    const setValue = function (field) {
                                        const formatValue = function (field, callBack) {
                                            return field.KoUseValues().map(function (useValue) {
                                                return callBack.call(null, useValue);
                                            }).filter(function (value) {
                                                return value !== '';
                                            }).join(UI_SEPARATOR).trim();
                                        };

                                        switch (field.Type) {
                                            case self.TypesFields.DateTime:
                                                return formatValue(field, function (useValue) {
                                                    if (useValue.Value() == '') {
                                                        return useValue.Value();
                                                    };

                                                    return dtLib.Date2String(new Date(useValue.Value()), !field.SpecialFields.IsIncludeTime);
                                                });
                                                break;

                                            case self.TypesFields.Boolean:
                                                const valueBoolean = field.KoUseValues()[0].Value();
                                                return `<input class="input-checkbox-wrapper__input" type="checkbox" readonly ${Boolean(valueBoolean) ? 'checked' : ''}/>`
                                                break;
                                            case self.TypesFields.Password:
                                                return formatValue(field, function (useValue) {
                                                    return useValue.Value().replace(/./gm, "*");
                                                });
                                                break;
                                            default:
                                                return formatValue(field, function (useValue) {
                                                    const value = useValue.Value();

                                                    if (typeof value === 'object') {
                                                        return value.Name();
                                                    } else {
                                                        return value;
                                                    };
                                                });
                                                break;
                                        }
                                    };

                                    field.HeaderRow = ko.observable({});
                                    field.RowList = ko.observableArray();

                                    field.KoUseDynamicRequired = ko.computed(function () {
                                        return !field.RowList().length && field.SpecialFields.IsRequired;
                                    });

                                    field.CreateRow = function () {
                                        const columnListLength = Object.keys(field.HeaderRow()).length;

                                        return field.Columns.sort(function (a, b) {
                                            return a.Order - b.Order;
                                        }).map(function (childField, iColumn) {
                                            const setChildField = self.CloneObject(childField.OriginObject);
                                            const rowIndex = field.RowList().length;

                                            setChildField.RowNumber = ko.observable(rowIndex);
                                            setChildField.IsTextLeft = ko.computed(function () {
                                                return setChildField.Type === self.TypesFields.Number || setChildField.Type === self.TypesFields.DateTime;
                                            });

                                            const cellValue = self.FindCellValue(childField.ColumnFieldID, rowIndex, childField.ID);
                                            if (cellValue && cellValue.IsReadOnly) {
                                                setChildField.SpecialFields.IsReadOnly = cellValue.IsReadOnly;
                                            }
                                       
                                            serializeFields.call(null, [setChildField]);

                                            return {
                                                Width: columnListLength ? field.HeaderRow().RowItem[iColumn].Width : ko.observable(150),
                                                Label: childField.SpecialFields.Label,
                                                ChildField: setChildField,
                                                Value: ko.observable(setValue(setChildField)),
                                                IsInvalid: ko.computed(function () {
                                                    return setChildField.KoUseValues().some(function (useValue) {
                                                        return useValue.IsInvalid() && field.KoUseDynamicShow();
                                                    });
                                                }),
                                                ShowResizeThumb: ko.observable(false)
                                            };
                                        });
                                    };

                                    field.AddRow = function () {
                                        field.RowList().push({
                                            Checked: ko.observable(false),
                                            RowItem: field.CreateRow()
                                        });

                                        field.RowList.valueHasMutated();
                                    };

                                    field.OnAddRow = function () {
                                        field.AddRow();

                                        if (self.CurrentType === self.TypesForm.Completed) {
                                            putValues();
                                        };
                                    };

                                    field.RemoveRow = function () {
                                        const checkedItems = field.RowList().filter(function (row) {
                                            if (row.Checked()) {
                                                return row;
                                            };
                                        });

                                        const newRowList = field.RowList().filter(function (row) {
                                            if (!checkedItems.includes(row)) {
                                                return row;
                                            };

                                            row.RowItem.forEach(function (column) {
                                                self.ClearFieldValue(column.ChildField);
                                            })
                                        });

                                        field.RowList(newRowList);

                                        if (self.CurrentType === self.TypesForm.Completed) {
                                            putValues();
                                        };
                                    };

                                    field.IsShowRemoveButton = ko.computed(function () {
                                        return field.RowList().some(function (row) {
                                            return row.Checked();
                                        });
                                    });

                                    field.ListByArray = function (objectList) {
                                        if (!objectList) {
                                            return;
                                        };

                                        return Object.values(objectList);
                                    };

                                    field.KoUseLoadItem = function (_nodes) {
                                        field.IsRender(true);

                                        const headerRow = {
                                            Checked: ko.computed({
                                                read: function () {
                                                    if (!field.RowList().length) {
                                                        return false;
                                                    };

                                                    return field.RowList().every(function (row) {
                                                        return row.Checked();
                                                    })
                                                },
                                                write: function (isChecked) {
                                                    field.RowList().forEach(function (row) {
                                                        row.Checked(isChecked);
                                                    });
                                                }
                                            }),
                                            RowItem: field.CreateRow()
                                        };

                                        field.HeaderRow(headerRow);

                                        if (self.CurrentType === self.TypesForm.Completed) {
                                            $.ajax(URL_SETTINGS).done(function (data) {
                                                const fieldSettings = data.FieldSettings;

                                                fieldSettings.forEach(function (settingItem) {
                                                    const currentColumn = field.HeaderRow().RowItem.find(function (column) {
                                                        return column.ChildField.ID === settingItem.FieldID;
                                                    });

                                                    if (!currentColumn) {
                                                        return;
                                                    };

                                                    currentColumn.Width(settingItem.Width);
                                                });

                                                field.HeaderRow.valueHasMutated();
                                            });
                                        } else {
                                            field.HeaderRow.valueHasMutated();
                                        };

                                        if (self.CurrentType === self.TypesForm.Completed) {
                                            const valuesTable = self.FormValuesCompleted.find(function (value) {
                                                return field.ID === value.ID;
                                            });

                                            if (!valuesTable || !valuesTable.Rows) {
                                                return;
                                            };
 
                                            const rowLength = valuesTable.Rows.length;

                                            for (let i = 0; i < rowLength; i++) {
                                                field.AddRow();
                                            };
                                        };
                                    };

                                    field.TotalWidth = ko.pureComputed(function () {
                                        let retval = 2;

                                        if (field.SpecialFields.Transposed) {
                                            field.RowList().forEach(function (row) {
                                                retval += row.RowItem[0].Width();
                                            });
                                        } else {
                                            if (field.ListByArray(field.HeaderRow()).length) {
                                                field.HeaderRow().RowItem.forEach(function (column) {
                                                    if (!column.ChildField.KoUseDynamicShow()) {
                                                        return;
                                                    };

                                                    retval += column.Width();
                                                });
                                            };
                                        }

                                        return retval;
                                    });

                                    field.EnableResizeThumb = function (column) {
                                        column.ShowResizeThumb(true);
                                    };

                                    field.DisableResizeThumb = function (column) {
                                        column.ShowResizeThumb(false);
                                    };

                                    field.MoveThumbData = ko.observable(null);

                                    field.MoveThumbData.subscribe(function (nValue) {
                                        if (nValue == null) {
                                            field.MoveThumbCancelTime = (new Date()).getTime();

                                            if (self.CurrentType === self.TypesForm.Completed) {
                                                const request = field.HeaderRow().RowItem.map(function (column) {
                                                    return {
                                                        FieldID: column.ChildField.ID,
                                                        Width: column.Width()
                                                    };
                                                });

                                                const requestData = {
                                                    FieldSettings: request
                                                };

                                                $.ajax(URL_SETTINGS, {
                                                    method: 'POST',
                                                    contentType: 'application/json',
                                                    data: JSON.stringify(requestData)
                                                });
                                            };
                                        };
                                    });

                                    field.MoveThumbCancelTime = (new Date()).getTime();

                                    field.CancelThumbResize = function () {
                                        if (!field.MoveThumbData()) {
                                            return;
                                        };

                                        const column = field.MoveThumbData().column;
                                        column.ShowResizeThumb(false);
                                        field.MoveThumbData(null);
                                    };

                                    field.ThumbResizeCatch = function (column, e) {
                                        if (e.button == 0) {
                                            field.MoveThumbData({ column: column, startX: e.screenX, startWidth: column.Width() });
                                            field.MoveThumbData().column.ShowResizeThumb(true);
                                        } else {
                                            field.CancelThumbResize();
                                        };
                                    };

                                    field.OnMouseMove = function (e) {
                                        if (!field.MoveThumbData()) {
                                            return;
                                        };

                                        const dx = e.screenX - field.MoveThumbData().startX;
                                        field.MoveThumbData().column.Width(Math.max(field.MoveThumbData().startWidth + dx, 50));
                                        field.MoveThumbData().column.ShowResizeThumb(true);
                                    };

                                    field.OnMouseUp = function (e) {
                                        field.CancelThumbResize();
                                    };
                                    
                                    $(document).bind('mousemove', field.OnMouseMove);
                                    $(document).bind('mouseup', field.OnMouseUp);

                                    field.EditValue = function (item) {
                                        const currentField = item.ChildField;
                                        currentField.IsPutByChange(false);

                                        let isSave = false;

                                        const buttonCancel = {
                                            text: getTextResource('ButtonCancel'),
                                            click: function (e) {
                                                ctrl.Close();
                                            }
                                        };

                                        const buttonSave = {
                                            text: getTextResource('ButtonSave'),
                                            click: function () {
                                                if (item.IsInvalid()) {
                                                    return;
                                                };

                                                isSave = true;

                                                ctrl.Close();

                                                item.Value(setValue(currentField));

                                                if (self.CurrentType === self.TypesForm.Completed) {
                                                    putValues();
                                                };
                                            }
                                        };

                                        const buttons = [buttonSave, buttonCancel];

                                        const ctrl = new fc.control(
                                            'fieldType',
                                            null,
                                            `${getTextResource('SDEditorCaption')} "${currentField.SpecialFields.Label}"`,
                                            true, true, true, 500, 500, buttons, null,
                                            `data-bind="template: {name: \'Shared/DynamicOptionsService/Templates/Items/${currentField.Type}\', afterRender: KoUseLoadItem }"`
                                        );

                                        ctrl.BeforeClose = function () {
                                            if (!isSave) {
                                                if (!item.Value()) {
                                                    self.ClearFieldValue(item.ChildField);
                                                } else {
                                                    const values = item.Value().split(UI_SEPARATOR)
                                                    values.forEach(function (value, i) {
                                                        const valueField = item.ChildField.KoUseValues()[i];
                                                        if (typeof valueField.Value() === 'object') {
                                                            valueField.Value().Name(value);
                                                        } else if (item.ChildField.Type === self.TypesFields.Boolean) {
                                                            const isChecked = value.includes('checked');
                                                            valueField.Value(isChecked);
                                                        } else {
                                                            valueField.Value(value);
                                                        };
                                                    });
                                                };

                                                self.RegionID = null;
                                            };
                                        };

                                        const ctrlD = ctrl.Show();

                                        $.when(ctrlD).done(function () {
                                            self.RegionID = ctrl.GetRegionID();
                                            const elem = $(`#${self.RegionID}`);
                                            elem.addClass('dynamic-options dynamic-options_form');
                                            const model = item.ChildField;
                                            ko.applyBindings(model, elem.get(0));
                                        });
                                    };

                                    break;

                                case self.TypesFields.Header:
                                case self.TypesFields.Separator:
                                case self.TypesFields.Group:
                                    break;
                                default:
                                    throw new Error(`${getTextResource('FieldWithTypeNotImplement')} ${field.Type}`);
                                    break;
                            }
                        });
                    }

                    serializeFields(tab.TabElements);

                    tab.TabElements.forEach(function (field, _i) {
                        if (field.Type === self.TypesFields.Table && field.Columns.length) {
                            field.Columns.forEach(function (column) {
                                column.OriginObject = self.CloneObject(column);
                            });

                            serializeFields(field.Columns);
                        };

                        if (field.Type === self.TypesFields.Group && field.Grouped.length) {
                            serializeFields(field.Grouped);
                        };
                    });

                    return tab;
                };

                self.GetTabElementsByType = function (type) {
                    const result = [];
                    self.FormTabs().forEach(function (tab) {
                        tab.TabElements.filter(function (tabElem) {
                            if (tabElem.Type !== type) return;
                            result.push(tabElem);
                        });
                    });

                    return result;
                };

                self.ResetData = function () {
                    self.FormData({});
                    self.FormTabs.removeAll();
                    self.FormValuesCompleted = null;
                    self.LoadTabsElementD = $.Deferred();
                    self.IsInit(false);
                };

                self.Validate = function () {
                    const state = {
                        valid: true,
                        callBack: null
                    };

                    // Валидируем на минимальную длинну полей
                    self.FormTabs().some(function (tab) {
                        return tab.TabElements.some(function (tabElement) {
                            const invalidValue = tabElement.KoUseValues().find(function (koValue) {
                                const isInvalidMinValue = function () {
                                    return tabElement.SpecialFields.hasOwnProperty('MinValue')
                                        && koValue.Value()
                                        && tabElement.Type !== self.TypesFields.Number
                                        && koValue.Value().length < tabElement.SpecialFields.MinValue
                                        && tabElement.KoUseDynamicShow();
                                };

                                if (isInvalidMinValue()) {
                                    return tabElement;
                                };

                                if (koValue.IsInvalidSetCustom) {
                                    koValue.IsInvalid(false);
                                };
                            });

                            if (invalidValue) {
                                const callBack = function () {
                                    const minValue = tabElement.SpecialFields.MinValue;
                                    const categoryName = tabElement.CategoryName;

                                    require(['sweetAlert'], function () {
                                        swal(`${getTextResource('ForField')} ${categoryName} ${getTextResource('MinLengthChar')} ${minValue}`);
                                    });

                                    const form = $(`#${self.FormId}`);
                                    const input = form.find(`[data-custom-id='${invalidValue.ID()}']`);

                                    invalidValue.IsInvalid(true);

                                    if (settings.changeTabCb) {
                                        settings.changeTabCb(input, tab);
                                    };
                                };

                                state.callBack = callBack;
                                state.valid = false;

                                return true;
                            }

                            return false;
                        });
                    });

                    // Валидируем на заполнение обязательных полей
                    self.FormTabs().some(function (tab) {
                        return tab.TabElements.some(function (tabElement) {
                            let invalidValue = null;

                            const isTable = tabElement.Type === self.TypesFields.Table;

                            if (isTable) {
                                tabElement.RowList().forEach(function (row) {
                                    row.RowItem.forEach(function (column) {
                                        column.ChildField.KoUseValues().find(function (koValue) {
                                            if (koValue.IsInvalid && koValue.IsInvalid()) {
                                                invalidValue = koValue;
                                            };
                                        });
                                    });
                                });
                            } else {
                                tabElement.KoUseValues().find(function (koValue) {
                                    if (koValue.IsInvalid && koValue.IsInvalid()) {
                                        invalidValue = koValue;
                                    };
                                });
                            };

                            if (invalidValue) {
                                const callBack = function () {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('FillRequiredFields'));
                                    });

                                    if (settings.changeTabCb) {
                                        const form = $(`#${self.FormId}`);
                                        const input = isTable
                                            ? form.find(`[data-custom-id='${tabElement.ID}']`)
                                            : form.find(`[data-custom-id='${invalidValue.ID()}']`);
                                        
                                        settings.changeTabCb(input, tab);
                                    };
                                };

                                state.callBack = callBack;
                                state.valid = false;

                                return true;
                            };

                            return false;
                        });
                    });

                    // Валидируем на кастомные валидаций
                    self.FormTabs().some(function (tab) {
                        return tab.TabElements.some(function (tabElement) {
                            const invalidValue = tabElement.KoUseValues().find(function (koValue) {
                                if (koValue.InvalidCustom) {
                                    return koValue;
                                };
                            });

                            if (invalidValue) {
                                const callBack = function () {
                                    require(['sweetAlert'], function () {
                                        swal(invalidValue.InvalidCustom.message);
                                    });

                                    if (settings.changeTabCb) {
                                        const form = $(`#${self.FormId}`);
                                        const input = form.find(`[data-custom-id='${invalidValue.ID()}']`);
                                        settings.changeTabCb(input, tab);

                                        if (invalidValue.InvalidCustom.callBack) {
                                            invalidValue.InvalidCustom.callBack();
                                        };
                                    };
                                };

                                state.callBack = callBack;
                                state.valid = false;

                                return true;
                            };

                            return false;
                        });
                    });

                    return state;
                };

                // Типы зависимовсти от юзера
                self.DependencesUserType = {
                    // Зависим от подразделение пользователя
                    SubdivisionID: 1,
                    // Зависим от организации пользователя
                    OrganizationID: 2
                };

                self.IsGetDependencesUserByOptions = function (field) {
                    return field.SpecialFields.hasOwnProperty('AllowedValueID') && field.SpecialFields.AllowedValueID;
                };

                self.GetDependencesUser = function () {
                    const dependences = self.Dependences();
                    const isUserDependences = dependences && dependences.hasOwnProperty('User');
                    return isUserDependences ? dependences.User : null;
                };

                self.IsBindingEmptyEnumItems = function (field, compareTypeValue, value) {
                    return field.SpecialFields.AllowedValueID == compareTypeValue && !value;
                };

                self.SetSelectValues = function (item, field) {
                    item.KoUseValues().forEach(function (koValue) {
                        if (!field.SpecialFields.UseSearch) {
                            return;
                        };

                        self.InitializeSearcher(koValue, field);
                    });
                };

                self.FindCellValue = function (tableID, rowNumber, columnID) {
                    if (!self.FormValuesCompleted) {
                        return undefined;
                    }
                    const table = self.FormValuesCompleted.find(function (el) { return el.ID === tableID });
                    const row = table && table.Rows && table.Rows.length
                        ? table.Rows.find(function (el) { return el.RowNumber === rowNumber; })
                        : undefined;
                    return row && row.Columns && row.Columns.length
                        ?  row.Columns.find(function (el) { return el.ID === columnID; })
                        : undefined;
                }
            }

            // Инициализация компонентов
            {
                // Датапикер
                self.InitializeDatePicker = function (koValue, setting) {
                    const node = $(`[data-custom-id=${koValue.ID()}]`);

                    if (!node.length) {
                        return;
                    };

                    const dateControl = new dtLib.control();
                    dateControl.init(node, setting);
                };

                const createParamsSearcher = function (field) {
                    const paramsSearcher = {
                        NoTOZ: true,
                        SubdivisionID: field.SpecialFields.SubdivisionID,
                        OrganizationID: null,
                    };

                    if (field.SpecialFields.OnlyMols) {
                        const OPERATIONS_ONLY_MOLS = [356]
                        paramsSearcher.OperationIds = OPERATIONS_ONLY_MOLS;
                    };

                    const dependencesUser = self.GetDependencesUser();

                    if (dependencesUser) {
                        switch (field.SpecialFields.AllowedValueID) {
                            case self.DependencesUserType.SubdivisionID:
                                paramsSearcher.SubdivisionID = dependencesUser.SubdivisionID;
                                break;
                            // Передаем организацию пользователя
                            case self.DependencesUserType.OrganizationID:
                                paramsSearcher.OrganizationID = dependencesUser.OrganizationID;
                                break;
                            default:
                        };
                    };

                    return paramsSearcher;
                };

                self.IsBindingEmptyByUser = function (field) {
                    const user = self.GetDependencesUser();

                    if (!user || !self.IsGetDependencesUserByOptions(field)) {
                        return false;
                    };

                    // если у текущего юзера нет id подразделения при выбранной опции или нет id организации при выбранной опции
                    return self.IsBindingEmptyEnumItems(field, self.DependencesUserType.SubdivisionID, user.SubdivisionID)
                        || self.IsBindingEmptyEnumItems(field, self.DependencesUserType.OrganizationID, user.OrganizationID);
                }

                // Поисковики
                self.InitializeUserSearcher = function (koValue, field) {
                    const id = self.RegionID || self.FormId;
                    const node = $(`#${id}`).find(`[data-custom-id=${koValue.ID()}]`);

                    if (!node.length) {
                        return;
                    };

                    if (self.IsBindingEmptyByUser(field)) {
                        self.InitializeSearcher(koValue, field);
                        return;
                    };

                    const fh = new fhModule.formHelper();
                    const paramsSearcher = createParamsSearcher(field);

                    const ctrlD = fh.SetTextSearcherToField(
                        node,
                        'WebUserSearcher',
                        'SearchControl/SearchTextFieldUserSearchControl',
                        paramsSearcher,
                        function (objectInfo) { //select
                            self.AjaxControl.Ajax(
                                null,
                                {
                                    dataType: "json",
                                    method: 'GET',
                                    url: `/api/users/${objectInfo.ID}` 
                                },
                                function (user) {
                                    field.KoUseSetValueCustom(user, koValue.Index);
                                });
                        },
                        function () { //reset
                            field.KoUseClearValue(koValue.Index);
                        },
                        null
                    );
                    //
                    return ctrlD;
                };

                self.InitializeSearcher = function (koValue, field) {
                    const id = self.RegionID || self.FormId;
                    const node = $(`#${id}`).find(`[data-custom-id=${koValue.ID()}]`);

                    if (!node.length) {
                        return;
                    };

                    const serializeSearchList = field.KoUseSelectList().map(function (item) {
                        return {
                            FullName: item.Name(),
                            ID: item.ID()
                        };
                    });

                    const fh = new fhModule.formHelper();

                    const ctrlD = fh.SetTextSearcherToField(
                        node,
                        null,
                        null,
                        {},
                        function (objectInfo) { //select
                            field.KoUseSetValueCustom(objectInfo, koValue.Index);
                        },
                        function () { //reset
                            field.KoUseClearValue(koValue.Index);
                        },
                        null,
                        null,
                        null,
                        serializeSearchList
                    );
                    //
                    return ctrlD;
                };

                // Динамические списки
                self.InitializeEnumUsers = function (field) {
                    const url = '/api/searchrequests/UserSearcherForFormParameter';
                    const paramsSearcher = createParamsSearcher(field);
                    const data = JSON.stringify({ Content: JSON.stringify(paramsSearcher) });

                    if (self.IsBindingEmptyByUser(field)) {
                        return;
                    };

                    const ajaxSettings = {
                        url: url,
                        method: 'POST',
                        data: data,
                        contentType: 'application/json',
                        complete: function (response) {
                            if (field.KoUseSelectList().length) {
                                field.KoUseSelectList.removeAll();
                            };

                            Array.from(response.responseJSON).forEach(function (simpleEnum) {
                                const comboBoxValue = self.CreateObjectModelSet(simpleEnum.FullName, simpleEnum.ID);
                                field.KoUseSelectList().push(comboBoxValue);
                            });

                            field.KoUseSelectList.valueHasMutated();
                        }
                    };

                    $.ajax(url, ajaxSettings);
                };

                self.InitializeEnumValues = function (field) {
                    if (field.KoUseSelectList().length) {
                        field.KoUseSelectList.removeAll();
                    };

                    Array.from(field.SpecialFields.List).forEach(function (fieldEnum) {
                        const comboBoxValue = self.CreateObjectModelSet(fieldEnum.Value, fieldEnum.ID);
                        field.KoUseSelectList().push(comboBoxValue);
                    });

                    field.KoUseValues.valueHasMutated();
                    field.KoUseSelectList.valueHasMutated();
                };

                self.InitializeSelectItem = function (field, url, fieldID, cbAfterCompleteRequest) {
                    $.ajax(url, {
                        complete: function (response) {
                            if (!response.responseJSON) {
                                return;
                            };

                            if (field.KoUseSelectList().length) {
                                field.KoUseSelectList.removeAll();
                            };

                            let responseArray = [];
                            const isResponseArray = Array.isArray(response.responseJSON);

                            if (!isResponseArray) {
                                responseArray.push(response.responseJSON);
                            } else {
                                responseArray = response.responseJSON;
                            };

                            responseArray.forEach(function (simpleEnum) {
                                let comboBoxValue = null;

                                const isHasDontStandartStructResponce = function () {
                                    return simpleEnum.hasOwnProperty('Parent');
                                };

                                if (isHasDontStandartStructResponce()) {
                                    comboBoxValue = self.CreateObjectModelSet(simpleEnum.Parent.Value, simpleEnum.Parent.ID);
                                } else {
                                    comboBoxValue = self.CreateObjectModelSet(simpleEnum.Name, simpleEnum[fieldID]);
                                };
                                
                                field.KoUseSelectList().push(comboBoxValue);
                            });

                            field.KoUseValues.valueHasMutated();
                            field.KoUseSelectList.valueHasMutated();

                            if (cbAfterCompleteRequest) {
                                cbAfterCompleteRequest();
                            };
                        }
                    });
                };
            }

            // Системное
            {
                self.AfterRender = function () {
                    self.RenderedD.resolve();
                };

                self.SendByServer = function () {
                    const validate = self.Validate();

                    if (!validate.valid) {
                        return validate;
                    };

                    if (!self.FormTabs().length) {
                        return null;
                    };

                    return self.SerializeTabsByServer();
                };

                self.Load = function () {
                    $.when(self.RenderedD).done(function () {
                        hideSpinner();
                    });
                };

                self.Load();
            }
        },
        
        Classes: {
            WorkOrderTemplate: 155,
            ServiceItem: 406,
            ServiceAttendance: 407,
            ProblemType: 708,
            ChangeRequestType: 710,
            MassIncidentType: 824,
        },
    };

    return module;
});