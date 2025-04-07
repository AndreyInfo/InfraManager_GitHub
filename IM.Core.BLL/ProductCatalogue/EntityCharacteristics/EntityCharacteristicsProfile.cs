using AutoMapper;
using InfraManager.DAL.Asset.Subclasses;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
internal sealed class EntityCharacteristicsProfile : Profile
{
    public EntityCharacteristicsProfile()
    {
        CreateMap<Motherboard, MotherboardDetails>();
        CreateMap<EntityCharacteristicsDataBase, Motherboard>();
        CreateMap<MotherboardDetails, EntityCharacteristicsDataBase>();

        CreateMap<Processor, ProcessorDetails>();
        CreateMap<EntityCharacteristicsDataBase, Processor>();
        CreateMap<ProcessorDetails, EntityCharacteristicsDataBase>();

        CreateMap<Memory, MemoryDetails>();
        CreateMap<EntityCharacteristicsDataBase, Memory>();
        CreateMap<MemoryDetails, EntityCharacteristicsDataBase>();


        CreateMap<VideoAdapter, VideoAdapterDetails>();
        CreateMap<EntityCharacteristicsDataBase, VideoAdapter>();
        CreateMap<VideoAdapterDetails, EntityCharacteristicsDataBase>();

        CreateMap<Soundcard, SoundcardDetails>();
        CreateMap<EntityCharacteristicsDataBase, Soundcard>();
        CreateMap<SoundcardDetails, EntityCharacteristicsDataBase>();

        CreateMap<NetworkAdapter, NetworkAdapterDetails>();
        CreateMap<EntityCharacteristicsDataBase, NetworkAdapter>();
        CreateMap<NetworkAdapterDetails, EntityCharacteristicsDataBase>();

        CreateMap<Storage, StorageDetails>();
        CreateMap<EntityCharacteristicsDataBase, Storage>();
        CreateMap<StorageDetails, EntityCharacteristicsDataBase>();

        CreateMap<CDAndDVDDrives, CDAndDVDDrivesDetails>();
        CreateMap<EntityCharacteristicsDataBase, CDAndDVDDrives>();
        CreateMap<CDAndDVDDrivesDetails, EntityCharacteristicsDataBase>();

        CreateMap<Floppydrive, FloppydriveDetails>();
        CreateMap<EntityCharacteristicsDataBase, Floppydrive>();
        CreateMap<FloppydriveDetails, EntityCharacteristicsDataBase>();

        CreateMap<StorageController, StorageControllerDetails>();
        CreateMap<EntityCharacteristicsDataBase, StorageController>();
        CreateMap<StorageControllerDetails, EntityCharacteristicsDataBase>();

        CreateMap<Modem, ModemDetails>();
        CreateMap<EntityCharacteristicsDataBase, Modem>();
        CreateMap<ModemDetails, EntityCharacteristicsDataBase>();
    }
}
