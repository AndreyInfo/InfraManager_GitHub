using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Settings;
using InfraManager.DAL;

namespace InfraManager.BLL.ServiceDesk
{
    internal class ServiceDeskObjectAccessIsNotRestricted : ISelfRegisteredService<ServiceDeskObjectAccessIsNotRestricted>
    {
        private readonly IUserAccessBLL _userAccess;
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<bool> _converter;

        public ServiceDeskObjectAccessIsNotRestricted(IUserAccessBLL userAccess, ISettingsBLL settings, IConvertSettingValue<bool> converter)
        {
            _userAccess = userAccess;
            _settings = settings;
            _converter = converter;
        }

        public bool IsSatisfiedBy(User user)
        {
            return !_converter.Convert(_settings.GetValue(SystemSettings.UseTTZ))
                || User.IsAdmin.IsSatisfiedBy(user)
                || _userAccess.UserHasOperation(user.IMObjID, OperationID.SD_General_Administrator);
        }
    }
}
