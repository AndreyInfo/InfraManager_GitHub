using IM.Core.Import.BLL.Interface;
using InfraManager;


namespace IM.Core.Import.BLL.Import
{
    internal class UserAdditionalParametersForSelect : IAdditionalParametersForSelect, ISelfRegisteredService<IAdditionalParametersForSelect>
    {
        private bool _restoreRemovedUsers = false;
        public object GetParametr()
        {
            return _restoreRemovedUsers;
        }

        public void SetParametr(object parametr)
        {
            _restoreRemovedUsers = (bool) parametr;
        }
    }
}
