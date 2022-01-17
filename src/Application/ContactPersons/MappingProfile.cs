using Alerting.Infrastructure.Data.DbContexts.Entities;
using AutoMapper;

namespace Alerting.Application.ContactPersons
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContactPerson, ContactPerson>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<ContactPerson, ContactPersonListItem>();
        }
    }
}