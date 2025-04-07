define(['knockout', 'jquery', 'ajax', 'imList'], function (ko, $, ajaxLib, imLib) {
	var module = {
		//сущность knockout, идентификатор класса сущности, селектор ajax-крутилки
		MassiveIncidentList: function (ko_object, objectClassID, ajaxSelector, readOnly_obj, isClient_obj) {
			var lself = this;
			//
			lself.baseApiUrl = '/api/MassIncidents';

			lself.isLoaded = ko.observable(false);//факт загруженности данных для объекта ko_object()
			lself.imList = null;//declared below
			lself.ajaxControl = new ajaxLib.control();
			lself.ajaxControl_add = new ajaxLib.control();
			lself.ajaxControl_remove = new ajaxLib.control();
			//
			lself.CheckData = function () {//функция загрузки списка (грузится только раз)
				if (!lself.isLoaded()) {
					$.when(lself.imList.Load()).done(function () {
						lself.isLoaded(true);
					});
				}
			};
			lself.ClearData = function () {//функция сброса данных
				lself.imList.List([]);
				//
				lself.isLoaded(false);
			};
		    //
			lself.PushData = function (list) {//функция загрузки списка 
			    var returnD = $.Deferred();
			    $.when(lself.imList.Push(list)).done(function () {
			        returnD.resolve();
			    });
			    return returnD.promise();
			};
            //
			lself.ReadOnly = readOnly_obj;//флаг только чтение, already observable
			lself.IsClient = isClient_obj;//флаг только чтение, already observable
			//
			lself.ItemsCount = ko.computed(function () {
				var retval = 0;
				if (lself.isLoaded())
					retval = lself.imList.List().length;
				//
				if (retval <= 0)
					return null;
				if (retval > 99)
					return '99';
				else return '' + retval;
			});
		    //
			lself.ShowObjectForm = function (massiveIncident) {
				const uri = `${lself.baseApiUrl}/${massiveIncident.ID}`;
			    require(['sdForms'], function (module) {
					var fh = new module.formHelper(true);
					fh.ShowMassIncident(uri, fh.Mode.Default);
				});
			};
			//

            var imListOptions = {};//параметры imList для списка 
			{
				imListOptions.aliasID = 'ID';
				//
				imListOptions.LoadAction = function () {
					
				};
			    //
				imListOptions.PushAction = function (list) {
				    if (list) {
				        var retvalD = $.Deferred();
				        require(['models/SDForms/SDForm.MassiveIncident'], function (massiveIncident) {
				            var retval = [];
				            ko.utils.arrayForEach(list, function (item) {
				                retval.push(new massiveIncident.MassiveIncident(lself.imList, item));
				            });
				            retvalD.resolve(retval);
				        });
				    }
				    return retvalD.promise();
				}
			}
			lself.imList = new imLib.IMList(imListOptions);
		}
	};
	return module;
});