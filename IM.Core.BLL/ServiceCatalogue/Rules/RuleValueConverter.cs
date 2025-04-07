using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.Rules
{
    internal class RuleValueConverter : IRuleValueConverter, ISelfRegisteredService<IRuleValueConverter>
    {
        private readonly IReadonlyRepository<Service> _services;
        private readonly IReadonlyRepository<ServiceAttendance> _serviceAttendances;
        private readonly IReadonlyRepository<ServiceItem> _serviceItems;
        private readonly IReadonlyRepository<Priority> _priorities;
        private readonly IReadonlyRepository<Urgency> _urgencies;
        private readonly IReadonlyRepository<CallType> _callType;
        private readonly IReadonlyRepository<Organization> _organization;
        private readonly IReadonlyRepository<JobTitle> _positions;
        private readonly IRepository<Subdivision> _subdivisionRepository;
        private readonly IRuleParameterConverter _ruleParameterConverter;

        public RuleValueConverter(
            IReadonlyRepository<Service> services,
            IReadonlyRepository<ServiceAttendance> serviceAttendances,
            IReadonlyRepository<ServiceItem> serviceItems,
            IReadonlyRepository<Priority> priorities,
            IReadonlyRepository<Urgency> urgencies,
            IReadonlyRepository<CallType> callType,
            IReadonlyRepository<Organization> organization,
            IReadonlyRepository<JobTitle> positions,
            IRuleParameterConverter ruleParameterConverter,
            IRepository<Subdivision> subdivisionRepository)
        {
            _services = services;
            _serviceAttendances = serviceAttendances;
            _serviceItems = serviceItems;
            _priorities = priorities;
            _urgencies = urgencies;
            _callType = callType;
            _organization = organization;
            _positions = positions;
            _ruleParameterConverter = ruleParameterConverter;
            _subdivisionRepository = subdivisionRepository;
        }

        public async ValueTask<RuleValue> ConvertFromBytesAsync(byte[] ruleValueData,
            CancellationToken cancellationToken = default)
        {
            var retval = new RuleValue();
            //
            if (ruleValueData == null)
                return retval;
            using (var stream = new MemoryStream(ruleValueData))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    var version = br.ReadString();
                    if (version != RuleValue.VERSION)
                        throw new VersionNotFoundException();
                    //
                    RuleValueType type;
                    int count;
                    FilterOperation operation;
                    long time;
                    Guid id;
                    //
                    while (stream.Position < stream.Length)
                    {
                        type = (RuleValueType)br.ReadByte();
                        //
                        switch (type)
                        {
                            case RuleValueType.Price:
                                retval.Price = br.ReadString();
                                break;
                            case RuleValueType.PromiseTime:
                                retval.PromiseTime = br.ReadInt64();
                                break;
                            case RuleValueType.ServiceAttendance:

                                count = br.ReadInt32();
                                var serviceAttendanceIds = new List<Guid>();

                                for (int i = 0; i < count; i++)
                                {
                                    serviceAttendanceIds.Add(new Guid(br.ReadString()));
                                }

                                retval.ServiceAttendances = await _serviceAttendances
                                    .With(x => x.Service)
                                    .ThenWith(x => x.Category)
                                    .ToArrayAsync(x => serviceAttendanceIds.Contains(x.ID), cancellationToken);
                                break;

                            case RuleValueType.DayOfWeek:
                                count = br.ReadInt32();
                                for (int i = 0; i < count; i++)
                                {
                                    DayOfWeek day = (DayOfWeek)br.ReadByte();
                                    retval.DayOfWeeks.Add(day);
                                }

                                break;

                            case RuleValueType.Priority:

                                count = br.ReadInt32();
                                var priorityIds = new List<Guid>();

                                for (int i = 0; i < count; i++)
                                {
                                    priorityIds.Add(new Guid(br.ReadString()));
                                }

                                retval.Priorities = await _priorities.ToArrayAsync(x => priorityIds.Contains(x.ID),
                                    cancellationToken);
                                break;

                            case RuleValueType.Urgency:

                                count = br.ReadInt32();
                                var urgencyIds = new List<Guid>();

                                for (int i = 0; i < count; i++)
                                {
                                    urgencyIds.Add(new Guid(br.ReadString()));
                                }

                                retval.Urgencies = await _urgencies.ToArrayAsync(x => urgencyIds.Contains(x.ID),
                                    cancellationToken);
                                break;

                            case RuleValueType.CallType:

                                count = br.ReadInt32();
                                var callTypeIds = new List<Guid>();

                                for (int i = 0; i < count; i++)
                                {
                                    callTypeIds.Add(new Guid(br.ReadString()));
                                }

                                retval.CallTypes = await _callType.With(x => x.Parent)
                                    .ToArrayAsync(x => callTypeIds.Contains(x.ID), cancellationToken);

                                break;

                            case RuleValueType.RegistrationTime:

                                operation = (FilterOperation)br.ReadInt32();
                                time = br.ReadInt64();
                                retval.TimeRegistrations.Add(operation, time);

                                break;

                            case RuleValueType.ServiceItem:

                                count = br.ReadInt32();
                                var serviceItemIds = new List<Guid>();

                                for (int i = 0; i < count; i++)
                                {
                                    serviceItemIds.Add(new Guid(br.ReadString()));
                                }

                                retval.ServiceItems = await _serviceItems
                                    .With(x => x.Service)
                                    .ThenWith(x => x.Category)
                                    .ToArrayAsync(x => serviceItemIds.Contains(x.ID), cancellationToken);
                                break;

                            case RuleValueType.Service:

                                count = br.ReadInt32();
                                var serviceIds = new List<Guid>();

                                for (int i = 0; i < count; i++)
                                {
                                    serviceIds.Add(new Guid(br.ReadString()));
                                }

                                retval.Services = await _services
                                    .With(x => x.Category)
                                    .ToArrayAsync(x => serviceIds.Contains(x.ID), cancellationToken);

                                break;
                            case RuleValueType.ClientPosition:

                                count = br.ReadInt32();
                                var positionIds = new List<int>();

                                for (int i = 0; i < count; i++)
                                {
                                    positionIds.Add(br.ReadInt32());
                                }

                                var positions = await _positions.ToArrayAsync(x => positionIds.Contains(x.ID),
                                    cancellationToken);
                                retval.ClientPositions = positions
                                    .Select(x => Tuple.Create(x.Name, x.ID))
                                    .ToArray();

                                break;

                            case RuleValueType.ClientOrgStructure:

                                count = br.ReadInt32();
                                var orgStructureData = new Dictionary<ObjectClass, List<Guid>>();

                                for (int i = 0; i < count; i++)
                                {
                                    var classID = (ObjectClass)br.ReadInt32();
                                    var organizationID = new Guid(br.ReadString());

                                    if (orgStructureData.ContainsKey(classID))
                                    {
                                        orgStructureData[classID].Add(organizationID);

                                        continue;
                                    }

                                    orgStructureData.Add(classID, new List<Guid>(new[] { organizationID }));
                                }

                                retval.ClientOrgStructures = new List<Tuple<string, ObjectClass, Guid, Guid[], Guid>>();

                                if (orgStructureData.ContainsKey(ObjectClass.Division))
                                {
                                    var subdivisionsFormDb =
                                        await _subdivisionRepository.With(x => x.ParentSubdivision).ToArrayAsync(x =>
                                            orgStructureData[ObjectClass.Division].Contains(x.ID), cancellationToken);

                                    List<Guid> idPath = new List<Guid>();
                                    foreach (var el in subdivisionsFormDb)
                                    {
                                        idPath.Add(el.ID);
                                        var parent = el.ParentSubdivision;

                                        while (parent != null)
                                        {
                                            idPath.Add(parent.ID);
                                            parent = parent.ParentSubdivision;
                                        }

                                        idPath.Reverse();

                                        retval.ClientOrgStructures.Add(
                                            new Tuple<string, ObjectClass, Guid, Guid[], Guid>(el.FullName,
                                                ObjectClass.Division,
                                                el.ID, idPath.ToArray(), el.OrganizationID));
                                    }
                                }

                                if (orgStructureData.ContainsKey(ObjectClass.Organizaton))
                                {
                                    var data = await _organization.ToArrayAsync(
                                        x => orgStructureData[ObjectClass.Organizaton].Contains(x.ID),
                                        cancellationToken);

                                    var organizations = data
                                        .Select(x => Tuple.Create(x.Name, ObjectClass.Organizaton, x.ID,
                                            Array.Empty<Guid>(),
                                            Guid.Empty)); //item4 and item5 not using in Organization

                                    retval.ClientOrgStructures.AddRange(organizations);
                                }

                                break;

                            case RuleValueType.ParameterTemplate:
                                count = br.ReadInt32();
                                for (int i = 0; i < count; i++)
                                {
                                    id = new Guid(br.ReadString());
                                    //
                                    int countData = br.ReadInt32();
                                    RuleParameterValue[] binaryValues = null;
                                    if (countData > 0)
                                    {
                                        byte[] valuesData = br.ReadBytes(countData);
                                        binaryValues = _ruleParameterConverter.ParametersFromBytes(valuesData);
                                    }

                                    //                                
                                    retval.ParameterValues.Add(id, binaryValues);
                                }

                                break;
                        }
                    }

                }
            }

            return retval;
        }

        public ValueTask<byte[]> ConvertToBytesAsync(RuleValue ruleValue, CancellationToken cancellationToken = default)
        {
            byte[] retval = null;
            //
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(RuleValue.VERSION);

                //
                if (ruleValue.Services.Any())
                {
                    bw.Write((byte)RuleValueType.Service);
                    bw.Write(ruleValue.Services.Length);

                    foreach (var service in ruleValue.Services)
                        bw.Write(service.ID.ToString());
                }
                //
                if (ruleValue.ServiceAttendances.Any())
                {
                    bw.Write((byte)RuleValueType.ServiceAttendance);
                    bw.Write(ruleValue.ServiceAttendances.Length);

                    foreach (var serviceAttendance in ruleValue.ServiceAttendances)
                        bw.Write(serviceAttendance.ID.ToString());
                }
                //
                if (ruleValue.ServiceItems.Any())
                {
                    bw.Write((byte)RuleValueType.ServiceItem);
                    bw.Write(ruleValue.ServiceItems.Length);

                    foreach (var serviceItem in ruleValue.ServiceItems)
                        bw.Write(serviceItem.ID.ToString());
                }
                //
                if (ruleValue.DayOfWeeks.Count > 0)
                {
                    bw.Write((byte)RuleValueType.DayOfWeek);
                    bw.Write(ruleValue.DayOfWeeks.Count);

                    foreach (var dayOfWeek in ruleValue.DayOfWeeks)
                        bw.Write((byte)dayOfWeek);
                }
                //
                if (ruleValue.Priorities.Any())
                {
                    bw.Write((byte)RuleValueType.Priority);
                    bw.Write(ruleValue.Priorities.Length);

                    foreach (var priority in ruleValue.Priorities)
                        bw.Write(priority.ID.ToString());
                }
                //
                if (ruleValue.Urgencies.Any())
                {
                    bw.Write((byte)RuleValueType.Urgency);
                    bw.Write(ruleValue.Urgencies.Length);

                    foreach (var urgency in ruleValue.Urgencies)
                        bw.Write(urgency.ID.ToString());
                }
                //
                if (ruleValue.CallTypes.Any())
                {
                    bw.Write((byte)RuleValueType.CallType);
                    bw.Write(ruleValue.CallTypes.Length);

                    foreach (var callType in ruleValue.CallTypes)
                        bw.Write(callType.ID.ToString());
                }
                //
                if (ruleValue.ClientPositions.Any())
                {
                    bw.Write((byte)RuleValueType.ClientPosition);
                    bw.Write(ruleValue.ClientPositions.Length);

                    foreach (var tuple in ruleValue.ClientPositions)
                        bw.Write(tuple.Item2);
                }
                //
                if (ruleValue.ClientOrgStructures.Count > 0)
                {
                    bw.Write((byte)RuleValueType.ClientOrgStructure);
                    bw.Write(ruleValue.ClientOrgStructures.Count);

                    foreach (var tuple in ruleValue.ClientOrgStructures)
                    {
                        bw.Write((int)tuple.Item2);
                        bw.Write(tuple.Item3.ToString());
                    }
                }
                //
                if (ruleValue.TimeRegistrations.Count > 0)
                {
                    foreach (var filterOperation in ruleValue.TimeRegistrations.Keys)
                    {
                        bw.Write((byte)RuleValueType.RegistrationTime);
                        bw.Write((int)filterOperation);
                        bw.Write(ruleValue.TimeRegistrations[filterOperation]);
                    }
                }
                //
                if (!string.IsNullOrEmpty(ruleValue.Price))
                {
                    bw.Write((byte)RuleValueType.Price);
                    bw.Write(ruleValue.Price);
                }
                //
                if (ruleValue.PromiseTime.HasValue)
                {
                    bw.Write((byte)RuleValueType.PromiseTime);
                    bw.Write(ruleValue.PromiseTime.Value);
                }
                //
                if (ruleValue.ParameterValues.Count > 0)
                {
                    bw.Write((byte)RuleValueType.ParameterTemplate);
                    bw.Write(ruleValue.ParameterValues.Count);

                    foreach (var data in ruleValue.ParameterValues)
                    {
                        bw.Write(data.Key.ToString());
                        //
                        if (data.Value == null)
                            bw.Write(0);

                        else
                        {
                            var valuesData = _ruleParameterConverter.ParametersToBytes(data.Value);

                            bw.Write(valuesData.Length);
                            bw.Write(valuesData);
                        }
                    }
                }
                //
                retval = stream.ToArray();
            }
            stream.Close();
            //
            return ValueTask.FromResult(retval);

        }
    }
}
