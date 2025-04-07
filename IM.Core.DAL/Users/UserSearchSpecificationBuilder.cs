using Inframanager;
using Microsoft.EntityFrameworkCore;
using System;

namespace InfraManager.DAL.Users
{
    internal class UserSearchSpecificationBuilder : 
        IBuildSearchSpecification<User>,
        ISelfRegisteredService<IBuildSearchSpecification<User>>
    {
        public Specification<User> Build(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentException("Empty text to search", nameof(searchText));
            }

            var pattern = searchText.ToLower().ToStartsWithPattern();

            return new Specification<User>(
                u => EF.Functions.Like(u.Surname.Trim().ToLower() + " " + u.Name.Trim().ToLower() + " " + u.Patronymic.ToLower(), pattern)
                    || EF.Functions.Like(u.Number.ToLower(), pattern)
                    || EF.Functions.Like(u.Email.ToLower(), pattern)
                    || EF.Functions.Like(u.Phone.ToLower(), pattern)
                    || EF.Functions.Like(u.Phone1.ToLower(), pattern)
                    || EF.Functions.Like(u.LoginName.ToLower(), pattern));
        }
    }
}
