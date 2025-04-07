using AutoMapper;
using System.Linq;

namespace InfraManager.UI.Web.AutoMapper
{
    public class PathsArrayResolver<TSource, TDest> :
        IMemberValueResolver<TSource, TDest, InframanagerObject[], string[]>
    {
        private readonly IServiceMapper<ObjectClass, IBuildResourcePath> _pathBuilder;

        public PathsArrayResolver(IServiceMapper<ObjectClass, IBuildResourcePath> pathBuilder)
        {
            _pathBuilder = pathBuilder;
        }

        public string[] Resolve(TSource source, TDest destination, InframanagerObject[] sourceMember, string[] destMember, ResolutionContext context)
        {
            return sourceMember
                .Select(x => _pathBuilder.Map(x.ClassId).GetPathToSingle(x.Id.ToString()))
                .ToArray();
        }
    }
}
