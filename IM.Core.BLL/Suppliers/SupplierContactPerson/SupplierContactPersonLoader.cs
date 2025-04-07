﻿using Inframanager.BLL;
using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;
using SupplierContactPersonEntity = InfraManager.DAL.Suppliers.SupplierContactPerson;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

internal sealed class SupplierContactPersonLoader 
    : ILoadEntity<Guid, SupplierContactPersonEntity, SupplierContactPersonDetails>
    , ISelfRegisteredService<ILoadEntity<Guid, SupplierContactPersonEntity, SupplierContactPersonDetails>>
{
    private readonly IFinder<SupplierContactPersonEntity> _finder;

    public SupplierContactPersonLoader(IFinder<SupplierContactPersonEntity> finder)
    {
        _finder = finder;
    }

    public Task<SupplierContactPersonEntity> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        return _finder
            .With(x => x.Supplier)
            .With(x => x.Position)
            .FindOrRaiseErrorAsync(id, cancellationToken);
    }
}