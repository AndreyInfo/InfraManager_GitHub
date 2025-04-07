using IM.Core.DM.BLL.Interfaces;
using IM.Core.DM.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using System.Linq;

namespace IM.Core.DM.BLL
{
    internal class ClassDataManagerBLL : IClassDataManagerBLL, ISelfRegisteredService<IClassDataManagerBLL>
    {
        private readonly IReadonlyRepository<InframanagerObjectClass> imClassRepository;

        public ClassDataManagerBLL(IReadonlyRepository<InframanagerObjectClass> imClassRepository)
        {
            this.imClassRepository = imClassRepository;
        }

        public ClassModel[] GetList()
        {
            return imClassRepository.Query()
                .OrderBy(x => x.Name)
                .Select(x => new ClassModel()
                {
                    ID = (int)x.ID,
                    Name = x.Name
                })
                .ToArray();
        }
    }
}
