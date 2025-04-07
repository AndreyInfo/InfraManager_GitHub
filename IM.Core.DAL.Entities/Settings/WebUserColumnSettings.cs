using System;

namespace InfraManager.DAL.Settings
{
    public class WebUserColumnSettings
    {
        private const string VirtualColumnPrefix = "parameter_";
        private const string NotShowColumnInTablePrefix = "fake_";

        protected WebUserColumnSettings()
        {
        }

        public WebUserColumnSettings(Guid userId, string listName, string memberName, bool isShowInTable)
        {
            UserId = userId;
            ListName = listName;
            var notShowColumnInTablePrefix = isShowInTable ? string.Empty : NotShowColumnInTablePrefix;
            MemberName = $"{notShowColumnInTablePrefix}{memberName}";
        }

        public Guid UserId { get; private set; }
        public string ListName { get; private set; }
        public string MemberName { get; private set; }
        public int Order { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
        public bool? SortAsc { get; set; }
        public bool? CtrlSortAsc { get; set; }
        public bool IsVirtual => MemberName.StartsWith(VirtualColumnPrefix);
        public bool IsShowColumnInTable => !MemberName.StartsWith(NotShowColumnInTablePrefix);
        public string CleanMemberName => MemberName.Replace(NotShowColumnInTablePrefix, "");
    }
}
