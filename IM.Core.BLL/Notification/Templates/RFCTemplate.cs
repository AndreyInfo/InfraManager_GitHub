using System;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.Notification.Templates
{

    [ObjectClassMapping(ObjectClass.ChangeRequest)]
    public class RFCTemplate : ITemplate<ChangeRequest>
    {
        public Guid ID { get; init; }

        [TemplateParameter("Адрес Web сервера СервисДеска")]
        public string WebServerAddress { get; set; }

        [TemplateParameter("Номер запроса")]
        public string NumberString { get; init; }

        [TemplateParameter("Краткое описание запроса")]
        public string Summary { get; init; }

        [TemplateParameter("Описание запроса")]
        public string Description { get; init; }

        [TemplateParameter("Сумма финансирования")]
        public int? FundingAmount { get; init; }

        [TemplateParameter("Число прикрепленных документов к запросу")]
        public string DocumentCountString { get; set; }

        [TemplateParameter("Цель запроса")]
        public string Target { get; init; }

        [TemplateParameter("Описание запроса в формате HTML")]
        public string HTMLDescription { get; init; }

        [TemplateParameter("Тип запроса")]
        public string RFCTypeFullName { get; init; }

        [TemplateParameter("Категория запроса")]
        public string RFCCategoryFullName { get; init; }

        [TemplateParameter("Приоритет запроса")]
        public string PriorityName { get; init; }

        [TemplateParameter("Срочность запроса")]
        public string UrgencyName { get; init; }

        [TemplateParameter("Влияние запроса")]
        public string InfluenceName { get; init; }

        [TemplateParameter("Когда решен запрос")]
        public string DateSolvedString { get; internal set; }

        [TemplateParameter("Число связанных заявок запроса")]
        public string CallCountString { get; set; }

        [TemplateParameter("Когда должен быть закрыт запрос")]
        public string DatePromisedString { get; internal set; }

        [TemplateParameter("Когда обнаружен запрос")]
        public string DateDetectedString { get; internal set; }

        [TemplateParameter("Когда закрыт запрос")]
        public string DateClosedString { get; internal set; }

        [TemplateParameter("Когда должен быть начат запрос")]
        public string DateStartedString { get; internal set; }

        [TemplateParameter("Число связей запроса на изменения")]
        public string DependencyObjectCountString { get; set; }

        [TemplateParameter("Число связей проблемы")]
        public string DependencyKEObjectCountString { get; set; }

        [TemplateParameter("Трудозатраты запроса")]
        public string ManhoursString { get; init; }

        [TemplateParameter("Оценка трудозатрат запроса")]
        public string ManhoursNormString { get; init; }

        [TemplateParameter("Число связанных заданий")]
        public string WorkOrderCountString { get; set; }

        [TemplateParameter("Число согласований, относящихся к данному запросу")]
        public string NegotiationCountString { get; set; }

        [TemplateParameter("Когда изменен запрос")]
        public string DateModifiedString { get; internal set; }

        [TemplateParameter("Фамилия заявителя запроса")]
        public string InitiatorLastName { get; init; }

        [TemplateParameter("Имя заявителя запроса")]
        public string InitiatorFirstName { get; init; }

        [TemplateParameter("Отчество заявителя запроса")]
        public string InitiatorPatronymic { get; init; }

        [TemplateParameter("Заявитель запроса")]
        public string InitiatorFullName { get; init; }

        [TemplateParameter("Логин заявителя запроса")]
        public string InitiatorLogin { get; init; }

        [TemplateParameter("Телефон заявителя запроса")]
        public string InitiatorPhone { get; init; }

        [TemplateParameter("Табельный номер заявителя запроса")]
        public string InitiatorNumber { get; init; }

        [TemplateParameter("E-mail заявителя запроса")]
        public string InitiatorEmail { get; init; }

        [TemplateParameter("Факс заявителя запроса")]
        public string InitiatorFax { get; init; }

        [TemplateParameter("Прочее заявителя запроса")]
        public string InitiatorPager { get; init; }

        [TemplateParameter("Должность заявителя запроса")]
        public string InitiatorPositionName { get; init; }

        [TemplateParameter("Здание заявителя запроса")]
        public string InitiatorBuildingName { get; init; }

        [TemplateParameter("Комната заявителя запроса")]
        public string InitiatorRoomName { get; init; }

        [TemplateParameter("Рабочее место заявителя запроса")]
        public string InitiatorWorkplaceName { get; init; }

        [TemplateParameter("Организация заявителя запроса")]
        public string InitiatorOrganizationName { get; init; }

        [TemplateParameter("Подразделение заявителя запроса")]
        public string InitiatorSubdivisionName { get; internal set; }

        [TemplateParameter("Фамилия владельца запроса")]
        public string OwnerLastName { get; init; }

        [TemplateParameter("Имя владельца запроса")]
        public string OwnerFirstName { get; init; }

        [TemplateParameter("Отчество владельца запроса")]
        public string OwnerPatronymic { get; init; }

        [TemplateParameter("Владелец запроса")]
        public string OwnerFullName { get; init; }

        [TemplateParameter("Логин владельца запроса")]
        public string OwnerLogin { get; init; }

        [TemplateParameter("Телефон владельца запроса")]
        public string OwnerPhone { get; init; }

        [TemplateParameter("Табельный номер владельца запроса")]
        public string OwnerNumber { get; init; }

        [TemplateParameter("E-mail владельца запроса")]
        public string OwnerEmail { get; init; }

        [TemplateParameter("Факс владельца запроса")]
        public string OwnerFax { get; init; }

        [TemplateParameter("Прочее владельца запроса")]
        public string OwnerPager { get; init; }

        [TemplateParameter("Должность владельца запроса")]
        public string OwnerPositionName { get; init; }

        [TemplateParameter("Здание владельца запроса")]
        public string OwnerBuildingName { get; init; }

        [TemplateParameter("Комната владельца запроса")]
        public string OwnerRoomName { get; init; }

        [TemplateParameter("Рабочее место владельца запроса")]
        public string OwnerWorkplaceName { get; init; }

        [TemplateParameter("Организация владельца запроса")]
        public string OwnerOrganizationName { get; init; }

        [TemplateParameter("Подразделение владельца запроса")]
        public string OwnerSubdivisionName { get; internal set; }

        [TemplateParameter("Группа запроса")]
        public string QueueName { get; set; }

        [TemplateParameter("Цифровой номер")]
        public string NumberOnly { get; init; }

    }
}
