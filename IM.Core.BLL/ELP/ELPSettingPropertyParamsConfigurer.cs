using System.Threading;
using Inframanager.BLL.Events;
using InfraManager.BLL.ProductCatalogue.Manufactures;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ELP
{
    internal class ELPSettingPropertyParamsConfigurer: IConfigureDefaultEventParamsBuilderCollection<ElpSetting>
    {
        private readonly IManufacturersBLL _manufacturesBLL;

        public ELPSettingPropertyParamsConfigurer(IManufacturersBLL manufacturesBLL)
        {
            _manufacturesBLL = manufacturesBLL;
        }
        public void Configure(IDefaultEventParamsBuildersCollection<ElpSetting> collection)
        {
            collection.HasProperty(x => x.Name).HasName("Название");
            collection.HasProperty(x => x.Note).HasName("Описание");
            //TODO сделать по другому без GetAwaiter().GetResult()
            //collection.HasProperty(x => x.VendorId).HasName("Производитель").HasConverter(x =>
            //    x.HasValue
            //        ? _manufacturesBLL.DetailsAsync(x.Value, CancellationToken.None).GetAwaiter().GetResult().Name
            //        : "Все производители");
        }
    }
}