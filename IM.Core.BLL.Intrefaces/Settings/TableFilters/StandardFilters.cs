namespace InfraManager.BLL.Settings.TableFilters
{
    public static class StandardFilters
    {
        public const string All = "_ALL_";

        public const string ProblemMyOpenOverdue = "_PROB_MYOPEN_OVERDUE_";
        public const string ProblemNoOwner = "_PROB_NO_OWNER_";
        public const string ProblemColleagueOverdue = "_PROB_COLLEG_OVERDUE_";
        public const string ProblemAllMy = "_PROB_ALLMY_";
        public const string ProblemAllMyClosed = "_PROB_ALLMY_CLOSE_";
        public const string ProblemAllMyOpened = "_PROB_ALLMY_OPEN_";
        public const string ProblemAllMySolvedAndWaiting = "_PROB_ALLMY_FIXED_WAIT_";
        public const string ProblemColleagueClosed = "_PROB_COLLEG_CLOSE_";
        public const string ProblemColleagueOpened = "_PROB_COLLEG_OPEN_";
        public const string ProblemColleagueSolvedAndWaiting = "_PROB_COLLEG_FIXED_WAIT_";

        public const string NegotiationStartedNeeded = "_NEG_STARTED_NEEDED_";

        public const string MyTasksAllMyNotClose = "_MYWOP_ALLMY_NOTCLOSE_";

        public const string CallEngineerAllMyNotAccomplished = "_ENGINEER_ALL_MY_NOTACCOMPLISH_";

        public const string WorkOrderAllMyInWorkNotAccomplished = "_WO_ALLMY_INWORK_NOT_ACCOMPLISH_";

        public const string CallClientAllMyInWork = "_CLIENT_ALL_MY_INWORK_";
        public const string CallClientOnlyMyInWork = "_CLIENT_ONLY_MY_INWORK_";
        public const string CallClientOnlyMyWaitClosed = "_CLIENT_ONLY_MY_WAIT_CLOSED_";
        public const string CallClientInWorkIinit = "_CLIENT_INWORK_I_INIT_";
        public const string CallClientAllMyClosed = "_CLIENT_ALL_MY_CLOSED_";
        public const string CallClientCollegsInWork = "_CLIENT_COLLEGS_INWORK_";
        public const string CallClientCollegsClosed = "_CLIENT_COLLEGS_CLOSED_";
        public const string CallClientOnlyMyInitedWaitClosed = "_CLIENT_ONLY_MY_INITED_WAIT_CLOSED_";

        public const string CallEngineerAllMyNotAccomplish = "_ENGINEER_ALL_MY_NOTACCOMPLISH_";
        public const string CallEngineerAllCollegNotAccomplish = "_ENGINEER_ALL_COLLEG_NOTACCOMPLISH_";
        public const string CallEngineerNoOwner = "_ENGINEER_NO_OWNER_";
        public const string CallEngineerMyNotStarted = "_ENGINEER_MY_NOT_STARTED_";
        public const string CallEngineerMyWaitClosed = "_ENGINEER_MY_WAIT_CLOSED_";
        public const string CallEngineerCollegAccomplishWaitClose = "_ENGINEER_COLLEG_ACCOMPLISH_WAIT_CLOSE_";
        public const string CallEngineerOpenIOwner = "_ENGINEER_OPEN_I_OWNER_";
        public const string CallEngineerOpenIExecutor = "_ENGINEER_OPEN_I_EXECUTOR_";
        public const string CallEngineerMyOpenOverdue = "_ENGINEER_MY_OPEN_OVERDUE_";
        public const string CallEngineerCollegOpenOverdue = "_ENGINEER_COLLEG_OPEN_OVERDUE_";
        public const string CallEngineerAllmy = "_ENGINEER_ALLMY_";
        public const string CallEngineerAllmyClosed = "_ENGINEER_ALLMY_CLOSED_";
        public const string CallEngineerAllCollegs = "_ENGINEER_ALL_COLLEGS_";
        public const string CallEngineerAllCollegsClose = "_ENGINEER_ALL_COLLEGS_CLOSE_";

        public const string WordOrderMyNotAcceptNotAccomplish = "_WO_MY_NOT_ACCEPT_NOT_ACCOMPLISH_";
        public const string WordOrderMyNotAccomplishOverdue = "_WO_MY_NOT_ACCOMPLISH_OVERDUE_";
        public const string WordOrderCollegNotAccomplishOverdue = "_WO_COLLEG_NOT_ACCOMPLISH_OVERDUE_";
        public const string WordOrderNoOwner = "_WO_NO_OWNER_";
        public const string WordOrderMyInitInWork = "_WO_MY_INIT_INWORK_";
        public const string WordOrderMyInitWait = "_WO_MY_INIT_WAIT_";
        public const string WorkOrderAllmy = "_WO_ALLMY_";
        public const string WorkOrderAllmyInWorkNotAccomplish = "_WO_ALLMY_INWORK_NOT_ACCOMPLISH_";
        public const string WorkOrderAllmyWaitNotAccomplish = "_WO_ALLMY_WAIT_NOT_ACCOMPLISH_";
        public const string WorkOrderAllmyClosed = "_WO_ALLMY_CLOSED_";
        public const string WorkOrderCollegNotAccomplish = "_WO_COLLEG_NOT_ACCOMPLISH_";
        public const string WorkOrderCollegWait = "_WO_COLLEG_WAIT_";
        public const string WorkOrderAssignByMeInWork = "_WO_ASSIGN_BY_ME_INWORK_";
        public const string WorkOrderAssignByMeWait = "_WO_ASSIGN_BY_ME_WAIT_";

        public const string MyTaskMyWopNoOwner = "_MYWOP_NO_OWNER_";
        public const string MyTaskMyWopMyOpenOverdue = "_MYWOP_MYOPEN_OVERDUE_";
        public const string MyTaskMyWopCollegOpenOverdue = "_MYWOP_COLLEG_OPEN_OVERDUE_";
        public const string MyTaskMyWopAllmy = "_MYWOP_ALLMY_";
        public const string MyTaskMyWopAllmyClose = "_MYWOP_ALLMY_CLOSE_";
        public const string MyTaskMyWopAllmyNotClose = "_MYWOP_ALLMY_NOTCLOSE_";
        public const string MyTaskMyWopAllColleg = "_MYWOP_ALLCOLLEG_";
        public const string MyTaskMyWopAllCollegInWork = "_MYWOP_ALLCOLLEG_INWORK_";
        public const string MyTaskMyWopAllCollegClose = "_MYWOP_ALLCOLLEG_CLOSE_";
        
        public const string NegotiationNegStartedNeeded = "_NEG_STARTED_NEEDED_";
        public const string NegotiationNegStartedNotNeeded = "_NEG_STARTED_NOT_NEEDED_";
        public const string NegotiationNegFinished = "_NEG_FINISHED_";
        public const string NegotiationNegNotStarted = "_NEG_NOT_STARTED_";

        public const string MassIncidentMyNotStarted = "_MI_MY_NOT_STARTED_";
        public const string MassIncidentMyOpenedOverdue = "_MI_MY_OPENED_OVERUDE_";
        public const string MassIncidentUnassigned = "_MI_UNASSIGNED_";
        public const string MassIncidentOthersOverdue = "_MI_OTHERS_OVERDUE_";
        public const string MassIncidentMy = "_MI_MY_";
        public const string MassIncidentMyInWork = "_MI_MY_IN_WORK_";
        public const string MassIncidentMyCompletedWaitingForConfirmation = "_MI_MY_COMPLETED_CONFIRM_";
        public const string MassIncidentMyClosed = "_MI_MY_CLOSED_";
        public const string MassIncidentNotMy = "_MI_NOT_MY_";
        public const string MassIncidentNotMyInWork = "_MI_NOT_MY_INWORK_";
        public const string MassIncidentNotMyWaitingForConfirmation = "_MI_NOT_MY_COMPLETED_CONFIRM_";
        public const string MassIncidentNotMyClosed = "_MI_NOT_MY_CLOSED_";
        public const string MassIncidentOwnedOpened = "_MI_OWNED_OPENED_";
        public const string MassIncidentExecutedOpened = "_MI_EXECUTED_OPENED_";
    }
}
