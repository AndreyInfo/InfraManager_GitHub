using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public interface IClassIM
    {
        public Task<string> GetClassNameAsync(ObjectClass classID, CancellationToken cancellationToken = default);

        public string GetClassName(ObjectClass classID);

        /// <summary>
        /// Возвращает классы по списку идентификаторов
        /// </summary>
        /// <param name="classIDs">Список классов объектов</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<InframanagerObjectClassData[]> GetClassesByIDsAsync(List<ObjectClass> classIDs, CancellationToken cancellationToken = default);
    }
}
