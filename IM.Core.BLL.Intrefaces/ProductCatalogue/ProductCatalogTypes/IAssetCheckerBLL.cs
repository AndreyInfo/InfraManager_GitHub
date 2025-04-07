using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

public interface IAssetCheckerBLL
{
    /// <summary>
    /// �������� ���������� ����� �������� � ���� �������� ���������
    /// </summary>
    /// <param name="type">��� �������� ���������</param>
    /// <param name="cancellationToken">����� ������</param>
    /// <returns>����� ���� ��������� ��� ���</returns>
    Task<bool> HasAssetAsync(ProductCatalogType type, CancellationToken cancellationToken);
}