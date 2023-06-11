using AutoMapper;

namespace Conflict.Server
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Message, MessageDto>();
            CreateMap<User, UserDto>();
        }
    }
}
