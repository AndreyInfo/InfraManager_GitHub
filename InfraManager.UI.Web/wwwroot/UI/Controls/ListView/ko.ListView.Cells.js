define(['knockout'], function (ko) {
    var module = {
        cell: function (column, row) {
            var self = this;
            //
            self.text = null;
            self.imageSource = null;
            //
            self.column = column;
            self.row = row;
            self.value = ko.observable('');

            //
            return self;
        },
        cellCreator: function (listView, obj, column, row) {
            var cell = new module.cell(column, row);
            //
            var funcOfDrawing = listView.options.drawCell();
            funcOfDrawing(obj, column, cell);
            //
            return cell;
        },

        //warning, used in webWorker
        textRepresenter: function (obj, column) {
            if (obj && column) {
                var val = obj[column.MemberName];
                if (val === undefined || val === null)
                    return '';
                else if (val === true)
                    return getTextResource('True');
                else if (val === false)
                    return getTextResource('False');
                else if (column.MemberName.indexOf('Utc') === 0)
                    return parseDate(Date.parse(getUtcDate(val)));
                else if (val == 2147483647)
                    return '\u221E';
                //
                return val;
            }
            return '';
        }
    }
    return module;
});