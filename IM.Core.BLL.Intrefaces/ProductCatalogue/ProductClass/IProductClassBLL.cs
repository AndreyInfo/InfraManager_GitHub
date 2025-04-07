namespace InfraManager.BLL.ProductCatalogue.ProductClass;

/// <summary>
/// Сопоставляет классы с другими классами
/// </summary>
public interface IProductClassBLL
{
    /// <summary>
    /// Получает класс модели сопоставленный с классом продукта
    /// </summary>
    /// <param name="productClass">Класс продукта</param>
    /// <returns>Класс модели</returns>
    ObjectClass? GetModelClassByProductClass(ObjectClass productClass);
}