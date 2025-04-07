using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement
{
    public static class UserAccessBLLExtensions
    {
        public static async Task<bool> ViewIsGrantedAsync(
            this IUserAccessBLL service,
            Guid userId,
            string view,
            CancellationToken cancellationToken = default)
        {
            var hasRoles = await service.HasRolesAsync(userId, cancellationToken);

            return hasRoles || ListView.IsClientView(view);
        }

        //TODO: Это нужно делать через IValidateUserPermissions (у нас не процедурное программирование)
        public static async Task ThrowIfNoAdminAsync(this IUserAccessBLL service, Guid userID, ILogger logger, CancellationToken cancellationToken)
        {
            if (await service.HasAdminRoleAsync(userID, cancellationToken))
                return;

            logger.LogTrace($"UserID = {userID} tried get/change data for admin, and this user is not admin ");
            throw new AccessDeniedException($"UserID = {userID} not has role Admin");
        }
    }
}
