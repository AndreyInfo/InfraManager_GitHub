using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

public interface IUserImportRepository
{
    IEnumerable<User> FromFullName(ICollection<IUserDetails> details, bool getRemoved);
    IEnumerable<User> FromFirstNameAndLastName(ICollection<IUserDetails> details, bool getRemoved);
    IEnumerable<User> FromLoginName(bool getRemoved, IEnumerable<string> logins);
    IEnumerable<User> FromNumber(ICollection<IUserDetails> details, bool getRemoved);
    IEnumerable<User> FromSid(ICollection<IUserDetails> details, bool getRemoved);
    IEnumerable<User> FromExternalID(ICollection<IUserDetails> details, bool getRemoved);
    IEnumerable<User> FromEmail(List<string> emails);
}