using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.DAL;

namespace InfraManager.BLL.Users;

public class UserModifier:
    IModifyObject<User, UserData>,
    ISelfRegisteredService<IModifyObject<User, UserData>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserAccessBLL _access;
    private readonly IMapper _mapper;

    public UserModifier(ICurrentUser currentUser,
        IUserAccessBLL access,
        IMapper mapper)
    {
        _currentUser = currentUser;
        _access = access;
        _mapper = mapper;
    }

    public async Task ModifyAsync(User entity, UserData data, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(data.Password) &&
            !await _access.HasAdminRoleAsync(_currentUser.UserId, cancellationToken))
        {
            throw new AccessDeniedException(
                $"User with ID = {_currentUser.UserId} has no rights to change Users passwords with ID = {entity.ID} ");
        }

        _mapper.Map(data, entity);
    }

    public void SetModifiedDate(User entity)
    {
    }
}