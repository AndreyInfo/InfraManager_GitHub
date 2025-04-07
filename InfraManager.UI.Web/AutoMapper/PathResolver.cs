using AutoMapper;

namespace InfraManager.UI.Web.AutoMapper
{
    public class PathResolver<TSource, TDest> :
        IMemberValueResolver<TSource, TDest, InframanagerObject?, string>
    {
        private readonly IServiceMapper<ObjectClass, IBuildResourcePath> _pathBuilder;

        public PathResolver(IServiceMapper<ObjectClass, IBuildResourcePath> pathBuilder)
        {
            _pathBuilder = pathBuilder;
        }

        public string Resolve(TSource source, TDest destination, InframanagerObject? sourceMember, string destMember, ResolutionContext context)
        {
            return sourceMember.HasValue
                ? _pathBuilder.Map(sourceMember.Value.ClassId).GetPathToSingle(sourceMember.Value.Id.ToString())
                : string.Empty;
        }
    }
}
