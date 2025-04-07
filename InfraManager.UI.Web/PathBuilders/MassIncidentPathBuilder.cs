using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.Core.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace InfraManager.UI.Web.PathBuilders
{
    [ObjectClassMapping(ObjectClass.MassIncident)]
    public class MassIncidentPathBuilder : IBuildResourcePath
    {
        private const string ResourceName = "massincidents";

        private readonly IMassIncidentBLL _service;
        private readonly IMemoryCache _cache;

        public MassIncidentPathBuilder(IMassIncidentBLL service, IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public string GetPathToList()
        {
            return $"/api/{ResourceName}";
        }

        // TODO: Выпилить костыль, после общего перехода на int идентификаторы
        public string GetPathToSingle(string id)
        {
            int massIncidentID;
            if (!int.TryParse(id, out massIncidentID))
            {
                var imObjID = new Guid(id);
                massIncidentID = _cache.GetOrCreate(
                    $"massIncidentID_{id}",
                    cacheEntry =>
                        _service
                            .GetDetailsArrayAsync(
                                new MassIncidentListFilter 
                                { 
                                    GlobalIdentifiers = new[] { imObjID } 
                                })
                            .Result
                            .FirstOrDefault()
                            .ID);
            }

            return $"/api/{ResourceName}/{massIncidentID}";
        }
    }
}
