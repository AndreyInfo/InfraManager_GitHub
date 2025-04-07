using IM.Core.DM.BLL.Interfaces;
using IM.Core.DM.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Core.DM.BLL
{
    internal class UserDataManagerBLL : IUserDataManagerBLL, ISelfRegisteredService<IUserDataManagerBLL>
    {
        private readonly IReadonlyRepository<User> userRepository;
        private readonly IReadonlyRepository<RoleOperation> roleOperationRepository;
        private readonly IReadonlyRepository<UserRole> userRoleRepository;
        private readonly IReadonlyRepository<JobTitle> positionRepository;

        public UserDataManagerBLL(
                    IReadonlyRepository<User> userRepository,
                    IReadonlyRepository<UserRole> userRoleRepository,
                    IReadonlyRepository<JobTitle> positionRepository,
                    IReadonlyRepository<RoleOperation> roleOperationRepository)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.roleOperationRepository = roleOperationRepository;
            this.positionRepository = positionRepository;
        }

        public HashSet<int> GetGrantedOperations(Guid userID)
        {
            return roleOperationRepository.Query()
                    .Where(x => userRoleRepository.Query().Any(z => z.UserID == userID && z.RoleID == x.RoleID))
                    .Distinct()
                    .Select(x => (int)x.OperationID)
                    .ToHashSet();
        }

        public UserModel GetUser(Guid id)
        {
            var query = from user in userRepository.Query().AsNoTracking()
                                        .Include(u => u.Position)
                                        .Where(x => x.IMObjID == id && x.Removed == false)
                        join position in positionRepository.Query().AsNoTracking()
                        on user.Position.ID equals position.ID
                        into opJoin
                        from position in opJoin.DefaultIfEmpty()
                        select new UserModel()
                        {
                            Id = user.IMObjID,
                            Name = user.Name ?? "",
                            Surname = user.Surname ?? "",
                            Patronymic = user.Patronymic ?? "",
                            Email = user.Email,
                            LoginName = user.LoginName,
                            PositionName = position.Name,
                            WebAccessIsGranted = user.SDWebAccessIsGranted,
                            WebPasswordHash = user.SDWebPassword,
                            HasAdminRole = userRoleRepository.Query().Any(z => z.UserID == user.IMObjID && z.RoleID == Role.AdminRoleId)
                        };
            return query.FirstOrDefault();
        }
    }
}
