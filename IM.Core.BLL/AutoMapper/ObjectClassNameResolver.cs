using AutoMapper;
using InfraManager.BLL.Settings;

namespace InfraManager.BLL.AutoMapper;

public class ObjectClassNameResolver<TSource, TDestination> : IMemberValueResolver<TSource, TDestination, ObjectClass, string>
{
    private readonly IClassIM _objectClassBll;

    public ObjectClassNameResolver(IClassIM objectClassBll)
    {
        _objectClassBll = objectClassBll;
    }

    public string Resolve(TSource source, TDestination destination, ObjectClass sourceMember, string destMember, ResolutionContext context)
    {
        return _objectClassBll.GetClassName(sourceMember);
    }
}