using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.Notification.Templates
{
    [ObjectClassMapping(ObjectClass.Substitution)]
    public class SubstitutionTemplate : ITemplate<DeputyUser>
    {
        [TemplateParameter("Фамилия заместителя")]
        public string ChildLastName { get; set; }

        [TemplateParameter("Имя заместителя")]
        public string ChildName { get; set; }

        [TemplateParameter("Отчество заместителя")]
        public string ChildPatronymic { get; set; }
        [TemplateParameter("Фамилия замещаемого")]
        public string ParentLastName { get; set; }

        [TemplateParameter("Имя замещаемого")]
        public string ParentName { get; set; }

        [TemplateParameter("Отчество замещаемого")]
        public string ParentPatronymic { get; set; }
        [TemplateParameter("Дата начала замещения")]
        public string DateDeputyWithString { get; set; }
        [TemplateParameter("Дата окончания замещения")]
        public string DateDeputyByString { get; set; }

    }
}
