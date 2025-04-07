using System;

namespace InfraManager.Core.Exceptions
{
    public sealed class TimeSheetObjectInUseException: ObjectInUseException
    {
        public TimeSheetObjectInUseException(Guid objectID, string objectName)
            : base(objectID, objectName)
        { }

        public override string Message
        {
            get
            {
                var friendlyClassName = Helpers.TypeHelper.GetFriendlyClassName(TargetSite.DeclaringType);
                if (string.IsNullOrEmpty(friendlyClassName))
                {
                    if (string.IsNullOrEmpty(this.ObjectName))
                        return "Объект пересекается с утвержденным табелем.";
                    else
                        return string.Format("Объект '{0}' пересекается с утвержденным табелем.", this.ObjectName);
                }
                else
                {
                    if (string.IsNullOrEmpty(this.ObjectName))
                        return string.Format("Объект [{0}] пересекается с утвержденным табелем.", friendlyClassName);
                    else
                        return string.Format("Объект '{0}' [{1}] пересекается с утвержденным табелем.", this.ObjectName, friendlyClassName);
                }
            }
        }
    }
}
