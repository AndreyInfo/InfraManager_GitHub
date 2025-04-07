using System;

namespace InfraManager.BLL.ServiceDesk.Problems;

public class ProblemDependencyByProblemFilter
{
    /// <summary>
    /// Уникальный удентификатор проблемы.
    /// </summary>
    public Guid ProblemID { get; init; }
}