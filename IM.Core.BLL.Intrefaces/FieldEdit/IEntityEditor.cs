using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.FieldEdit
{
    public interface IEntityEditor
    {
        /// <summary>
        /// Определяет извесетн ли тип модифицируемого объекта
        /// </summary>
        /// <param name="objectClass"></param>
        /// <returns></returns>
        bool CanHandle(ObjectClassModel objectClass);

        /// <summary>
        /// Выполняет модификацию поля объекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResult<SetFieldResult, BaseError>> HandleAsync(SetFieldRequest model, CancellationToken cancellationToken);
    }
}
