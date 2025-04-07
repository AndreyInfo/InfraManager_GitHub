using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using System;

namespace InfraManager.BLL.FieldEdit
{
    internal class EntityEditorProvider : IEntityEditorProvider, ISelfRegisteredService<IEntityEditorProvider>
    {
        private readonly IServiceMapper<ObjectClass, IEntityEditor> _mapper;

        public EntityEditorProvider(IServiceMapper<ObjectClass, IEntityEditor> mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public IEntityEditor GetEntityEditor(ObjectClassModel classModel)
        {
            return _mapper.Map((ObjectClass)classModel.ObjClassID);
        }
    }
}
