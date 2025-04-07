define(['jquery', 'knockout'], function ($, ko) {
    return {
        init: function () {
            if (window.location.href.toLowerCase().indexOf('authenticate') != -1)
                return;

            var linkStart = document.baseURI;
            if (typeof linkStart == 'undefined') {
                linkStart = window.location.href;
            }

            var timer = null;
            timer = setInterval(function () {

                $(".ui-dialog-title").each(function () {
                    if (this.hidden == "true") return;
                    var childrenExist = $(this).children('.entity-link');
                    if (childrenExist.length > 0)
                        return;
                    var currentText = this.innerText.replace(/^\s*/,'');
                    var block = "";
                    var type = "";

                    if (currentText.match(/^(Call №|Заявка №)(\d+)( )/)) {
                        currentText = currentText.replace(/^(Call №|Заявка №)(\d+)( )/, '$2 ');
                        var data = currentText.split(' ');
                        var Number = data[0];
                        type = "call";
                        block = GenerateBlock(linkStart, type, Number);
                    }
                    else if (currentText.match(/^(Task №|Задание №)(\d+)( )/)) {
                        currentText = currentText.replace(/^(Task №|Задание №)(\d+)( )/, '$2 ');
                        var data = currentText.split(' ');
                        var Number = data[0];
                        type = "task";
                        block = GenerateBlock(linkStart, type, Number);
                    }
                    else if (currentText.match(/^(Problem №|Проблема №)(\d+)( )/)) {
                        currentText = currentText.replace(/^(Problem №|Проблема №)(\d+)( )/, '$2 ');
                        var data = currentText.split(' ');
                        var Number = data[0];
                        type = "problem";
                        block = GenerateBlock(linkStart, type, Number);
                    }
                    else if (currentText.match(/^(RFC №|Запрос на изменения №)(\d+)( )/)) {
                        currentText = currentText.replace(/^(RFC №|Запрос на изменения №)(\d+)( )/, '$2 ');
                        var data = currentText.split(' ');
                        var Number = data[0];
                        type = "rfc";
                        block = GenerateBlock(linkStart, type, Number);
                    }
                    else if (currentText.match(/^(Mass Incident №|Массовый инцидент №)(\d+)/)) {
                        currentText = currentText.replace(/^(Mass Incident №|Массовый инцидент №)(\d+)/, '$2 ');
                        var data = currentText.split(' ');
                        var Number = data[0];
                        type = "massincident";
                        block = GenerateBlock(linkStart, type, Number);
                    }

                    if (block != "") {
                        $newBlock = $(block);
                        var stringSelector = "span[data-bind='text: \$captionText']";
                        var children = $(this).children(stringSelector);
                        if (children.length > 0) {
                            $(children[0]).after(block);
                        }
                        var childrenExist = $(this).children('.entity-link');
                        if (childrenExist.length > 0) {
                            document.querySelector('.entity-link').addEventListener('click', 
                            //$(childrenExist[0]).click(
                                function () {
                                var text = $(this).attr('href');
                                if (window.clipboardData && window.clipboardData.setData) {
                                    return clipboardData.setData('Text', text);
                                } else if (document.queryCommandSupported && document.queryCommandSupported("copy")) {
                                    var tmp = document.createElement("textarea");
                                    tmp.textContent = text;
                                    tmp.value = text;
                                    document.body.appendChild(tmp);

                                    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
                                        var range = document.createRange();
                                        range.selectNodeContents(tmp);
                                        window.getSelection().addRange(range);
                                    } else {
                                        tmp.setSelectionRange(0, text.length);
                                        tmp.focus();
                                    }
                                    
                                    document.execCommand('copy');
                                    document.body.removeChild(tmp);
                                }
                            });

                        }

                    }
                });
            }, 1000);
        }
    }
});



function GenerateBlock(linkStart, type, Number) {
    var block = "";

    if (type == 'call' || type == 'task' || type == 'problem' || type == 'rfc' || type == 'massincident') {
        var Prefix = "";
        var ParamName = "";
        var Hint = "";
        if (type == 'call') {
            ParamName = "callNumber";
            Hint = getTextResource('LinksGenerator_Hint_Call');
        }
        else if (type == 'task') {
            ParamName = "workorderNumber";
            Hint = getTextResource('LinksGenerator_Hint_Workorder');
        }
        else if (type == 'problem') {
            ParamName = "problemNumber";
            Hint = getTextResource('LinksGenerator_Hint_Problem');
        }
        else if (type == 'rfc') {
            ParamName = "rfcNumber";
            Hint = getTextResource('LinksGenerator_Hint_Rfc');
        }
        else if (type == 'massincident') {
            ParamName = "massIncidentNumber";
            Hint = getTextResource('LinksGenerator_Hint_MassIncident');
        }
        block = "<span title='" + Hint + "' class='entity-link' style='margin-left:10px; margin-right:15px; width: 16px !important; height: 16px; top:3px; background-image: url(" + linkStart + "scripts/plugins/insert_hiper1.svg); position:relative; display: inline-block; white-space: nowrap; cursor: pointer;' href='" + linkStart + "?" + ParamName + "=" + Prefix + Number + "'></span>";
    }
    return block;
}