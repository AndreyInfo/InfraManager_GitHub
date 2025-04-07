using AutoMapper;
using InfraManager.BLL.Settings.UserFields;

namespace InfraManager.BLL.AutoMapper
{
    public class UserFieldNameResolver<TSrc, TDest>
        : IMemberValueResolver<TSrc, TDest, UserField, string>
    {
        private readonly IServiceMapper<UserFieldType, IUserFieldNameBLL> _providerMapper;

        public UserFieldNameResolver(
            IServiceMapper<UserFieldType, IUserFieldNameBLL> providerMapper)
        {
            _providerMapper = providerMapper;
        }

        public string Resolve(TSrc source, TDest destination, UserField sourceMember, string destMember, ResolutionContext context)
        {
            return _providerMapper.Map(sourceMember.Type).GetName(sourceMember.Number);
        }
    }
}
