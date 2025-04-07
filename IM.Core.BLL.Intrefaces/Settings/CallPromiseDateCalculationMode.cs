using InfraManager.BLL.Localization;

namespace InfraManager.BLL.Settings;

public enum CallPromiseDateCalculationMode : byte
{
    [FriendlyName("От даты создания")]
    SinceDateCreation = 0,

    [FriendlyName("От даты регистрации")]
    SinceDateRegistration = 1,

    [FriendlyName("От сейчас")]
    SinceNow = 2
}
