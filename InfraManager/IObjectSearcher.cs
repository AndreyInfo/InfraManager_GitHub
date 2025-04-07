using System.Collections.Generic;

namespace InfraManager.ComponentModel
{
    public interface IObjectSearcher
    {
        List<IObject> Search(string text);
        void Cancel();
    }
}
