using InfraManager.Core.Exceptions;
using System;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Helpers
{
    public static class ValidatorHelper
    {
        public static string CreateErrorMessage(ArgumentValidationException ex)
        {
            if (ex.ErrorType == ValidationErrorType.None || String.IsNullOrWhiteSpace(ex.PropertyName))
                return ex.Message;
            //
            var localeName = Resources.ResourceManager.GetString(@"Property_" + ex.PropertyName);
            var localePhrase = Resources.ResourceManager.GetString(@"ValidationErrorType_" + Enum.GetName(typeof(ValidationErrorType), ex.ErrorType));
            var resourseValue = Resources.ResourceManager.GetString(@"Value_" + ex.Value);
            var retval = String.Format(localePhrase, localeName, String.IsNullOrWhiteSpace(resourseValue) ? ex.Value : resourseValue);
            //
            return retval;
        }
    }
}
