using AutoMapper;
using InfraManager.BLL.Localization;

namespace InfraManager.BLL.AutoMapper;

public class LocalizedTextResolver<TSource, TDestination> : IMemberValueResolver<TSource, TDestination, string, string>
{
    private readonly ILocalizeText _localizer;

    public LocalizedTextResolver(ILocalizeText localizer)
    {
        _localizer = localizer;
    }

    public string Resolve(TSource source, TDestination destination, string sourceMember, string destMember, ResolutionContext context)
    {
        return string.IsNullOrWhiteSpace(sourceMember) ? null : _localizer.Localize(sourceMember);
    }
}