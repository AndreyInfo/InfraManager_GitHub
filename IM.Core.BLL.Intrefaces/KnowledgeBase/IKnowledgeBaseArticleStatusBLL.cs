using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.KnowledgeBase;


/// <summary>
/// ���������� ��� ������� � ��������� ������ ���� ������
/// </summary>
public interface IKnowledgeBaseArticleStatusBLL
{
    /// <summary>
    /// ������� ������ ���� ������(��������� �� ������������� ��� �� ���������� �������)
    /// </summary>
    /// <param name="id">������������� ������� ���� ������</param>
    /// <param name="cancellationToken"></param>
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    
    
    /// <summary>
    /// ��������� ������ �������� ���� ������
    /// </summary>
    /// <param name="filterBy">������</param>
    /// <param name="cancellationToken"></param>
    /// <returns>������� ���� ������</returns>
    Task<KBArticleStatusDetails[]> GetDetailsArrayAsync(LookupListFilter filterBy,
        CancellationToken cancellationToken = default);

    
    /// <summary>
    /// ��������� ������ ������� ���� ������
    /// </summary>
    /// <param name="id">������������� ������� ���� ������</param>
    /// <param name="cancellationToken"></param>
    /// <returns>������ ���� ������</returns>
    Task<KBArticleStatusDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

    
    /// <summary>
    /// ��������� ������ ���� ������
    /// </summary>
    /// <param name="data">������� ��������</param>
    /// <param name="cancellationToken"></param>
    /// <returns>����������� ������</returns>
    Task<KBArticleStatusDetails> AddAsync(LookupData data, CancellationToken cancellationToken = default);


    /// <summary>
    /// �������� ������ ���� ������
    /// </summary>
    /// <param name="id">������������� ����������� �������</param>
    /// <param name="data">������� ��������</param>
    /// <param name="cancellationToken"></param>
    /// <returns>���������� ������</returns>
    Task<KBArticleStatusDetails> UpdateAsync(Guid id, LookupData data,
        CancellationToken cancellationToken = default);
}