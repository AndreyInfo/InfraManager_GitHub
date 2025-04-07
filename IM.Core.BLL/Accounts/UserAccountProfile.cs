using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.Accounts;

namespace InfraManager.BLL.Accounts
{
    public class UserAccountProfile : Profile
    {
        public UserAccountProfile()
        {
            CreateMap<UserAccount, UserAccountDetails>()
            .ForMember(destination => destination.TypeText, mapper => mapper.MapFrom<LocalizedEnumResolver<UserAccount, UserAccountDetails, UserAccountTypes>, UserAccountTypes>(source => source.Type))
            .ForMember(destination => destination.AuthenticationProtocolText, mapper => mapper.MapFrom<LocalizedEnumResolver<UserAccount, UserAccountDetails, HashAlgorithms>, HashAlgorithms>(source => source.AuthenticationProtocol))
            .ForMember(destination => destination.PrivacyProtocolText, mapper => mapper.MapFrom<LocalizedEnumResolver<UserAccount, UserAccountDetails, CryptographicAlgorithms>, CryptographicAlgorithms>(source => source.PrivacyProtocol));

            CreateMap<UserAccountData, UserAccount>();
        }
    }
}
