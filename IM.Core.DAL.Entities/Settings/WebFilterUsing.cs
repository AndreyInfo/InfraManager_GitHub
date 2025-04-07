using System;

namespace InfraManager.DAL.Settings
{
    public partial class WebFilterUsing
    {
        public Guid FilterId { get; init; }
        public Guid UserId { get; init; }
        public virtual WebFilter Filter { get; init; }
        public DateTime UtcDateLastUsage { get; set; }
    }
}