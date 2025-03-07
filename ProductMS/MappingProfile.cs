using AutoMapper;
using ProductMS.DTO;
using ProductMS.Entities;



namespace ProductMS;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegistrationDto, User>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

    }
}
