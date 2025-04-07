using System;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Notification.Templates
{
    [ObjectClassMapping(ObjectClass.WorkOrder)]
    public class WorkOrderTemplate : ITemplate<WorkOrder>
    {
        public Guid ID { get; init; }

        [TemplateParameter("Номер задания")]
        public string NumberString { get; init; }

        [TemplateParameter("Название задания")]
        public string Name { get; init; }

        [TemplateParameter("Описание задания")]
        public string Description { get; init; }

        [TemplateParameter("Описание задания в формате HTML")]
        public string HTMLDescription { get; init; }

        [TemplateParameter("Трудозатраты задания")]
        public string ManhoursString { get; init; }

        [TemplateParameter("Оценка трудозатрат задания")]
        public string ManhoursNormString { get; init; }

        [TemplateParameter("С чем связано задание")]
        public string WorkOrderReferenceString { get; init; }

        [TemplateParameter("Когда создано задание")]
        public string DateCreatedString { get; internal set; }

        [TemplateParameter("Когда изменено задание")]
        public string DateModifiedString { get; internal set; }

        [TemplateParameter("Когда назначено задание")]
        public string DateAssignedString { get; internal set; }

        [TemplateParameter("Когда принято задание")]
        public string DateAcceptedString { get; internal set; }

        [TemplateParameter("Когда должно быть выполнено задание")]
        public string DatePromisedString { get; internal set; }

        [TemplateParameter("Когда начато выполнение задания")]
        public string DateStartedString { get; internal set; }

        [TemplateParameter("Когда завершено задание")]
        public string DateAccomplishedString { get; internal set; }

        [TemplateParameter("Тип задания")]
        public string WorkOrderTypeName { get; init; }

        [TemplateParameter("Приоритет задания")]
        public string WorkOrderPriorityName { get; init; }

        [TemplateParameter("Фамилия инициатора задания")]
        public string InitiatorLastName { get; init; }

        [TemplateParameter("Имя инициатора задания")]
        public string InitiatorFirstName { get; init; }

        [TemplateParameter("Отчество инициатора задания")]
        public string InitiatorPatronymic { get; init; }

        [TemplateParameter("Инициатор задания")]
        public string InitiatorFullName { get; init; }

        [TemplateParameter("Логин инициатора задания")]
        public string InitiatorLogin { get; init; }

        [TemplateParameter("Телефон инициатора задания")]
        public string InitiatorPhone { get; init; }

        [TemplateParameter("Табельный номер инициатора задания")]
        public string InitiatorNumber { get; init; }

        [TemplateParameter("E-mail инициатора задания")]
        public string InitiatorEmail { get; init; }

        [TemplateParameter("Факс инициатора задания")]
        public string InitiatorFax { get; init; }

        [TemplateParameter("Прочее инициатора задания")]
        public string InitiatorPager { get; init; }

        [TemplateParameter("Должность инициатора задания")]
        public string InitiatorPositionName { get; init; }

        [TemplateParameter("Здание инициатора задания")]
        public string InitiatorBuildingName { get; init; }

        [TemplateParameter("Комната инициатора задания")]
        public string InitiatorRoomName { get; init; }

        [TemplateParameter("Рабочее место инициатора задания")]
        public string InitiatorWorkplaceName { get; init; }

        [TemplateParameter("Организация инициатора задания")]
        public string InitiatorOrganizationName { get; init; }

        [TemplateParameter("Подразделение инициатора задания")]
        public string InitiatorSubdivisionName { get; internal set; }

        [TemplateParameter("Фамилия назначившего задание")]
        public string AssignorLastName { get; init; }

        [TemplateParameter("Имя назначившего задание")]
        public string AssignorFirstName { get; init; }

        [TemplateParameter("Отчество назначившего задание")]
        public string AssignorPatronymic { get; init; }

        [TemplateParameter("Назначивший задание")]
        public string AssignorFullName { get; init; }

        [TemplateParameter("Логин назначившего задание")]
        public string AssignorLogin { get; init; }

        [TemplateParameter("Телефон назначившего задание")]
        public string AssignorPhone { get; init; }

        [TemplateParameter("Табельный номер назначившего задание")]
        public string AssignorNumber { get; init; }

        [TemplateParameter("E-mail назначившего задание")]
        public string AssignorEmail { get; init; }

        [TemplateParameter("Факс назначившего задание")]
        public string AssignorFax { get; init; }

        [TemplateParameter("Прочее назначившего задание")]
        public string AssignorPager { get; init; }

        [TemplateParameter("Должность назначившего задание")]
        public string AssignorPositionName { get; init; }

        [TemplateParameter("Здание назначившего задание")]
        public string AssignorBuildingName { get; init; }

        [TemplateParameter("Комната назначившего задание")]
        public string AssignorRoomName { get; init; }

        [TemplateParameter("Рабочее место назначившего задание")]
        public string AssignorWorkplaceName { get; init; }

        [TemplateParameter("Организация назначившего задание")]
        public string AssignorOrganizationName { get; init; }

        [TemplateParameter("Подразделение назначившего задание")]
        public string AssignorSubdivisionName { get; internal set; }

        [TemplateParameter("Фамилия исполнителя задания")]
        public string ExecutorLastName { get; init; }

        [TemplateParameter("Имя исполнителя задания")]
        public string ExecutorFirstName { get; init; }

        [TemplateParameter("Отчество исполнителя задания")]
        public string ExecutorPatronymic { get; init; }

        [TemplateParameter("Исполнитель задания")]
        public string ExecutorFullName { get; init; }

        [TemplateParameter("Логин исполнителя задания")]
        public string ExecutorLogin { get; init; }

        [TemplateParameter("Телефон исполнителя задания")]
        public string ExecutorPhone { get; init; }

        [TemplateParameter("Табельный номер исполнителя задания")]
        public string ExecutorNumber { get; init; }

        [TemplateParameter("E-mail исполнителя задания")]
        public string ExecutorEmail { get; init; }

        [TemplateParameter("Факс исполнителя задания")]
        public string ExecutorFax { get; init; }

        [TemplateParameter("Прочее исполнителя задания")]
        public string ExecutorPager { get; init; }

        [TemplateParameter("Должность исполнителя задания")]
        public string ExecutorPositionName { get; init; }

        [TemplateParameter("Здание исполнителя задания")]
        public string ExecutorBuildingName { get; init; }

        [TemplateParameter("Комната исполнителя задания")]
        public string ExecutorRoomName { get; init; }

        [TemplateParameter("Рабочее место исполнителя задания")]
        public string ExecutorWorkplaceName { get; init; }

        [TemplateParameter("Организация исполнителя задания")]
        public string ExecutorOrganizationName { get; init; }

        [TemplateParameter("Подразделение исполнителя задания")]
        public string ExecutorSubdivisionName { get; internal set; }

        [TemplateParameter("Группа задания")]
        public string QueueName { get; init; }

        [TemplateParameter("Число прикрепленных документов к заданию")]
        public string DocumentCountString { get; internal set; }

        [TemplateParameter("Число согласований, относящихся к данному заданию")]
        public string NegotiationCountString { get; set; }

        [TemplateParameter("Число связей задания")]
        public string DependencyObjectCountString { get; set; }

        [TemplateParameter("Адрес Web сервера СервисДеска")]
        public string WebServerAddress { get; set; }

        [TemplateParameter("Состояние")]
        public string EntityStateName { get; init; }

        [TemplateParameter("Цифровой номер")]
        public string NumberOnly { get; init; }

    }
}
