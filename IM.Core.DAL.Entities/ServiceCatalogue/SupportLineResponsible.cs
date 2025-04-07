using System;

namespace InfraManager.DAL.ServiceCatalogue
{
    public class SupportLineResponsible
    {
        public Guid ObjectID {get;set;}

        public ObjectClass ObjectClassID {get;set;}

        public byte LineNumber {get;set;}

        public Guid OrganizationItemID {get;set;}

        public ObjectClass OrganizationItemClassID { get; set; }

    }
}
