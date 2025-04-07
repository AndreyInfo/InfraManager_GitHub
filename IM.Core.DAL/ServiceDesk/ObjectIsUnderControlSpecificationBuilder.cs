using Inframanager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    internal class ObjectIsUnderControlSpecificationBuilder<T> : IBuildObjectIsUnderControlSpecification<T>
        where T : IGloballyIdentifiedEntity
    {
        private readonly ObjectClass _classID;
        private readonly DbSet<CustomControl> _customControls;

        public ObjectIsUnderControlSpecificationBuilder(IObjectClassProvider<T> classProvider, DbSet<CustomControl> customControls)
        {
            _classID = classProvider.GetObjectClass();
            _customControls = customControls;
        }

        public Specification<T> Build(Guid userID)
        {
            return new Specification<T>(
                obj => _customControls.Any(
                    x => x.UserId == userID && x.ObjectClass == _classID && x.ObjectId == obj.IMObjID));
        }
    }
}
