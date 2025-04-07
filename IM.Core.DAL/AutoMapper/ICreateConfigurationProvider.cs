using AutoMapper;

namespace InfraManager.DAL.AutoMapper
{
    internal interface ICreateConfigurationProvider<TSource, TDestination>
    {
        IConfigurationProvider Create();
    }

    internal interface ICreateConfigurationProvider<TSource, TDestination, TParam>
    {
        IConfigurationProvider Create(TParam param);
    }
}
