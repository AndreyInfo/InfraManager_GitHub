using Inframanager.BLL;
using System;

namespace InfraManager.BLL.OrganizationStructure
{
    public class SubdivisionListFilter
    {
        public Guid? OrganizationID { get; init; }
        //public NullablePropertyWrapper<Guid> ParentID { get; init; } TODO: научиться парсить этот тип из квери строки и тогда раскоментировать
        public Guid? ParentID { get; init; }
        public bool OnlyRoots { get; init; }
    
        public string OrganizationNameSearch { get; init; }
        
        public string SortBy { get; init; }
        public bool Ascending { get; init; }
    }
}
