using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters
{
    public interface IBuildTreeSettings
    {
        TreeSettings Build(IEnumerable<string> searcherParams);
    }
}
