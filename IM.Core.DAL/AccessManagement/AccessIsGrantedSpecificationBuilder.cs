using Inframanager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.AccessManagement
{
    internal class AccessIsGrantedSpecificationBuilder<T> : IBuildAccessIsGrantedSpecification<T>
        where T : class, IGloballyIdentifiedEntity
    {
        private readonly DbSet<T> _db;

        public AccessIsGrantedSpecificationBuilder(DbSet<T> db)
        {
            _db = db;
        }

        public Specification<T> Build(User user)
        {
            return new AccessIsGrantedSpecification<T>(_db, user.IMObjID);
        }

        public Specification<T> Build(Guid userID)
        {
            return new AccessIsGrantedSpecification<T>(_db, userID);
        }

        private class AccessIsGrantedSpecification<T> : Specification<T> where T : class, IGloballyIdentifiedEntity
        {
            private readonly DbSet<T> _db;
            private readonly Guid _userID;

            public AccessIsGrantedSpecification(DbSet<T> db, Guid userID)
                : base(entity => 
                    DbFunctions.AccessIsGranted(
                        typeof(T).GetObjectClassOrRaiseError(),
                        entity.IMObjID,
                        userID,
                        ObjectClass.User,
                        AccessTypes.TOZ_org,
                        false))
            {
                _db = db;
                _userID = userID;
            }

            protected override Func<T, bool> GetFunc()
            {
                return entity => _db
                    .Any(
                        x => x.IMObjID == entity.IMObjID 
                            && DbFunctions.AccessIsGranted(
                                typeof(T).GetObjectClassOrRaiseError(),
                                x.IMObjID,
                                _userID,
                                ObjectClass.User,
                                AccessTypes.TOZ_org,
                                false));
            }

            protected override bool Additive => false;
        }
    }
}
