using System;

namespace InfraManager.DAL;

public interface IProduct<out TModel>
    where TModel : IProductModel
{
    public TModel Model { get; }
}