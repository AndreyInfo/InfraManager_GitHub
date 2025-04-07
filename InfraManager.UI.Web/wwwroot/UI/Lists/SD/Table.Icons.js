define(['iconHelper'], function (ih) {
    var module = {
        isTextShowingInColumn: function (obj, column, viewName) {
            if (viewName == 'CallForTable') {
                switch (column.MemberName) {
                    case "IsOverdue":
                    case "IsFinished":
                        return false;
                    case "UnreadMessageCount":
                        return obj.UnreadMessageCount > 0;
                    case "DocumentCount":
                        return obj.DocumentCount > 0;
                    case "ProblemCount":
                        return obj.ProblemCount > 0;
                    case "WorkOrderCount":
                        return obj.WorkOrderCount > 0;
                }
            }
            else if (viewName == 'ClientCallForTable') {
                switch (column.MemberName) {
                    case "UnreadMessageCount":
                        return obj.UnreadMessageCount > 0;
                    case "DocumentCount":
                        return obj.DocumentCount > 0;
                }
            }
            else if (viewName == 'WorkOrderForTable') {
                switch (column.MemberName) {
                    case "IsOverdue":
                    case "IsFinished":
                        return false;
                    case "UnreadMessageCount":
                        return obj.UnreadMessageCount > 0;
                    case "DocumentCount":
                        return obj.DocumentCount > 0;
                }
            }
            else if (viewName == 'ProblemForTable') {
                switch (column.MemberName) {
                    case "IsOverdue":
                    case "IsFinished":
                        return false;
                    case "UnreadMessageCount":
                        return obj.UnreadMessageCount > 0;
                    case "DocumentCount":
                        return obj.DocumentCount > 0;
                    case "CallCount":
                        return obj.CallCount > 0;
                    case "WorkOrderCount":
                        return obj.WorkOrderCount > 0;
                }
            }
            else if (viewName == 'CommonForTable' || viewName == 'CustomControlForTable' || viewName == 'NegotiationForTable') {
                switch (column.MemberName) {
                    case "IsOverdue":
                    case "IsFinished":
                        return false;
                    case "UnreadMessageCount":
                        return obj.UnreadMessageCount > 0;
                    case "DocumentCount":
                        return obj.DocumentCount > 0;
                }
            }
            //
            return true;
        },
        getImageSourceInColumn: function (obj, column, viewName) {
            if (viewName == 'CallForTable') {
                switch (column.MemberName) {
                    case 'TypeFullName':
                        return obj.TypeImage;
                    case 'PriorityName':
                        return obj.PriorityImage;
                    case 'IsOverdue':
                        return obj.IsOverdue === true ? ih.OVERDUE_ICON_NAME : null;
                    case 'IsFinished':
                        return obj.IsFinished === true ? ih.FINISHED_ICON_NAME : null;
                    case 'UnreadMessageCount':
                        return obj.UnreadMessageCount > 0 ? ih.UNREADEDMESSAGE_ICON_NAME : null;
                    case 'DocumentCount':
                        return obj.DocumentCount > 0 ? ih.ATTACHMENT_ICON_NAME : null;
                    case 'EntityStateName':
                        return obj.WorkflowImage;
                }
            }
            else if (viewName == 'ClientCallForTable') {
                switch (column.MemberName) {
                    case 'TypeFullName':
                        return obj.TypeImage;
                    case 'UnreadMessageCount':
                        return obj.UnreadMessageCount > 0 ? ih.UNREADEDMESSAGE_ICON_NAME : null;
                    case 'DocumentCount':
                        return obj.DocumentCount > 0 ? ih.ATTACHMENT_ICON_NAME : null;
                    case 'EntityStateName':
                        return obj.WorkflowImage;
                }
            }
            else if (viewName == 'WorkOrderForTable') {
                switch (column.MemberName) {
                    case 'TypeName':
                        return obj.TypeImage;
                    case 'PriorityName':
                        return obj.PriorityImage;
                    case 'IsOverdue':
                        return obj.IsOverdue === true ? ih.OVERDUE_ICON_NAME : null;
                    case 'IsFinished':
                        return obj.IsFinished === true ? ih.FINISHED_ICON_NAME : null;
                    case 'UnreadMessageCount':
                        return obj.UnreadMessageCount > 0 ? ih.UNREADEDMESSAGE_ICON_NAME : null;
                    case 'DocumentCount':
                        return obj.DocumentCount > 0 ? ih.ATTACHMENT_ICON_NAME : null;
                    case 'EntityStateName':
                        return obj.WorkflowImage;
                }
            }
            else if (viewName == 'ProblemForTable') {
                switch (column.MemberName) {
                    case 'TypeFullName':
                        return obj.TypeImage;
                    case 'PriorityName':
                        return obj.PriorityImage;
                    case 'IsOverdue':
                        return obj.IsOverdue === true ? ih.OVERDUE_ICON_NAME : null;
                    case 'IsFinished':
                        return obj.IsFinished === true ? ih.FINISHED_ICON_NAME : null;
                    case 'UnreadMessageCount':
                        return obj.UnreadMessageCount > 0 ? ih.UNREADEDMESSAGE_ICON_NAME : null;
                    case 'DocumentCount':
                        return obj.DocumentCount > 0 ? ih.ATTACHMENT_ICON_NAME : null;
                    case 'EntityStateName':
                        return obj.WorkflowImage;
                }
            }
            else if (viewName == 'CommonForTable' || viewName == 'CustomControlForTable' || viewName == 'NegotiationForTable') {
                switch (column.MemberName) {
                    case 'TypeFullName':
                        return obj.TypeImage;
                    case 'PriorityName':
                        return obj.PriorityImage;
                    case 'IsOverdue':
                        return obj.IsOverdue === true ? ih.OVERDUE_ICON_NAME : null;
                    case 'IsFinished':
                        return obj.IsFinished === true ? ih.FINISHED_ICON_NAME : null;
                    case 'UnreadMessageCount':
                        return obj.UnreadMessageCount > 0 ? ih.UNREADEDMESSAGE_ICON_NAME : null;
                    case 'DocumentCount':
                        return obj.DocumentCount > 0 ? ih.ATTACHMENT_ICON_NAME : null;
                    case 'EntityStateName':
                        return obj.WorkflowImage;
                }
            }
            //
            return null;
        }
    }
    return module;
});