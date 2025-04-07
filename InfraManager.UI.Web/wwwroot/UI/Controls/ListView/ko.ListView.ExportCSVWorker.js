(function () {   
    self.onmessage = function (e) {
        var csvContent = '';
        //args of postMessage to worker
        var objectList = e.data.objectList;
        var columnList = e.data.columnList;
        //
        self.locale = e.data.locale;
        self.resourceArrayFromServer = e.data.resourceArrayFromServer;
        self.getTextResource = function (resourceName) {
            return resourceArrayFromServer[resourceName];
        };
        self.applicationVersion = e.data.applicationVersion;
        {//init typeHelper
            self.define = function (arrayOfRequire, func) {//skip require.js
                func();
            };
            self.importScripts('../../../scripts/utility/typeHelper.js?uniqueVector=im_' + self.applicationVersion);
        }
        {//init cells
            var m_cells = null;
            self.define = function (arrayOfRequire, func) {//skip require.js
                m_cells = func();
            };
            self.importScripts('./ko.ListView.Cells.js?uniqueVector=im_' + self.applicationVersion);
        }
        //
        {//headers
            columnList.forEach(function (column) {
                var val = column.Text;
                var cellText = val ? val.toString() : '';
                cellText = cellText.split(';').join(' ').split('\r').join('').split('\n').join(' ');
                csvContent += cellText + ';';
            });
            csvContent += '\r\n';
        }
        objectList.forEach(function (obj) {
            let tmp = '';
            columnList.forEach(function (column) {
                var val = m_cells.textRepresenter(obj, column);
                var cellText = val ? val.toString() : '';
                cellText = cellText.split(';').join(' ').split('\r').join('').split('\n').join(' ');
                tmp += cellText + ';';
            });
            csvContent += tmp + '\r\n';
        });
        postMessage(csvContent);
    };
})(this);