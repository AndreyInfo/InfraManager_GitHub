using IM.Core.DM.BLL.Interfaces;
using IM.Core.DM.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using System.Linq;

namespace IM.Core.DM.BLL
{
    internal class OperationDataManagerBLL : IOperationDataManagerBLL, ISelfRegisteredService<IOperationDataManagerBLL>
    {
        private readonly IReadonlyRepository<InframanagerObjectClass> _imClassRepository;
        private readonly IReadonlyRepository<InfraManager.DAL.AccessManagement.Operation> _operationRepository;

        public OperationDataManagerBLL(
                    IReadonlyRepository<InframanagerObjectClass> imClassRepository,
                    IReadonlyRepository<InfraManager.DAL.AccessManagement.Operation> operationRepository)
        {
            this._imClassRepository = imClassRepository;
            this._operationRepository = operationRepository;
        }

        public OperationModel[] GetList()
        {
            var query =
                   from operations in _operationRepository.Query()
                   join classes in _imClassRepository.Query()
                   on operations.ClassID equals classes.ID
                   into opJoin
                   from classes in opJoin.DefaultIfEmpty()
                   orderby classes.Name, operations.Name
                   select new OperationModel()
                   {
                       ID = (int)operations.ID,
                       Name = operations.OperationName,
                       Note = operations.Description,
                       ClassId = (int)operations.ClassID,
                       ClassName = classes.Name ?? ""
                   };
            return query.ToArray();
        }
    }
}
