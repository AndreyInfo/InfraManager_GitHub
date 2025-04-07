using AutoMapper;

namespace InfraManager.DAL.ServiceDesk
{
    public class NoteDataProfile : Profile
    {
        public NoteDataProfile()
        {
            CreateMap<Note, NoteData>();
        }
    }
}
