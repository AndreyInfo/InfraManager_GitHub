using FluentValidation;
using Inframanager.BLL;
using Inframanager.BLL.Validation;
using InfraManager.BLL.Localization;
using InfraManager.BLL.ServiceDesk.FormDataValue;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    /// <summary>
    /// Проверяет данные CallData на возможность создать экземпляр Call
    /// </summary>
    internal class CallDataValidator : 
        FluentValidator<CallData>, // наследуем базовый валидатор на основе FluentValidation
        ISelfRegisteredService<IValidateObject<CallData>> // саморегистрируемся как валидатор объекта CallData
    {
        public CallDataValidator(ILocalizeText localizer) : base(localizer)
        {
        }

        protected override void Initializing()
        {
            RuleFor(x => x.CallTypeID).NotNull().WithMessage($"Call type is required");
            // Не user-friendly сообщение, интерфейс должен контролировать заполнение

            When(x => x.FormValuesData is not null,
                () => When(x => x.FormValuesData.Values is not null, 
                    () => {
                        RuleForEach(x => x.FormValuesData.Values).Must(ValidateDataItem);
                        RuleFor(x => x.FormValuesData.FormID).NotNull();
                    }));
        }


        private bool ValidateDataItem(DataItem dataItem)
            => true; // TODO
    }
}
