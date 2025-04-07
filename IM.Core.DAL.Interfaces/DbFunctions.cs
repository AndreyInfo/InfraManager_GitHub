using System;

namespace InfraManager.DAL
{
    public static class DbFunctions
    {
        public static DateTime? TruncateSeconds(DateTime? date) =>
            throw new NotSupportedException();

        public static bool AccessIsGranted(
            ObjectClass classId,
            Guid id,
            Guid ownerId,
            ObjectClass ownerClassId,
            AccessManagement.AccessTypes type,
            bool propagate) =>
            throw new NotSupportedException();

        public static string GetReasonName(ObjectClass? classId, Guid? id) =>
            throw new NotSupportedException();

        public static string CastAsString(string value) =>
            throw new NotSupportedException();

        public static string CastAsString(int value) =>
            throw new NotSupportedException();

        public static string GetFullObjectName(ObjectClass? classId, Guid? id) => 
            throw new NotSupportedException();
        
        public static string GetFullObjectLocation(ObjectClass? classId, Guid? id) => 
            throw new NotSupportedException();

        public static string GetCategoryClassFullName(Guid id) => 
            throw new NotImplementedException();

        public static string GetFullTimeZoneName(string timezoneId) =>
            throw new NotImplementedException();

        public static string GetFullCalendarWorkScheduleName(Guid? id) =>
            throw new NotImplementedException();

        public static string GetCategoryFullName(Guid id) =>
            throw new NotImplementedException();

        public static DateTime? CastAsDateTime(DateTime? value) =>
            throw new NotImplementedException();

        public static decimal? CastAsDecimal(decimal? value) =>
            throw new NotImplementedException();

        public static bool ItemInOrganizationItem(
            ObjectClass? orgItemClassID,
            Guid? orgItemID,
            ObjectClass? itemClassID,
            Guid? itemID) =>
            throw new NotSupportedException();
    }
}
