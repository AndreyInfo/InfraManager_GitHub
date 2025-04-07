
using System.Text;
using IM.Core.Import.BLL.Import;
using InfraManager.DAL.Accounts;

namespace IM.Core.Import.BLL.Interface.Import.Models
{
    public record ImportModel
    {
        public string Organization { get; set; }
        public string? OrganizationName { get; set; }
        public string OrganizationNote { get; set; }
        public string? OrganizationExternalID { get; set; }
        
        public string Subdivision { get; set; }
        public string SubdivisionName { get; set; }
        public string SubdivisionNote { get; set; }
        public string SubdivisionExternalID { get; set; }
        public string SubdivisionOrganization { get; set; }
        public string SubdivisionOrganizationExternalID { get; set; }
        public IEnumerable<string> SubdivisionParent { get; set; }
        public string SubdivisionParentExternalID { get; set; }
        public string User { get; set; }
        public string? UserLastName { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserPatronymic { get; set; }
        public string? UserLogin { get; set; }
        public string UserPosition { get; set; }
        public string UserPhone { get; set; }
        public string UserFax { get; set; }
        //Почему то назвали так поле "Примечание"
        public string UserPager { get; set; }
        public string UserEmail { get; set; }
        public string UserNote { get; set; }
        public string UserSID { get; set; }
        public string UserOrganization { get; set; }
        public IEnumerable<string> UserSubdivision { get; set; }
        public string? UserWorkplace { get; set; }
        //"Табличный номер"
        public string UserNumber { get; set; }
        public string UserOrganizationExternalID { get; set; }
        public string UserSubdivisionExternalID { get; set; }
        //"Идентификатор"
        public string? UserExternalID { get; set; }
        public string UserPhoneInternal { get; set; }
        public string UserManager { get; set; }
        
        public ImportTypeEnum? ImportType { get; set; }

        private static string MakeString(string? s) => s ?? "(null)";
        
        private static string MakeString(IEnumerable<string>? enumerable)
        {
            switch (enumerable)
            {
                case null:
                    return "(null)";
                default:
                {
                    var list = enumerable as List<string> ?? enumerable.ToList();
                    var listRepresentation = string.Join('\n', list.Select(MakeString));
                    return list.Any() ? listRepresentation : "(пусто)";
                }
            }
        }

        public string FullName => UserLastName + " " + UserFirstName + " " + UserPatronymic;


        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            return stringBuilder
                .AppendLine($"{nameof(Organization)} = {MakeString(Organization)}")
                .AppendLine($"{nameof(OrganizationName)} = {MakeString(OrganizationName)}")
                .AppendLine($"{nameof(OrganizationNote)} = {MakeString(OrganizationNote)}")
                .AppendLine($"{nameof(OrganizationExternalID)} = {MakeString(OrganizationExternalID)}")
                .AppendLine($"{nameof(Subdivision)} = {MakeString(Subdivision)}")
                .AppendLine($"{nameof(SubdivisionName)} = {MakeString(SubdivisionName)}")
                .AppendLine($"{nameof(SubdivisionNote)} = {MakeString(SubdivisionNote)}")
                .AppendLine($"{nameof(SubdivisionOrganization)} = {MakeString(SubdivisionOrganization)}")
                .AppendLine($"{nameof(SubdivisionParent)} = {MakeString(SubdivisionParent)}")
                .AppendLine($"{nameof(SubdivisionExternalID)} = {MakeString(SubdivisionExternalID)}")
                .AppendLine($"{nameof(SubdivisionOrganizationExternalID)} = {MakeString(SubdivisionOrganizationExternalID)}")
                .AppendLine($"{nameof(SubdivisionParentExternalID)} = {MakeString(SubdivisionParentExternalID)}")
                .AppendLine($"{nameof(User)} = {MakeString(User)}")
                .AppendLine($"{nameof(UserEmail)} = {MakeString(UserEmail)}")
                .AppendLine($"{nameof(UserFax)} = {MakeString(UserFax)}")
                .AppendLine($"{nameof(UserLogin)} = {MakeString(UserLogin)}")
                .AppendLine($"{nameof(UserManager)} = {MakeString(UserManager)}")
                .AppendLine($"{nameof(UserNote)} = {MakeString(UserNote)}")
                .AppendLine($"{nameof(UserNumber)} = {MakeString(UserNumber)}")
                .AppendLine($"{nameof(UserOrganization)} = {MakeString(UserOrganization)}")
                .AppendLine($"{nameof(UserPager)} = {MakeString(UserPager)}")
                .AppendLine($"{nameof(UserPatronymic)} = {MakeString(UserPatronymic)}")
                .AppendLine($"{nameof(UserPhone)} = {MakeString(UserPhone)}")
                .AppendLine($"{nameof(UserPosition)} = {MakeString(UserPosition)}")
                .AppendLine($"{nameof(UserSubdivision)} = {MakeString(UserSubdivision)}")
                .AppendLine($"{nameof(UserWorkplace)} = {MakeString(UserWorkplace)}")
                .AppendLine($"{nameof(UserFirstName)} = {MakeString(UserFirstName)}")
                .AppendLine($"{nameof(UserLastName)} = {MakeString(UserLastName)}")
                .AppendLine($"{nameof(UserPhoneInternal)} = {MakeString(UserPhoneInternal)}")
                .AppendLine($"{nameof(UserExternalID)} = {MakeString(UserExternalID)}")
                .AppendLine($"{nameof(UserSID)} = {MakeString(UserSID)}")
                .AppendLine($"{nameof(UserOrganizationExternalID)} = {MakeString(UserOrganizationExternalID)}")
                .AppendLine($"{nameof(UserSubdivisionExternalID)} = {MakeString(UserSubdivisionExternalID)}")
                .ToString();

        }
    }
}
