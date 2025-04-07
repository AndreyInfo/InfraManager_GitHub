define(['jquery'], function ($) {
    //look at  InfraManager.Web.BLL.Helpers.ImageHelper
    //
    //server side icon calculations, because browser will get bad icon and will get bad request
    //
    //var WORKFLOW_ICON_PREFIX = 'ws_';
    //var CALLTYPE_ICON_PREFIX = 'callType_';
    //var WORKORDERTYPE_ICON_PREFIX = 'workOrderType_';
    //var PROBLEMTYPE_ICON_PREFIX = 'problemType_';
    //var CALLPRIORITY_ICON_PREFIX = 'priority_';
    //var WORKORDERPRIORITY_ICON_PREFIX = 'workOrderPriority_';
    //var PROBLEMPRIORITY_ICON_PREFIX = 'priority_';
    ////
    //var DEFAULT_PATH_ICONS = 'Data/Icons/';
    //var DEFAULT_ICON_EXTENSION = '.png';
    //
    var OVERDUE_ICON_NAME = 'UI/Controls/ListView/Icons/overdue.png';
    var FINISHED_ICON_NAME = 'UI/Controls/ListView/Icons/finished.png';
    var UNREADEDMESSAGE_ICON_NAME = 'UI/Controls/ListView/Icons/unrededMessage.png';
    var ATTACHMENT_ICON_NAME = 'UI/Controls/ListView/Icons/attachments.png';

    //function getCallTypeIconSource(callTypeID) {
    //    return DEFAULT_PATH_ICONS + CALLTYPE_ICON_PREFIX + callTypeID.toString() + DEFAULT_ICON_EXTENSION;
    //};

    //function getCallPriorityIconSource(callPriorityID) {
    //    return DEFAULT_PATH_ICONS + CALLPRIORITY_ICON_PREFIX + callPriorityID.toString() + DEFAULT_ICON_EXTENSION;
    //};

    //function getWorkOrderTypeIconSource(workOrderTypeID) {
    //    return DEFAULT_PATH_ICONS + WORKORDERTYPE_ICON_PREFIX + workOrderTypeID.toString() + DEFAULT_ICON_EXTENSION;
    //};

    //function getWorkOrderPriorityIconSource(workOrderPriorityID) {
    //    return DEFAULT_PATH_ICONS + WORKORDERPRIORITY_ICON_PREFIX + workOrderPriorityID.toString() + DEFAULT_ICON_EXTENSION;
    //};

    //function getProblemTypeIconSource(problemTypeID) {
    //    return DEFAULT_PATH_ICONS + PROBLEMTYPE_ICON_PREFIX + problemTypeID.toString() + DEFAULT_ICON_EXTENSION;
    //};

    //function getProblemPriorityIconSource(problemPriorityID) {
    //    return DEFAULT_PATH_ICONS + PROBLEMPRIORITY_ICON_PREFIX + problemPriorityID.toString() + DEFAULT_ICON_EXTENSION;
    //};

    //function removeUnspecifiedFileNameChars(value) {
    //    var invalidStr = '"<>|\r\b\t\n\v\f:*?\\/';
    //    for (var i = 0; i < invalidStr.length; i++)
    //        value = value.split(invalidStr[i]).join('X');
    //    //
    //    value = value.split('.').join('_');
    //    return value;
    //};
    //function getWorkflowIconSource(workflowSchemeIdentifier, workflowSchemeVersion, entityStateName) {
    //    if (!workflowSchemeIdentifier || !workflowSchemeVersion || !entityStateName)
    //        return null;
    //    //
    //    return DEFAULT_PATH_ICONS + WORKFLOW_ICON_PREFIX + removeUnspecifiedFileNameChars(workflowSchemeIdentifier) + "_" + removeUnspecifiedFileNameChars(workflowSchemeVersion) + "_" + removeUnspecifiedFileNameChars(entityStateName) + DEFAULT_ICON_EXTENSION;
    //};

    function getIconByClassID(classID) {
        var str = '' + classID;
        switch (str) {
            case '33': return 'ci-adapter-icon';
            case '16': return 'ci-cable-icon';
            case '165': return 'ci-dataEntity-icon';
            case '164': return 'ci-device_application-icon';
            case '349': return 'ci-logical_component-icon';
            case '120': return 'ci-material-icon';
            case '5': return 'ci-network-icon';
                //case '': return 'ci-network_logical-icon';
            case '8': return 'ci-outlet-icon';
            case '7': return 'ci-panel-icon';
            case '34': return 'ci-peripheral-icon';
            case '4': return 'ci-rack-icon';
            case '71': return 'ci-soft_installation-icon';
            case '223': return 'ci-soft_licence-icon';
            case '360': return 'ci-system-icon';
            case '6': return 'ci-terminal-icon';
                //case '': return 'ci-terminal_logical-icon';
            case '350': return 'ci-volume-icon';
            case '95': return 'model-adapter-icon';
            case '107': return 'model-material-icon';
            case '93': return 'model-network-icon';
                //case '': return 'model-outlet-icon';
                //case '': return 'model-panel-icon';
            case '96': return 'model-peripheral-icon';
                //case '': return 'model-rack-icon';
            case '97': return 'model-software-icon';
            case '94': return 'model-terminal-icon';
                //
            case '115': return 'ci-serviceContract-icon';
            case '386': return 'ci-network-icon';
            case '409': return 'ci-NodeKE-icon';
            case '410': return 'ci-SwitchКЕ-icon';
            case '411': return 'ci-RauterКЕ-icon';
            case '412': return 'ci-PrintServerКЕ-icon';
            case '413': return 'ci-DBaseKE-icon';
            case '414': return 'ci-WinSerserКЕ-icon';
            case '419': return 'ci-HostKE-icon';
                //
            default: console.log('unknown classID for icon: ' + classID); return '';
        };
    };
    //
    //
    return {
        getIconByClassID: getIconByClassID,
        //
        //getCallTypeIconSource: getCallTypeIconSource,
        //getCallPriorityIconSource: getCallPriorityIconSource,
        //getWorkOrderTypeIconSource: getWorkOrderTypeIconSource,
        //getWorkOrderPriorityIconSource: getWorkOrderPriorityIconSource,
        //getProblemTypeIconSource: getProblemTypeIconSource,
        //getProblemPriorityIconSource: getProblemPriorityIconSource,
        //getWorkflowIconSource: getWorkflowIconSource,
        //
        OVERDUE_ICON_NAME: OVERDUE_ICON_NAME,
        FINISHED_ICON_NAME: FINISHED_ICON_NAME,
        UNREADEDMESSAGE_ICON_NAME: UNREADEDMESSAGE_ICON_NAME,
        ATTACHMENT_ICON_NAME: ATTACHMENT_ICON_NAME,
    };
});