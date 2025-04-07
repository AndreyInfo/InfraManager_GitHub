define([], function () {
    var module = {
        //расширяет модель списка набором полезных функций
        init: function (targetModel, listView) {
            var self = targetModel;
            //
            self.getRowByID = function (objectID) {
                if (listView != null) {
                    var row = listView.rowViewModel.getRowByObjectID(objectID);
                    return row;
                }
                return null;
            };
            self.getRowByObject = function (objectID, objectClassID, options) {
                var list = listView.rowViewModel.sortedRowList();
                if (!list) {
                    return null;
                }

                for (var i = list.length - 1; i >= 0; i--) {
                    var row = list[i];
                    if (row.object[options.ObjectIDName] == objectID && row.object[options.ObjectClassIDName] == objectClassID)
                        return row;
                }

                return null;
            };
            self.removeRowByID = function (objectID) {
                var row = self.getRowByID(objectID);
                if (row != null && listView != null) {
                    listView.rowViewModel.disposeRow(row, true);
                    listView.calculateVisibleRows();
                    return true;
                }
                return false;
            };
            self.appendObjectList = function (objectList, unshiftMode) {
                if (listView != null && objectList != null) {
                    var retval = listView.rowViewModel.append(objectList, unshiftMode);
                    listView.calculateVisibleRows();
                    return retval;
                }
                return [];
            };
            //
            self.setRowAsLoaded = function (row) {
                if (row != null)
                    row.type(0);//mark row as default
            };
            self.setRowAsOutdated = function (row) {
                if (row != null)
                    row.type(1);//mark row as outdated    
            };
            self.setRowAsNewer = function (row) {
                if (row != null)
                    row.type(2);//mark row as newer
            };
        }
    }
    return module;
});