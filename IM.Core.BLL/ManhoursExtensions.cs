using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL
{
    internal static class ManhoursExtensions
    {
        /// <summary>
        /// Преобразует число минут трудозатрат / оценки в локализованное текстовое представление 
        /// </summary>
        /// <param name="textLocalizer">Ссылка на сервис локализации</param>
        /// <param name="culture">Идентификатор целевой локали</param>
        /// <param name="manhours">Значение трудозатрат в минутах</param>
        /// <returns>Текстовое представление длительности</returns>
        public static string ManhoursToString(this ILocalizeText textLocalizer, string culture, int manhours)
        {
            return string.Format(
                textLocalizer.Localize(Resources.ManhoursDisplayFormat, culture),
                TimeSpan.FromMinutes(manhours));
        }
    }
}
