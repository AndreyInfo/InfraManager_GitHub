using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Operations;

public interface ILifeCycleListOperationQuery
{
    /// <summary>
    /// ������ �� ��������� ������� � �������� ��������� ������
    /// </summary>
    /// <param name="rolesID">������ ��������������� �����</param>
    /// <param name="lifeCycleStateID">������������� ���������</param>
    /// <param name="cancellationToken"></param>
    /// <returns>������ � �������� ��������� ������</returns>
    Task<GroupedLifeCycleListItem[]> ExecuteAsync(Guid[] rolesID, Guid? lifeCycleStateID, CancellationToken cancellationToken = default);
}